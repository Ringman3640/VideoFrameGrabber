using FluentAssertions;
using VideoFrameGrabber.Scaling;

namespace VideoFrameGrabber.Tests.UnitTests.Scaling;

public class MultiplyScalerUnitTests
{
    // T-1
    [Theory]
    [MemberData(nameof(GetMultiplierValues))]
    public void Constructor_MultiplierValue_MultiplierMatchesValue(double multiplier)
    {
        MultiplyScaler? multiplyScaler = null;

        multiplyScaler = new(multiplier);

        multiplyScaler.Should().NotBeNull();
        multiplyScaler.Multiplier.Should().Be(multiplier);
    }

    // T-2
    [Theory]
    [MemberData(nameof(GetMultiplierAndSizeValues))]
    public void GetScaleParameters_MultiplierAndInputSizeValues_ResultMatchesExpectedDimensions (
        double multiplier,
        Size inputSize,
        Size expectedSize
    ) {
        MultiplyScaler multiplyScaler = new(multiplier);
        ScaleParameters? scaleParameters = null;

        scaleParameters = multiplyScaler.GetScaleParameters(inputSize.Width, inputSize.Height);

        scaleParameters.Should().NotBeNull();
        scaleParameters.Value.Width.Should().Be(expectedSize.Width);
        scaleParameters.Value.Height.Should().Be(expectedSize.Height);
    }

    /// <summary>
    /// Gets multiplier values of type <see cref="double"/>.
    /// </summary>
    public static IEnumerable<object[]> GetMultiplierValues()
    {
        yield return new object[] { 1d };
        yield return new object[] { 10d };
        yield return new object[] { 9999d };
        yield return new object[] { 0.1d };
        yield return new object[] { 0.001d };
        yield return new object[] { 0.9d };
        yield return new object[] { 0d };
        yield return new object[] { double.MaxValue };
        yield return new object[] { -1d };
        yield return new object[] { -10d };
        yield return new object[] { -9999d };
        yield return new object[] { -0.1d };
        yield return new object[] { -0.001d };
        yield return new object[] { -0.9d };
        yield return new object[] { double.MinValue };
    }

    /// <summary>
    /// Gets multiplier values of type <see cref="double"/> and gets input and expected
    /// <see cref="Size"/> dimensions.
    /// </summary>
    public static IEnumerable<object[]> GetMultiplierAndSizeValues()
    {
        Size maxSizeBox = new(int.MaxValue, int.MaxValue);
        Size zeroSize = new(0, 0);

        yield return new object[] { 1d, new Size(100, 100), new Size(100, 100) };
        yield return new object[] { 2d, new Size(100, 100), new Size(200, 200) };
        yield return new object[] { 0.5d, new Size(100, 100), new Size(50, 50) };
        yield return new object[] { 0.1d, new Size(100, 100), new Size(10, 10) };
        yield return new object[] { 0, new Size(100, 100), zeroSize };
        yield return new object[] { 1d, zeroSize, zeroSize };
        yield return new object[] { 999d, zeroSize, zeroSize };
        yield return new object[] { double.MaxValue, zeroSize, zeroSize };
        yield return new object[] { double.MaxValue, new Size(100, 100), maxSizeBox };
        yield return new object[] { double.MaxValue, maxSizeBox, maxSizeBox };
        yield return new object[] { 2d, maxSizeBox, maxSizeBox };
    }

    /// <summary>
    /// Represents a width and height size.
    /// </summary>
    /// <remarks>
    /// This struct serves as syntax above raw width and height values. It makes the test code
    /// easier to understand by grouping corresponding width and height values.
    /// </remarks>
    public readonly struct Size
    {
        public int Width { get; init; }
        public int Height { get; init; }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
