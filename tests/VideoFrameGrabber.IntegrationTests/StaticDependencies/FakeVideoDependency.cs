namespace VideoFrameGrabber.IntegrationTests.StaticDependencies;

/// <summary>
/// Provides a fake MP4 video file dependency for error handling testing.
/// </summary>
/// <remarks>
/// This class is implemented as a singleton. Access the singleton instance from the
/// <see cref="FakeVideoDependency.Instance"/> property.
/// </remarks>
class FakeVideoDependency
{
    private const string RESOURCE_DIRECTORY = "./TestResources/InvalidVideo";
    private const string FAKE_VIDEO_NAME = "video.mp4";

    private static FakeVideoDependency? _instance = null;
    private static readonly object instanceLock = new();

    /// <summary>
    /// Gets the singleton instance of <see cref="FakeVideoDependency"/>.
    /// </summary>
    public static FakeVideoDependency Instance
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
    /// Gets a path to a fake MP4 video file.
    /// </summary>
    public string FakeVideoPath { get; }

    /// <summary>
    /// Constructor. It's self-explanatory. Tired of writing cookie-cutter docs.
    /// </summary>
    private FakeVideoDependency()
    {
        FakeVideoPath = Path.Join(RESOURCE_DIRECTORY, FAKE_VIDEO_NAME);

        if (!File.Exists(FakeVideoPath))
        {
            throw new InvalidOperationException($"Could not find the fake video in {RESOURCE_DIRECTORY}");
        }
    }
}
