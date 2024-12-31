namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Represents scaling dimension values to be applied as parameters to an FFmpeg operation.
    /// </summary>
    public readonly struct ScaleParameters
    {
        /// <summary>
        /// Gets the width component of the current <see cref="ScaleParameters"/> instance.
        /// </summary>
        public int Width
        {
            get;
#if NET5_0_OR_GREATER
            init;
#endif
        }

        /// <summary>
        /// Gets the height component value of the current <see cref="ScaleParameters"/> instance.
        /// </summary>
        public int Height
        {
            get;
#if NET5_0_OR_GREATER
            init;
#endif
        }

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
