using System.IO.Compression;
using System.Net;

namespace VideoFrameGrabber.IntegrationTests.StaticDependencies;

/// <summary>
/// Provides FFmpeg dependencies for xUnit tests.
/// </summary>
public class FFmpegDependency
{
    private static FFmpegDependency? _instance = null;
    private static readonly object instanceLock = new();

    private const string FFMPEG_DEPENDENCY_FOLDER = "./TestResources/FullFFmpeg";
    private const string FFMPEG_DOWNLOAD_URL = "https://github.com/GyanD/codexffmpeg/releases/download/7.1/ffmpeg-7.1-full_build.zip";
    private const string FFMPEG_DOWNLOAD_ZIP_NAME = "downloaded-ffmpeg.zip";

    /// <summary>
    /// Gets the static global FFmpegDependency instance.
    /// </summary>
    /// <remarks>
    /// This get operation blocks and may take a while if it needs to download FFmpeg.
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
    /// absolute paths to a local FFmpeg executable.
    /// </summary>
    /// <remarks>
    /// Creating an <see cref="FFmpegDependency"/> will attempt to search for an FFmpeg
    /// executable at a specified dependency folder. If FFmpeg cannot be found, it will download
    /// FFmpeg 7.1 binaries complied by gyan.dev on GitHub.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// A local FFmpeg executable cannot be found after attempting to download. Check
    /// <see cref="FFmpegDependency"/> implementation.
    /// </exception>
    private FFmpegDependency()
    {
        if (InitializeFFmpegPath())
        {
            return;
        }

        // Try to delete the FFmpeg depedency folder + its contents
        // This is in case a download attempt was tried before but was stopped mid-way
        try
        {
            Directory.Delete(FFMPEG_DEPENDENCY_FOLDER, true);
        }
        catch (Exception except)
        {
            // Only ignore the exception if it indicates that the directory was not found (this is
            // normal behavior). All other exceptions are unintended and should be notified to the
            // tester.
            if (except is not DirectoryNotFoundException)
            {
                throw;
            }
        }

        Directory.CreateDirectory(FFMPEG_DEPENDENCY_FOLDER);
        DownloadFFmpegZip();
        ExtractFFmpegZip();
        DeleteFFmpegZip();

        if (!InitializeFFmpegPath())
        {
            throw new InvalidOperationException("Failed to get FFmpeg executable path");
        }
    }

    /// <summary>
    /// Attempts to initialize the <see cref="AbsoluteFilePath"/>, <see cref="RelativeFilePath"/>,
    /// <see cref="AbsoluteFolderPath"/>, and <see cref="RelativeFolderPath"/> variables with a
    /// valid path to a local FFmpeg executable.
    /// </summary>
    /// <returns>
    /// <c>true</c> if an FFmpeg executable is successfully found. Otherwise returns <c>false</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// If an FFmpeg executbale program is successfully found, <see cref="AbsoluteFilePath"/> will
    /// contain an absolute path to the executable, and <see cref="RelativeFilePath"/> will
    /// contain a relative path to the same executable. If an FFmpeg executable is not found,
    /// <see cref="AbsoluteFilePath"/> and <see cref="RelativeFilePath"/> will be an empty.
    /// </para>
    /// <para>
    /// This method relies on the <see cref="FFMPEG_DEPENDENCY_FOLDER"/> constant value.
    /// </para>
    /// </remarks>
    private bool InitializeFFmpegPath()
    {
        if (!Directory.Exists(FFMPEG_DEPENDENCY_FOLDER))
        {
            AbsoluteFilePath = "";
            return false;
        }

        IEnumerable<string> dpFolderDirectories = Directory.EnumerateDirectories(FFMPEG_DEPENDENCY_FOLDER);
        foreach (string directory in dpFolderDirectories)
        {
            string potentialFFmpegPath = Path.GetFullPath(Path.Join(directory, "./bin/ffmpeg.exe"));
            if (File.Exists(potentialFFmpegPath))
            {
                AbsoluteFilePath = potentialFFmpegPath;
                RelativeFilePath = "./" + Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, AbsoluteFilePath);
                AbsoluteFolderPath = Path.GetDirectoryName(AbsoluteFilePath)!;
                RelativeFolderPath = "./" + Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, AbsoluteFolderPath);
                return true;
            }
        }

        AbsoluteFilePath = "";
        return false;
    }

    /// <summary>
    /// Downloads a ZIP file of FFmpeg from the resource specified in
    /// <see cref="FFMPEG_DOWNLOAD_URL"/> The ZIP file will be saved in
    /// <see cref="FFMPEG_DEPENDENCY_FOLDER"/>.
    /// </summary>
    /// <remarks>
    /// This method relies on the <see cref="FFMPEG_DEPENDENCY_FOLDER"/>,
    /// <see cref="FFMPEG_DOWNLOAD_URL"/>, and <see cref="FFMPEG_DOWNLOAD_URL"/> constant values.
    /// </remarks>
    private void DownloadFFmpegZip()
    {
        string downloadToFile = Path.Join(FFMPEG_DEPENDENCY_FOLDER, FFMPEG_DOWNLOAD_ZIP_NAME);

        // WebClient is depricated but I'm using it over HttpClient since HttpClient only
        // supports async methods. The download operation must the synchronous and implementing
        // async await behavior for a method that's supposed to be called from the constructor
        // is a pain in the ass.
        using (var client = new WebClient())
        {
            client.DownloadFile(FFMPEG_DOWNLOAD_URL, downloadToFile);
        }
    }

    /// <summary>
    /// Deletes the FFmpeg ZIP file downloaded from calling <see cref="DownloadFFmpegZip"/>.
    /// </summary>
    /// <remarks>
    /// This method relies on the <see cref="FFMPEG_DEPENDENCY_FOLDER"/> and
    /// <see cref="FFMPEG_DOWNLOAD_ZIP_NAME"/> constant values.
    /// </remarks>
    private void DeleteFFmpegZip()
    {
        string downloadedFFmpegZipPath = Path.Join(FFMPEG_DEPENDENCY_FOLDER, FFMPEG_DOWNLOAD_ZIP_NAME);
        File.Delete(downloadedFFmpegZipPath);
    }

    /// <summary>
    /// Extracts the contents of the ZIP file downloaded using <see cref="DownloadFFmpegZip"/> to
    /// <see cref="FFMPEG_DEPENDENCY_FOLDER"/>.
    /// </summary>
    /// <remarks>
    /// This method relies on the <see cref="FFMPEG_DEPENDENCY_FOLDER"/>,
    /// <see cref="FFMPEG_DOWNLOAD_URL"/>, and <see cref="FFMPEG_DOWNLOAD_URL"/> constant
    /// values.
    /// </remarks>
    private void ExtractFFmpegZip()
    {
        string downloadedFFmpegZipPath = Path.Join(FFMPEG_DEPENDENCY_FOLDER, FFMPEG_DOWNLOAD_ZIP_NAME);
        ZipFile.ExtractToDirectory(downloadedFFmpegZipPath, FFMPEG_DEPENDENCY_FOLDER);
    }
}
