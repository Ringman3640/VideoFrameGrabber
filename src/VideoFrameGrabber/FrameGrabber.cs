using System;
using VideoFrameGrabber.FFmpegServicing;

namespace VideoFrameGrabber
{
    public class FrameGrabber
    {
        private readonly IFFmpegServicer ffmpegServicer;

        /// <summary>
        /// Gets or sets the <see cref="FrameExtractionModifiers"/> instance that this
        /// <see cref="FrameGrabber"/> uses by default when extracting a frame.
        /// </summary>
        public FrameExtractionModifiers Modifiers { get; set; } = new();

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

        /// <summary>
        /// Exposes static methods for formatting strings needed for FFmpeg operations.
        /// </summary>
        /// <remarks>
        /// This subclass is functionally a private subclass. However, it is marked internal for
        /// testing accessibility. Other classes within this assembly should not call methods in
        /// this class.
        /// </remarks>
        internal static class FFmpegFormatting
        {
            /// <summary>
            /// Gets the FFmpeg arguments string for extracting a frame at a specific seek time with
            /// the output piped to stdout.
            /// </summary>
            /// <remarks>
            /// <para>
            /// This method will return an FFmpeg arguments string with the following format:
            /// </para>
            /// <para>
            /// <c>
            /// -ss [Seek Time] -i "[Video Path]" -v:f "[Filters]" -vframes 1 -f image2pipe -c:v [Codec] -
            /// </c>
            /// </para>
            /// </remarks>
            internal static string GetFFmpegArgs(TimeSpan seekTime, string videoPath, string filters, ImageFormat format)
            {
                string seekTimeTerm = FormatSeekTime(seekTime);
                string codecTerm = format.VideoCodecName;
                return $"-ss {seekTimeTerm} -i \"{videoPath}\" -v:f \"{filters}\" -vframes 1 -f image2pipe -c:v {codecTerm} -";
            }

            /// <summary>
            /// Gets the FFmpeg arguments string for extracting a frame at a specific seek time with
            /// the output saved to a specific path.
            /// </summary>
            /// <remarks>
            /// <para>
            /// This method will return an FFmpeg arguments string with the following format:
            /// </para>
            /// <para>
            /// <c>
            /// -ss [Seek Time] -i "[Video Path]" -v:f "[Filters]" -vframes 1 "[Output Path]"
            /// </c>
            /// </para>
            /// </remarks>
            internal static string GetFFmpegArgs(TimeSpan seekTime, string videoPath, string filters, string outputPath)
            {
                string seekTimeTerm = FormatSeekTime(seekTime);
                return $"-ss {seekTimeTerm} -i \"{videoPath}\" -v:f \"{filters}\" -vframes 1 \"{outputPath}\"";
            }

            /// <summary>
            /// Gets the FFmpeg arguments string for extracting the last frame of a video with the
            /// output piped to stdout.
            /// </summary>
            /// <summary>
            /// Gets the FFmpeg arguments string for extracting a frame at a specific seek time with
            /// the output saved to a specific path.
            /// </summary>
            /// <remarks>
            /// <para>
            /// This method will return an FFmpeg arguments string with the following format:
            /// </para>
            /// <para>
            /// <c>
            /// -sseof -3 -i "[Video Path]" -update 1 -v:f "[Filters]" -f image2pipe -c:v [Codec] -
            /// </c>
            /// </para>
            /// </remarks>
            internal static string GetFFmpegArgs(string videoPath, string filters, ImageFormat format)
            {
                string codecTerm = format.VideoCodecName;
                return $"-sseof -3 -i \"{videoPath}\" -update 1 -v:f \"{filters}\" -f image2pipe -c:v {codecTerm} -";
            }

            /// <summary>
            /// Gets the FFmpeg arguments string for extracting the last frame of a video with the
            /// output saved to a specific path.
            /// </summary>
            /// <summary>
            /// Gets the FFmpeg arguments string for extracting a frame at a specific seek time with
            /// the output saved to a specific path.
            /// </summary>
            /// <remarks>
            /// <para>
            /// This method will return an FFmpeg arguments string with the following format:
            /// </para>
            /// <para>
            /// <c>
            /// -sseof -3 -i "[Video Path]" -update 1 -v:f "[Filters]" "[Output Path]"
            /// </c>
            /// </para>
            /// </remarks>
            internal static string GetFFmpegArgs(string videoPath, string filters, string outputPath)
            {
                return $"-sseof -3 -i \"{videoPath}\" -update 1 -v:f \"{filters}\" \"{outputPath}\"";
            }

            /// <summary>
            /// Converts a <see cref="TimeSpan"/> seek time into a string representation appropriate
            /// for an FFmpeg seek time argument string.
            /// </summary>
            /// <param name="time">The <see cref="TimeSpan"/> instance to convert.</param>
            /// <returns>
            /// An appropriate <see cref="string"/> representation of <paramref name="time"/> for
            /// FFmpeg.
            /// </returns>
            /// <remarks>
            /// <para>
            /// There are two supported formats for the FFmpeg seek time argument. A sexagesimal
            /// time representation in the form HH:MM:SS:MS, or just seconds with an optional
            /// decimal place for milliseconds. This method returns the seconds format for
            /// simplicity.
            /// </para>
            /// <para>
            /// If the value of <paramref name="time"/> is negative, the returned string will be
            /// "0".
            /// </para>
            /// </remarks>
            internal static string FormatSeekTime(TimeSpan time)
            {
                double totalSeconds = Math.Max(0d, time.TotalSeconds);
                return totalSeconds.ToString();
            }
        }
    }
}
