namespace VideoFrameGrabber.Cropping
{
    /// <summary>
    /// Represents cropping parameter values to be applied to an FFmpeg operation.
    /// </summary>
    public readonly struct CropParameters
    {
        /// <summary>
        /// Gets the width of the crop frame.
        /// </summary>
        public int Width
        {
            get;
#if NET5_0_OR_GREATER
            init;
#endif
        }

        /// <summary>
        /// Gets the height of the crop frame.
        /// </summary>
        public int Height
        {
            get;
#if NET5_0_OR_GREATER
            init;
#endif
        }

        /// <summary>
        /// Gets the X component of the crop frame's offset coordinate.
        /// </summary>
        public int X
        {
            get;
#if NET5_0_OR_GREATER
            init;
#endif
        }

        /// <summary>
        /// Gets the Y component of the crop frame's offset coordinate.
        /// </summary>
        public int Y
        {
            get;
#if NET5_0_OR_GREATER
            init;
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CropParameters"/> structure to a specified
        /// width and height value pair.
        /// </summary>
        /// <param name="width">Width length in pixels of the crop frame.</param>
        /// <param name="height">Height length in pixels of the crop frame.</param>
        public CropParameters(int width, int height)
        {
            Width = width;
            Height = height;
            X = 0;
            Y = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CropParameters"/> structure to a specified
        /// width, height, and offset coordinate position.
        /// </summary>
        /// <param name="x">Horizontal offset of the crop frame in pixels.</param>
        /// <param name="y">Vertical offset of the crop frame in pixels.</param>
        /// <inheritdoc cref="CropParameters(int, int)"/>
        public CropParameters(int width, int height, int x, int y)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }
    }
}
