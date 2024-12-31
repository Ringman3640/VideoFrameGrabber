using System;

namespace VideoFrameGrabber.Scaling
{
    /// <summary>
    /// Represents a scaler that uses a custom scaling function to produce a scale value.
    /// </summary>
    public class CustomScaler : ScaleProvider
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

        protected override void PerformScale(ref int width, ref int height)
        {
            ScaleParameters result = ScaleFunction(width, height);
            width = result.Width;
            height = result.Height;
        }
    }
}
