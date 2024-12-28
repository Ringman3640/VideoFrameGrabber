using System;
using VideoFrameGrabber.Cropping;
using VideoFrameGrabber.Scaling;

namespace VideoFrameGrabber
{
    public class FrameExtractionModifiers
    {
        public TimeSpan SeekTime { get; set; }

        public IScaleProvider? Scaling { get; set; }

        public ICropProvider? Cropping { get; set; }
    }
}
