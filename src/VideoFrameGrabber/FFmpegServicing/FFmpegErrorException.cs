﻿using System;

namespace VideoFrameGrabber.FFmpegServicing
{
    /// <summary>
    /// The exception that is thrown when an FFmpeg execution produces an error.
    /// </summary>
    internal class FFmpegErrorException : Exception
    {
        private const string BASE_MESSAGE = "The FFmpeg execution threw an error.";

        /// <summary>
        /// Gets the error message generated by the FFmpeg execution.
        /// </summary>
        public string FFmpegError { get; }

        /// <summary>
        /// Initializes a new <see cref="FFmpegErrorException"/> instance with the given FFmpeg
        /// error message.
        /// </summary>
        /// <param name="ffmpegError"></param>
        public FFmpegErrorException(string ffmpegError) : base(BASE_MESSAGE)
        {
            FFmpegError = ffmpegError;
        }
    }
}
