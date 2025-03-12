namespace VideoFrameGrabber.IntegrationTests.StaticDependencies;

/// <summary>
/// Provides wrong/non-existant paths to FFmpeg executable files for xUnit testing.
/// </summary>
/// /// The <see cref="WrongFFmpegPathDependency"/> is a singleton class that provides absolute and
/// relative paths to an existing empty directory. This directory is intended to be used to test
/// <see cref="FrameGrabber"/> when given a path that does not have FFmpeg.
/// </remarks>
public class WrongFFmpegPathDependency
{
    private const string WRONG_FFMPEG_PATH_DEPENDENCY_FOLDER = "./TestResources/WrongFFmpegPath";

    private static WrongFFmpegPathDependency? _instance = null;
    private static readonly object instanceLock = new();

    /// <summary>
    /// Gets the static global <see cref="WrongFFmpegPathDependency"/> instance.
    /// </summary>
    public static WrongFFmpegPathDependency Instance
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
    /// Gets an absolute path to an FFmpeg executable file that does not exist.
    /// </summary>
    public string AbsoluteFilePath { get; private set; }

    /// <summary>
    /// Gets a relative path to an FFmpeg executable file that does not exist.
    /// </summary>
    public string RelativeFilePath { get; private set; }

    /// <summary>
    /// Gets an absolute path to a folder that does not contain FFmpeg.
    /// </summary>
    public string AbsoluteFolderPath { get; private set; }

    /// <summary>
    /// Gets a relative path to a folder that does not contain FFmpeg.
    /// </summary>
    public string RelativeFolderPath { get; private set; }

    /// <summary>
    /// Initializes a <see cref="WrongFFmpegPathDependency"/> instance that provides relative
    /// and absolute file and folder paths that do not exist or do not contain an ffmpeg.exe file.
    /// </summary>
    private WrongFFmpegPathDependency()
    {
        RelativeFilePath = WRONG_FFMPEG_PATH_DEPENDENCY_FOLDER + "./ffmpeg.exe";
        AbsoluteFilePath = Path.GetFullPath(RelativeFilePath);

        RelativeFolderPath = WRONG_FFMPEG_PATH_DEPENDENCY_FOLDER;
        AbsoluteFolderPath = Path.GetFullPath(RelativeFolderPath);
    }
}
