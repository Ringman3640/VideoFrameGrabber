using System;

namespace VideoFrameGrabber.Cropping
{
    /// <summary>
    /// Represents a <see cref="CropProvider"/> of constant size.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is immutable.
    /// </para>
    /// </remarks>
    public class SizeCropper : CropProvider
    {
        /// <summary>
        /// Gets the pixel width length that this <see cref="SizeCropper"/> will apply.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the pixel hight length that this <see cref="SizeCropper"/> will apply.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the pixel X (horizontal) offset value that this <see cref="SizeCropper"/> will
        /// apply.
        /// </summary>
        /// <remarks>
        /// This value is only used if the value of <see cref="Align"/> is
        /// <see cref="CropAlign.None"/>.
        /// </remarks>
        public int X { get; }

        /// <summary>
        /// Gets the pixel Y (vertical) offset value that this <see cref="SizeCropper"/> will apply.
        /// </summary>
        /// <inheritdoc cref="X" path="/remarks"/>
        public int Y { get; }

        /// <summary>
        /// Gets the <see cref="CropAlign"/> used to align the crop frame during a crop operation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value is used to compute X and Y offset values of the crop frame to align it to
        /// a specific directional position.
        /// </para>
        /// <para>
        /// If this value is <see cref="CropAlign.None"/>, align is ignored and <see cref="X"/> and
        /// <see cref="Y"/> is used to provide an absolute offset position.
        /// </para>
        /// </remarks>
        public CropAlign Align { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeCropper"/> class with the specified
        /// crop width and height values.
        /// </summary>
        /// <param name="width">The pixel width of the crop.</param>
        /// <param name="height">The pixel height of the crop.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="width"/> or <paramref name="height"/> was less than
        /// <see cref="CropProvider.MinimumCropDimensionSize"/>.
        /// </exception>
        /// <remarks>
        /// This constructor will initialize a <see cref="SizeCropper"/> that has X and Y offset
        /// values of 0.
        /// </remarks>
        public SizeCropper(int width, int height)
        {
            if (width < MinimumCropDimensionSize)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            if (height < MinimumCropDimensionSize)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            Width = width;
            Height = height;
            X = 0;
            Y = 0;
            Align = CropAlign.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeCropper"/> class with the specified
        /// crop with, crop height, X offset, and Y offset values.
        /// </summary>
        /// <inheritdoc cref="SizeCropper(int, int)" path="/param[@name='width']"/>
        /// <inheritdoc cref="SizeCropper(int, int)" path="/param[@name='height']"/>
        /// <param name="x">The pixel X offset of the crop.</param>
        /// <param name="y">The pixel Y offset of the crop.</param>
        /// <inheritdoc cref="SizeCropper(int, int)" path="/exception[@cref='ArgumentOutOfRangeException']"/>
        public SizeCropper(int width, int height, int x, int y)
        {
            if (width < MinimumCropDimensionSize)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            if (height < MinimumCropDimensionSize)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            Width = width;
            Height = height;
            X = x;
            Y = y;
            Align = CropAlign.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeCropper"/> class with the specified
        /// crop with, crop height, and <see cref="CropAlign"/> values.
        /// </summary>
        /// <inheritdoc cref="SizeCropper(int, int)" path="/param[@name='width']"/>
        /// <inheritdoc cref="SizeCropper(int, int)" path="/param[@name='height']"/>
        /// <param name="align">The <see cref="CropAlign"/> position value of the crop.</param>
        /// <inheritdoc cref="SizeCropper(int, int)" path="/exception[@cref='ArgumentOutOfRangeException']"/>
        public SizeCropper(int width, int height, CropAlign align)
        {
            if (width < MinimumCropDimensionSize)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            if (height < MinimumCropDimensionSize)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            Width = width;
            Height = height;
            X = 0;
            Y = 0;
            Align = align;
        }

        protected override void PerformCrop(ref int width, ref int height, ref int x, ref int y)
        {
            if (Align == CropAlign.None)
            {
                width = Width;
                height = Height;
                x = X;
                y = Y;
            }
            else
            {
                CropOffset offset = GetCropOffsetFromAlign(Width, Height, width, height, Align);

                width = Width;
                height = Height;
                x = offset.X;
                y = offset.Y;
            }
        }
    }
}
