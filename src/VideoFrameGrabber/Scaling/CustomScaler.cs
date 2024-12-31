using System;

namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Represents a scaler that uses a custom scaling function to produce a scale value.
    /// </summary>
    public class CustomScaler : IScaleProvider
    {
        /// <summary>
        /// Gets the user-defined function that performs the scaling operation.
        /// </summary>
        public Func<int, int, ScaleParameters> ScaleFunction { get; }

        /// <summary>
        /// Initializes a new intance of the <see cref="CustomScaler"/> class with the specified
        /// scaler function.
        /// </summary>
        /// <param name="scaleFunction">A user-defined function that provides a scale value.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="scaleFunction"/> was <c>null</c>.
        /// </exception>
        public CustomScaler(Func<int, int, ScaleParameters> scaleFunction)
        {
            if (scaleFunction == null)
            {
                throw new ArgumentNullException(nameof(scaleFunction));
            }

            ScaleFunction = scaleFunction;
        }

        public ScaleParameters GetScaleParameters(int inputWidth, int inputHeight)
        {
            ScaleParameters result = ScaleFunction(inputWidth, inputHeight);
            return new ScaleParameters(
                result.Width <= 0 ? 1 : result.Width,
                result.Height <= 0 ? 1 : result.Height
            );
        }
    }
}
