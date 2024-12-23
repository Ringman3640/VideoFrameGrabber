using System;
using System.Diagnostics;
using System.IO;

namespace VideoFrameGrabber
{
    public class FrameGrabber
    {
        private readonly string ffmpegLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameGrabber"/> class given a valid
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
                throw new FormatException($"The file specified by {nameof(ffmpegPath)} is not a valid FFmpeg executable.");
            }

            ffmpegLocation = Path.GetFullPath(ffmpegPath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameGrabber"/> class with a specified
        /// exact path to an FFmpeg executable.
        /// </summary>
        /// <param name="internalFlag">
        /// A value used to differentiate from constructing with <see cref="FrameGrabber(string)"/>.
        /// This can either be true or false (value is not used).
        /// </param>
        /// <param name="exactPath">An exact FFmpeg path that will be used by the instance.</param>
        /// <remarks>
        /// This constructor is not intended for public use and should only be called internally
        /// by constructor helper methods (like <see cref="FromSystem"/>). No checking is performed
        /// on <paramref name="exactPath"/>, so the path provided must be validated before calling
        /// this constructor.
        /// </remarks>
        private FrameGrabber(bool internalFlag, string exactPath)
        {
            _ = internalFlag;
            ffmpegLocation = exactPath;
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
        /// <see cref="FrameGrabber(bool, string)"/> for internal instance construction.
        /// </remarks>
        /// <seealso cref="FrameGrabber(bool, string)"/>
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
        /// FFmpeg executable if found or if
        /// the found FFmpeg executable is not valid.
        /// </remarks>
        public static FrameGrabber FromSystem()
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

            return new FrameGrabber(true, foundFFmpegPath);
        }

        public byte[] ExtractFrame(string videoPath)
        {
            throw new NotImplementedException(nameof(ExtractFrame));
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
