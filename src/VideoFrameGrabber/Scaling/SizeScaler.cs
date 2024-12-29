namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Represents a scaler of constant size.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use this class to represent a known, constant width and height for a scaling operation.
    /// </para>
    /// <para>
    /// This class is immutable.
    /// </para>
    /// </remarks>
    public class SizeScaler : IScaleProvider
    {
        /// <summary>
        /// Gets the pixel width component that this <see cref="SizeScaler"/> provides.
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// Gets the pixel height component that this <see cref="SizeScaler"/> provides.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeScaler"/> class with the specified
        /// width and height.
        /// </summary>
        /// <param name="width">The pixel width of the scaler.</param>
        /// <param name="height">The pixel height of the scaler.</param>
        public SizeScaler(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public ScaleParameters GetScaleParameters(int inputWidth, int inputHeight)
        {
            return new ScaleParameters(Width, Height);
        }
    }
}
