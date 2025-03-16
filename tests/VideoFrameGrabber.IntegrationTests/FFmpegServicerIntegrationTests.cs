using FluentAssertions;
using VideoFrameGrabber.FFmpegServicing;
using VideoFrameGrabber.IntegrationTests.StaticDependencies;

namespace VideoFrameGrabber.IntegrationTests;

public class FFmpegServicerIntegrationTests
{
    private readonly FFmpegDependency ffmpeg = FFmpegDependency.Instance;
    private readonly FakeFFmpegDependency fakeFFmpeg = FakeFFmpegDependency.Instance;
    private readonly WrongFFmpegPathDependency wrongFFmpegPath = WrongFFmpegPathDependency.Instance;
    private readonly FrameExtractionTestsDependency frameExtractionTests = FrameExtractionTestsDependency.Instance;

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

    [Fact]
    public void CallFFmpegWithResult_ExtractFrameArgs_ReturnsImageBytes()
    {
        string videoPath = frameExtractionTests.EldenRingGameplay.VideoPath;
        string args = $"-ss 00:00:00 -i \"{videoPath}\" -vframes 1 -f image2pipe -";
        FFmpegServicer ffmpegServicer = FFmpegServicer.FromSystem();
        Exception? exception = null;
        byte[]? results = null;

        try
        {
            results = ffmpegServicer.CallFFmpegWithResult(args);
        }
        catch (Exception except)
        {
            exception = except;
        }

        exception.Should().BeNull();
        results.Should().NotBeNull();
        results.Should().NotBeEmpty();
    }

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
    }
}
