using FluentAssertions;
using VideoFrameGrabber.Scaling;
using VideoFrameGrabber.Tests.Utilities;

namespace VideoFrameGrabber.Tests.UnitTests.Scaling;

public class SizeScalerUnitTests
{
    // T-1
    [Theory]
    [MemberData(nameof(GetWidthHeightValues))]
    public void Constructor_ValidSizeValues_WidthAndHeightMatchSizeValues(int width, int height)
    {
        CommonTests.ConstructorTests.CorrectlyInitializes(
            constructInstance: () => new SizeScaler(width, height),
            checks: [
                (scaler) => scaler.Width == width,
                (scaler) => scaler.Height == height
            ]
        );
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
        CommonTests.ConstructorTests.ThrowsException(
            constructInstance: () => new SizeScaler(width, height),
            exceptionType: typeof(ArgumentOutOfRangeException)
        );
    }

    // T-3
    [Theory]
    [MemberData(nameof(GetWidthHeightValues))]
    public void GetScaleParameters_ZeroSize_ReturnedScaleParametersMatchConstructedSizeValues(int width, int height)
    {
        CommonTests.ScaleProviderTests.GetsCorrectScaleParameters(
            scaler: new SizeScaler(width, height),
            inputSize: new Size(0, 0),
            expectedSize: new Size(width, height)
        );
    }

    // T-4
    [Theory]
    [MemberData(nameof(GetWidthHeightValues))]
    public void GetScaleParameters_MaximumSize_ReturnedScaleParametersMatchConstructedSizeValues(int width, int height)
    {
        CommonTests.ScaleProviderTests.GetsCorrectScaleParameters(
            scaler: new SizeScaler(width, height),
            inputSize: new Size(int.MaxValue, int.MaxValue),
            expectedSize: new Size(width, height)
        );
    }

    // T-5
    [Theory]
    [MemberData(nameof(GetWidthHeightValues))]
    public void GetScaleParameters_MinimumSize_ReturnedScaleParametersMatchConstructedSizeValues(int width, int height)
    {
        CommonTests.ScaleProviderTests.GetsCorrectScaleParameters(
            scaler: new SizeScaler(width, height),
            inputSize: new Size(int.MinValue, int.MinValue),
            expectedSize: new Size(width, height)
        );
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
