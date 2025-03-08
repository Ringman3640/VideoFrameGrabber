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

    /// <summary>
    /// Tests the validity of <see cref="FrameGrabber.FFmpegFormatting.FormatSeekTime(TimeSpan)"/>
    /// given <see cref="TimeSpan"/> values.
    /// </summary>
    /// <remarks>
    /// A string representation of a <see cref="TimeSpan"/> is provided instead of the
    /// <see cref="TimeSpan"/> itself to prevent needing to use the MethodData attribute, which
    /// would require defining a separate method. Simply providing InlineData attributes is more
    /// clean, but it only supports constant expressions, so <see cref="TimeSpan"/> objects cannot
    /// be directly added to it. Instead, a string time is given, which is parsed into an equivalent
    /// <see cref="TimeSpan"/>.
    /// </remarks>
    [Theory]
    [InlineData("00:00:01", "1")]
    [InlineData("00:00:05", "5")]
    [InlineData("00:01:20", "80")]
    [InlineData("10:00:00", "36000")]
    [InlineData("23:59:59", "86399")]
    [InlineData("00:00:01.5", "1.5")]
    [InlineData("00:40:30.5", "2430.5")]
    [InlineData("00:00:00.1", "0.1")]
    [InlineData("00:00:00.01", "0.01")]
    [InlineData("00:00:00.99", "0.99")]
    [InlineData("23:59:59.99", "86399.99")]
    [InlineData("00:00:00", "0")]
    [InlineData("00:00:00.0", "0")]
    [InlineData("-00:00:01", "0")]
    [InlineData("-00:01:20", "0")]
    [InlineData("-23:59:59", "0")]
    [InlineData("-00:40:30.5", "00")]
    [InlineData("-00:00:00.1", "0")]
    [InlineData("-23:59:59.99", "0")]
    public void FormatSeekTime_TimeSpan_ReturnsExpectedString(string timeSpanValue, string expected)
    {
        TimeSpan timespan = TimeSpan.Parse(timeSpanValue);
        string result;

        result = FrameGrabber.FFmpegFormatting.FormatSeekTime(timespan);

        result.Should().BeEquivalentTo(expected);
    }
}
