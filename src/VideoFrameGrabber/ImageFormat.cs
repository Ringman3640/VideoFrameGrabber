namespace VideoFrameGrabber
{
    /// <summary>
    /// Represents an image format that is used to specify the output format of an FFmpeg operation.
    /// </summary>
    public sealed class ImageFormat
    {
        /// <summary>
        /// Gets the predefined <see cref="ImageFormat"/> for the JPEG image format.
        /// </summary>
        public static ImageFormat Jpg;

        /// <summary>
        /// Gets the predefined <see cref="ImageFormat"/> for the JPEG image format.
        /// </summary>
        /// <remarks>
        /// This predefined <see cref="ImageFormat"/> has the same <see cref="VideoCodecName"/> as
        /// <see cref="Jpg"/>, but uses the extended JPEG extension name for
        /// <see cref="ExtensionName"/> (jpeg instead of jpg).
        /// </remarks>
        public static ImageFormat Jpeg;

        /// <summary>
        /// Gets the predefined <see cref="ImageFormat"/> for the PNG image format.
        /// </summary>
        public static ImageFormat Png;

        /// <summary>
        /// Gets the predefined <see cref="ImageFormat"/> for the WebP image format.
        /// </summary>
        public static ImageFormat WebP;

        /// <summary>
        /// Gets the predefined <see cref="ImageFormat"/> for the BMP image format.
        /// </summary>
        public static ImageFormat Bmp;

        /// <summary>
        /// Gets the predefined <see cref="ImageFormat"/> for the GIF image format.
        /// </summary>
        public static ImageFormat Gif;

        /// <summary>
        /// Gets the predefined <see cref="ImageFormat"/> for the TIFF image format.
        /// </summary>
        public static ImageFormat Tiff;

        /// <summary>
        /// Gets the predefined <see cref="ImageFormat"/> that should be used in operations that do
        /// not specify an image format. This will use the JPEG format.
        /// </summary>
        /// <remarks>
        /// The <see cref="ImageFormat"/> instance of this property is the exact same instance
        /// used by <see cref="Jpg"/>.
        /// </remarks>
        public static ImageFormat Default;

        /// <summary>
        /// Gets the extension name of the image format.
        /// </summary>
        public string ExtensionName { get; }

        /// <summary>
        /// Gets the video codec name used to convert to the image format.
        /// </summary>
        public string VideoCodecName { get; }

        /// <summary>
        /// Initializes the static common <see cref="ImageFormat"/> properties.
        /// </summary>
        static ImageFormat()
        {
            // The following video codec names were found using the "ffmepg -codecs" command. Each
            // codec name was tested by using the name in the -c:v flag of an ffmpeg command that
            // extracts a single frame from a video file. The image was output to standard out and
            // the binary was saved to an output file with no extension. The file was sent through
            // https://www.checkfiletype.com/ to validate the format.

            Jpg = new ImageFormat(
                extensionName: "jpg",
                videoCodecName: "mjpeg");

            Jpeg = new ImageFormat(
                extensionName: "jpeg",
                videoCodecName: "mjpeg");

            Png = new ImageFormat(
                extensionName: "png",
                videoCodecName: "png");

            WebP = new ImageFormat(
                extensionName: "webp",
                videoCodecName: "webp");

            Bmp = new ImageFormat(
                extensionName: "bmp",
                videoCodecName: "bmp");

            Gif = new ImageFormat(
                extensionName: "gif",
                videoCodecName: "gif");

            Tiff = new ImageFormat(
                extensionName: "tiff",
                videoCodecName: "tiff");

            Default = Jpg;
        }

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
