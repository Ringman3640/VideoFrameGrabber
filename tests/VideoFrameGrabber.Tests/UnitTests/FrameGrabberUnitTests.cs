using FluentAssertions;
using Moq;
using VideoFrameGrabber.FFmpegServicing;

namespace VideoFrameGrabber.Tests.UnitTests;

public class FrameGrabberUnitTests
{
    private readonly Mock<IFFmpegServicer> ffmpegServicerMock;
    private readonly FrameGrabber frameGrabber;

    public FrameGrabberUnitTests()
    {
        ffmpegServicerMock = new Mock<IFFmpegServicer>();
        frameGrabber = new FrameGrabber(ffmpegServicerMock.Object);
    }

    /// <summary>
    /// Tests if providing an empty value (null or empty string) to ExtractFrame throws an
    /// exception.
    /// </summary>
    [Theory]
    [InlineData(null, typeof(ArgumentNullException))]
    [InlineData("", typeof(ArgumentException))]
    [InlineData("   ", typeof(ArgumentException))]
    public void ExtractFrame_EmptyArgument_ThrowsException(string? argument, Type exceptionType)
    {
        throw new NotImplementedException();
    }
}
