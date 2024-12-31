using System;
using VideoFrameGrabber.Cropping;
using VideoFrameGrabber.ImageFormatting;
using VideoFrameGrabber.Scaling;

namespace VideoFrameGrabber
{
    /// <summary>
    /// Represents a collection of modifications to apply to a frame extraction operation.
    /// </summary>
    public class FrameExtractionModifiers
    {
        /// <summary>
        /// Gets or set the video seek time that specifies which frame should be extracted from a
        /// video file.
        /// </summary>
        /// <remarks>
        /// If the time indicated by <see cref="SeekTime"/> is longer than the length of a video
        /// file, that indicates that the last frame of the video file should be extracted.
        /// </remarks>
        public TimeSpan SeekTime { get; set; }

        /// <summary>
        /// Gets or sets the scaling modifier that should be applied to an extracted image frame.
        /// </summary>
        public ScaleProvider? Scaling { get; set; }

        /// <summary>
        /// Gets or sets the cropping modifier that should be applied to an extracted image frame.
        /// </summary>
        public ICropProvider? Cropping { get; set; }

        /// <summary>
        /// Indicates the order of scaling and cropping applied to an extracted image frame.
        /// </summary>
        public ScaleCropOrder ModifierOrder { get; set; }

        /// <summary>
        /// Gets or sets the image format that extracted image frames should be formatted to.
        /// </summary>
        public IImageFormatProvider? ImageFormat { get; set; }
    }
}
