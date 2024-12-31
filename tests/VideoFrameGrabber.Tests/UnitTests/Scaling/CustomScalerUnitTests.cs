using FluentAssertions;
using VideoFrameGrabber.Scaling;

namespace VideoFrameGrabber.Tests.UnitTests.Scaling;

public class CustomScalerUnitTests
{
    // T-1
    [Theory]
    [MemberData(nameof(GetScaleFunctionValues))]
    public void Constructor_ValidScaleFunction_Succeeds(Func<int, int, ScaleParameters> scaleFunction)
    {
        CustomScaler? scaler = null;

        try
        {
            scaler = new(scaleFunction);
        }
        catch { }

        scaler.Should().NotBeNull();
    }

    // T-2
    [Fact]
    public void Constructor_NullValue_ThrowsArgumentNullException()
    {
        CustomScaler? scaler = null;
        Exception? exception = null;

        try
        {
            scaler = new(null!);
        }
        catch (Exception except)
        {
            exception = except;
        }

        scaler.Should().BeNull();
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    // T-3
    [Theory]
    [MemberData(nameof(GetScaleFunctionValues))]
    public void GetScaleParameters_ScaleFunctionAndSquareInputSizeValues_ResultMatchesResultFromScaleFunction(
        Func<int, int, ScaleParameters> scaleFunction
    ) {
        int inputWidth = 100;
        int inputHeight = 100;
        CustomScaler scaler = new(scaleFunction);
        ScaleParameters expectedResult = scaleFunction(inputWidth, inputHeight);
        ScaleParameters? scaleParameters = null;

        try
        {
            scaleParameters = scaler.GetScaleParameters(inputWidth, inputHeight);
        }
        catch { }

        scaleParameters.Should().NotBeNull();
        scaleParameters!.Value.Width.Should().Be(expectedResult.Width);
        scaleParameters!.Value.Height.Should().Be(expectedResult.Height);
    }

    [Fact]
    public void GetScaleParameters_SquareInputSizeWithScaleFunctionReturningZeroScale_ReturnsOneByOneScale()
    {
        int inputWidth = 100;
        int inputHeight = 100;
        CustomScaler scaler = new((inWidth, inHeight) => new ScaleParameters(0, 0));
        ScaleParameters? scaleParameters = null;

        try
        {
            scaleParameters = scaler.GetScaleParameters(inputWidth, inputHeight);
        }
        catch { }

        scaleParameters.Should().NotBeNull();
        scaleParameters!.Value.Width.Should().Be(1);
        scaleParameters!.Value.Height.Should().Be(1);
    }

    [Fact]
    public void GetScaleParameters_SquareInputSizeWithScaleFunctionReturningNegativeScale_ReturnsOneByOneScale()
    {
        int inputWidth = 100;
        int inputHeight = 100;
        CustomScaler scaler = new((inWidth, inHeight) => new ScaleParameters(-1, -1));
        ScaleParameters? scaleParameters = null;

        try
        {
            scaleParameters = scaler.GetScaleParameters(inputWidth, inputHeight);
        }
        catch { }

        scaleParameters.Should().NotBeNull();
        scaleParameters!.Value.Width.Should().Be(1);
        scaleParameters!.Value.Height.Should().Be(1);
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
                return new ScaleParameters(0, 0);
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
