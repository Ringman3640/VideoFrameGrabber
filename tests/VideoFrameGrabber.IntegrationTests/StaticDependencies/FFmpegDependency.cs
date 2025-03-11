using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VideoFrameGrabber.IntegrationTests.StaticDependencies;

/// <summary>
/// Provides FFmpeg dependencies for xUnit tests.
/// </summary>
public class FFmpegDependency
{
    private static FFmpegDependency? _instance = null;
    private static readonly object instanceLock = new();

    /// <summary>
    /// Gets the static global FFmpegDependency instance.
    /// </summary>
    /// <remarks>
    /// The first class is initialized when <see cref="Instance"/> is first called. During
    /// initialization, the class assumes that FFmpeg is located on the PATH environment variable
    /// and gets absolute and relative file and containing folder paths for the FFmpeg executable.
    /// </remarks>
    public static FFmpegDependency Instance
    {
        get
        {
            lock (instanceLock)
            {
                _instance ??= new();
                return _instance;
            }
        }
    }

    /// <summary>
    /// Gets a absolute file path to a local FFmpeg executable.
    /// </summary>
    public string AbsoluteFilePath { get; private set; } = "";

    /// <summary>
    /// Gets a relative file path from the base executable directory to a local FFmpeg executable.
    /// </summary>
    public string RelativeFilePath { get; private set; } = "";

    /// <summary>
    /// Gets an absolute path to a folder that contains a local FFmpeg executable.
    /// </summary>
    public string AbsoluteFolderPath { get; private set; } = "";

    /// <summary>
    /// Gets a relative path from the base executable directory to a folder that contains a local
    /// FFmpeg executable.
    /// </summary>
    public string RelativeFolderPath { get; private set; } = "";

    /// <summary>
    /// Initializes an <see cref="FFmpegDependency"/> instance used to provide relative and
    /// absolute paths to an FFmpeg executable.
    /// </summary>
    /// <remarks>
    /// Creating an <see cref="FFmpegDependency"/> will attempt to search for an FFmpeg
    /// executable on the global PATH environment variable.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// A local FFmpeg executable cannot be found after attempting to download. Check
    /// <see cref="FFmpegDependency"/> implementation.
    /// </exception>
    /// <inheritdoc cref="FindFFmpegFile" path="/"/>
    private FFmpegDependency()
    {
        AbsoluteFilePath = FindFFmpegFile();
        RelativeFilePath = "./" + Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, AbsoluteFilePath);
        AbsoluteFolderPath = Path.GetDirectoryName(AbsoluteFilePath)!;
        RelativeFolderPath = "./" + Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, AbsoluteFolderPath);
    }

    /// <summary>
    /// Gets an absolute path to a system-accessible FFmpeg executable.
    /// </summary>
    /// <returns>An absolute file path to FFmpeg.</returns>
    /// <exception cref="InvalidOperationException">
    /// Attempted to run on an OS that is not Windows or Linux.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Could not find FFmpeg on PATH.
    /// </exception>
    private string FindFFmpegFile()
    {
        Process process = new();
        process.StartInfo.RedirectStandardOutput = true;

        // The Process class is used to execute a command to find the absolute path to a global
        // program. This command is system-dependent and is different for Windows and Linux.
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // The "where" command can be used on Windows to find the file path. It only needs the
            // name of the executable to find.
            process.StartInfo.FileName = "where";
            process.StartInfo.Arguments = "ffmpeg";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // The "whereis" command is used on Linux to find the file path. It also needs the name
            // of the executable. The "-b" argument specifies to only return the binary path. It
            // would also include manuals otherwise.
            process.StartInfo.FileName = "whereis";
            process.StartInfo.Arguments = "-b ffmpeg";
        }
        else
        {
            throw new InvalidOperationException("Only Windows and Linux systems are supported");
        }

        process.Start();
        StreamReader reader = process.StandardOutput;
        string processOutput = reader.ReadToEnd();
        process.WaitForExit();

        // Since different commands are used for Windows and Linux, the results need to be parsed
        // differently.
        string ffmpegPath = "";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // On Windows, the output of "where" is simply the path of the executable if found. If
            // not found, it returns "INFO: Could not find files for the given pattern(s)."
            if (!processOutput.StartsWith("INFO:"))
            {
                ffmpegPath = processOutput.Trim();
            }
        }
        else
        {
            // On Linux, the output of "whereis" starts with the requested file name and a colon.
            // It then displays the file path a space after the colon, if found.
            // EX:
            // ~ whereis -b ffmpeg
            // ~ ffmpeg: [ffmpeg location]
            //           OR
            // ~ ffmpeg:
            int substringStart = processOutput.IndexOf(":") + 2; // +2 to move the start after ": "
            ffmpegPath = processOutput.Substring(substringStart).Trim();
        }

        if (string.IsNullOrEmpty(ffmpegPath))
        {
            throw new InvalidOperationException("Could not find FFmpeg in system");
        }

        return ffmpegPath;
    }
}
