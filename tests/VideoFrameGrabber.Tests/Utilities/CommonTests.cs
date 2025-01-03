using FluentAssertions;
using VideoFrameGrabber.Scaling;
using VideoFrameGrabber.Tests.UnitTests.Scaling;

namespace VideoFrameGrabber.Tests.Utilities;

/// <summary>
/// Exposes static methods that provide common testing implementations.
/// </summary>
public static class CommonTests
{
    /// <summary>
    /// Gets tests related to instance construction.
    /// </summary>
    public static ConstructorTests Constructor { get; } = new ConstructorTests();

    /// <summary>
    /// Gets tests related to <see cref="ScaleProvider"/> instances.
    /// </summary>
    public static ScaleProviderTests Scaler { get; } = new ScaleProviderTests();

    /// <summary>
    /// Represents a set of tests for instance construction.
    /// </summary>
    public class ConstructorTests
    {
        public static void CorrectlyInitializes<T>(Func<T> constructInstance, Func<T, bool>[] checks)
        {
            T? instance = default;

            try
            {
                instance = constructInstance();
            }
            catch { }

            instance.Should().NotBeNull();
            foreach(var check in checks)
            {
                check(instance!).Should().BeTrue();
            }
        }

        public static void ThrowsException(Func<object> constructInstance, Type exceptionType)
        {
            object? instance = null;
            Exception? exception = null;

            try
            {
                instance = constructInstance();
            }
            catch (Exception recievedException)
            {
                exception = recievedException;
            }

            instance.Should().BeNull();
            exception.Should().NotBeNull().And.BeOfType(exceptionType);
        }
    }

    /// <summary>
    /// Represents a set of tests for <see cref="ScaleProvider"/> operations.
    /// </summary>
    public class ScaleProviderTests
    {
        public static void GetsCorrectScaleParameters(
            ScaleProvider scaler,
            int inputWidth,
            int inputHeight,
            int expectedWidth,
            int expectedHeight
        ) {
            ScaleParameters? scaleParameters = null;

            try
            {
                scaleParameters = scaler.GetScaleParameters(inputWidth, inputHeight);
            }
            catch { }

            scaleParameters.Should().NotBeNull();
            scaleParameters!.Value.Width.Should().Be(expectedWidth);
            scaleParameters!.Value.Height.Should().Be(expectedHeight);
        }

        public static void GetsCorrectScaleParameters(
            ScaleProvider scaler,
            int inputWidth,
            int inputHeight,
            Size expectedSize
        ) {
            GetsCorrectScaleParameters(
                scaler,
                inputWidth,
                inputHeight,
                expectedSize.Width,
                expectedSize.Height
            );
        }

        public static void GetsCorrectScaleParameters(
            ScaleProvider scaler,
            Size inputSize,
            Size expectedSize
        ) {
            GetsCorrectScaleParameters(
                scaler,
                inputSize.Width,
                inputSize.Height,
                expectedSize.Width,
                expectedSize.Height
            );
        }

        public static void GetsCorrectScaleParameters(
            ScaleProvider scaler,
            int inputWidth,
            int inputHeight,
            ScaleParameters expectedScale
        ) {
            GetsCorrectScaleParameters(
                scaler,
                inputWidth,
                inputHeight,
                expectedScale.Width,
                expectedScale.Height
            );
        }

        public static void GetsCorrectScaleParameters(
            ScaleProvider scaler,
            Size inputSize,
            ScaleParameters expectedScale
        ) {
            GetsCorrectScaleParameters(
                scaler,
                inputSize.Width,
                inputSize.Height,
                expectedScale.Width,
                expectedScale.Height
            );
        }
    }
}
