using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VideoFrameGrabber.FFmpegServicing
{
    /// <summary>
    /// Represents an FFmpeg executable servicer that exposes common FFmpeg operations for frame
    /// extraction.
    /// </summary>
    internal class FFmpegServicer : IFFmpegServicer
    {
        private const string LOGLEVEL_STATEMENT = "-loglevel error";

        private readonly string ffmpegLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="FFmpegServicer"/> class given a valid
        /// FFmpeg executable file.
        /// </summary>
        /// <param name="ffmpegPath">
        /// Path that leads to an FFmpeg executable. This can either be an absolute/relative path to
        /// an FFmpeg executable file, or an absolute/relative path to a directory containing
        /// <c>ffmpeg.exe</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ffmpegPath"/> was <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="ffmpegPath"/> was an empty string (excluding whitespace).
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// An FFmpeg executable could not be found from <paramref name="ffmpegPath"/>
        /// </exception>
        /// <exception cref="FormatException">
        /// The file found from <paramref name="ffmpegPath"/> is not a valid FFmpeg executable.
        /// </exception>
        public FFmpegServicer(string ffmpegPath)
        {
            if (ffmpegPath is null)
            {
                throw new ArgumentNullException(nameof(ffmpegPath));
            }
            ffmpegPath = ffmpegPath.Trim();
            if (ffmpegPath == "")
            {
                throw new ArgumentException($"No FFmpeg path specified ({nameof(ffmpegPath)} was empty).");
            }

            // If ffmpegPath is a directory path, check if ffmpeg.exe exists in the directory
            if (Directory.Exists(ffmpegPath))
            {
                string ffmpegPathInDirectory = Path.GetFullPath(ffmpegPath + "./ffmpeg.exe");
                if (!File.Exists(ffmpegPathInDirectory))
                {
                    throw new FileNotFoundException($"Folder path specified in {nameof(ffmpegPath)} does not contain ffmpeg.exe.");
                }
                ffmpegPath = ffmpegPathInDirectory;
            }
            // Otherwise, ffmpegPath should point to an existing FFmpeg executable file
            else if (!File.Exists(ffmpegPath))
            {
                throw new FileNotFoundException($"Could not find the FFmpeg file specified in {nameof(ffmpegPath)}.");
            }

            if (!FFmpegValid(ffmpegPath))
            {
                throw new FormatException($"The file specified by {nameof(ffmpegPath)} is not a valid FFmpeg executable.");
            }

            ffmpegLocation = Path.GetFullPath(ffmpegPath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FFmpegServicer"/> class with a specified
        /// exact path to an FFmpeg executable.
        /// </summary>
        /// <param name="internalFlag">
        /// A value used to differentiate from constructing with
        /// <see cref="FFmpegServicer(string)"/>. This can either be true or false (value is not
        /// used).
        /// </param>
        /// <param name="exactPath">An exact FFmpeg path that will be used by the instance.</param>
        /// <remarks>
        /// This constructor is not intended for public use and should only be called internally
        /// by constructor helper methods (like <see cref="FromSystem"/>). No checking is performed
        /// on <paramref name="exactPath"/>, so the path provided must be validated before calling
        /// this constructor.
        /// </remarks>
        private FFmpegServicer(bool internalFlag, string exactPath)
        {
            _ = internalFlag;
            ffmpegLocation = exactPath;
        }

        /// <summary>
        /// Attempts to create a new <see cref="FFmpegServicer"/> instance using a shared FFmpeg
        /// executable from the system, such as from System32 and from the PATH environment
        /// variable.
        /// </summary>
        /// <returns>A new <see cref="FFmpegServicer"/> instance.</returns>
        /// <exception cref="FileNotFoundException">
        /// FFmpeg could not be found in the system.
        /// </exception>
        /// <exception cref="FormatException">
        /// The found FFmpeg executable is not valid.
        /// </exception>
        /// <remarks>
        /// This method attempts to find an FFmpeg executable from the system using the
        /// <a href="https://learn.microsoft.com/en-us/windows/win32/api/shlwapi/nf-shlwapi-pathfindonpatha">PathFindOnPathA</a>
        /// Windows API function, which searches for a given file in the system's standard
        /// directories (like System32 and directories in PATH). An exception will be thrown if no
        /// FFmpeg executable if found or if the found FFmpeg executable is not valid.
        /// </remarks>
        public static FFmpegServicer FromSystem()
        {
            string? foundFFmpegPath = WinApiUtil.FindPathOfProgram("ffmpeg.exe");
            if (foundFFmpegPath is null)
            {
                throw new FileNotFoundException("Could not find a shared ffmpeg.exe in the system.");
            }
            if (!FFmpegValid(foundFFmpegPath))
            {
                throw new FormatException($"The shared ffmpeg.exe in the system is not a valid FFmpeg executable.");
            }

            return new FFmpegServicer(true, foundFFmpegPath);
        }

        /// <summary>
        /// Call FFmpeg with the given argument string to perform an action.
        /// </summary>
        /// <param name="args">The argument string to provide FFmpeg.</param>
        /// <exception cref="FFmpegErrorException">
        /// The FFmoeg execution threw an error.
        /// </exception>
        /// <remarks>
        /// <para>
        /// This method does not expect FFmpeg to return any results to standard output and thus
        /// does not return anything. This method is effectively an optimization of
        /// <see cref="CallFFmpegWithResult(string)"/> if the execution is not expected to return
        /// anything.
        /// </para>
        /// <para>
        /// Any FFmpeg execution called with the method will automatically have its loglevel set
        /// to 'error'. This means that FFmpeg will only output error-level statements or above.
        /// </para>
        /// </remarks>
        public void CallFFmpegWithoutResult(string args)
        {
            Process process = new();
            process.StartInfo.FileName = ffmpegLocation;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.Arguments = $"{LOGLEVEL_STATEMENT} {args}";

            process.Start();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (error.Length > 0)
            {
                throw new FFmpegErrorException(error);
            }
        }

        /// <summary>
        /// Call FFmpeg with the given argument string to perform an action with an expected return
        /// value.
        /// </summary>
        /// <inheritdoc cref="CallFFmpegWithoutResult(string)" path="/param[@name='args']"/>
        /// <inheritdoc cref="CallFFmpegWithoutResult(string)" path="/exception"/>
        /// <returns>The standard output of FFmpeg as a <see cref="byte"/> array.</returns>
        /// <remarks>
        /// <para>
        /// If FFmpeg does not return any output to standard output, an empty <see cref="byte"/>
        /// array will be returned.
        /// </para>
        /// <para>
        /// Any FFmpeg execution called with the method will automatically have its loglevel set
        /// to 'error'. This means that FFmpeg will only output error-level statements or above.
        /// </para>
        /// </remarks>
        public byte[] CallFFmpegWithResult(string args)
        {
            Process process = new();
            process.StartInfo.FileName = ffmpegLocation;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.Arguments = $"{LOGLEVEL_STATEMENT} {args}";

            process.Start();

            // Reading the std error needs to be asynchronous since reading from out and error
            // synchronously in series may result in a deadlock.
            Task<string> readErrorTask = process.StandardError.ReadToEndAsync();

            // Here, the std out is read synchronously
            byte[] output;
            int readBufferSize = 4096; // TODO: add mechanism for users to specify buffer size
            using (MemoryStream memoryStream = new())
            {
                byte[] readBuffer = new byte[readBufferSize];
                Stream outputStream = process.StandardOutput.BaseStream;
                int readCount = 0;
                while ((readCount = outputStream.Read(readBuffer, 0, readBufferSize)) != 0)
                {
                    memoryStream.Write(readBuffer, 0, readCount);
                }
                output = memoryStream.ToArray();
            }

            // Result of async std error read is collected. This may block if the FFmpeg process is
            // not done writing to std error.
            string error = readErrorTask.Result;

            process.WaitForExit();

            if (error.Length > 0)
            {
                throw new FFmpegErrorException(error);
            }

            return output;
        }

        /// <summary>
        /// Gets the metadata information of a video file.
        /// </summary>
        /// <param name="videoPath">Path to the video file.</param>
        /// <returns>
        /// A <see cref="VideoMetadata"/> struct that contains metadata information about the
        /// specified video file.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="videoPath"/> could not be found or accessed.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// FFmpeg could not pull the metadata from the video specified in
        /// <paramref name="videoPath"/>.
        /// </exception>
        public VideoMetadata GetVideoMetadata(string videoPath)
        {
            Process process = new();
            process.StartInfo.FileName = ffmpegLocation;

            // TODO: This is vulnerable to command injection, sanitize later
            process.StartInfo.Arguments = $"-hide_banner -i \"{videoPath}\" -f ffmetadata -";
            process.StartInfo.RedirectStandardError = true;

            TimeSpan? videoDuration = null;
            Tuple<int, int>? videoResolution = null;

            process.Start();
            StreamReader outputStream = process.StandardError; // Read from error since FFmpeg outputs to console through std::err
            string? outputLine = null;
            while ((outputLine = outputStream.ReadLine()?.Trim()) is not null)
            {
                if (outputLine.StartsWith("Duration"))
                {
                    videoDuration = ExtractDurationFromFFmpegLine(outputLine);
                }
                if (outputLine.StartsWith("Stream"))
                {
                    videoResolution = ExtractResolutionFromFFmpegLine(outputLine);
                }

                if (videoDuration is not null && videoResolution is not null)
                {
                    break;
                }
            }
            process.WaitForExit();

            if (videoDuration is null || videoResolution is null)
            {
                throw new ArgumentException($"Could not get metadata info from the video at {videoPath}");
            }

            return new VideoMetadata(videoResolution.Item1, videoResolution.Item2, videoDuration.Value);
        }

        /// <summary>
        /// Performs a surface-level validation of an FFmpeg executable file given its path.
        /// </summary>
        /// <param name="ffmpegPath">Path to the target file to validate.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="ffmpegPath"/> points to a valid FFmpeg executable file.
        /// Otherwise returns <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method only performs a surface-level validation of the given executable file.
        /// Specifically, it attempts to call the executable and checks if the first output line
        /// contains "ffmpeg".
        /// </remarks>
        private static bool FFmpegValid(string ffmpegPath)
        {
            Process process = new();
            process.StartInfo.FileName = ffmpegPath;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            try
            {
                process.Start();
                // FFmpeg outputs console outputs to standard error, not standard output
                string? firstLineOutput = process.StandardError.ReadLine();
                if (firstLineOutput?.Contains("ffmpeg") ?? false)
                {
                    return true;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// Extracts the duration value of a video from an FFmpeg output line as a
        /// <see cref="TimeSpan"/> instance.
        /// </summary>
        /// <param name="line">The FFmpeg output line that contains the video duration.</param>
        /// <returns>
        /// A <see cref="TimeSpan"/> corresponding to the video duration if successful. Otherwise
        /// returns <c>null</c>.
        /// </returns>
        private static TimeSpan? ExtractDurationFromFFmpegLine(string line)
        {
            // This Regex pattern searches for the "00:00:00.00" duration format. From what I
            // observed from calling FFmpeg, the millisecond value place always seems to be included
            // even if the duration does not have milliseconds. It also seems to always have exactly
            // two decimals.
            const string pattern = @"\d\d:\d\d:\d\d.\d\d";

            Match match = Regex.Match(line, pattern);
            if (!match.Success)
            {
                return null;
            }

            try
            {
                return TimeSpan.Parse(line.Substring(match.Index, match.Length));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Extracts the resolution values of a video from an FFmpeg output line as a
        /// <see cref="Tuple"/> pair of integers.
        /// </summary>
        /// <param name="line">The FFmpeg output line that contains the video resolution.</param>
        /// <returns>
        /// A <see cref="Tuple"/> corresponding to the video resolution if successful (horizontal
        /// resolution as the first value, vertical resolution as the second value). Otherwise
        /// returns <c>null</c>.
        /// </returns>
        private static Tuple<int, int>? ExtractResolutionFromFFmpegLine(string line)
        {
            // This regex pattern searches for the "0000x0000" resolution format. Specifically, it
            // searches for two integer values separated by an 'x' character. Each integer value
            // represents a size for each dimension of the video resolution. The integers must have
            // 2 - 5 digits (inclusive). The lower-bounds 2 is because some output from FFmpeg
            // includes integer numbers prepended with '0x'. The 2 is to ignore these values. The
            // upper bounds 5 is really just there since what fucking video file needs to have a
            // dimension larger than 99,999 pixel that would be insane.
            const string pattern = @"\d{2,5}x\d{2,5}";
            Match match = Regex.Match(line, pattern);
            if (!match.Success)
            {
                return null;
            }

            string resolutionString = line.Substring(match.Index, match.Length);
            int separatorIdx = resolutionString.IndexOf('x');

            // Don't need to check parse values since these are guaranteed to be digit characters
            // from the Regex above.
            int horizontalResolution = int.Parse(resolutionString.Substring(0, separatorIdx));
            int verticalResolution = int.Parse(resolutionString.Substring(separatorIdx + 1));

            return new Tuple<int, int>(horizontalResolution, verticalResolution);
        }
    }
}
