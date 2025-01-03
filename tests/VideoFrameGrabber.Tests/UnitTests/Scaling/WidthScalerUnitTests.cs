using FluentAssertions;
using VideoFrameGrabber.Scaling;
using VideoFrameGrabber.Tests.Utilities;

namespace VideoFrameGrabber.Tests.UnitTests.Scaling;

public class WidthScalerUnitTests
{
    // T-1
    [Theory]
    [ClassData(typeof(CommonTestValues.SingleInts.AllPositive))]
    public void Constructor_ValidWidthValue_WidthMatchesValue(int width)
    {
        CommonTests.ConstructorTests.CorrectlyInitializes(
            constructInstance: () => new WidthScaler(width),
            checks: [
                (scaler) => scaler.Width == width
            ]
        );
    }

    // T-2
    [Theory]
    [InlineData(0)]
    [ClassData(typeof(CommonTestValues.SingleInts.AllNegative))]
    public void Constructor_ZeroOrNegativeValue_ThrowsArgumentOutOfRangeException(int width)
    {
        CommonTests.ConstructorTests.ThrowsException(
            constructInstance: () => new WidthScaler(width),
            exceptionType: typeof(ArgumentOutOfRangeException)
        );
    }

    // T-3
    [Theory]
    [MemberData(nameof(GetWidthAndInputAndExpectedValues))]
    public void GetScaleParameters_WidthAndInputSizeValues_ResultMatchesExpectedSize(
        int width,
        Size inputSize,
        Size expectedSize
    ) {
        CommonTests.ScaleProviderTests.GetsCorrectScaleParameters(
            scaler: new WidthScaler(width),
            inputWidth: inputSize.Width,
            inputHeight: inputSize.Height,
            expectedSize: expectedSize
        );
    }

    /// <summary>
    /// Gets a width value, an input <see cref="Size"/> value, and an expected <see cref="Size"/>
    /// value. Use to test <see cref="WidthScaler.GetScaleParameters(int, int)"/>.
    /// </summary>
    public static IEnumerable<object[]> GetWidthAndInputAndExpectedValues()
    {
        int max = int.MaxValue;

        yield return new object[] { 100, new Size(100, 1), new Size(100, 1) };
        yield return new object[] { 100, new Size(100, 50), new Size(100, 50) };
        yield return new object[] { 100, new Size(50, 50), new Size(100, 100) };
        yield return new object[] { 100, new Size(10, 50), new Size(100, 500) };
        yield return new object[] { 100, new Size(1, 50), new Size(100, 5000) };
        yield return new object[] { 1, new Size(10, 10), new Size(1, 1) };
        yield return new object[] { 1, new Size(10, 50), new Size(1, 5) };
        yield return new object[] { 1, new Size(10, 5000), new Size(1, 500) };
        yield return new object[] { 1, new Size(max, max), new Size(1, 1) };
        yield return new object[] { max, new Size(1, 1), new Size(max, max) };
        yield return new object[] { max, new Size(max, max), new Size(max, max) };
        yield return new object[] { max, new Size(1, max), new Size(max, max) };
        yield return new object[] { max, new Size(100, max), new Size(max, max) };
        yield return new object[] { max, new Size(999999, max), new Size(max, max) };
    }
}
