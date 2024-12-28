namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Provides fuctionality for an object to return a <see cref="ScaleParameters"/> instance.
    /// </summary>
    public interface IScaleProvider
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
        ScaleParameters GetScaleParameters(int inputWidth, int inputHeight);
    }
}
