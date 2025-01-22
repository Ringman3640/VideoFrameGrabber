using System;

namespace VideoFrameGrabber.Cropping
{
    /// <summary>
    /// Represents a <see cref="CropProvider"/> with custom cropping function to produce a crop
    /// result.
    /// </summary>
    public class CustomCropper : CropProvider
    {
        /// <summary>
        /// Gets the cropping function used to produce a crop result.
        /// </summary>
        public Func<int, int, CropParameters> CropFunction { get; }

        /// <summary>
        /// Initializes a new intance of the <see cref="CustomCropper"/> class with the specified
        /// crop function.
        /// </summary>
        /// <param name="cropFunction">
        /// A user-defined function that provides a <see cref="CropParameters"/> value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="cropFunction"/> was <c>null</c>.
        /// </exception>
        public CustomCropper(Func<int, int, CropParameters> cropFunction)
        {
            if (cropFunction is null)
            {
                throw new ArgumentNullException(nameof(cropFunction));
            }

            CropFunction = cropFunction;
        }

        protected override void PerformCrop(ref int width, ref int height, ref int x, ref int y)
        {
            CropParameters result = CropFunction(width, height);
            width = result.Width;
            height = result.Height;
            x = result.X;
            y = result.Y;
        }
    }
}
