using VideoFrameGrabber.Cropping;
using VideoFrameGrabber.Tests.UnitTests.Scaling;
using VideoFrameGrabber.Tests.Utilities;

namespace VideoFrameGrabber.Tests.UnitTests.Cropping;

public class SizeCropperUnitTests : CropProviderUnitTestBase
{
    [Theory]
    [ClassData(typeof(CommonTestValues.IntPairs.AllPositive))]
    public void Constructor_PositiveSizeValues_SizeValuesMatch(
        int width,
        int height
    ) {
        CommonTests.Constructor.CorrectlyInitializes(
            constructInstance: () => new SizeCropper(width, height),
            checks: [
                (cropper) => cropper.Width == width,
                (cropper) => cropper.Height == height
            ]
        );
    }

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

    [Theory]
    [MemberData(nameof(GetVariablePositiveInts), parameters: 4)]
    public void Constructor_PositiveSizeAndOffsetValues_SizeAndOffsetValuesMatch(
        int width,
        int height,
        int x,
        int y
    ) {
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

    [Theory]
    [ClassData(typeof(CommonTestValues.IntPairs.ContainsNegatives))]
    [MemberData(nameof(AppendIntPairsContainingZero), parameters: new int[] { 1, 1 })]
    [MemberData(nameof(AppendIntPairsContainingNegatives), parameters: new int[] { 1, 1 })]
    public void Constructor_PositiveSizeAndNegativeOrZeroOffsetValues_ThrowsArgumentOutOfRangeException(
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

    [Theory]
    [InlineData(1, 1, CropAlign.Center)]
    [InlineData(1, 1, CropAlign.TopLeft)]
    [InlineData(1, 1, CropAlign.TopRight)]
    [InlineData(1, 1, CropAlign.BottomLeft)]
    [InlineData(1, 1, CropAlign.BottomRight)]
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

    [Theory]
    [MemberData(nameof(GetPositiveCropSizeAndInputSizeValues))]
    public void GetCropParameters_PositiveSizeAndInputValues_ResultMatchesCropSize(
        Size cropSize,
        Size inputSize
    ) {
        CommonTests.CropProvider.GetsCorrectCropParameters(
            cropper: new SizeCropper(cropSize.Width, cropSize.Height),
            inputSize: inputSize,
            expectedSize: cropSize
        );
    }

    [Theory]
    [MemberData(nameof(GetPositiveCropSizeAndInputSizeAndOffsetValues))]
    public void GetCropParameters_PositiveSizeAndOffsetAndInputValues_ResultMatchesCropSize(
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

    [Theory]
    [MemberData(nameof(GetPositiveCropSizeAndInputSizeAndCropAlignValues))]
    public void GetCropParameters_PositiveSizeAndCropAlignAndInputValues_ResultMatchesCropSize(
        Size cropSize,
        Size inputSize,
        CropAlign align
    ) {
        throw new NotImplementedException();
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
        yield return new object[] { new Size(100, 100), new Size(1000, 1000), CropAlign.TopLeft };
        yield return new object[] { new Size(100, 100), new Size(1000, 1000), CropAlign.TopRight };
        yield return new object[] { new Size(100, 100), new Size(1000, 1000), CropAlign.Center };
        yield return new object[] { new Size(100, 100), new Size(1000, 1000), CropAlign.BottomLeft };
        yield return new object[] { new Size(100, 100), new Size(1000, 1000), CropAlign.BottomRight };
    }
}
