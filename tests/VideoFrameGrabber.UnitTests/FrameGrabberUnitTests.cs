using FluentAssertions;
using Moq;
using VideoFrameGrabber.FFmpegServicing;

namespace VideoFrameGrabber.UnitTests;

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
    [InlineData("-00:40:30.5", "0")]
    [InlineData("-00:00:00.1", "0")]
    [InlineData("-23:59:59.99", "0")]
    public void FormatSeekTime_TimeSpan_ReturnsExpectedString(string timeSpanValue, string expected)
    {
        TimeSpan timespan = TimeSpan.Parse(timeSpanValue);
        string result;

        result = FrameGrabber.FFmpegFormatting.FormatSeekTime(timespan);

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Tests the validity of
    /// <see cref="FrameGrabber.FFmpegFormatting.GetFFmpegArgs(TimeSpan, string, string, ImageFormat)"/>.
    /// </summary>
    [Fact]
    public void GetFFmpegArgs_SeekTimeAndVideoPathAndFiltersAndImageFormatArguments_ReturnsExpectedArgs() {
        TimeSpan seekTime = TimeSpan.Parse("00:00:30");
        string videoPath = "./videos/super_cool_video.mp4";
        string filters = "scale=150:150";
        ImageFormat imageFormat = ImageFormat.Jpg;
        string expectedArgs = "-ss 30 -i \"./videos/super_cool_video.mp4\" -v:f \"scale=150:150\" -vframes 1 -f image2pipe -c:v mjpeg -";
        string receivedArgs;

        receivedArgs = FrameGrabber.FFmpegFormatting.GetFFmpegArgs(seekTime, videoPath, filters, imageFormat);

        receivedArgs.Should().BeEquivalentTo(expectedArgs);
    }

    /// <summary>
    /// Tests the validity of
    /// <see cref="FrameGrabber.FFmpegFormatting.GetFFmpegArgs(TimeSpan, string, string, string)"/>.
    /// </summary>
    [Fact]
    public void GetFFmpegArgs_SeekTimeAndVideoPathAndFiltersAndOutputPathArguments_ReturnsExpectedArgs()
    {
        TimeSpan seekTime = TimeSpan.Parse("00:00:05.5");
        string videoPath = "./videos/super_lame_video.mp4";
        string filters = "scale=600:0,crop=200:200";
        string outputPath = "./thumbs/thumb.png";
        string expectedArgs = "-ss 5.5 -i \"./videos/super_lame_video.mp4\" -v:f \"scale=600:0,crop=200:200\" -vframes 1 \"./thumbs/thumb.png\"";
        string receivedArgs;

        receivedArgs = FrameGrabber.FFmpegFormatting.GetFFmpegArgs(seekTime, videoPath, filters, outputPath);

        receivedArgs.Should().BeEquivalentTo(expectedArgs);
    }

    /// <summary>
    /// Tests the validity of
    /// <see cref="FrameGrabber.FFmpegFormatting.GetFFmpegArgs(string, string, ImageFormat)"/>.
    /// </summary>
    [Fact]
    public void GetFFmpegArgs_VideoPathAndFiltersAndImageFormatArguments_ReturnsExpectedArgs()
    {
        string videoPath = "./videos/insane_switch_axe_clip.mp4";
        string filters = "crop=100:100:0:0";
        ImageFormat imageFormat = ImageFormat.Bmp;
        string expectedArgs = "-sseof -3 -i \"./videos/insane_switch_axe_clip.mp4\" -update 1 -v:f \"crop=100:100:0:0\" -f image2pipe -c:v bmp -";
        string receivedArgs;

        receivedArgs = FrameGrabber.FFmpegFormatting.GetFFmpegArgs(videoPath, filters, imageFormat);

        receivedArgs.Should().BeEquivalentTo(expectedArgs);
    }

    /// <summary>
    /// Tests the validity of
    /// <see cref="FrameGrabber.FFmpegFormatting.GetFFmpegArgs(string, string, string)"/>.
    /// </summary>
    [Fact]
    public void GetFFmpegArgs_VideoPathAndFiltersAndOutputPathArguments_ReturnsExpectedArgs()
    {
        string videoPath = "./videos/insane_charge_blade_clip.mov";
        string filters = "scale=500:0";
        string outputPath = "./thumbs/bruh.gif";
        string expectedArgs = "-sseof -3 -i \"./videos/insane_charge_blade_clip.mov\" -update 1 -v:f \"scale=500:0\" \"./thumbs/bruh.gif\"";
        string receivedArgs;

        receivedArgs = FrameGrabber.FFmpegFormatting.GetFFmpegArgs(videoPath, filters, outputPath);

        receivedArgs.Should().BeEquivalentTo(expectedArgs);
    }
}
