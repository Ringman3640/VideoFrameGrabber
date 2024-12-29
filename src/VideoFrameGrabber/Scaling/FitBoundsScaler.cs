using System;

namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Represents a scaler that scales an image to fit fully within a bounding rectangle.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="FitBoundsScaler"/> modifies the size of an image to fully fit within the
    /// provided bounding rectangle. This means that the image will be scaled up if it is smaller
    /// than the bounding rectangle, or the image will be scaled down if it is larger than the
    /// bounding rectangle.
    /// </para>
    /// <para>
    /// If scaling occurs, <see cref="FitBoundsScaler"/> attempts to maintain the aspect ratio of
    /// the image.
    /// </para>
    /// <para>
    /// To make an image only scale if it is larger than the bounding rectangle, use
    /// <see cref="BoundsScaler"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="BoundsScaler"/>
    public class FitBoundsScaler : IScaleProvider
    {
        /// <summary>
        /// Gets the pixel width of the bounds size.
        /// </summary>
        public int WidthBounds { get; }

        /// <summary>
        /// Gets the pixel height of the bounds size.
        /// </summary>
        public int HeightBounds { get; }

        /// <summary>
        /// Gets the aspect ratio of the bounds size. This is <see cref="WidthBounds"/> divided by
        /// <see cref="HeightBounds"/>.
        /// </summary>
        private double BoundsAspectRatio { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FitBoundsScaler"/> class with the specified
        /// bounds dimensions.
        /// </summary>
        /// <param name="widthBounds">The pixel width of the bounds.</param>
        /// <param name="heightBounds">The pixel height of the bounds.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="widthBounds"/> or <paramref name="heightBounds"/> was zero or negative.
        /// </exception>
        /// <remarks>
        /// The values for <paramref name="widthBounds"/> and <paramref name="heightBounds"/> must
        /// be positive. This comes from the requirement for images to have a non-zero size.
        /// </remarks>
        public FitBoundsScaler(int widthBounds, int heightBounds)
        {
            if (widthBounds <= 0 || heightBounds <= 0)
            {
                throw new ArgumentOutOfRangeException("Both bound values must be positive (not zero or negative).");
            }

            WidthBounds = widthBounds;
            HeightBounds = heightBounds;
            BoundsAspectRatio = widthBounds / heightBounds;
        }

        public ScaleParameters GetScaleParameters(int inputWidth, int inputHeight)
        {
            int outWidth;
            int outHeight;
            double inputAspectRatio = (double)inputWidth / inputHeight;

            if (inputAspectRatio > BoundsAspectRatio)
            {
                // Case: Input size is wider than bounds size, set output width to bounds width and
                // adjust output height to keep aspect ratio
                outWidth = WidthBounds;
                outHeight = (int)(outWidth / inputAspectRatio);
            }
            else if (inputAspectRatio < BoundsAspectRatio)
            {
                // Case: Iput size is taller than bounds size, set output height to bounds height and
                // adjust output width to keep aspect ratio
                outHeight = HeightBounds;
                outWidth = (int)(outHeight * inputAspectRatio);
            }
            else
            {
                // Case: Aspect ratios are the same, just set input size to bounds size
                outWidth = WidthBounds;
                outHeight = HeightBounds;
            }

            if (outWidth <= 0)
            {
                outWidth = 1;
            }
            if (outHeight <= 0)
            {
                outHeight = 1;
            }

            return new ScaleParameters(outWidth, outHeight);
        }
    }
}
