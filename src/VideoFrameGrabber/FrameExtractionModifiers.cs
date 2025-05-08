using System;
using VideoFrameGrabber.Cropping;
using VideoFrameGrabber.Scaling;

namespace VideoFrameGrabber
{
    /// <summary>
    /// Represents a collection of modifications to apply to a frame extraction operation.
    /// </summary>
    public class FrameExtractionModifiers
    {
        private readonly object mutex = new();

        /// <summary>
        /// Gets or set the video seek time that specifies which frame should be extracted from a
        /// video file.
        /// </summary>
        /// <remarks>
        /// If the time indicated by <see cref="SeekTime"/> is longer than the length of a video
        /// file, that indicates that the last frame of the video file should be extracted.
        /// </remarks>
        public TimeSpan SeekTime
        {
            get
            {
                lock (mutex)
                {
                    return seekTime;
                }
            }
            set
            {
                lock (mutex)
                {
                    seekTime = value;
                }
            }
        }
        private TimeSpan seekTime;

        /// <summary>
        /// Gets or sets the scaling modifier that should be applied to an extracted image frame.
        /// </summary>
        public ScaleProvider? Scaling
        {
            get
            {
                lock (mutex)
                {
                    return scaling;
                }
            }
            set
            {
                lock (mutex)
                {
                    scaling = value;
                }
            }
        }
        ScaleProvider? scaling;

        /// <summary>
        /// Gets or sets the cropping modifier that should be applied to an extracted image frame.
        /// </summary>
        public CropProvider? Cropping
        {
            get
            {
                lock (mutex)
                {
                    return cropping;
                }
            }
            set
            {
                lock (mutex)
                {
                    cropping = value;
                }
            }
        }
        CropProvider? cropping;

        /// <summary>
        /// Indicates the order of scaling and cropping applied to an extracted image frame.
        /// </summary>
        public ScaleCropOrder Order
        {
            get
            {
                lock (mutex)
                {
                    return order;
                }
            }
            set
            {
                lock (mutex)
                {
                    order = value;
                }
            }
        }
        ScaleCropOrder order;

        /// <summary>
        /// Gets or sets the image format that extracted image frames should be formatted to.
        /// </summary>
        public ImageFormat? ImageFormat
        {
            get
            {
                lock (mutex)
                {
                    return imageFormat;
                }
            }
            set
            {
                lock (mutex)
                {
                    imageFormat = value;
                }
            }
        }
        ImageFormat? imageFormat;

        /// <summary>
        /// Creates a shallow copy of the <see cref="FrameExtractionModifiers"/> instance in an
        /// atomic proceedure.
        /// </summary>
        /// <returns>
        /// A shallow copy of the current <see cref="FrameExtractionModifiers"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method is intended to get a copy of a <see cref="FrameExtractionModifiers"/>
        /// instance that is accessed by multiple threads. It is guaranteed that no properties of
        /// the <see cref="FrameExtractionModifiers"/> will change values when the copy starts, up
        /// until the copy is completed.
        /// </para>
        /// <para>
        /// This method only performs a shallow copy since all properties of
        /// <see cref="FrameExtractionModifiers"/> are immutable. Even if a reference to a property
        /// is shared in multiple locations, the reference itself cannot change in value(s).
        /// </para>
        /// </remarks>
        public FrameExtractionModifiers GetThreadSafeCopy()
        {
            lock (mutex)
            {
                FrameExtractionModifiers shallowCopy = new();

                shallowCopy.seekTime = seekTime;
                shallowCopy.scaling = scaling;
                shallowCopy.cropping = cropping;
                shallowCopy.order = order;
                shallowCopy.imageFormat = imageFormat;

                return shallowCopy;
            }
        }
    }
}
