using System;

namespace VideoFrameGrabber.FFmpegServicing
{
    /// <summary>
    /// Represents the metadata of a video file as extracted by FFmpeg.
    /// </summary>
    internal readonly struct VideoMetadata
    {
        /// <summary>
        /// Gets the pixel width of the video dimensions.
        /// </summary>
        public int VideoWidth
        {
            get;
#if NET5_0_OR_GREATER
            init;
#endif
        }

        /// <summary>
        /// Gets the pixel heigth of the video dimensions.
        /// </summary>
        public int VideoHeight
        {
            get;
#if NET5_0_OR_GREATER
            init;
#endif
        }

        /// <summary>
        /// Gets the length of the video.
        /// </summary>
        public TimeSpan VideoLength
        {
            get;
#if NET5_0_OR_GREATER
            init;
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoMetadata"/> structure to a specified
        /// video width, height, and length value.
        /// </summary>
        /// <param name="videoWidth">The pixel width of the video dimensions.</param>
        /// <param name="videoHeight">The pixel height of the video dimensions.</param>
        /// <param name="videoLength">The length of the video.</param>
        public VideoMetadata(int videoWidth, int videoHeight, TimeSpan videoLength)
        {
            VideoWidth = videoWidth;
            VideoHeight = videoHeight;
            VideoLength = videoLength;
        }
    }
}
