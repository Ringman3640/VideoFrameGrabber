using System;
using System.Diagnostics;
using System.IO;

namespace VideoFrameGrabber
{
    public class FrameGrabber
    {
        private readonly string ffmpegLocation;

        public FrameGrabber(string ffmpegPath)
        {
            if (ffmpegPath is null)
            {
                throw new ArgumentNullException(nameof(ffmpegPath));
            }
            ffmpegPath.Trim();
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
                throw new ArgumentException($"The file specified by {nameof(ffmpegPath)} is not a valid FFmpeg executable.");
            }

            ffmpegLocation = Path.GetFullPath(ffmpegPath);
        }

        private FrameGrabber(bool internalFlag, string exactPath)
        {
            _ = internalFlag;
            ffmpegLocation = exactPath;
        }

        public static FrameGrabber FromSystem()
        {
            string? foundFFmpegPath = WinApiUtil.FindPathOfProgram("ffmpeg.exe");
            if (foundFFmpegPath is null)
            {
                throw new ArgumentException("Could not find a shared ffmpeg.exe in the system.");
            }
            if (!FFmpegValid(foundFFmpegPath))
            {
                throw new ArgumentException($"The shared ffmpeg.exe in the system is not a valid FFmpeg executable.");
            }

            return new FrameGrabber(true, foundFFmpegPath);
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
            Process process = new Process();
            process.StartInfo.FileName = ffmpegPath;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            try
            {
                process.Start();
                // FFmpeg outputs console outputs to standard error, not standard output
                string firstLineOutput = process.StandardError.ReadLine();
                if (firstLineOutput.Contains("ffmpeg"))
                {
                    return true;
                }
            }
            catch { }

            return false;
        }

    }
}
