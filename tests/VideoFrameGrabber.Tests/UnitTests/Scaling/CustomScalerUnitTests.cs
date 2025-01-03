using FluentAssertions;
using VideoFrameGrabber.Scaling;
using VideoFrameGrabber.Tests.Utilities;

namespace VideoFrameGrabber.Tests.UnitTests.Scaling;

public class CustomScalerUnitTests
{
    // T-1
    [Theory]
    [MemberData(nameof(GetScaleFunctionValues))]
    public void Constructor_ValidScaleFunction_Succeeds(Func<int, int, ScaleParameters> scaleFunction)
    {
        CommonTests.ConstructorTests.CorrectlyInitializes(
            constructInstance: () => new CustomScaler(scaleFunction),
            checks: []
        );
    }

    // T-2
    [Fact]
    public void Constructor_NullValue_ThrowsArgumentNullException()
    {
        CommonTests.ConstructorTests.ThrowsException(
            constructInstance: () => new CustomScaler(null!),
            exceptionType: typeof(ArgumentNullException)
        );
    }

    // T-3
    [Theory]
    [MemberData(nameof(GetScaleFunctionValues))]
    public void GetScaleParameters_ScaleFunctionAndSquareInputSizeValues_ResultMatchesResultFromScaleFunction(
        Func<int, int, ScaleParameters> scaleFunction
    ) {
        int inputWidth = 100;
        int inputHeight = 100;
        ScaleParameters expectedResult = scaleFunction(inputWidth, inputHeight);

        CommonTests.ScaleProviderTests.GetsCorrectScaleParameters(
            scaler: new CustomScaler(scaleFunction),
            inputWidth: inputWidth,
            inputHeight: inputHeight,
            expectedScale: expectedResult
        );
    }

    [Fact]
    public void GetScaleParameters_SquareInputSizeWithScaleFunctionReturningZeroScale_ReturnsOneByOneScale()
    {
        CustomScaler scaler = new((inWidth, inHeight) => new ScaleParameters(0, 0));
        int inputWidth = 100;
        int inputHeight = 100;
        ScaleParameters expectedResult = new(1, 1);

        CommonTests.ScaleProviderTests.GetsCorrectScaleParameters(
            scaler: scaler,
            inputWidth: inputWidth,
            inputHeight: inputHeight,
            expectedScale: expectedResult
        );
    }

    [Fact]
    public void GetScaleParameters_SquareInputSizeWithScaleFunctionReturningNegativeScale_ReturnsOneByOneScale()
    {
        CustomScaler scaler = new((inWidth, inHeight) => new ScaleParameters(-1, -1));
        int inputWidth = 100;
        int inputHeight = 100;
        ScaleParameters expectedResult = new(1, 1);

        CommonTests.ScaleProviderTests.GetsCorrectScaleParameters(
            scaler: scaler,
            inputWidth: inputWidth,
            inputHeight: inputHeight,
            expectedScale: expectedResult
        );
    }

    /// <summary>
    /// Gets scale provider callback methods. Use to test
    /// <see cref="CustomScaler.CustomScaler(Func{int, int, ScaleParameters})"/>.
    /// </summary>
    public static IEnumerable<object[]> GetScaleFunctionValues()
    {
        yield return new object[] 
        {
            (int inWidth, int inHeight) =>
            {
                return new ScaleParameters(1, 1);
            }
        };

        yield return new object[]
        {
            (int inWidth, int inHeight) =>
            {
                return new ScaleParameters(inWidth, inHeight);
            }
        };

        yield return new object[]
        {
            (int inWidth, int inHeight) =>
            {
                FitBoundsScaler fitBoundsScaler = new(100, 100);
                return fitBoundsScaler.GetScaleParameters(inWidth, inHeight);
            }
        };
    }
}
