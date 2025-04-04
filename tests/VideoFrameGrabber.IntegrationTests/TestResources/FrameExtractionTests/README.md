# TestResources/FrameExtractionTests
This README file describes how the FrameExtractionTests test resource works and how it is formatted.

## About
FrameExtractionTests is a test resource that contains test videos and associated extracted frames.
Information about the video file itself, such as the video length and video resolution, is also
included. This is intended to test the accuracy of extracted frames using the VideoFrameGrabber
library and to test the ability to extract metadata from video files.

This resource is broken into test groups. A test group is essentially a video file grouped with its
test frames and a JSON file that describes the video information. A test frame is an image frame
from the corresponding video file that results from a collection of FFmpeg arguments.

The [FrameExtractionTestsDependency](../../StaticDependencies/FrameExtractionTestsDependency.cs)
static class handles exposing test groups and test classes to unit tests. It groups tests to their
corresponding test groups and associates FFmpeg arguments with tests.

## Resource Format
Each test video lives in its own directory within the resource directory. This containing directory
is responsible for naming the test group of the video file and it contains the video file, the video
info file, and extracted frames that are generated for testing. It effectively represents the test
group. At a minimum each test group directory needs the following files:

- `video.[extension]`
- `video_info.json`

`video.[extension]` is the actual video file. `[extension]` refers to the file extension of the
video file. This can be any valid video format extension, such as `mp4` and `mov`. `video_info.json`
is a JSON file that contains information about the video file. Specifically, the JSON file includes
the video's length and the video's with and height.

## video_info.json Format
Each test group needs its own `video_info.json` file that provides the length and dimensions of the
test group video file. Each of these metrics are provided as JSON attribute.

The following is the general structure of the JSON file:

```
{
    "VideoLength": (String) Video length in a TimeSpan-parsable format,
    "VideoWidth": (Int) Horizontal resolution of the video,
    "VideoHeight": (Int) Vertical resolution of the video
}
```

### VideoLength
The value of video length is a string representation of a
[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan?view=net-9.0). Internally,
this string value is provided to the
[TimeSpan.Parse](https://learn.microsoft.com/en-us/dotnet/api/system.timespan.parse?view=net-9.0#system-timespan-parse(system-string))
method to create an equivalent TimeSpan. This method supports multiple string formats, such as
HH:MM:SS.MS.

### VideoWidth and VideoHeight
The width and height of the video. These are in pixel units.

## Getting Video Info
The contents of the `video_info.json` file for each test group needs to be manually filled out. The
information for each attribute can be obtained by passing the video file to FFmpeg in the command
line. The following command can be used:

```
ffmpeg -i [video path]
```

The video resolution can also be pulled from the video file's property page on Windows File Expoler.
However, the video length provided by the properties page is not very accurate and doesn't seem to
give milliseconds.

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

## tests.json Format
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
