using FluentAssertions;
using VideoFrameGrabber.Scaling;

namespace VideoFrameGrabber.Tests.UnitTests.Scaling;

public class WidthScalerUnitTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(99999)]
    [InlineData(int.MaxValue)]
    public void Constructor_ValidWidthValue_WidthMatchesValue(int width)
    {
        WidthScaler? scaler = null;

        try
        {
            scaler = new(width);
        }
        catch { }

        scaler.Should().NotBeNull();
        scaler!.Width.Should().Be(width);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-99999)]
    [InlineData(int.MinValue)]
    public void Constructor_ZeroOrNegativeValue_ThrowsArgumentOutOfRangeException(int width)
    {
        WidthScaler? scaler = null;
        Exception? exception = null;

        try
        {
            scaler = new(width);
        }
        catch (Exception except)
        {
            exception = except;
        }

        scaler.Should().BeNull();
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    [Theory]
    [MemberData(nameof(GetWidthAndInputAndExpectedValues))]
    public void GetScaleParameters_WidthAndInputSizeValues_ResultMatchesExpectedSize(
        int width,
        Size inputSize,
        Size expectedSize
    ) {
        WidthScaler scaler = new(width);
        ScaleParameters? scaleParameters = null;

        try
        {
            scaleParameters = scaler.GetScaleParameters(inputSize.Width, inputSize.Height);
        }
        catch { }

        scaleParameters.Should().NotBeNull();
        scaleParameters!.Value.Width.Should().Be(expectedSize.Width);
        scaleParameters!.Value.Height.Should().Be(expectedSize.Height);
    }

    /// <summary>
    /// Gets a width value, an input <see cref="Size"/> value, and an expected <see cref="Size"/>
    /// value. Use to test <see cref="WidthScaler.GetScaleParameters(int, int)"/>.
    /// </summary>
    public static IEnumerable<object[]> GetWidthAndInputAndExpectedValues()
    {
        int max = int.MaxValue;

        yield return new object[] { 100, new Size(100, 1), new Size(100, 1) };
        yield return new object[] { 100, new Size(100, 50), new Size(100, 50) };
        yield return new object[] { 100, new Size(50, 50), new Size(100, 100) };
        yield return new object[] { 100, new Size(10, 50), new Size(100, 500) };
        yield return new object[] { 100, new Size(1, 50), new Size(100, 5000) };
        yield return new object[] { 1, new Size(10, 10), new Size(1, 1) };
        yield return new object[] { 1, new Size(10, 50), new Size(1, 5) };
        yield return new object[] { 1, new Size(10, 5000), new Size(1, 500) };
        yield return new object[] { 1, new Size(max, max), new Size(1, 1) };
        yield return new object[] { max, new Size(1, 1), new Size(max, max) };
        yield return new object[] { max, new Size(max, max), new Size(max, max) };
        yield return new object[] { max, new Size(1, max), new Size(max, max) };
        yield return new object[] { max, new Size(100, max), new Size(max, max) };
        yield return new object[] { max, new Size(999999, max), new Size(max, max) };
    }
}
