using VideoFrameGrabber.Cropping;
using VideoFrameGrabber.Tests.UnitTests.Scaling;
using VideoFrameGrabber.Tests.Utilities;

namespace VideoFrameGrabber.Tests.UnitTests.Cropping;

public class SizeCropperUnitTests : CropProviderUnitTestBase
{
    // T-1
    [Theory]
    [ClassData(typeof(CommonTestValues.IntPairs.AllPositive))]
    public void Constructor_ValidSizeValues_SizeValuesMatch(
        int width,
        int height
    ) {
        if (width < CropProvider.MinimumCropDimensionSize)
        {
            width = CropProvider.MinimumCropDimensionSize;
        }
        if (height < CropProvider.MinimumCropDimensionSize)
        {
            height = CropProvider.MinimumCropDimensionSize;
        }

        CommonTests.Constructor.CorrectlyInitializes(
            constructInstance: () => new SizeCropper(width, height),
            checks: [
                (cropper) => cropper.Width == width,
                (cropper) => cropper.Height == height
            ]
        );
    }

    // T-2
    [Theory]
    [ClassData(typeof(CommonTestValues.IntPairs.ContainsNegatives))]
    public void Constructor_ZeroOrNegativeSizeValues_ThrowsArgumentOutOfRangeException(
        int width,
        int height
    ) {
        CommonTests.Constructor.ThrowsException(
            constructInstance: () => new SizeCropper(width, height),
            exceptionType: typeof(ArgumentOutOfRangeException)
        );
    }

    // T-3
    [Theory]
    [MemberData(nameof(GetVariablePositiveInts), parameters: 4)]
    public void Constructor_ValidSizeAndOffsetValues_SizeAndOffsetValuesMatch(
        int width,
        int height,
        int x,
        int y
    ) {
        if (width < CropProvider.MinimumCropDimensionSize)
        {
            width = CropProvider.MinimumCropDimensionSize;
        }
        if (height < CropProvider.MinimumCropDimensionSize)
        {
            height = CropProvider.MinimumCropDimensionSize;
        }

        CommonTests.Constructor.CorrectlyInitializes(
            constructInstance: () => new SizeCropper(width, height, x, y),
            checks: [
                (cropper) => cropper.Width == width,
                (cropper) => cropper.Height == height,
                (cropper) => cropper.X == x,
                (cropper) => cropper.Y == y
            ]
        );
    }

    // T-4
    [Theory]
    [MemberData(nameof(GetVariableNegativeWithZeroInts), parameters: 4)]
    public void Constructor_ZeroOrNegativeSizeAndOffsetValues_ThrowsArgumentOutOfRangeException(
        int width,
        int height,
        int x,
        int y
    ) {
        CommonTests.Constructor.ThrowsException(
            constructInstance: () => new SizeCropper(width, height, x, y),
            exceptionType: typeof(ArgumentOutOfRangeException)
        );
    }

    // T-5
    [Theory]
    [InlineData(100, 100, CropAlign.Center)]
    [InlineData(100, 100, CropAlign.TopLeft)]
    [InlineData(100, 100, CropAlign.TopRight)]
    [InlineData(100, 100, CropAlign.BottomLeft)]
    [InlineData(100, 100, CropAlign.BottomRight)]
    public void Constructor_PositiveSizeAndCropAlignValues_SizeAndAlignValuesMatchAndOffsetIsZero(
        int width,
        int height,
        CropAlign align
    ) {
        CommonTests.Constructor.CorrectlyInitializes(
            constructInstance: () => new SizeCropper(width, height, align),
            checks: [
                (cropper) => cropper.Width == width,
                (cropper) => cropper.Height == height,
                (cropper) => cropper.Align == align,
                (cropper) => cropper.X == 0,
                (cropper) => cropper.Y == 0,
            ]
        );
    }

    // T-6
    [Theory]
    [MemberData(nameof(GetPositiveCropSizeAndInputSizeValues))]
    public void GetCropParameters_PositiveCropSizeAndInputSizeValues_ResultMatchesCropSize(
        Size cropSize,
        Size inputSize
    ) {
        CommonTests.CropProvider.GetsCorrectCropParameters(
            cropper: new SizeCropper(cropSize.Width, cropSize.Height),
            inputSize: inputSize,
            expectedSize: cropSize
        );
    }

