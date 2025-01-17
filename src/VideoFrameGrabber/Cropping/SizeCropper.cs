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
            throw new System.NotImplementedException();
        }

        public SizeCropper(int width, int height, int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public SizeCropper(int width, int height, CropAlign align)
        {
            throw new System.NotImplementedException();
        }

        protected override void PerformCrop(ref int width, ref int height, ref int x, ref int y)
        {
            throw new System.NotImplementedException();
        }
    }
}
