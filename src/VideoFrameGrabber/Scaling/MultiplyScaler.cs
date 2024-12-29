namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Represents a scaler that multiplies the size of an image by a specified multiplier.
    /// </summary>
    public class MultiplyScaler : IScaleProvider
    {
        /// <summary>
        /// Gets the multiplier value that is applied in <see cref="GetScaleParameters(int, int)"/>.
        /// </summary>
        public double Multiplier { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplyScaler"/> class with the specified
        /// multiplier value.
        /// </summary>
        /// <param name="multiplier">The multiplier value to apply for scaling operations.</param>
        public MultiplyScaler(double multiplier)
        {
            Multiplier = multiplier;        
        }

        /// <remarks>
        /// If the result of a multiply operation results in an overload, the result is capped to
        /// <see cref="int.MaxValue"/> or <see cref="int.MinValue"/> (depending on the sign of
        /// <see cref="Multiplier"/>. This is applied to both dimensions of the result
        /// <see cref="ScaleParameters"/> independently, which may cause differences between the
        /// input and output aspect ratio sizes if only one dimension overflows.
        /// </remarks>
        /// <inheritdoc cref="IScaleProvider.GetScaleParameters(int, int)"/>
        public ScaleParameters GetScaleParameters(int inputWidth, int inputHeight)
        {
            double outputWidth;
            try
            {
                checked
                {
                    outputWidth = inputWidth * Multiplier;
                }
            }
            catch
            {
                outputWidth = Multiplier < 0 ? double.MinValue : double.MaxValue;
            }

            double outputHeight;
            try
            {
                checked
                {
                    outputHeight = inputHeight * Multiplier;
                }
            }
            catch
            {
                outputHeight = Multiplier < 0 ? double.MinValue : double.MaxValue;
            }

            return new ScaleParameters(
                outputWidth > int.MaxValue ? int.MaxValue : (int)outputWidth,
                outputHeight > int.MaxValue ? int.MaxValue : (int)outputHeight
            );
        }
    }
}
