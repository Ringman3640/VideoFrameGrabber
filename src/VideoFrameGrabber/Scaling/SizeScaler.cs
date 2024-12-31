using System;

namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Represents a scaler of constant size.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use this class to represent a known, constant width and height for a scaling operation.
    /// </para>
    /// <para>
    /// This class is immutable.
    /// </para>
    /// </remarks>
    public class SizeScaler : ScaleProvider
    {
        /// <summary>
        /// Gets the pixel width component that this <see cref="SizeScaler"/> provides.
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// Gets the pixel height component that this <see cref="SizeScaler"/> provides.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeScaler"/> class with the specified
        /// width and height.
        /// </summary>
        /// <param name="width">The pixel width of the scaler.</param>
        /// <param name="height">The pixel height of the scaler.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="width"/> or <paramref name="height"/> was zero or negative.
        /// </exception>
        public SizeScaler(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));

            }

            Width = width;
            Height = height;
        }

        protected override void PerformScale(ref int width, ref int height)
        {
            width = Width;
            height = Height;
        }
    }
}
