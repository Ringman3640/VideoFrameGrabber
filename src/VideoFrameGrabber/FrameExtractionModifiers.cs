using System;
using VideoFrameGrabber.Cropping;
using VideoFrameGrabber.ImageFormatting;
using VideoFrameGrabber.Scaling;

namespace VideoFrameGrabber
{
    public class FrameExtractionModifiers
    {
        public TimeSpan SeekTime { get; set; }

        public IScaleProvider? Scaling { get; set; }

        public ICropProvider? Cropping { get; set; }

        public ScaleCropOrder ModifierOrder { get; set; }

        public IImageFormatProvider? ImageFormat { get; set; }
    }
}
