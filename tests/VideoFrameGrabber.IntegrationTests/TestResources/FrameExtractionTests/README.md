# TestResources/FrameExtractionTests
This README file describes how the ExtractedFrames test resource works and how it is formatted.

## About
ExtractedFrames is a test resource that contains test videos and associated extracted frames. This
is intended to test the accuracy of extracted frames using the VideoFrameGrabber library.

This resource is broken into test groups. A test group is essentially a video file grouped with its
test frames. A test frame is an image frame from the corresponding video file that results from a
collection of FFmpeg arguments.

The [FrameExtractionTestsDependency](../../StaticDependencies/FrameExtractionTestsDependency.cs)
static class handles exposing test groups and test classes to unit tests. It groups tests to their
corresponding test groups and associates FFmpeg arguments with tests.

## Auto-Generated Content
FrameExtractionTestsDependency auto-generates test resources when the static class instance is first
retrieved. Specifically, it will use FFmpeg to extract image frame results for each test group using
the test FFmpeg arguments on the test group's video file.

The tests for each test group are defined in the file [tests.json](./tests.json). This
is a JSON file that describes a set of tests that should be run on each test group.

Auto-generation is only executed if the test results have not been generated yet, or if the
tests.json file is updated. FrameExtractionTestsDependency keeps track of this by producing a file
named `generated_timestamp`. This is a file that will contains the string-representation of
tests.json's LastWriteTime as a Unix timestamp. If this file does not exist, or if its timestamp is
not the same as tests.json's LastWriteTime timestamp, FrameExtractionTestsDependency will perform
auto-generation.

## tests.json format
Each individual test is defined in tests.json by attributes that correspond to FFmpeg CLI arguments.
The top-level JSON object contains an attribute named `tests`, whose value represents an array of
test objects.

The following is the general structure of the JSON file:

```
{
    "tests": [
        {
            "TestName": (String) The name of the test,
            "SeekTime": (String) Seek time in the HH.MM.SS format,
            "ScaleInfo": {
                "Width": (Int) The FFmpeg scale width argument,
                "Height": (Int) The FFmpeg scale height argument
            } OR undefined OR null,
            "CropInfo": {
                "Width": (Int) The FFmpeg crop width argument,
                "Height": (Int) The FFmpeg crop height argument,
                "XPos": (Int) The FFmpeg crop X position argument,
                "YPos": (Int) The FFmpeg crop Y position argument
            } OR undefined OR null,
            "ScaleFirst" (Bool OR undefined) Indicates if cropping or scaling should occur first
        }
    ]
}
```

### TestName
The name of the test. This should be in PascalCase.

### SeekTime
The seek time that is used to extract a frame from the test group video. If this value is larger
than the length of the video, the last frame of the video should be extracted during the
auto-generation process.

### ScaleInfo
An object that provides FFmpeg scaling arguments. This is optional.

### CropInfo
An object that provides FFmpeg cropping arguments. This is optional.

### ScaleFirst
A bool that indicates the order of cropping and scaling. This is optional. The default behavior is
to do scaling first.
