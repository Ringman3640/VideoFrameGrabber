using System;

namespace VideoFrameGrabber.Cropping
{
    public class SizeCropper : CropProvider
    {
        public int Width { get; }

        public int Height { get; }

        public int X { get; }

        public int Y { get; }

        public CropAlign Align { get; }

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
