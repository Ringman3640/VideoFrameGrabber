using FluentAssertions;
using VideoFrameGrabber.IntegrationTests.StaticDependencies;

namespace VideoFrameGrabber.IntegrationTests;

public class FrameGrabberIntegrationTests
{
    private FFmpegDependency ffmpeg;

    public FrameGrabberIntegrationTests()
    {
        ffmpeg = FFmpegDependency.Instance;
    }

    [Fact]
    public void ExtractFrame_NullString_ThrowsArgumentNullException()
    {
        FrameGrabber grabber = new(ffmpeg.AbsoluteFilePath);
        Exception? exception = null;

        try
        {
            _ = grabber.ExtractFrame(null!);
        }
        catch (Exception except)
        {
            exception = except;
        }

        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void ExtractFrame_AllVideosAndArgumentsInFrameExtractionTests_ReturnsImageBytesSimilarToExpected()
    {
        throw new NotImplementedException();
    }
}
