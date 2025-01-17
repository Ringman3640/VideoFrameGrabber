using FluentAssertions;
using VideoFrameGrabber.Cropping;
using VideoFrameGrabber.Tests.UnitTests.Scaling;

namespace VideoFrameGrabber.Tests.UnitTests.Cropping;

public class CropProviderUnitTests
{
    // T-1
    [Fact]
    public void GetCropOffsetFromAlign_AllCropAlignValues_DoesNotThrowException()
    {
        CropAlign[] alignValues = (CropAlign[])Enum.GetValues(typeof(CropAlign));
        foreach (CropAlign align in alignValues)
        {
            int cropWidth = 0;
            int cropHeight = 0;
            int inputWidth = 0;
            int inputHeight = 0;
            Exception? exception = null;

            try
            {
                _ = CropProvider.GetCropOffsetFromAlign(cropWidth, cropHeight, inputWidth, inputHeight, align);
            }
            catch (Exception except)
            {
                exception = except;
            }

            exception.Should().BeNull();
        }
    }

    public void GetCropOffsetFromAlign_TopLeftAlignAndCropAndInputValues_ReturnsExpectedOffsetValues(
        CropAlign align,
        Size cropSize,
        Size inputSize,
        int expectedX,
        int expectedY
    ) {

    }
}
