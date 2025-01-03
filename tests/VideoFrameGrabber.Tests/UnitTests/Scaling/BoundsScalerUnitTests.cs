using FluentAssertions;
using VideoFrameGrabber.Scaling;
using VideoFrameGrabber.Tests.Utilities;

namespace VideoFrameGrabber.Tests.UnitTests.Scaling;

public class BoundsScalerUnitTests
{
    // T-1
    [Theory]
    [InlineData(1, 1)]
    [InlineData(100, 100)]
    [InlineData(99999, 99999)]
    [InlineData(int.MaxValue, int.MaxValue)]
    public void Constructor_PositiveBoundsValues_WidthAndHeightBoundsMatchValues(
        int width,
        int height
    ) {
        CommonTests.ConstructorTests.CorrectlyInitializes(
            constructInstance: () => new BoundsScaler(width, height),
            checks: [
                (scaler) => scaler.WidthBounds == width,
                (scaler) => scaler.HeightBounds == height
            ]
        );
    }

    // T-2
    [Theory]
    [InlineData(0, 0)]
    [InlineData(-100, -100)]
    [InlineData(-1, -1)]
    [InlineData(-99999, -99999)]
    [InlineData(int.MinValue, int.MinValue)]
    [InlineData(100, -100)]
    [InlineData(1, -1)]
    [InlineData(99999, -99999)]
    [InlineData(int.MaxValue, int.MinValue)]
    [InlineData(-100, 100)]
    [InlineData(-1, 1)]
    [InlineData(-99999, 99999)]
    [InlineData(int.MinValue, int.MaxValue)]
    public void Constructor_NegativeAndZeroBoundsValues_ThrowsArgumentOutOfRangeException(
        int width,
        int height
    ) {
        CommonTests.ConstructorTests.ThrowsException(
            constructInstance: () => new BoundsScaler(width, height),
            exceptionType: typeof(ArgumentOutOfRangeException)
        );
    }

    // T-3
    [Theory]
    [MemberData(nameof(GetBoundsAndInputAndExpectedSizeValues))]
    public void GetScaleParameters_BoundsSizeAndInputSizeValues_ResultMatchesExpectedDimensions(
        Size boundsSize,
        Size inputSize,
        Size expectedSize
    ) {
        CommonTests.ScaleProviderTests.GetsCorrectScaleParameters(
            scaler: new BoundsScaler(boundsSize.Width, boundsSize.Height),
            inputSize: inputSize,
            expectedSize: expectedSize
        );
    }

    /// <summary>
    /// Gets a set of trio <see cref="Size"/> values to test the
    /// <see cref="BoundsScaler.GetScaleParameters(int, int)"/> method.
    /// </summary>
    public static IEnumerable<object[]> GetBoundsAndInputAndExpectedSizeValues()
    {
        int max = int.MaxValue;

        yield return new object[] { new Size(100, 100), new Size(10, 10), new Size(10, 10) };
        yield return new object[] { new Size(100, 100), new Size(100, 100), new Size(100, 100) };
        yield return new object[] { new Size(max, max), new Size(max, max), new Size(max, max) };
        yield return new object[] { new Size(10, 10), new Size(100, 100), new Size(10, 10) };
        yield return new object[] { new Size(10, 10), new Size(max, max), new Size(10, 10) };
        yield return new object[] { new Size(100, 100), new Size(200, 100), new Size(100, 50) };
        yield return new object[] { new Size(10, 10), new Size(100, 10), new Size(10, 1) };
        yield return new object[] { new Size(1, 1), new Size(999, 1), new Size(1, 1) };
        yield return new object[] { new Size(100, 100), new Size(100, 200), new Size(50, 100) };
        yield return new object[] { new Size(10, 10), new Size(10, 100), new Size(1, 10) };
        yield return new object[] { new Size(1, 1), new Size(1, 999), new Size(1, 1) };
    }
}
