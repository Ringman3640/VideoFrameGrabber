using System;
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

            // Case: ffmpegPath specifies ffmpeg.exe is in PATH
            if (ffmpegPath == "ffmpeg")
            {
                string? foundFFmpegFullPath = WinApiUtil.FindPathOfProgram("ffmpeg.exe");
                if (foundFFmpegFullPath is null)
                {
                    throw new ArgumentException("Could not find ffmpeg.exe in PATH variable.");
                }
                // ValidateFFmpegFile(foundFFmpegFullPath)
                ffmpegLocation = foundFFmpegFullPath;
                return;
            }

            FileAttributes ffmpegPathAttributes;
            try
            {
                ffmpegPathAttributes = File.GetAttributes(ffmpegPath);
            }
            catch (Exception except)
            {
                FormatExceptionFromFileGetAttributes(except, nameof(ffmpegPath));

                // Rethrow exception if FormatExceptionFromFileGetAttributes cannot reformat the
                // exception.
                // This will also guarantee to the compiler that ffmpegPathAttributes is initialized
                // after exiting the try catch block
                throw;
            }

            // Case: ffmpegPath specifies a directory with ffmpeg.exe
            if (ffmpegPathAttributes.HasFlag(FileAttributes.Directory))
            {
                string ffmpegPathInDirectory = Path.GetFullPath(ffmpegPath + "./ffmpeg.exe");
                if (!File.Exists(ffmpegPathInDirectory))
                {
                    throw new FileNotFoundException($"Folder path specified in {nameof(ffmpegPath)} does not contain ffmpeg.exe");
                }
                // ValidateFFmpegFile(ffmpegPathInDirectory);
                ffmpegLocation = ffmpegPathInDirectory;
                return;
            }

            // Case: ffmpegPath is a direct path to an ffmpeg executable
            string fullDirectFfmpegPath = Path.GetFullPath(ffmpegPath);
            if (!File.Exists(fullDirectFfmpegPath))
            {
                throw new FileNotFoundException($"Could not find the FFmpeg file specified in {nameof(ffmpegPath)}");
            }
            // ValidateFFmpegFile(fullDirectFfmpegPath);
            ffmpegLocation = fullDirectFfmpegPath;
        }

        /// <summary>
        /// Reformats an exception thrown by <see cref="File.GetAttributes(string)"/>.
        /// </summary>
        /// <param name="except">
        /// The exception thrown by <see cref="File.GetAttributes(string)"/>.
        /// </param>
        /// <param name="parameterName">
        /// Name of the method parameter that contains the exception-causing path.
        /// </param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private void FormatExceptionFromFileGetAttributes(Exception except, string parameterName)
        {
            if (except is FileNotFoundException)
            {
                throw new FileNotFoundException($"Could not find a file at path specified in {parameterName}.");
            }
            if (except is UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException($"Caller does not have permission to acces the file specified in {parameterName}.");
            }
        }

        private void ValidateFFmpegFile(string ffmpegPath)
        {
            throw new NotImplementedException(nameof(ValidateFFmpegFile));
        }

    }
}
