namespace VideoFrameGrabber.IntegrationTests.StaticDependencies;

/// <summary>
/// Provides a temporary empty directory for testing.
/// </summary>
/// <remarks>
/// This class creates a new directory on the current machine. Use the <see cref="Dispose"/> method
/// to delete the directory after testing is done.
/// </remarks>
public sealed class TestDirectoryDependency : IDisposable
{
    /// <summary>
    /// Path to the director that will contain all created directories.
    /// </summary>
    private const string DIRECTORY_ROOT_LOCATION = "./TestResources/TestDirectories";

    private static bool performedInitialCleanup = false;
    private static int directoryNameCounter = 0;
    private readonly object initializeLock = new();

    /// <summary>
    /// Gets the absolute path to the directory.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Initializes a <see cref="TestDirectoryDependency"/> instance that points to a newly-created
    /// directory.
    /// </summary>
    public TestDirectoryDependency()
    {
        string directoryName;

        lock (initializeLock)
        {
            if (performedInitialCleanup == false)
            {
                PerformInitialCleanup();
                performedInitialCleanup = true;
            }

            directoryName = directoryNameCounter.ToString();
            ++directoryNameCounter;
        }

        string relativePath = System.IO.Path.Join(DIRECTORY_ROOT_LOCATION, $"./{directoryName}/");
        Path = System.IO.Path.GetFullPath(relativePath);
        Directory.CreateDirectory(Path);
    }

    /// <summary>
    /// Deletes any old directories that may have remained in <see cref="DIRECTORY_ROOT_LOCATION"/>
    /// after previous testing.
    private static void PerformInitialCleanup()
    {
        IEnumerable<string> directoryPaths = Directory.EnumerateDirectories(DIRECTORY_ROOT_LOCATION);

        foreach (string directoryPath in directoryPaths)
        {
            Directory.Delete(directoryPath, true);
        }
    }

    /// <summary>
    /// Deletes the test directory created by this instance, including the contents of the
    /// directory.
    /// </summary>
    public void Dispose()
    {
        Directory.Delete(Path, true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Finalizer for <see cref="TestDirectoryDependency"/>. Deletes the created test directory.
    /// </summary>
    /// <remarks>
    /// This finalizer is only added in case the user does not call <see cref="Dispose"/>.
    /// </remarks>
    ~TestDirectoryDependency()
    {
        Dispose();
    }
}
