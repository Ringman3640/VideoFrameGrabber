namespace VideoFrameGrabber
{
    /// <summary>
    /// Represents an image format that is used to specify the output format of an FFmpeg operation.
    /// </summary>
    public sealed class ImageFormat
    {
        /// <summary>
        /// Gets the extension name of the image format.
        /// </summary>
        public string ExtensionName { get; }

        /// <summary>
        /// Gets the video codec name used to convert to the image format.
        /// </summary>
        public string VideoCodecName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageFormat"/> class with the specified
        /// extension name and video codec name.
        /// </summary>
        /// <param name="extensionName">The extension name.</param>
        /// <param name="videoCodecName">The video codec name.</param>
        public ImageFormat(string extensionName, string videoCodecName)
        {
            ExtensionName = extensionName;
            VideoCodecName = videoCodecName;
        }
    }
}
