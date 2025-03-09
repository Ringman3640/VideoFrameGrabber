using FluentAssertions;
using VideoFrameGrabber.Scaling;
using VideoFrameGrabber.UnitTests.Utilities;

namespace VideoFrameGrabber.UnitTests.UnitTests.ScalingTests;

public class BoundsScalerUnitTests
{
    // T-1
    [Theory]
    [ClassData(typeof(CommonTestValues.IntPairs.AllPositive))]
    public void Constructor_PositiveBoundsValues_WidthAndHeightBoundsMatchValues(
        int width,
        int height
    ) {
        CommonTests.Constructor.CorrectlyInitializes(
            constructInstance: () => new BoundsScaler(width, height),
            checks: [
                (scaler) => scaler.WidthBounds == width,
                (scaler) => scaler.HeightBounds == height
            ]
        );
    }

    // T-2
    [Theory]
    [ClassData(typeof(CommonTestValues.IntPairs.ContainsZero))]
    [ClassData(typeof(CommonTestValues.IntPairs.ContainsNegatives))]
    public void Constructor_NegativeAndZeroBoundsValues_ThrowsArgumentOutOfRangeException(
        int width,
        int height
    ) {
        CommonTests.Constructor.ThrowsException(
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
        CommonTests.ScaleProvider.GetsCorrectScaleParameters(
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
