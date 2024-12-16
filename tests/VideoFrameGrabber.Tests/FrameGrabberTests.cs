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
    public void Constructor_EnvPathArgumentValid_Succeeds()
    {
        FrameGrabber? grabber = null;

        try
        {
            grabber = new("ffmpeg");
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
            grabber = new(ffmpeg.AbsolutePath);
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
            grabber = new(fakeFfmpeg.AbsolutePath);
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
            grabber = new(ffmpeg.RelativePath);
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
            grabber = new(fakeFfmpeg.RelativePath);
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
