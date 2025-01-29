using System;
using VideoFrameGrabber.FFmpegServicing;

namespace VideoFrameGrabber
{
    public class FrameGrabber
    {
        private readonly IFFmpegServicer ffmpegServicer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameGrabber"/> class given a valid
        /// FFmpeg executable file.
        /// </summary>
        /// <inheritdoc cref="FFmpegServicer(string)"/>
        public FrameGrabber(string ffmpegPath)
        {
            ffmpegServicer = new FFmpegServicer(ffmpegPath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameGrabber"/> class with a specified
        /// <see cref="IFFmpegServicer"/> instance.
        /// </summary>
        /// <param name="ffmpegServicer">
        /// An <see cref="IFFmpegServicer"/> instance that will be used by the newly-created
        /// <see cref="FrameGrabber"/> instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ffmpegServicer"/> is null.
        /// </exception>
        /// <remarks>
        /// This constructor is not intended for public use and should only be called internally
        /// by constructor helper methods (like <see cref="FromSystem"/>), or for testing. No
        /// validation checking is performed on <paramref name="ffmpegServicer"/>.
        /// </remarks>
        internal FrameGrabber(IFFmpegServicer ffmpegServicer)
        {
            if (ffmpegServicer is null)
            {
                throw new ArgumentNullException(nameof(ffmpegServicer));
            }
            this.ffmpegServicer = ffmpegServicer;
        }

        /// <summary>
        /// DO NOT CALL THIS CONSTRUCTOR NO NO NO NO NO NO NEVER NO. This is the default constructor
        /// for <see cref="FrameGrabber"/> that will ALWAYS throw an exception if called.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// You called this constructor. Do not call this constructor.
        /// </exception>
        /// <remarks>
        /// This is the default constructor for <see cref="FrameGrabber"/>. It is marked private to
        /// indicate that this constructor should not be called. This is because a
        /// <see cref="FrameGrabber"/> instance NEEDS a path to FFmpeg to be in a valid state. Use
        /// <see cref="FrameGrabber(string)"/> for general use or
        /// <see cref="FrameGrabber(FFmpegServicer)"/> for internal instance construction.
        /// </remarks>
        /// <seealso cref="FrameGrabber(string)"/>
        /// <seealso cref="FrameGrabber(FFmpegServicer)"/>
        private FrameGrabber()
        {
            throw new InvalidOperationException($"{nameof(FrameGrabber)} cannot be initialized with default constructor.");
        }

        /// <summary>
        /// Attempts to create a new <see cref="FrameGrabber"/> instance using a shared FFmpeg
        /// executable from the system, such as from System32 and from the PATH environment
        /// variable.
        /// </summary>
        /// <returns>A new <see cref="FrameGrabber"/> instance.</returns>
        /// <inheritdoc cref="FFmpegServicer.FromSystem"/>
        public static FrameGrabber FromSystem()
        {
            FFmpegServicer ffmpegServicer = FFmpegServicer.FromSystem();
            return new FrameGrabber(ffmpegServicer);
        }

        public byte[] ExtractFrame(string videoPath)
        {
            if (videoPath is null)
            {
                throw new ArgumentNullException(nameof(videoPath));
            }
            videoPath = videoPath.Trim();
            if (videoPath.Length == 0)
            {
                throw new ArgumentException($"{nameof(videoPath)} does not contain a path.");
            }

            throw new NotImplementedException(nameof(ExtractFrame));
        }
    }
}