    // T-7
    [Theory]
    [MemberData(nameof(GetPositiveCropSizeAndInputSizeAndOffsetValues))]
    public void GetCropParameters_PositiveCropSizeAndOffsetAndInputSizeValues_ResultMatchesCropSize(
        Size cropSize,
        Size inputSize,
        int xOffset,
        int yOffset
    ) {
        CommonTests.CropProvider.GetsCorrectCropParameters(
            cropper: new SizeCropper(cropSize.Width, cropSize.Height),
            inputSize: inputSize,
            expectedCrop: new CropParameters(cropSize.Width, cropSize.Height, xOffset, yOffset)
        );
    }

    // T-8
    [Theory]
    [MemberData(nameof(GetPositiveCropSizeAndInputSizeAndCropAlignValues))]
    public void GetCropParameters_PositiveCropSizeAndCropAlignAndInputValues_ResultMatchesCropSizeAndExpectedOffsetFromCropProvider(
        Size cropSize,
        Size inputSize,
        CropAlign align
    ) {
        CropProvider.CropOffset expectedOffset = CropProvider.GetCropOffsetFromAlign(
            cropSize.Width, cropSize.Height, inputSize.Width, inputSize.Height, align);

        CommonTests.CropProvider.GetsCorrectCropParameters(
            cropper: new SizeCropper(cropSize.Width, cropSize.Height, align),
            inputSize: inputSize,
            expectedCrop: new CropParameters(cropSize.Width, cropSize.Height, expectedOffset.X, expectedOffset.Y)
        );
    }

    public static IEnumerable<object[]> GetPositiveCropSizeAndInputSizeValues()
    {
        yield return new object[] { new Size(100, 100), new Size(1000, 1000) };
        yield return new object[] { new Size(10, 10), new Size(1000, 1000) };
        yield return new object[] { new Size(1, 1), new Size(1000, 1000) };
        yield return new object[] { new Size(100, 100), new Size(100, 100) };
        yield return new object[] { new Size(1, 1), new Size(100, 100) };
        yield return new object[] { new Size(int.MaxValue, int.MaxValue), new Size(int.MaxValue, int.MaxValue) };
        yield return new object[] { new Size(100, 100), new Size(int.MaxValue, int.MaxValue) };
        yield return new object[] { new Size(10, 10), new Size(int.MaxValue, int.MaxValue) };
        yield return new object[] { new Size(1, 1), new Size(int.MaxValue, int.MaxValue) };
    }

    public static IEnumerable<object[]> GetPositiveCropSizeAndInputSizeAndOffsetValues()
    {
        yield return new object[] { new Size(100, 100), new Size(1000, 1000), 0, 0 };
        yield return new object[] { new Size(100, 100), new Size(1000, 1000), 100, 0 };
        yield return new object[] { new Size(100, 100), new Size(1000, 1000), 0, 100 };
        yield return new object[] { new Size(100, 100), new Size(1000, 1000), 100, 100 };
    }

    public static IEnumerable<object[]> GetPositiveCropSizeAndInputSizeAndCropAlignValues()
    {
        List<Size[]> cropSizesAndInputSizes =
        [
            [new Size(100, 100), new Size(500, 500)],
            [new Size(1, 100), new Size(500, 500)],
            [new Size(100, 1), new Size(500, 500)],
            [new Size(1, 1), new Size(500, 500)],
            [new Size(1, 500), new Size(500, 500)],
            [new Size(500, 1), new Size(500, 500)],
            [new Size(500, 500), new Size(500, 500)],
            [new Size(500, 500), new Size(520, 520)],
        ];

        foreach (Size[] sizes in cropSizesAndInputSizes)
        {
            foreach (CropAlign align in (CropAlign[])Enum.GetValues(typeof(CropAlign)))
            {
                yield return new object[] { sizes[0], sizes[1], align };
            }
        }
    }
}
