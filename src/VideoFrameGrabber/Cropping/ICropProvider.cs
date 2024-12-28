namespace VideoFrameGrabber.Cropping
{
    /// <summary>
    /// Provides fuctionality for an object to return a <see cref="CropParameters"/> instance.
    /// </summary>
    public interface ICropProvider
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
        public CropParameters GetCropParameters(int inputWidth, int inputHeight);
    }
}
