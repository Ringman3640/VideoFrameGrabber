using System;

namespace VideoFrameGrabber.Cropping
{
    /// <summary>
    /// Provides cropping functionality by creating <see cref="CropParameters"/> values. This is an
    /// abstract class.
    /// </summary>
    public abstract class CropProvider
    {
        /// <summary>
        /// Gets the smallest allowed size for a crop dimension (height or width value).
        /// </summary>
        /// <remarks>
        /// This is the minimum value allowed for a dimension value in an FFmpeg crop filter. If a
        /// value less than this is input, an error will be thrown by FFmpeg. This value is not
        /// explicitly stated in the documentation, but was instead pulled from general testing
        /// using the crop filter.
        /// </remarks>
        public static int MinimumCropDimensionSize { get; } = 2;

        /// <summary>
        /// Gets a <see cref="CropParameters"/> instance given the width and height of an input
        /// image.
        /// </summary>
        /// <param name="inputWidth"></param>
        /// <param name="inputHeight"></param>
        /// <returns>
        /// A <see cref="CropParameters"/> instance that corresponds to the provided input image
        /// dimensions.
        /// </returns>
        public CropParameters GetCropParameters(int inputWidth, int inputHeight)
        {
            int width = Math.Max(inputWidth, MinimumCropDimensionSize);
            int height = Math.Max(inputHeight, MinimumCropDimensionSize);
            int x = 0;
            int y = 0;

            PerformCrop(ref width, ref height, ref x, ref y);

            // It would be better to use Math.Clamp here but it's not supported in .NET framework
            width = Math.Max(width, MinimumCropDimensionSize);
            width = Math.Min(width, inputWidth);
            height = Math.Max(height, MinimumCropDimensionSize);
            height = Math.Min(height, inputHeight);

            return new CropParameters(width, height, x, y);
        }

        /// <summary>
        /// Performs a cropping operation for the given width, height, and x and y values. This
        /// method directly changes the provided reference values.
        /// </summary>
        /// <param name="width">Reference to the crop width value.</param>
        /// <param name="height">Reference to the crop height value.</param>
        /// <param name="x">Reference to the crop X offset value.</param>
        /// <param name="y">Reference to the crop Y offset value.</param>
        /// <remarks>
        /// <para>
        /// This is the method that derived classes of <see cref="CropProvider"/> should implement
        /// with their respective cropping behaivor. Users of derived <see cref="CropProvider"/>
        /// classes indirectly call this method by calling
        /// <see cref="GetCropParameters(int, int)"/>, which calls this method.
        /// </para>
        /// <para>
        /// <paramref name="width"/> and <paramref name="height"/> are guaranteed to contain values
        /// of 1 or greater when this method is called.
        /// </para>
        /// </remarks>
        protected abstract void PerformCrop(ref int width, ref int height, ref int x, ref int y);

        /// <summary>
        /// Gets the calculated offset values for a crop rectangle for a given
        /// a <see cref="CropAlign"/> value.
        /// </summary>
        /// <param name="cropWidth">The width of the crop rectangle.</param>
        /// <param name="cropHeight">The height of the crop rectangle.</param>
        /// <param name="inputWidth">The width of the original image.</param>
        /// <param name="inputHeight">The height of the original image.</param>
        /// <param name="align">
        /// The <see cref="CropAlign"/> enum that indicates how the crop rectangle should be aligned
        /// within the original image.
        /// </param>
        /// <returns>
        /// A <see cref="CropOffset"/> that contains the calculated offset values.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Regardless of the values of <paramref name="align"/>, if the crop rectangle along any
        /// dimension is larger than the original image, the offset along that dimension will be
        /// zero.
        /// </para>
        /// <para>
        /// For example, if <paramref name="cropWidth"/> is <c>1000</c> and if
        /// <paramref name="inputWidth"/> is <c>500</c>, the the X value of the returned
        /// <see cref="CropOffset"/> will be <c>0</c>, regardless of the value of
        /// <paramref name="align"/>. This is because the crop rectangle width extends beyond the
        /// dimensions of the original image.
        /// </para>
        /// <para>
        /// This method is only intended to be used by derived <see cref="CropProvider"/> classes.
        /// It is only marked internal to make it accessible for testing.
        /// </para>
        /// </remarks>
        internal static CropOffset GetCropOffsetFromAlign(
            int cropWidth,
            int cropHeight,
            int inputWidth,
            int inputHeight,
            CropAlign align)
        {
            int xOffset;
            int yOffset;
            int availableWidth = Math.Max(0, inputWidth - cropWidth);
            int availableHeight = Math.Max(0, inputHeight - cropHeight);

            switch (align)
            {
                case (CropAlign.None):
                    xOffset = 0;
                    yOffset = 0;
                    break;

                case (CropAlign.TopLeft):
                    xOffset = 0;
                    yOffset = 0;
                    break;

                case (CropAlign.TopCenter):
                    xOffset = availableWidth / 2;
                    yOffset = 0;
                    break;

                case (CropAlign.TopRight):
                    xOffset = availableWidth;
                    yOffset = 0;
                    break;

                case (CropAlign.CenterLeft):
                    xOffset = 0;
                    yOffset = availableHeight / 2;
                    break;

                case (CropAlign.Center):
                    xOffset = availableWidth / 2;
                    yOffset = availableHeight / 2;
                    break;

                case (CropAlign.CenterRight):
                    xOffset = availableWidth;
                    yOffset = availableHeight / 2;
                    break;

                case (CropAlign.BottomLeft):
                    xOffset = 0;
                    yOffset = availableHeight;
                    break;

                case (CropAlign.BottomCenter):
                    xOffset = availableWidth / 2;
                    yOffset = availableHeight;
                    break;

                case (CropAlign.BottomRight):
                    xOffset = availableWidth;
                    yOffset = availableHeight;
                    break;

                default:
                    throw new NotImplementedException($"The {Enum.GetName(align.GetType(), align)}" +
                        $" CropAlign value does not have a corresponding implementation.");
            }

            return new CropOffset(xOffset, yOffset);
        }

        /// <summary>
        /// Represents a set of offset values for crop rectangle of a crop operation.
        /// </summary>
        internal readonly struct CropOffset
        {
            /// <summary>
            /// The horizontal offset component of the crop rectangle.
            /// </summary>
            public int X { get; }

            /// <summary>
            /// The vertical offset component of the crop rectangle.
            /// </summary>
            public int Y { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="CropOffset"/> struct with the specified
            /// X and Y offset values.
            /// </summary>
            /// <param name="x">The X component of the offset.</param>
            /// <param name="y">The Y component of the offset.</param>
            public CropOffset(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
