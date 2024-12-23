using FluentAssertions;
using VideoFrameGrabber.Tests.ClassFixtures;
using VideoFrameGrabber.Tests.StaticDependencies;

namespace VideoFrameGrabber.Tests.UnitTests;

public class FrameGrabberUnitTests :
    IClassFixture<FakeFFmpegDependencyFixture>,
    IClassFixture<WrongFFmpegPathDependencyFixture>
{
    private FFmpegDependency ffmpeg;
    private FakeFFmpegDependencyFixture fakeFfmpeg;
    private WrongFFmpegPathDependencyFixture wrongFfmpeg;

    public FrameGrabberUnitTests(
        FakeFFmpegDependencyFixture fakeFfmpegFixture,
        WrongFFmpegPathDependencyFixture wrongFfmpegPathFixture)
    {
        ffmpeg = FFmpegDependency.Instance;
        fakeFfmpeg = fakeFfmpegFixture;
        wrongFfmpeg = wrongFfmpegPathFixture;
    }

    // T-1
    [Fact]
    public void FromSystem_Succeeds()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = FrameGrabber.FromSystem();
        }
        catch { }

        grabber.Should().NotBeNull();
    }

    // T-2
    [Fact]
    public void Constructor_AbsoluteFilePathArgumentValid_Succeeds()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new(ffmpeg.AbsoluteFilePath);
        }
        catch { }

        grabber.Should().NotBeNull();
    }

    // T-3
    [Fact]
    public void Constructor_AbsoluteFilePathArgumentInvalid_ThrowsFormatException()
    {
        FrameGrabber? grabber = null;
        Exception? exception = null;

        try
        {
            grabber = new(fakeFfmpeg.AbsoluteFilePath);
        }
        catch(Exception except)
        {
            exception = except;
        }

        grabber.Should().BeNull();
        exception.Should().BeOfType<FormatException>();
    }

    // T-4
    [Fact]
    public void Constructor_WrongAbsoluteFilePathArgument_ThrowsFileNotFoundException()
    {
        FrameGrabber? grabber = null;
        Exception? exception = null;

        try
        {
            grabber = new(wrongFfmpeg.AbsoluteFilePath);
        }
        catch(Exception except)
        {
            exception = except;
        }

        grabber.Should().BeNull();
        exception.Should().BeOfType<FileNotFoundException>();
    }

    // T-5
    [Fact]
    public void Constructor_RelativeFilePathArgumentValid_Succeeds()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new(ffmpeg.RelativeFilePath);
        }
        catch { }

        grabber.Should().NotBeNull();
    }

    // T-6
    [Fact]
    public void Constructor_RelativeFilePathArgumentInvalid_ThrowsFormatException()
    {
        FrameGrabber? grabber = null;
        Exception? exception = null;

        try
        {
            grabber = new(fakeFfmpeg.RelativeFilePath);
        }
        catch(Exception except)
        {
            exception = except;
        }

        grabber.Should().BeNull();
        exception.Should().BeOfType<FormatException>();
    }

    // T-7
    [Fact]
    public void Constructor_WrongRelativeFilePathArgument_ThrowsFileNotFoundException()
    {
        FrameGrabber? grabber = null;
        Exception? exception = null;

        try
        {
            grabber = new(wrongFfmpeg.RelativeFilePath);
        }
        catch (Exception except)
        {
            exception = except;
        }

        grabber.Should().BeNull();
        exception.Should().BeOfType<FileNotFoundException>();
    }

    // T-8
    [Fact]
    public void Constructor_AbsoluteFolderPathArgument_Succeeds()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new(ffmpeg.AbsoluteFolderPath);
        }
        catch { }

        grabber.Should().NotBeNull();
    }

    // T-9
    [Fact]
    public void Constructor_AbsoluteFolderPathArgumentInvalid_ThrowsFormatException()
    {
        FrameGrabber? grabber = null;
        Exception? exception = null;

        try
        {
            grabber = new(fakeFfmpeg.AbsoluteFolderPath);
        }
        catch (Exception except)
        {
            exception = except;
        }

        grabber.Should().BeNull();
        exception.Should().BeOfType<FormatException>();
    }

    // T-10
    [Fact]
    public void Constructor_WrongAbsoluteFolderPathArgument_ThrowsFileNotFoundException()
    {
        FrameGrabber? grabber = null;
        Exception? exception = null;

        try
        {
            grabber = new(wrongFfmpeg.AbsoluteFolderPath);
        }
        catch (Exception except)
        {
            exception = except;
        }

        grabber.Should().BeNull();
        exception.Should().BeOfType<FileNotFoundException>();
    }

    // T-11
    [Fact]
    public void Constructor_RelativeFolderPathArgument_Succeeds()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new(ffmpeg.RelativeFolderPath);
        }
        catch { }

        grabber.Should().NotBeNull();
    }

    // T-12
    [Fact]
    public void Constructor_RelativeFolderPathArgumentInvalid_ThrowsFormatException()
    {
        FrameGrabber? grabber = null;
        Exception? exception = null;

        try
        {
            grabber = new(fakeFfmpeg.RelativeFolderPath);
        }
        catch (Exception except)
        {
            exception = except;
        }

        grabber.Should().BeNull();
        exception.Should().BeOfType<FormatException>();
    }

    // T-13
    [Fact]
    public void Constructor_WrongRelativeFolderPathArgument_ThrowsFileNotFoundException()
    {
        FrameGrabber? grabber = null;
        Exception? exception = null;

        try
        {
            grabber = new(wrongFfmpeg.RelativeFolderPath);
        }
        catch (Exception except)
        {
            exception = except;
        }

        grabber.Should().BeNull();
        exception.Should().BeOfType<FileNotFoundException>();
    }

    // T-14
    [Fact]
    public void Constructor_NullArgument_ThrowsArgumentNullException()
    {
        FrameGrabber? grabber = null;
        Exception? exception = null;

        try
        {
            grabber = new(null!);
        }
        catch (Exception except)
        {
            exception = except;
        }

        grabber.Should().BeNull();
        exception.Should().BeOfType<ArgumentNullException>();
    }

    // T-15
    [Fact]
    public void Constructor_EmptyStringArgument_ThrowsArgumentException()
    {
        FrameGrabber? grabber = null;
        Exception? exception = null;

        try
        {
            grabber = new("");
        }
        catch (Exception except)
        {
            exception = except;
        }

        grabber.Should().BeNull();
        exception.Should().BeOfType<ArgumentException>();
    }

    // T-16
    [Fact]
    public void Constructor_WhitespaceStringArgument_ThrowsArgumentException()
    {
        FrameGrabber? grabber = null;
        Exception? exception = null;

        try
        {
            grabber = new("       ");
        }
        catch (Exception except)
        {
            exception = except;
        }

        grabber.Should().BeNull();
        exception.Should().BeOfType<ArgumentException>();
    }

    // T-17
    [Theory]
    [InlineData(null, typeof(ArgumentNullException))]
    [InlineData("", typeof(ArgumentException))]
    [InlineData("   ", typeof(ArgumentException))]
    public void ExtractFrame_InvalidArgument_ThrowsException(string? argument, Type exceptionType)
    {
        FrameGrabber grabber = new(ffmpeg.AbsoluteFilePath);
        Exception? exception = null;

        try
        {
            _ = grabber.ExtractFrame(argument!);
        }
        catch (Exception except)
        {
            exception = except;
        }

        exception.Should().BeOfType(exceptionType);
    }
}
