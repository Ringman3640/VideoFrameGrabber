using FluentAssertions;
using VideoFrameGrabber.Tests.ClassFixtures;
using VideoFrameGrabber.Tests.StaticDependencies;

namespace VideoFrameGrabber.Tests;

public class FrameGrabberTests :
    IClassFixture<FakeFFmpegDependencyFixture>,
    IClassFixture<WrongFFmpegPathDependencyFixture>
{
    private FFmpegDependency ffmpeg;
    private FakeFFmpegDependencyFixture fakeFfmpeg;
    private WrongFFmpegPathDependencyFixture wrongFfmpeg;

    public FrameGrabberTests(
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
    public void Constructor_AbsoluteFilePathArgumentInvalid_Fails()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new(fakeFfmpeg.AbsoluteFilePath);
        }
        catch { }

        grabber.Should().BeNull();
    }

    // T-4
    [Fact]
    public void Constructor_WrongAbsoluteFilePathArgument_Fails()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new(wrongFfmpeg.AbsoluteFilePath);
        }
        catch { }

        grabber.Should().BeNull();
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
    public void Constructor_RelativeFilePathArgumentInvalid_Fails()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new(fakeFfmpeg.RelativeFilePath);
        }
        catch { }

        grabber.Should().BeNull();
    }

    // T-7
    [Fact]
    public void Constructor_WrongRelativeFilePathArgument_Fails()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new(wrongFfmpeg.RelativeFilePath);
        }
        catch { }

        grabber.Should().BeNull();
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
    public void Constructor_AbsoluteFolderPathArgumentInvalid_Fails()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new(fakeFfmpeg.AbsoluteFolderPath);
        }
        catch { }

        grabber.Should().BeNull();
    }

    // T-10
    [Fact]
    public void Constructor_WrongAbsoluteFolderPathArgument_Fails()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new(wrongFfmpeg.AbsoluteFolderPath);
        }
        catch { }

        grabber.Should().BeNull();
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
    public void Constructor_RelativeFolderPathArgumentInvalid_Fails()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new(fakeFfmpeg.RelativeFolderPath);
        }
        catch { }

        grabber.Should().BeNull();
    }

    // T-13
    [Fact]
    public void Constructor_WrongRelativeFolderPathArgument_Fails()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new(wrongFfmpeg.RelativeFolderPath);
        }
        catch { }

        grabber.Should().BeNull();
    }

    // T-14
    [Fact]
    public void Constructor_NullArgument_Fails()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new(null);
        }
        catch { }

        grabber.Should().BeNull();
    }

    // T-15
    [Fact]
    public void Constructor_EmptyStringArgument_Fails()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new("");
        }
        catch { }

        grabber.Should().BeNull();
    }

    // T-16
    [Fact]
    public void Constructor_WhitespaceStringArgument_Fails()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new("       ");
        }
        catch { }

        grabber.Should().BeNull();
    }
}
