using FluentAssertions;
using VideoFrameGrabber.Tests.ClassFixtures;
using VideoFrameGrabber.Tests.CollectionFixtures;

namespace VideoFrameGrabber.Tests;

[Collection("FFmpegDependency collection")]
public class FrameGrabberTests :
    IClassFixture<FakeFFmpegDependencyFixture>,
    IClassFixture<WrongFFmpegPathDependencyFixture>
{
    private FFmpegDependencyFixture ffmpeg;
    private FakeFFmpegDependencyFixture fakeFfmpeg;
    private WrongFFmpegPathDependencyFixture wrongFfmpeg;

    public FrameGrabberTests(
        FFmpegDependencyFixture ffmpegFixture,
        FakeFFmpegDependencyFixture fakeFfmpegFixture,
        WrongFFmpegPathDependencyFixture wrongFfmpegPathFixture)
    {
        ffmpeg = ffmpegFixture;
        fakeFfmpeg = fakeFfmpegFixture;
        wrongFfmpeg = wrongFfmpegPathFixture;
    }

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
}
