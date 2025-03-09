namespace VideoFrameGrabber.UnitTests.ClassFixtures;

/// <summary>
/// Provides wrong/non-existant paths to FFmpeg executable files for xUnit testing.
/// </summary>
public class WrongFFmpegPathDependencyFixture
{
    private const string WRONG_FFMPEG_PATH_DEPENDENCY_FOLDER = "./TestResources/WrongFFmpegPath";

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
    /// Initializes a <see cref="WrongFFmpegPathDependencyFixture"/> instance that provides relative
    /// and absolute file and folder paths that do not exist or do not contain an ffmpeg.exe file.
    /// </summary>
    public WrongFFmpegPathDependencyFixture()
    {
        RelativeFilePath = WRONG_FFMPEG_PATH_DEPENDENCY_FOLDER + "./ffmpeg.exe";
        AbsoluteFilePath = Path.GetFullPath(RelativeFilePath);

        RelativeFolderPath = WRONG_FFMPEG_PATH_DEPENDENCY_FOLDER;
        AbsoluteFolderPath = Path.GetFullPath(RelativeFolderPath);
    }
}
