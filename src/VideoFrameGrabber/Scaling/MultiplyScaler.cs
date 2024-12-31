using System;

namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Represents a scaler that multiplies the size of an image by a specified multiplier.
    /// </summary>
    /// <remarks>
    /// If the result of a multiply operation results in an overload, the result is capped to
    /// <see cref="int.MaxValue"/> This is applied to both dimensions of the result
    /// <see cref="ScaleParameters"/> independently, which may cause differences between the input
    /// and output aspect ratio sizes if only one dimension overflows.
    /// </remarks>
    public class MultiplyScaler : ScaleProvider
    {
        /// <summary>
        /// Gets the multiplier value that is applied in <see cref="GetScaleParameters(int, int)"/>.
        /// </summary>
        public double Multiplier { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplyScaler"/> class with the specified
        /// multiplier value.
        /// </summary>
        /// <param name="multiplier">The multiplier value to apply for scaling operations.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="multiplier"/> was zero or negative.
        /// </exception>
        public MultiplyScaler(double multiplier)
        {
            if (multiplier <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(multiplier));
            }

            Multiplier = multiplier;
        }

        protected override void PerformScale(ref int width, ref int height)
        {
            double largeWidth;
            try
            {
                checked
                {
                    largeWidth = width * Multiplier;
                }
            }
            catch
            {
                largeWidth = double.MaxValue;
            }

            double largeHeight;
            try
            {
                checked
                {
                    largeHeight = height * Multiplier;
                }
            }
            catch
            {
                largeHeight = double.MaxValue;
            }

            width = largeWidth > int.MaxValue ? int.MaxValue : (int)largeWidth;
            height = largeHeight > int.MaxValue ? int.MaxValue : (int)largeHeight;
        }
    }
}
