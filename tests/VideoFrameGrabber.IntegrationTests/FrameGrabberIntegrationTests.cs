using FluentAssertions;
using VideoFrameGrabber.IntegrationTests.StaticDependencies;

namespace VideoFrameGrabber.IntegrationTests;

public class FrameGrabberIntegrationTests
{
    private static readonly FFmpegDependency ffmpeg = FFmpegDependency.Instance;
    private static readonly FrameExtractionTestsDependency frameExtractionTests = FrameExtractionTestsDependency.Instance;

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
