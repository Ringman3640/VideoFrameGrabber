using FluentAssertions;
using VideoFrameGrabber.Scaling;

namespace VideoFrameGrabber.Tests.UnitTests.Scaling;

public class SizeScalerUnitTests
{
    // T-1
    [Theory]
    [MemberData(nameof(GetWidthHeightValues))]
    public void Constructor_ValidSizeValues_WidthAndHeightMatchSizeValues(int width, int height)
    {
        SizeScaler? scaler = null;

        scaler = new(width, height);

        scaler.Should().NotBeNull();
        scaler.Width.Should().Be(width);
        scaler.Height.Should().Be(height);
    }

    // T-2
    [Theory]
    [InlineData(0, 0)]
    [InlineData(100, 0)]
    [InlineData(0, 100)]
    [InlineData(-1, 100)]
    [InlineData(100, -1)]
    [InlineData(int.MinValue, int.MaxValue)]
    [InlineData(int.MaxValue, int.MinValue)]
    [InlineData(int.MinValue, int.MinValue)]
    public void Constructor_ZeroOrNegativeValues_ThrowsArgumentOutOfRangeException(int width, int height)
    {
        SizeScaler? scaler = null;
        Exception? exception = null;

        try
        {
            scaler = new(width, height);
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
    [MemberData(nameof(GetWidthHeightValues))]
    public void GetScaleParameters_ZeroSize_ReturnedScaleParametersMatchConstructedSizeValues(int width, int height)
    {
        SizeScaler scaler = new(width, height);
        ScaleParameters? scaleParameters = null;

        scaleParameters = scaler.GetScaleParameters(0, 0);

        scaleParameters.Should().NotBeNull();
        scaleParameters.Value.Width.Should().Be(width);
        scaleParameters.Value.Height.Should().Be(height);
    }

    // T-4
    [Theory]
    [MemberData(nameof(GetWidthHeightValues))]
    public void GetScaleParameters_MaximumSize_ReturnedScaleParametersMatchConstructedSizeValues(int width, int height)
    {
        SizeScaler scaler = new(width, height);
        ScaleParameters? scaleParameters = null;

        scaleParameters = scaler.GetScaleParameters(int.MaxValue, int.MaxValue);

        scaleParameters.Should().NotBeNull();
        scaleParameters.Value.Width.Should().Be(width);
        scaleParameters.Value.Height.Should().Be(height);
    }

    // T-5
    [Theory]
    [MemberData(nameof(GetWidthHeightValues))]
    public void GetScaleParameters_MinimumSize_ReturnedScaleParametersMatchConstructedSizeValues(int width, int height)
    {
        SizeScaler scaler = new(width, height);
        ScaleParameters? scaleParameters = null;

        scaleParameters = scaler.GetScaleParameters(int.MinValue, int.MinValue);

        scaleParameters.Should().NotBeNull();
        scaleParameters.Value.Width.Should().Be(width);
        scaleParameters.Value.Height.Should().Be(height);
    }

    /// <summary>
    /// Gets test data that specifies width and height values.
    /// </summary>
    public static IEnumerable<object[]> GetWidthHeightValues()
    {
        yield return new object[] { 1, 1 };
        yield return new object[] { 1000, 1000 };
        yield return new object[] { 1, 99999 };
        yield return new object[] { 99999, 1 };
        yield return new object[] { 99999, 99999 };
        yield return new object[] { int.MaxValue, 1 };
        yield return new object[] { 1, int.MaxValue };
        yield return new object[] { int.MaxValue, int.MaxValue };
    }
}
