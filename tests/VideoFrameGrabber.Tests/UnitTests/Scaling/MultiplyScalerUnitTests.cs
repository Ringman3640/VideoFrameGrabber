using FluentAssertions;
using VideoFrameGrabber.Scaling;

namespace VideoFrameGrabber.Tests.UnitTests.Scaling;

public class MultiplyScalerUnitTests
{
    // T-1
    [Theory]
    [InlineData(1d)]
    [InlineData(10d)]
    [InlineData(9999d)]
    [InlineData(0.9d)]
    [InlineData(0.5d)]
    [InlineData(0.1d)]
    [InlineData(0.01d)]
    [InlineData(double.MaxValue)]
    public void Constructor_PositiveValue_MultiplierMatchesValue(double multiplier)
    {
        MultiplyScaler? multiplyScaler = null;

        try
        {
            multiplyScaler = new(multiplier);
        }
        catch { }

        multiplyScaler.Should().NotBeNull();
        multiplyScaler!.Multiplier.Should().Be(multiplier);
    }

    // T-2
    [Theory]
    [InlineData(0d)]
    [InlineData(-1d)]
    [InlineData(-10d)]
    [InlineData(-9999d)]
    [InlineData(-0.9d)]
    [InlineData(-0.5d)]
    [InlineData(-0.1d)]
    [InlineData(-0.01d)]
    [InlineData(double.MinValue)]
    public void Constructor_ZeroOrNegativeValue_ThrowsArgumentOutOfRangeException(double multiplier)
    {
        MultiplyScaler? multiplyScaler = null;
        Exception? exception = null;

        try
        {
            multiplyScaler = new(multiplier);
        }
        catch (Exception except)
        {
            exception = except;
        }

        multiplyScaler.Should().BeNull();
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    // T-3
    [Theory]
    [MemberData(nameof(GetMultiplierAndSizeValues))]
    public void GetScaleParameters_MultiplierAndInputSizeValues_ResultMatchesExpectedDimensions (
        double multiplier,
        Size inputSize,
        Size expectedSize
    ) {
        MultiplyScaler multiplyScaler = new(multiplier);
        ScaleParameters? scaleParameters = null;

        try
        {
            scaleParameters = multiplyScaler.GetScaleParameters(inputSize.Width, inputSize.Height);
        }
        catch { }

        scaleParameters.Should().NotBeNull();
        scaleParameters!.Value.Width.Should().Be(expectedSize.Width);
        scaleParameters!.Value.Height.Should().Be(expectedSize.Height);
    }

    /// <summary>
    /// Gets multiplier values of type <see cref="double"/> and gets input and expected
    /// <see cref="Size"/> dimensions.
    /// </summary>
    public static IEnumerable<object[]> GetMultiplierAndSizeValues()
    {
        Size maxSizeBox = new(int.MaxValue, int.MaxValue);

        yield return new object[] { 1d, new Size(100, 100), new Size(100, 100) };
        yield return new object[] { 2d, new Size(100, 100), new Size(200, 200) };
        yield return new object[] { 0.5d, new Size(100, 100), new Size(50, 50) };
        yield return new object[] { 0.1d, new Size(100, 100), new Size(10, 10) };
        yield return new object[] { double.MaxValue, new Size(100, 100), maxSizeBox };
        yield return new object[] { double.MaxValue, maxSizeBox, maxSizeBox };
        yield return new object[] { 2d, maxSizeBox, maxSizeBox };
    }
}
