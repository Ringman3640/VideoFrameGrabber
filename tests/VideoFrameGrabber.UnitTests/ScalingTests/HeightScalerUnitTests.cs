﻿using FluentAssertions;
using VideoFrameGrabber.Scaling;
using VideoFrameGrabber.UnitTests.Utilities;

namespace VideoFrameGrabber.UnitTests.UnitTests.ScalingTests;

public  class HeightScalerUnitTests
{
    // T-1
    [Theory]
    [ClassData(typeof(CommonTestValues.SingleInts.AllPositive))]
    public void Constructor_ValidHeightValue_HeightMatchesValue(int height)
    {
        CommonTests.Constructor.CorrectlyInitializes(
            constructInstance: () => new HeightScaler(height),
            checks: [
                (scaler) => scaler.Height == height
            ]
        );
    }

    // T-2
    [Theory]
    [InlineData(0)]
    [ClassData(typeof(CommonTestValues.SingleInts.AllNegative))]
    public void Constructor_ZeroOrNegativeValue_ThrowsArgumentOutOfRangeException(int height)
    {
        CommonTests.Constructor.ThrowsException(
            constructInstance: () => new HeightScaler(height),
            exceptionType: typeof(ArgumentOutOfRangeException)
        );
    }

    // T-3
    [Theory]
    [MemberData(nameof(GetHeightAndInputAndExpectedValues))]
    public void GetScaleParameters_HeightAndInputSizeValues_ResultMatchesExpectedSize(
        int height,
        Size inputSize,
        Size expectedSize
    ) {
        HeightScaler scaler = new(height);

        CommonTests.ScaleProvider.GetsCorrectScaleParameters(
            scaler: scaler,
            inputWidth: inputSize.Width,
            inputHeight: inputSize.Height,
            expectedSize: expectedSize
        );
    }

    /// <summary>
    /// Gets a height value, an input <see cref="Size"/> value, and an expected <see cref="Size"/>
    /// value. Use to test <see cref="HeightScaler.GetScaleParameters(int, int)"/>.
    /// </summary>
    public static IEnumerable<object[]> GetHeightAndInputAndExpectedValues()
    {
        int max = int.MaxValue;

        yield return new object[] { 100, new Size(1, 100), new Size(1, 100) };
        yield return new object[] { 100, new Size(50, 100), new Size(50, 100) };
        yield return new object[] { 100, new Size(50, 50), new Size(100, 100) };
        yield return new object[] { 100, new Size(50, 10), new Size(500, 100) };
        yield return new object[] { 100, new Size(50, 1), new Size(5000, 100) };
        yield return new object[] { 1, new Size(10, 10), new Size(1, 1) };
        yield return new object[] { 1, new Size(50, 10), new Size(5, 1) };
        yield return new object[] { 1, new Size(5000, 10), new Size(500, 1) };
        yield return new object[] { 1, new Size(max, max), new Size(1, 1) };
        yield return new object[] { max, new Size(1, 1), new Size(max, max) };
        yield return new object[] { max, new Size(max, max), new Size(max, max) };
        yield return new object[] { max, new Size(max, 1), new Size(max, max) };
        yield return new object[] { max, new Size(max, 100), new Size(max, max) };
        yield return new object[] { max, new Size(max, 999999), new Size(max, max) };
    }
}
