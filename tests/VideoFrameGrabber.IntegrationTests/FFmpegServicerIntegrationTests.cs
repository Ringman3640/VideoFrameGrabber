using FluentAssertions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using VideoFrameGrabber.FFmpegServicing;
using VideoFrameGrabber.IntegrationTests.StaticDependencies;

namespace VideoFrameGrabber.IntegrationTests;

public class FFmpegServicerIntegrationTests
{
    private static readonly FFmpegDependency ffmpeg = FFmpegDependency.Instance;
    private static readonly FakeFFmpegDependency fakeFFmpeg = FakeFFmpegDependency.Instance;
    private static readonly WrongFFmpegPathDependency wrongFFmpegPath = WrongFFmpegPathDependency.Instance;
    private static readonly FrameExtractionTestsDependency frameExtractionTests = FrameExtractionTestsDependency.Instance;
    private static readonly FakeVideoDependency fakeVideo = FakeVideoDependency.Instance;

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> can successfully initialize given an absolute path to
    /// FFmpeg.
    /// </summary>
    [Fact]
    public void Constructor_AbsoluteFFmpegFilePath_Succeeds()
    {
        TestHelpers.FFmpegServicerConstructorSucceeds(ffmpeg.AbsoluteFilePath);
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> can successfully initialize given a relative path to
    /// FFmpeg.
    /// </summary>
    [Fact]
    public void Constructor_RelativeFFmpegFilePath_Succeeds()
    {
        TestHelpers.FFmpegServicerConstructorSucceeds(ffmpeg.RelativeFilePath);
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> can successfully initialize given an absolute path to
    /// a folder containing FFmpeg.
    /// </summary>
    [Fact]
    public void Constructor_AbsoluteFFmpegFolderPath_Succeeds()
    {
        TestHelpers.FFmpegServicerConstructorSucceeds(ffmpeg.AbsoluteFolderPath);
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> can successfully initialize given a relative path to a
    /// folder containing FFmpeg.
    /// </summary>
    [Fact]
    public void Constructor_RelativeFFmpegFolderPath_Succeeds()
    {
        TestHelpers.FFmpegServicerConstructorSucceeds(ffmpeg.RelativeFolderPath);
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> fails to initialize when given a <c>null</c> string.
    /// </summary>
    [Fact]
    public void Constructor_NullString_ThrowsArgumentNullException()
    {
        TestHelpers.FFmpegServicerConstructorFails(
            ffmpegPath: null!,
            exceptionType: typeof(ArgumentNullException));
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> fails to initialize when given an empty string
    /// (including whitespace).
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\n")]
    [InlineData("\t")]
    public void Constructor_EmptyString_ThrowsArgumentException(string emptyString)
    {
        TestHelpers.FFmpegServicerConstructorFails(
            ffmpegPath: emptyString,
            exceptionType: typeof(ArgumentException));
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> fails given an absolute or relative path to a wrong
    /// FFmpeg file or a directory that does not contain FFmpeg.
    /// </summary>
    [Fact]
    public void Constructor_WrongAbsoluteFFmpegFilePath_ThrowsFileNotFoundException()
    {
        TestHelpers.FFmpegServicerConstructorFails(
            ffmpegPath: wrongFFmpegPath.AbsoluteFilePath,
            exceptionType: typeof(FileNotFoundException));
    }

    /// <inheritdoc cref="Constructor_WrongAbsoluteFFmpegFilePath_ThrowsFileNotFoundException"/>
    [Fact]
    public void Constructor_WrongAbsoluteFFmpegFolderPath_ThrowsFileNotFoundException()
    {
        TestHelpers.FFmpegServicerConstructorFails(
            ffmpegPath: wrongFFmpegPath.AbsoluteFolderPath,
            exceptionType: typeof(FileNotFoundException));
    }

    /// <inheritdoc cref="Constructor_WrongAbsoluteFFmpegFilePath_ThrowsFileNotFoundException"/>
    [Fact]
    public void Constructor_WrongRelativeFFmpegFilePath_ThrowsFileNotFoundException()
    {
        TestHelpers.FFmpegServicerConstructorFails(
            ffmpegPath: wrongFFmpegPath.RelativeFilePath,
            exceptionType: typeof(FileNotFoundException));
    }

    /// <inheritdoc cref="Constructor_WrongAbsoluteFFmpegFilePath_ThrowsFileNotFoundException"/>
    [Fact]
    public void Constructor_WrongRelativeFFmpegFolderPath_ThrowsFileNotFoundException()
    {
        TestHelpers.FFmpegServicerConstructorFails(
            ffmpegPath: wrongFFmpegPath.RelativeFolderPath,
            exceptionType: typeof(FileNotFoundException));
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> fails given an absolute or relative path to an
    /// invalid FFmpeg file or a directory containing an invalid FFmpeg file.
    /// </summary>
    [Fact]
    public void Constructor_FakeAbsoluteFFmpegFilePath_ThrowsFormatException()
    {
        TestHelpers.FFmpegServicerConstructorFails(
            ffmpegPath: fakeFFmpeg.AbsoluteFilePath,
            exceptionType: typeof(FormatException));
    }

    /// <inheritdoc cref="Constructor_FakeAbsoluteFFmpegFilePath_ThrowsFormatException"/>
    [Fact]
    public void Constructor_FakeAbsoluteFFmpegFolderPath_ThrowsFormatException()
    {
        TestHelpers.FFmpegServicerConstructorFails(
            ffmpegPath: fakeFFmpeg.AbsoluteFolderPath,
            exceptionType: typeof(FormatException));
    }

    /// <inheritdoc cref="Constructor_FakeAbsoluteFFmpegFilePath_ThrowsFormatException"/>
    [Fact]
    public void Constructor_FakeRelativeFFmpegFilePath_ThrowsFormatException()
    {
        TestHelpers.FFmpegServicerConstructorFails(
            ffmpegPath: fakeFFmpeg.RelativeFilePath,
            exceptionType: typeof(FormatException));
    }

    /// <inheritdoc cref="Constructor_FakeAbsoluteFFmpegFilePath_ThrowsFormatException"/>
    [Fact]
    public void Constructor_FakeRelativeFFmpegFolderPath_ThrowsFormatException()
    {
        TestHelpers.FFmpegServicerConstructorFails(
            ffmpegPath: fakeFFmpeg.RelativeFolderPath,
            exceptionType: typeof(FormatException));
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> can be initialized from calling
    /// <see cref="FFmpegServicer.FromSystem"/>.
    /// </summary>
    /// <remarks>
    /// This test assumes that FFmpeg is installed on the testing system and on the PATH environment
    /// variable.
    /// </remarks>
    [Fact]
    public void FromSystem_Succeeds()
    {
        FFmpegServicer? ffmpegServicer = null;

        try
        {
            ffmpegServicer = FFmpegServicer.FromSystem();
        }
        catch { }

        ffmpegServicer.Should().NotBeNull();
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> can successfully call FFmpeg.
    /// </summary>
    [Fact]
    public void CallFFmpegWithoutResult_ExtractFrameArgs_GeneratesImageFile()
    {
        using TestDirectoryDependency testDirectory = new();
        string videoPath = frameExtractionTests.EldenRingGameplay.VideoPath;
        string outputPath = Path.Join(testDirectory.Path, "out.jpg");
        string args = $"-ss 00:00:00 -i \"{videoPath}\" -vframes 1 \"{outputPath}\"";
        FFmpegServicer ffmpegServicer = FFmpegServicer.FromSystem();
        Exception? exception = null;

        try
        {
            ffmpegServicer.CallFFmpegWithoutResult(args);
        }
        catch (Exception except)
        {
            exception = except;
        }

        exception.Should().BeNull();
        File.Exists(outputPath).Should().BeTrue();
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> correctly throws an exception given invalid arguments.
    /// </summary>
    [Fact]
    public void CallFFmpegWithoutResult_InvalidArgs_ThrowsFFmpegErrorException()
    {
        string args = "ermm... what the sigma";
        FFmpegServicer ffmpegServicer = FFmpegServicer.FromSystem();
        Exception? exception = null;

        try
        {
            ffmpegServicer.CallFFmpegWithoutResult(args);
        }
        catch (Exception except)
        {
            exception = except;
        }

        exception.Should().NotBeNull();
        exception.Should().BeOfType<FFmpegErrorException>();
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> can successfully return a valid image as bytes given
    /// an args string.
    /// </summary>
    [Fact]
    public void CallFFmpegWithResult_ExtractFrameArgs_ReturnsValidImageBytes()
    {
        string videoPath = frameExtractionTests.EldenRingGameplay.VideoPath;
        string args = $"-ss 00:00:00 -i \"{videoPath}\" -vframes 1 -f image2pipe -";
        FFmpegServicer ffmpegServicer = FFmpegServicer.FromSystem();
        Exception? exception = null;
        byte[]? result = null;

        try
        {
            result = ffmpegServicer.CallFFmpegWithResult(args);
        }
        catch (Exception except)
        {
            exception = except;
        }

        exception.Should().BeNull();
        TestHelpers.BytesAreValidImage(result!).Should().BeTrue();
    }

    /// <inheritdoc cref="CallFFmpegWithoutResult_InvalidArgs_ThrowsFFmpegErrorException"/>
    [Fact]
    public void CallFFmpegWithResult_InvalidArgs_ThrowsFFmpegErrorException()
    {
        string args = "this is an invalid args string";
        FFmpegServicer ffmpegServicer = FFmpegServicer.FromSystem();
        Exception? exception = null;

        try
        {
            _ = ffmpegServicer.CallFFmpegWithResult(args);
        }
        catch (Exception except)
        {
            exception = except;
        }

        exception.Should().NotBeNull();
        exception.Should().BeOfType<FFmpegErrorException>();
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> can correctly extract metadata information from a
    /// video.
    /// </summary>
    [Fact]
    public void GetVideoMetadata_FrameExtractionVideos_ReturnsExpectedMetadataValues()
    {
        foreach (var (_, testGroup) in frameExtractionTests.TestGroups)
        {
            string videoPath = testGroup.VideoPath;
            TimeSpan expectedLength = testGroup.VideoLength;
            int expectedWidth = testGroup.VideoWidth;
            int expectedHeight = testGroup.VideoHeight;

            FFmpegServicer ffmpegServicer = FFmpegServicer.FromSystem();
            VideoMetadata? metadata = null;

            metadata = ffmpegServicer.GetVideoMetadata(videoPath);

            metadata.Should().NotBeNull();
            metadata.Value.VideoLength.Should().Be(expectedLength);
            metadata.Value.VideoWidth.Should().Be(expectedWidth);
            metadata.Value.VideoHeight.Should().Be(expectedHeight);
        }
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer"/> correctly throws an exception when trying to extract
    /// metadata from an invalid video file.
    /// </summary>
    [Fact]
    public void GetVideoMetadata_InvalidVideoFile_ThrowsArgumentException()
    {
        FFmpegServicer ffmpegServicer = FFmpegServicer.FromSystem();
        Exception? exception = null;

        try
        {
            _ = ffmpegServicer.GetVideoMetadata(fakeVideo.FakeVideoPath);
        }
        catch (Exception except)
        {
            exception = except;
        }

        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentException>();
    }

    /// <summary>
    /// Tests if <see cref="FFmpegServicer.GetVideoMetadata(string)"/> correctly throws an exception
    /// an invalid file path (like <c>null</c> or whitespace only).
    /// </summary>
    /// <param name="pathArgument"></param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetVideoMetadata_InvalidPathArgument_ThrowsArgumentException(string? pathArgument)
    {
        FFmpegServicer ffmpegServicer = FFmpegServicer.FromSystem();
        Exception? exception = null;

        try
        {
            _ = ffmpegServicer.GetVideoMetadata(pathArgument!);
        }
        catch (Exception except)
        {
            exception = except;
        }

        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentException>();
    }

    private static class TestHelpers
    {
        public static void FFmpegServicerConstructorSucceeds (string ffmpegPath)
        {
            FFmpegServicer? ffmpegServicer = null;

            ffmpegServicer = new(ffmpegPath);

            ffmpegServicer.Should().NotBeNull();
        }

        public static void FFmpegServicerConstructorFails (string ffmpegPath, Type exceptionType)
        {
            FFmpegServicer? ffmpegServicer = null;
            Exception? exception = null;

            try
            {
                ffmpegServicer = new(ffmpegPath);
            }
            catch (Exception except)
            {
                exception = except;
            }

            exception.Should().BeOfType(exceptionType);
            ffmpegServicer.Should().BeNull();
        }

        public static bool BytesAreValidImage(byte[] bytes)
        {
            try
            {
                using (Image.Load<Rgba32>(bytes))
                {
                    // Do nothing here
                    // The 'using' statement just makes sure the image is discarded immediately if
                    // the Image successfully loads.
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
