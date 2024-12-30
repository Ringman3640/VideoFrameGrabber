using System;

namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Represents a scaler that sets the height of an image to a specific value while attempting to
    /// maintain the image's aspect ratio.
    /// </summary>
    /// <remarks>
    /// <see cref="HeightScaler"/> attempts to maintain the aspect ratio of the image while applying
    /// scaling values. However, this may fail if the width of the image is larger that its height
    /// and <see cref="HeightScaler"/> scales the image height to a value close to
    /// <see cref="int.MaxValue"/>. Since a dimension of an image cannot exceed
    /// <see cref="int.MaxValue"/>, the width value would be clamped down in this scenario.
    /// </remarks>
    public class HeightScaler
    {
        /// <summary>
        /// Gets the height pixel length that will be applied to the image.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeightScaler"/> class with the specified
        /// height length.
        /// </summary>
        /// <param name="length">Pixel length of the height value.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> was zero or negative.
        /// </exception>
        public HeightScaler(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            Height = length;
        }

        public ScaleParameters GetScaleParameters(int inputWidth, int inputHeight)
        {
            double outWidth;
            double widthMultiplier = (double)inputWidth / inputHeight;

            try
            {
                checked
                {
                    outWidth = Height * widthMultiplier;
                }
            }
            catch
            {
                outWidth = int.MaxValue;
            }

            return new ScaleParameters(
                outWidth > int.MaxValue ? int.MaxValue : (int)outWidth,
                Height
            );
        }
    }
}
