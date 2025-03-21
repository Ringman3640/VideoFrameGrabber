﻿using System;

namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Represents a scaler that sets the width of an image to a specific value while attempting to
    /// maintain the image's aspect ratio.
    /// </summary>
    /// <remarks>
    /// <see cref="WidthScaler"/> attempts to maintain the aspect ratio of the image while applying
    /// scaling values. However, this may fail if the height of the image is larger that its width
    /// and <see cref="WidthScaler"/> scales the image width to a value close to
    /// <see cref="int.MaxValue"/>. Since a dimension of an image cannot exceed
    /// <see cref="int.MaxValue"/>, the height value would be clamped down in this scenario.
    /// </remarks>
    public class WidthScaler : ScaleProvider
    {
        /// <summary>
        /// Gets the width pixel length that will be applied to the image.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WidthScaler"/> class with the specified
        /// width length.
        /// </summary>
        /// <param name="length">Pixel length of the width value.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> was zero or negative.
        /// </exception>
        public WidthScaler(int length)
        {
            if (length <= 0 )
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            Width = length;
        }

        protected override void PerformScale(ref int width, ref int height)
        {
            double largeHeight;
            double heightMultiplier = (double)height / width;

            try
            {
                checked
                {
                    largeHeight = Width * heightMultiplier;
                }
            }
            catch
            {
                largeHeight = int.MaxValue;
            }

            width = Width;
            height = largeHeight > int.MaxValue ? int.MaxValue : (int)largeHeight;
        }
    }
}
