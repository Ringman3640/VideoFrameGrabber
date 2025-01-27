namespace VideoFrameGrabber.FFmpegServicing
{
    /// <summary>
    /// Exposes methods for calling FFmpeg.
    /// </summary>
    internal interface IFFmpegServicer
    {
        /// <summary>
        /// Calls FFmpeg to perform an operation defined by the given string argument.
        /// </summary>
        /// <param name="args">The CLI argument string to provide FFmpeg.</param>
        /// <returns>
        /// The result of the FFmpeg call as a <see cref="byte"/> array. If no result is returned
        /// by FFmpeg, the array will be empty.
        /// </returns>
        public byte[] CallFFmpegWithResult(string args);

        /// <summary>
        /// Calls FFmpeg to perform an operation defined by the given string argument. Does not
        /// return any result from the operation.
        /// </summary>
        /// <remarks>
        /// This method is effectively an optimization of <see cref="CallFFmpegWithResult(string)"/>
        /// since it performs the same action, but ignores the result, if any, of the FFmpeg
        /// operation.
        /// </remarks>
        /// <inheritdoc cref="CallFFmpegWithResult(string)" path="/param[@name='args']"/>
        public void CallFFmpegWithoutResult(string args);

        /// <summary>
        /// Gets the <see cref="VideoMetadata"/> of a video file.
        /// </summary>
        /// <param name="videoPath">A file path to the video.</param>
        /// <returns>
        /// A new <see cref="VideoMetadata"/> instance that represents the metadata of the video.
        /// </returns>
        public VideoMetadata GetVideoMetadata(string videoPath);
    }
}
