namespace VideoFrameGrabber.IntegrationTests.StaticDependencies;

/// <summary>
/// Provides fake FFmpeg dependencies for xUnit tests.
/// </summary>
/// <remarks>
/// The <see cref="FakeFFmpegDependency"/> is a singleton class that provides absolute and relative
/// paths to a fake FFmpeg executable, or a directory containing a fake FFmpeg executable. This
/// is intended to be used for testing how <see cref="FrameGrabber"/> handles potentially
/// incorrect FFmpeg files.
public class FakeFFmpegDependency
{
    private const string FAKE_FFMPEG_DEPENDENCY_FOLDER = "./TestResources/FakeFFmpeg";

    private static FakeFFmpegDependency? _instance = null;
    private static readonly object instanceLock = new();

    /// <summary>
    /// Gets the static global <see cref="FakeFFmpegDependency"/> instance.
    /// </summary>
    public static FakeFFmpegDependency Instance
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
    /// Gets an absolute file path to a fake FFmpeg executable.
    /// </summary>
    public string AbsoluteFilePath { get; private set; }

    /// <summary>
    /// Gets a relative file path from the base executable directory to a fake FFmpeg executable.
    /// </summary>
    public string RelativeFilePath { get; private set; }

    /// <summary>
    /// Gets an absolute path to a folder containing a fake FFmpeg executable.
    /// </summary>
    public string AbsoluteFolderPath { get; private set; }

    /// <summary>
    /// Gets a relative path from the base executable directory to a folder containing a fake FFmpeg
    /// executable.
    /// </summary>
    public string RelativeFolderPath { get; private set; }

    /// <summary>
    /// Initializes a <see cref="FakeFFmpegDependency"/> instance used to provide absolute
    /// and relative paths to a fake FFmpeg executable.
    /// </summary>
    /// <remarks>
    /// <see cref="FakeFFmpegDependency"/> will provide paths to a fake FFmpeg executable.
    /// Specifically, the fake FFmpeg executable is an invalid executable file that will not
    /// properly execute on any Windows system. Use this executbale to test for FFmpeg validation.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// A local FFmpeg executable cannot be found after attempting to download. Check
    /// <see cref="FakeFFmpegDependency"/> implementation.
    /// </exception>
    private FakeFFmpegDependency()
    {
        string relativeUnformattedPath = Path.Join(FAKE_FFMPEG_DEPENDENCY_FOLDER, "./ffmpeg.exe");
        AbsoluteFilePath = Path.GetFullPath(relativeUnformattedPath);
        RelativeFilePath = "./" + Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, AbsoluteFilePath);
        AbsoluteFolderPath = Path.GetFullPath(FAKE_FFMPEG_DEPENDENCY_FOLDER);
        RelativeFolderPath = FAKE_FFMPEG_DEPENDENCY_FOLDER;

        if (!File.Exists(AbsoluteFilePath))
        {
            throw new InvalidOperationException("Failed to get fake FFmpeg executable path");
        }
    }
}
