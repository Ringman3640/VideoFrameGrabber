using FluentAssertions;
using VideoFrameGrabber.Cropping;
using VideoFrameGrabber.Tests.UnitTests.Scaling;
using VideoFrameGrabber.Tests.Utilities;

namespace VideoFrameGrabber.Tests.UnitTests.Cropping;

public class CustomCropperUnitTests
{
    // T-1
    [Theory]
    [MemberData(nameof(Constructor_ValidCropFunction_Succeeds_Data))]
    public void Constructor_ValidCropFunction_Succeeds(Func<int, int, CropParameters> cropFunction)
    {
        CommonTests.Constructor.CorrectlyInitializes(
            constructInstance: () => new CustomCropper(cropFunction),
            checks: []
        );
    }
    public static IEnumerable<object[]> Constructor_ValidCropFunction_Succeeds_Data()
    {
        yield return new object[]
        {
            (int width, int height) => new CropParameters(width - 10, height - 10)
        };

        yield return new object[]
        {
            (int width, int height) => new CropParameters(10, 10)
        };

        yield return new object[]
        {
            (int width, int height) =>
            {
                Random rand = new();
                int newWidth = rand.Next(CropProvider.MinimumCropDimensionSize, width);
                int newHeight = rand.Next(CropProvider.MinimumCropDimensionSize, height);

                return new CropParameters(newWidth, newHeight);
            }
        };
    }

    // T-2
    [Fact]
    public void Contstructor_Null_ThrowsArgumentNullException()
    {
        CommonTests.Constructor.ThrowsException(
            constructInstance: () => new CustomCropper(null!),
            exceptionType: typeof(ArgumentNullException)
        );
    }

    // T-3
    [Theory]
    [MemberData(nameof(GetCropParameters_InputSizeAndValidCropFunction_ReturnsExpectedCropParameters_Data))]
    public void GetCropParameters_InputSizeAndValidCropFunction_ReturnsExpectedCropParameters(
        Size inputSize,
        Func<int, int, CropParameters> cropFunction
    ) {
        CropParameters rawCropParameters = cropFunction(inputSize.Width, inputSize.Height);
        CropParameters expectedCropParameters = new(
            width: Math.Min(inputSize.Width, rawCropParameters.Width),
            height: Math.Min(inputSize.Height, rawCropParameters.Height),
            x: rawCropParameters.X,
            y: rawCropParameters.Y
        );

        CommonTests.CropProvider.GetsCorrectCropParameters(
            cropper: new CustomCropper(cropFunction),
            inputSize: inputSize,
            expectedCrop: expectedCropParameters
        );
    }
    public static IEnumerable<object[]> GetCropParameters_InputSizeAndValidCropFunction_ReturnsExpectedCropParameters_Data()
    {
        int minLength = CropProvider.MinimumCropDimensionSize;

        yield return new object[]
        {
            new Size(100, 100),
            (int width, int height) => new CropParameters(100, 100, 1, 1)
        };

        yield return new object[]
        {
            new Size(1000, 100),
            (int width, int height) => new CropParameters(10000, 10000, 1, 1)
        };

        yield return new object[]
        {
            new Size(int.MaxValue, int.MaxValue),
            (int width, int height) => new CropParameters(minLength, int.MaxValue, -100, -100)
        };

        yield return new object[]
        {
            new Size(int.MaxValue, int.MaxValue),
            (int width, int height) => new CropParameters(int.MaxValue, minLength, -9999, -9999)
        };

        yield return new object[]
        {
            new Size(int.MaxValue, int.MaxValue),
            (int width, int height) => new CropParameters(width / 2, height / 2, 99999, 99999)
        };
    }

    [Theory]
    [MemberData(nameof(GetCropParameters_InputSizeAndInvalidProducingCropFunction_ReturnsExpectedCropParameters_Data))]
    public void GetCropParameters_InputSizeAndInvalidProducingCropFunction_ReturnsExpectedCropParameters(
        Size inputSize,
        Func<int, int, CropParameters> cropFunction
    ) {
        CropParameters rawCropParameters = cropFunction(inputSize.Width, inputSize.Height);
        CropParameters expectedCropParameters = new(
            width: Math.Clamp(rawCropParameters.Width, CropProvider.MinimumCropDimensionSize, inputSize.Width),
            height: Math.Clamp(rawCropParameters.Height, CropProvider.MinimumCropDimensionSize, inputSize.Height),
            x: rawCropParameters.X,
            y: rawCropParameters.Y
        );

        CommonTests.CropProvider.GetsCorrectCropParameters(
            cropper: new CustomCropper(cropFunction),
            inputSize: inputSize,
            expectedCrop: expectedCropParameters
        );
    }
    public static IEnumerable<object[]> GetCropParameters_InputSizeAndInvalidProducingCropFunction_ReturnsExpectedCropParameters_Data()
    {
        int minLength = CropProvider.MinimumCropDimensionSize;

        yield return new object[]
        {
            new Size(100, 100),
            (int width, int height) => new CropParameters(0, 0)
        };

        yield return new object[]
        {
            new Size(100, 100),
            (int width, int height) => new CropParameters(-1, -1)
        };

        yield return new object[]
        {
            new Size(100, 100),
            (int width, int height) => new CropParameters(int.MinValue, int.MinValue)
        };

        yield return new object[]
        {
            new Size(100, 100),
            (int width, int height) => new CropParameters(minLength - 1, minLength - 1)
        };

        yield return new object[]
        {
            new Size(100, 100),
            (int width, int height) => new CropParameters(9999, minLength - 1)
        };

        yield return new object[]
        {
            new Size(100, 100),
            (int width, int height) => new CropParameters(minLength - 1, 9999)
        };
    }
}
