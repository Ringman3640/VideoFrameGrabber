﻿using FluentAssertions;
using VideoFrameGrabber.Scaling;

namespace VideoFrameGrabber.Tests.UnitTests.Scaling;

public class FitBoundsScalerUnitTests
{
    // T-1
    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 10)]
    [InlineData(99999, 99999)]
    [InlineData(int.MaxValue, 1)]
    [InlineData(1, int.MaxValue)]
    [InlineData(int.MaxValue, int.MaxValue)]
    public void Constructor_ValidBoundValues_WidthAndHeightBoundsMatchValues(
        int width,
        int height
    ) {
        FitBoundsScaler? scaling = null;

        try
        {
            scaling = new(width, height);
        }
        catch { }

        scaling.Should().NotBeNull();
        scaling!.WidthBounds.Should().Be(width);
        scaling!.HeightBounds.Should().Be(height);
    }

    // T-2
    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    [InlineData(0, 0)]
    [InlineData(-1, 1)]
    [InlineData(1, -1)]
    [InlineData(-1, -1)]
    [InlineData(int.MinValue, 1)]
    [InlineData(1, int.MinValue)]
    [InlineData(int.MinValue, int.MinValue)]
    public void Constructor_ZeroOrNegativeValues_ThrowsArgumentOutOfRangeException(
        int width,
        int height
    ) {
        FitBoundsScaler? scaling = null;
        Exception? exception = null;

        try
        {
            scaling = new(width, height);
        }
        catch (Exception except)
        {
            exception = except;
        }

        scaling.Should().BeNull();
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    // T-3
    [Theory]
    [MemberData(nameof(GetBoundsAndInputAndExpectedSizeValues))]
    public void GetScaleParameters_BoundsSizeAndInputSizeValues_ResultMatchesExpectedDimensions(
        Size boundsSize,
        Size inputSize,
        Size expectedSize
    ) {
        FitBoundsScaler scaler = new(boundsSize.Width, boundsSize.Height);
        ScaleParameters? scaleParameters = null;

        try
        {
            scaleParameters = scaler.GetScaleParameters(inputSize.Width, inputSize.Height);
        }
        catch { }

        scaleParameters.Should().NotBeNull();
        scaleParameters!.Value.Width.Should().Be(expectedSize.Width);
        scaleParameters!.Value.Height.Should().Be(expectedSize.Height);
    }

    /// <summary>
    /// Gets a set of trio <see cref="Size"/> values to test the
    /// <see cref="BoundsScaler.GetScaleParameters(int, int)"/> method.
    /// </summary>
    public static IEnumerable<object[]> GetBoundsAndInputAndExpectedSizeValues()
    {
        int max = int.MaxValue;

        yield return new object[] { new Size(100, 100), new Size(10, 10), new Size(100, 100) };
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
        yield return new object[] { new Size(100, 100), new Size(10, 20), new Size(50, 100) };
        yield return new object[] { new Size(100, 100), new Size(10, 50), new Size(20, 100) };
        yield return new object[] { new Size(100, 100), new Size(1, 10), new Size(10, 100) };
        yield return new object[] { new Size(100, 100), new Size(20, 10), new Size(100, 50) };
        yield return new object[] { new Size(100, 100), new Size(50, 10), new Size(100, 20) };
        yield return new object[] { new Size(100, 100), new Size(10, 1), new Size(100, 10) };
        yield return new object[] { new Size(max, max), new Size(1, 1), new Size(max, max) };
        yield return new object[] { new Size(max, max), new Size(999, 999), new Size(max, max) };
        yield return new object[] { new Size(100, 10), new Size(1, 1), new Size(10, 10) };
        yield return new object[] { new Size(100, 10), new Size(2, 1), new Size(20, 10) };
        yield return new object[] { new Size(100, 10), new Size(5, 1), new Size(50, 10) };
        yield return new object[] { new Size(100, 10), new Size(10, 1), new Size(100, 10) };
        yield return new object[] { new Size(100, 10), new Size(1, 2), new Size(5, 10) };
        yield return new object[] { new Size(100, 10), new Size(1, 5), new Size(2, 10) };
        yield return new object[] { new Size(100, 10), new Size(1, 10), new Size(1, 10) };
        yield return new object[] { new Size(10, 100), new Size(1, 1), new Size(10, 10) };
        yield return new object[] { new Size(10, 100), new Size(1, 2), new Size(10, 20) };
        yield return new object[] { new Size(10, 100), new Size(1, 5), new Size(10, 50) };
        yield return new object[] { new Size(10, 100), new Size(1, 10), new Size(10, 100) };
        yield return new object[] { new Size(10, 100), new Size(2, 1), new Size(10, 5) };
        yield return new object[] { new Size(10, 100), new Size(5, 1), new Size(10, 2) };
        yield return new object[] { new Size(10, 100), new Size(10, 1), new Size(10, 1) };
    }
}