using FluentAssertions;
using VideoFrameGrabber.Scaling;

namespace VideoFrameGrabber.Tests.UnitTests.Scaling;

public  class HeightScalerUnitTests
{
    // T-1
    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(99999)]
    [InlineData(int.MaxValue)]
    public void Constructor_ValidHeightValue_HeightMatchesValue(int height)
    {
        HeightScaler? scaler = null;

        try
        {
            scaler = new(height);
        }
        catch { }

        scaler.Should().NotBeNull();
        scaler!.Height.Should().Be(height);
    }

    // T-2
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-99999)]
    [InlineData(int.MinValue)]
    public void Constructor_ZeroOrNegativeValue_ThrowsArgumentOutOfRangeException(int height)
    {
        HeightScaler? scaler = null;
        Exception? exception = null;

        try
        {
            scaler = new(height);
        }
        catch (Exception except)
        {
            exception = except;
        }

        scaler.Should().BeNull();
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    // T-3
    [Theory]
    [MemberData(nameof(GetHeightAndInputAndExpectedValues))]
    public void GetScaleParameters_HeightAndInputSizeValues_ResultMatchesExpectedSize(
        int height,
        Size inputSize,
        Size expectedSize
    )
    {
        HeightScaler scaler = new(height);
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
    /// Gets a height value, an input <see cref="Size"/> value, and an expected <see cref="Size"/>
    /// value. Use to test <see cref="HeightScaler.GetScaleParameters(int, int)"/>.
    /// </summary>
    public static IEnumerable<object[]> GetHeightAndInputAndExpectedValues()
    {
        int max = int.MaxValue;

        yield return new object[] { 100, new Size(1, 100), new Size(1, 100) };
        yield return new object[] { 100, new Size(50, 100), new Size(50, 100) };
        yield return new object[] { 100, new Size(50, 50), new Size(100, 100) };
        yield return new object[] { 100, new Size(50, 10), new Size(500, 100) };
        yield return new object[] { 100, new Size(50, 1), new Size(5000, 100) };
        yield return new object[] { 1, new Size(10, 10), new Size(1, 1) };
        yield return new object[] { 1, new Size(50, 10), new Size(5, 1) };
        yield return new object[] { 1, new Size(5000, 10), new Size(500, 1) };
        yield return new object[] { 1, new Size(max, max), new Size(1, 1) };
        yield return new object[] { max, new Size(1, 1), new Size(max, max) };
        yield return new object[] { max, new Size(max, max), new Size(max, max) };
        yield return new object[] { max, new Size(max, 1), new Size(max, max) };
        yield return new object[] { max, new Size(max, 100), new Size(max, max) };
        yield return new object[] { max, new Size(max, 999999), new Size(max, max) };
    }
}
