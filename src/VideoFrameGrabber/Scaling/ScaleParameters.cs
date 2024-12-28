namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Represents scaling dimension values to be applied as parameters to an FFmpeg operation.
    /// </summary>
    public struct ScaleParameters
    {
        /// <summary>
        /// Gets the width component of the current <see cref="ScaleParameters"/> instance.
        /// </summary>
#if NET5_0_OR_GREATER
        public int Width { get; init; }
#else
        public int Width { get; }
#endif

    /// <summary>
    /// Gets the height component value of the current <see cref="ScaleParameters"/> instance.
    /// </summary>
#if NET5_0_OR_GREATER
        public int Height { get; init; }
#else
        public int Height { get; }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleParameters"/> structure to a specified
        /// width and height value pair.
        /// </summary>
        /// <param name="width">Width length in pixels.</param>
        /// <param name="height">Height length in pixels.</param>
        public ScaleParameters(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
