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

            string? confirmedFFmpegLocation = CheckFFmpegInEnvironmentPath(ffmpegPath)
                ?? CheckFFmpegInDirectoryPath(ffmpegPath)
                ?? CheckFFmpegFromFilePath(ffmpegPath)
                ?? null;

            if (confirmedFFmpegLocation == null)
            {
                throw new FileNotFoundException($"Could not find the FFmpeg file specified in {nameof(ffmpegPath)}");
            }

            ValidateFFmpegFile(confirmedFFmpegLocation, nameof(ffmpegPath));
            ffmpegLocation = Path.GetFullPath(confirmedFFmpegLocation);
        }

        private string? CheckFFmpegInEnvironmentPath(string suggestedPath)
        {
            if (suggestedPath != "ffmpeg")
            {
                return null;
            }

            string? foundFFmpegFullPath = WinApiUtil.FindPathOfProgram("ffmpeg.exe");
            if (foundFFmpegFullPath is null)
            {
                throw new ArgumentException("Could not find ffmpeg.exe in PATH variable.");
            }

            return foundFFmpegFullPath;
        }

        private string? CheckFFmpegInDirectoryPath(string suggestedPath)
        {
            if (!Directory.Exists(suggestedPath))
            {
                return null;
            }

            string ffmpegPathInDirectory = Path.GetFullPath(suggestedPath + "./ffmpeg.exe");
            if (!File.Exists(ffmpegPathInDirectory))
            {
                throw new FileNotFoundException($"Folder path specified does not contain ffmpeg.exe");
            }

            return ffmpegPathInDirectory;
        }

        private string? CheckFFmpegFromFilePath(string suggestedPath)
        {
            return File.Exists(suggestedPath) ? suggestedPath : null;
        }

        /// <summary>
        /// Performs a surface-level validation of an FFmpeg executable file givien its path.
        /// </summary>
        /// <param name="ffmpegPath">Path to the target file to validate.</param>
        /// <param name="parameterName">
        /// Name of the higher-level parameter name that contains <paramref name="ffmpegPath"/>.
        /// Used to format exception messages.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="ffmpegPath"/> is not a valid FFmpeg executable file.
        /// </exception>
        /// <remarks>
        /// <para>
        /// This method only performs a surface-level validation of the given executable file.
        /// Specifically, it attempts to call the executable and checks if the first output line
        /// contains "ffmpeg".
        /// </para>
        /// <para>
        /// If the executable cannot be called, or if the executable does not output the word
        /// "ffmpeg" in the first line, a <see cref="ArgumentException"/> will be thrown.
        /// </para>
        /// </remarks>
        private void ValidateFFmpegFile(string ffmpegPath, string parameterName)
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
                    return;
                }
            }
            catch { }

            throw new ArgumentException($"The file specified by {parameterName} is not a valid FFmpeg executable.");
        }

    }
}
