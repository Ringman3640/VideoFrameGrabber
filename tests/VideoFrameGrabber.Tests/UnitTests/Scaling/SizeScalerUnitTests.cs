using FluentAssertions;
using VideoFrameGrabber.Scaling;
using VideoFrameGrabber.Tests.Utilities;

namespace VideoFrameGrabber.Tests.UnitTests.Scaling;

public class SizeScalerUnitTests
{
    // T-1
    [Theory]
    [ClassData(typeof(CommonTestValues.IntPairs.AllPositive))]
    public void Constructor_ValidSizeValues_WidthAndHeightMatchSizeValues(int width, int height)
    {
        CommonTests.Constructor.CorrectlyInitializes(
            constructInstance: () => new SizeScaler(width, height),
            checks: [
                (scaler) => scaler.Width == width,
                (scaler) => scaler.Height == height
            ]
        );
    }

    // T-2
    [Theory]
    [ClassData(typeof(CommonTestValues.IntPairs.ContainsZero))]
    [ClassData(typeof(CommonTestValues.IntPairs.ContainsNegatives))]
    public void Constructor_ZeroOrNegativeValues_ThrowsArgumentOutOfRangeException(int width, int height)
    {
        CommonTests.Constructor.ThrowsException(
            constructInstance: () => new SizeScaler(width, height),
            exceptionType: typeof(ArgumentOutOfRangeException)
        );
    }

    // T-3
    [Theory]
    [ClassData(typeof(CommonTestValues.IntPairs.AllPositive))]
    public void GetScaleParameters_ZeroSize_ReturnedScaleParametersMatchConstructedSizeValues(int width, int height)
    {
        CommonTests.ScaleProvider.GetsCorrectScaleParameters(
            scaler: new SizeScaler(width, height),
            inputSize: new Size(0, 0),
            expectedSize: new Size(width, height)
        );
    }

    // T-4
    [Theory]
    [ClassData(typeof(CommonTestValues.IntPairs.AllPositive))]
    public void GetScaleParameters_MaximumSize_ReturnedScaleParametersMatchConstructedSizeValues(int width, int height)
    {
        CommonTests.ScaleProvider.GetsCorrectScaleParameters(
            scaler: new SizeScaler(width, height),
            inputSize: new Size(int.MaxValue, int.MaxValue),
            expectedSize: new Size(width, height)
        );
    }

    // T-5
    [Theory]
    [ClassData(typeof(CommonTestValues.IntPairs.AllPositive))]
    public void GetScaleParameters_MinimumSize_ReturnedScaleParametersMatchConstructedSizeValues(int width, int height)
    {
        CommonTests.ScaleProvider.GetsCorrectScaleParameters(
            scaler: new SizeScaler(width, height),
            inputSize: new Size(int.MinValue, int.MinValue),
            expectedSize: new Size(width, height)
        );
    }
}
