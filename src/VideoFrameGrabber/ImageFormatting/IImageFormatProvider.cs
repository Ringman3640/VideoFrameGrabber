namespace VideoFrameGrabber.ImageFormatting
{
    /// <summary>
    /// Exposes methods for specifying an output image format for FFmpeg operations.
    /// </summary>
    public interface IImageFormatProvider
    {
        /// <summary>
        /// Gets the FFmpeg codec name for an image format.
        /// </summary>
        /// <returns>The FFmpeg codec name for the image format.</returns>
        /// <remarks>
        /// <para>
        /// During an FFmpeg operation to extract a single frame from a video file, the <c>-c:v</c>
        /// flag (specify video codec) can be used to indicate the export image format when using
        /// pipes. 
        /// </para>
        /// <para>
        /// The value returned from this method is the value that should be applied to the
        /// <c>-c:v</c> flag for a specific image format.
        /// </para>
        /// </remarks>
        public string GetVideoCodecName();

        /// <summary>
        /// Gets the file extension name for an image format.
        /// </summary>
        /// <returns>The file extension name for the image format.</returns>
        /// <remarks>
        /// This method only returns the name of the image format extension, not including the
        /// period character that separates the file name from the extension.
        /// </remarks>
        public string GetExtensionName();
    }
}
