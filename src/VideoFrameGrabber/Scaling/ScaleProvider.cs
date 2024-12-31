namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Provides fuctionality for an object to return a <see cref="ScaleParameters"/> instance.
    /// </summary>
    public abstract class ScaleProvider
    {
        /// <summary>
        /// Gets a <see cref="ScaleParameters"/> instance given the width and height of an input
        /// image.
        /// </summary>
        /// <param name="inputWidth">Width of the input image in pixels.</param>
        /// <param name="inputHeight">Height of the input image in pixels.</param>
        /// <returns>
        /// A <see cref="ScaleParameters"/> instance that corresponds to the provided input image
        /// dimensions.
        /// </returns>
        public ScaleParameters GetScaleParameters(int inputWidth, int inputHeight)
        {
            int width = inputWidth <= 0 ? 1: inputWidth;
            int height = inputHeight <= 0 ? 1: inputHeight;

            PerformScale(ref width, ref height);

            if (width <= 0)
            {
                width = 1;
            }
            if (height <= 0)
            {
                height = 1;
            }

            return new ScaleParameters(width, height);
        }

        /// <summary>
        /// Performs a scaling operation for the given width and height values. This method directly
        /// changes the provided reference values.
        /// </summary>
        /// <param name="width">Reference to the scaler width value.</param>
        /// <param name="height">Reference to the scaler height value.</param>
        /// <remarks>
        /// <para>
        /// This is the method that derived classes of <see cref="ScaleProvider"/> should implement
        /// with their respective scaling behaivor. Users of derived <see cref="ScaleProvider"/>
        /// classes indirectly call this method by calling
        /// <see cref="GetScaleParameters(int, int)"/>, which calls this method.
        /// </para>
        /// <para>
        /// <paramref name="width"/> and <paramref name="height"/> are guaranteed to contain values
        /// of 1 or greater when the method is called.
        /// </para>
        /// </remarks>
        protected abstract void PerformScale(ref int width, ref int height);
    }
}
