using FluentAssertions;

namespace VideoFrameGrabber.Tests.UnitTests;

public class FrameGrabberUnitTests
{
    /// <summary>
    /// Tests if FrameGrabber relinquishes the implementation of FromSystem to its FFmpegServicer.
    /// </summary>
    [Fact]
    public void FromSystem_CallsFFmpegServicerFromSystem()
    {
        throw new NotImplementedException();
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
