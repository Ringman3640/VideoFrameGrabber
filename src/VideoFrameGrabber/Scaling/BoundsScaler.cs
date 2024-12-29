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
    public class BoundsScaler : IScaleProvider
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
            if (inputWidth <= WidthBounds && inputHeight <= HeightBounds)
            {
                // No shrinking needed, input is within bounds
                return new ScaleParameters(inputWidth, inputHeight);
            }

            int outputWidth;
            int outputHeight;
            double inputAspectRatio = (double)inputWidth / inputHeight;
            
            if (inputAspectRatio > BoundsAspectRatio)
            {
                // Case: Input size is wider than bounds size, set input width to bounds width and
                // adjust input height to keep aspect ratio
                outputWidth = WidthBounds;
                outputHeight = (int)(outputWidth / inputAspectRatio);
            }
            else if (inputAspectRatio < BoundsAspectRatio)
            {
                // Case: Iput size is taller than bounds size, set input height to bounds height and
                // adjust input width to keep aspect ratio
                outputHeight = HeightBounds;
                outputWidth = (int)(outputHeight * inputAspectRatio);
            }
            else
            {
                // Case: Aspect ratios are the same, just shrink input size to bounds size
                outputWidth = WidthBounds;
                outputHeight = HeightBounds;
            }

            if (outputWidth <= 0)
            {
                outputWidth = 1;
            }
            if (outputHeight <= 0)
            {
                outputHeight = 1;
            }

            return new ScaleParameters(outputWidth, outputHeight);
        }
    }
}
