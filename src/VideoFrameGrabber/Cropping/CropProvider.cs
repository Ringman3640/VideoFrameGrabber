namespace VideoFrameGrabber.Cropping
{
    /// <summary>
    /// Provides cropping functionality by creating <see cref="CropParameters"/> values. This is an
    /// abstract class.
    /// </summary>
    public abstract class CropProvider

    {
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
            int width = inputWidth <= 0 ? 1 : inputWidth;
            int height = inputHeight <= 0 ? 1 : inputHeight;
            int x = 0;
            int y = 0;

            PerformCrop(ref width, ref height, ref x, ref y);

            if (width <= 0)
            {
                width = 1;
            }
            if (height <= 0)
            {
                height = 1;
            }

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
    }
}
