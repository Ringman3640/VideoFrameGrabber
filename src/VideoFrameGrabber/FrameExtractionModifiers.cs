using System;
using VideoFrameGrabber.Scaling;

namespace VideoFrameGrabber
{
    public class FrameExtractionModifiers
    {
        public TimeSpan SeekTime { get; set; }

        public IScaleProvider? Scaling { get; set; }
    }
}
