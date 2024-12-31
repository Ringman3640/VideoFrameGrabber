using System;

namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Represents a scaler that limits scale sizes to a bounds size.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="BoundsScaler"/> only modifies the size of an image if the dimensions of the
    /// image exceed the dimensions of its bounds size. If the image is larger that the bounds size,
    /// the image will be scaled down to fit within the bounds.
    /// </para>
    /// <para>
    /// If scaling occurs, <see cref="BoundsScaler"/> attempts to maintain the aspect ratio of the
    /// image.
    /// </para>
    /// <para>
    /// To make an image take up as much space as possible within the bounding rectangle, use
    /// <see cref="FitBoundsScaler"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="FitBoundsScaler"/>
    public class BoundsScaler : ScaleProvider
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
        /// Initializes a new instance of the <see cref="BoundsScaler"/> class with the specified
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
        public BoundsScaler(int widthBounds, int heightBounds)
        {
            if (widthBounds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(widthBounds));
            }
            if (heightBounds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(heightBounds));
            }

            WidthBounds = widthBounds;
            HeightBounds = heightBounds;
            BoundsAspectRatio = widthBounds / heightBounds;
        }

        protected override void PerformScale(ref int width, ref int height)
        {
            if (width <= WidthBounds && height <= HeightBounds)
            {
                // No shrinking needed, input is within bounds
                return;
            }

            double inputAspectRatio = (double)width / height;

            if (inputAspectRatio > BoundsAspectRatio)
            {
                // Case: Input size is wider than bounds size, set input width to bounds width and
                // adjust input height to keep aspect ratio
                width = WidthBounds;
                height = (int)(width / inputAspectRatio);
            }
            else if (inputAspectRatio < BoundsAspectRatio)
            {
                // Case: Iput size is taller than bounds size, set input height to bounds height and
                // adjust input width to keep aspect ratio
                height = HeightBounds;
                width = (int)(height * inputAspectRatio);
            }
            else
            {
                // Case: Aspect ratios are the same, just shrink input size to bounds size
                width = WidthBounds;
                height = HeightBounds;
            }
        }
    }
}
