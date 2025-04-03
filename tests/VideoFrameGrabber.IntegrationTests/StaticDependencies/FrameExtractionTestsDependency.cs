using System.Collections.Frozen;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using VideoFrameGrabber.IntegrationTests.StaticDependencies.FrameExtractionTestsJsonDefinitions;

namespace VideoFrameGrabber.IntegrationTests.StaticDependencies;

/// <summary>
/// Provides frame extraction test dependencies for xUnit tests.
/// </summary>
public class FrameExtractionTestsDependency
{
    private const string RESOURCE_DIRECTORY = "./TestResources/FrameExtractionTests";
    private const string TESTS_FILE_NAME = "tests.json";

    private static FrameExtractionTestsDependency? _instance = null;
    private static readonly object instanceLock = new();

    /// <summary>
    /// Gets the static global <see cref="FrameExtractionTestsDependency"/> instance.
    /// </summary>
    /// /// <remarks>
    /// This get operation blocks and may take a while if it needs generate test results.
    /// </remarks>
    public static FrameExtractionTestsDependency Instance
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
    /// Gets a dictionary of all test groups mapped by their test group name.
    /// </summary>
    public FrozenDictionary<string, FrameExtractionTestGroup> TestGroups { get; private set; }

    [TestGroupProperty]
    public FrameExtractionTestGroup EldenRingGameplay { get; private set; }

    [TestGroupProperty]
    public FrameExtractionTestGroup MonsterHunterGameplay { get; private set; }

    [TestGroupProperty]
    public FrameExtractionTestGroup ContentWarningGameplay { get; private set; }

    [TestGroupProperty]
    public FrameExtractionTestGroup VerticalFallingSnow { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FrameExtractionTestsDependency"/> class that
    /// initializes all of its properties.
    /// </summary>
    /// <remarks>
    /// This constructor uses reflection to initialize test group properties. To add another test
    /// group to the class, create a property whose name matches the directory name of the test
    /// group and annotate the property with <see cref="TestGroupPropertyAttribute"/>.
    /// </remarks>
    private FrameExtractionTestsDependency()
    {
        Dictionary<string, FrameExtractionTestGroup> tempTestGroups = new();

        FrameExtractionTestGroup.RootDirectory = RESOURCE_DIRECTORY;
        FrameExtractionTestGroup.TestFilePath = Path.Join(RESOURCE_DIRECTORY, TESTS_FILE_NAME);

        // Initialize test group properties with reflection
        foreach (PropertyInfo property in typeof(FrameExtractionTestsDependency).GetProperties())
        {
            if (!Attribute.IsDefined(property, typeof(TestGroupPropertyAttribute)))
            {
                continue;
            }

            FrameExtractionTestGroup testGroupObject = new(property.Name);
            property.SetValue(this, testGroupObject);
            tempTestGroups[property.Name] = testGroupObject;
        }

        TestGroups = tempTestGroups.ToFrozenDictionary();
    }
}

/// <summary>
/// Represents a test group for a video file.
/// </summary>
public class FrameExtractionTestGroup
{
    private const string VIDEO_FILE_NAME = "video.mp4";
    private const string GENERATED_TIMESTAMP_FILE_NAME = "generated_timestamp";
    private const string GENERATED_FRAMES_DIRECTORY_NAME = "frames";
    private const string RESULT_FRAME_EXTENSION = ".jpg";

    private static FFmpegDependency ffmpeg = FFmpegDependency.Instance;

    /// <summary>
    /// Gets or sets the root directory that all <see cref="FrameExtractionTestGroup"/> instances
    /// work within.
    /// </summary>
    /// <remarks>
    /// The root directory is the parent directory of all test group directories. This property must
    /// be a non-empty string before calling <see cref="FrameExtractionTestGroup(string)"/>.
    /// </remarks>
    public static string RootDirectory { get; set; } = "";

    /// <summary>
    /// Gets or sets the path to the global test file used by all test groups.
    /// </summary>
    /// <remarks>
    /// This property must be a non-empty string before calling
    /// <see cref="FrameExtractionTestGroup(string)"/>.
    /// </remarks>
    public static string TestFilePath { get; set; } = "";

    /// <summary>
    /// Gets the path of the test group directory.
    /// </summary>
    public string TestGroupDirectory { get; private set; }

    /// <summary>
    /// Gets the path of the video file assigned to the test group.
    /// </summary>
    public string VideoPath { get; private set; }

    /// <summary>
    /// Gets a dictionary of all supported tests for this test group mapped by the test's name.
    /// </summary>
    public FrozenDictionary<string, TestInfo> Tests { get; private set; }

    /// <summary>
    /// Gets a set of all test names that the test group does not support.
    /// </summary>
    /// <remarks>
    /// A test may be unsupported due to the size of the video file associated with a test group.
    /// Some tests require a test group's video file to be a minimum size.
    /// </remarks>
    public FrozenSet<string> UnsupportedTests { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FrameExtractionTestGroup"/> class given the
    /// name of the test group directory.
    /// </summary>
    /// <param name="testGroupName">The name of the test group directory.</param>
    /// <exception cref="InvalidOperationException">
    /// <see cref="RootDirectory"/> is empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <see cref="TestFilePath"/> is empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The indicated test group does not have a video file.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="testGroupName"/> does not exist.
    /// </exception>
    public FrameExtractionTestGroup(string testGroupName)
    {
        if (RootDirectory == "")
        {
            throw new InvalidOperationException($"{nameof(RootDirectory)} must be set before calling constructor.");
        }

        if (TestFilePath == "")
        {
            throw new InvalidOperationException($"{nameof(TestFilePath)} must be set before calling constructor.");
        }

        TestGroupDirectory = Path.Join(RootDirectory, testGroupName);
        if (!Directory.Exists(TestGroupDirectory))
        {
            throw new ArgumentException($"The {testGroupName} test group directory does not exist.");
        }

        VideoPath = Path.Join(TestGroupDirectory, VIDEO_FILE_NAME);
        if (!File.Exists(VideoPath))
        {
            throw new InvalidOperationException($"The {testGroupName} test group does not have a {VIDEO_FILE_NAME} file.");
        }

        if (NeedToGenerateTestFrames())
        {
            GenerateAllTestFrames();
        }

        LoadTestFrames();
    }

    /// <summary>
    /// Loads all supported and unsupported tests to <see cref="Tests"/> and
    /// <see cref="UnsupportedTests"/>.
    /// </summary>
    private void LoadTestFrames()
    {
        Dictionary<string, TestInfo> tempTests = new();
        HashSet<string> tempUnsupportedTests = new();

        string framesDirectory = Path.Join(TestGroupDirectory, GENERATED_FRAMES_DIRECTORY_NAME);
        foreach(TestDefinition test in GetTestDefinition().Tests)
        {
            string testResultPath = Path.Join(framesDirectory, test.TestName + RESULT_FRAME_EXTENSION);
            if (!File.Exists(testResultPath))
            {
                tempUnsupportedTests.Add(test.TestName);
                continue;
            }

            TestInfo testInfo = new()
            {
                TestName = test.TestName,
                ResultPath = testResultPath,
                SeekTime = test.SeekTime,
                ScaleFirst = test.ScaleFirst ?? true
            };

            if (test.ScaleInfo is not null)
            {
                testInfo.ScaleInfo = new()
                {
                    Width = test.ScaleInfo.Width,
                    Height = test.ScaleInfo.Height
                };
            }

            if (test.CropInfo is not null)
            {
                testInfo.CropInfo = new()
                {
                    Width = test.CropInfo.Width,
                    Height = test.CropInfo.Height,
                    XPos = test.CropInfo.XPos,
                    YPos = test.CropInfo.YPos
                };
            }

            tempTests[test.TestName] = testInfo;
        }

        Tests = tempTests.ToFrozenDictionary();
        UnsupportedTests = tempUnsupportedTests.ToFrozenSet();
    }

    /// <summary>
    /// Gets if tests frames need to be generated by checking the generated timestamp file in the
    /// test group directory.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the test frames need to be generated. Otherwise returns <c>false</c>.
    /// </returns>
    private bool NeedToGenerateTestFrames()
    {
        string generatedTimeFilePath = Path.Join(TestGroupDirectory, GENERATED_TIMESTAMP_FILE_NAME);
        if (!File.Exists(generatedTimeFilePath))
        {
            return true;
        }

        DateTime testFileTime = File.GetLastWriteTimeUtc(TestFilePath);
        long testFileTimestamp = (long)(testFileTime - DateTime.UnixEpoch).TotalMilliseconds;

        long generatedTimestamp = long.Parse(File.ReadAllText(generatedTimeFilePath));

        return generatedTimestamp != testFileTimestamp;
    }

    /// <summary>
    /// Generates test frame results for all tests in this test group.
    /// </summary>
    /// <remarks>
    /// This method will delete the frame directory (and its contents) specified by
    /// <see cref="GENERATED_FRAMES_DIRECTORY_NAME"/> if it exists.
    /// </remarks>
    private void GenerateAllTestFrames()
    {
        string generatedFramesDirectory = Path.Join(TestGroupDirectory, GENERATED_FRAMES_DIRECTORY_NAME);

        // Delete old frames folder and recreate it to do fresh generation
        try
        {
            Directory.Delete(generatedFramesDirectory, true);
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
        Directory.CreateDirectory(generatedFramesDirectory);

        // Generate frames
        foreach (TestDefinition test in GetTestDefinition().Tests)
        {
            // Try to generate test frame with the provided seek time. If that fails, try to get
            // the last frame in case the video is shorter than the given seek time.
            if (!GenerateTestFrame(test, generatedFramesDirectory)) {
                GenerateLastTestFrame(test, generatedFramesDirectory);
            }
        }

        CreateGeneratedTimestampFile();
    }

    /// <summary>
    /// Attempts to generate/extract a frame from a video file given a set of test parameters.
    /// </summary>
    /// <param name="test">The test information used to generate the frame.</param>
    /// <param name="saveDirectory">A path to a directory to save the extracted frame to.</param>
    /// <returns><c>true</c> if the frame was generateded. Otherwise returns <c>false</c>.</returns>
    private bool GenerateTestFrame(TestDefinition test, string saveDirectory)
    {
        string seekArg = $"-ss {test.SeekTime}";
        string inputArg = $"-i {VideoPath}";
        string filterArg = GetFFmpegFilterArgument(test);
        string safeFilePath = Path.Join(saveDirectory, test.TestName + RESULT_FRAME_EXTENSION);
        string arguments = $"{seekArg} {inputArg} {filterArg} -frames:v 1 {safeFilePath}";

        RunFFmpeg(arguments);

        return File.Exists(safeFilePath);
    }

    /// <summary>
    /// Attempts to generate/extract the last frame from a video given a set of test parameters.
    /// </summary>
    /// <remarks>
    /// This method will ignore the <see cref="TestDefinition.SeekTime"/> parameter of
    /// <paramref name="test"/> since it only extracts the final frame of the video. Use this method
    /// if <see cref="GenerateTestFrame(TestDefinition, string, string)"/> fails due to the seek
    /// time being longer than the length of the video.
    /// </remarks>
    /// <inheritdoc cref="GenerateTestFrame(TestDefinition, string, string)"/>
    private bool GenerateLastTestFrame(TestDefinition test, string saveDirectory)
    {
        string inputArg = $"-i {VideoPath}";
        string filterArg = GetFFmpegFilterArgument(test);
        string safeFilePath = Path.Join(saveDirectory, test.TestName + RESULT_FRAME_EXTENSION);
        string arguments = $"-sseof -3 {inputArg} {filterArg} -update true -q:v 1 {safeFilePath}";

        RunFFmpeg(arguments);

        return File.Exists(safeFilePath);
    }

    /// <summary>
    /// Executes an FFmpeg process with the given arguments string.
    /// </summary>
    /// <param name="arguments">The CLI arguments to pass to FFmpeg.</param>
    private void RunFFmpeg(string arguments)
    {
        Process ffmpegProcess = new();
        ffmpegProcess.StartInfo.FileName = ffmpeg.AbsoluteFilePath;
        ffmpegProcess.StartInfo.Arguments = arguments;
        ffmpegProcess.Start();
        ffmpegProcess.WaitForExit();
    }

    /// <summary>
    /// Formats an string for FFmpeg's filter CLI argument given the scale and crop information from
    /// a <see cref="TestDefinition"/> object.
    /// </summary>
    /// <param name="test">
    /// The <see cref="TestDefinition"/> that contains scale and crop info.
    /// </param>
    /// <returns>A string that represents a formatted FFmpeg filter CLI argument.</returns>
    private string GetFFmpegFilterArgument(TestDefinition test)
    {
        if (test.CropInfo is null && test.ScaleInfo is null)
        {
            return "";
        }

        string scaleArg = test.ScaleInfo is null ? "" : $"scale={test.ScaleInfo.Width}:{test.ScaleInfo.Height}";
        string cropArg = test.CropInfo is null ? ""
            : $"crop={test.CropInfo.Width}:{test.CropInfo.Height}:{test.CropInfo.XPos}:{test.CropInfo.YPos}";

        if (test.ScaleFirst is not null && test.ScaleFirst == false)
        {
            return $"-vf \"{cropArg}{(test.CropInfo is null ? "" : ",")}{scaleArg}\"";
        }
        return $"-vf \"{scaleArg}{(test.ScaleInfo is null ? "" : ",")}{cropArg}\"";
    }

    /// <summary>
    /// Gets the collection of tests defined in the test file pathed to by
    /// <see cref="TestFilePath"/>.
    /// </summary>
    /// <returns>The collection of tests as a <see cref="TestCollectionDefinition"/>.</returns>
    private TestCollectionDefinition GetTestDefinition()
    {
        string testFileContents = File.ReadAllText(TestFilePath);
        return JsonSerializer.Deserialize<TestCollectionDefinition>(testFileContents)!;
    }

    /// <summary>
    /// Generates a timestamp file that indicates when frame-generation occured.
    /// </summary>
    private void CreateGeneratedTimestampFile()
    {
        string savePath = Path.Join(TestGroupDirectory, GENERATED_TIMESTAMP_FILE_NAME);

        DateTime testFileTime = File.GetLastWriteTimeUtc(TestFilePath);
        long testFileTimestamp = (long)(testFileTime - DateTime.UnixEpoch).TotalMilliseconds;

        File.WriteAllText(savePath, testFileTimestamp.ToString());
    }

    /// <summary>
    /// Describes a frame extraction test along with its expected output.
    /// </summary>
    public struct TestInfo
    {
        /// <summary>
        /// The name of the test.
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// A file path that leads to the test's resulting image.
        /// </summary>
        public string ResultPath { get; set; }

        /// <summary>
        /// The seek time that the image frame was taken from.
        /// </summary>
        public string SeekTime { get; set; }

        /// <summary>
        /// The scaling parameters applied to the test.
        /// </summary>
        public TestScalingInfo? ScaleInfo { get; set; }

        /// <summary>
        /// The cropping parameters applied to the test.
        /// </summary>
        public TestCroppingInfo? CropInfo { get; set; }

        /// <summary>
        /// Idicates if the scaling or cropping modifier should be applied first.
        /// </summary>
        public bool ScaleFirst { get; set; }
    }

    /// <summary>
    /// Describes the parameters for an FFmpeg scaling filter.
    /// </summary>
    public struct TestScalingInfo
    {
        /// <summary>
        /// The output width of the scaling filter in pixels.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The output height of the scaling filter in pixels.
        /// </summary>
        public int Height { get; set; }
    }

    /// <summary>
    /// Describes the parameters for an FFmpeg cropping filter.
    /// </summary>
    public struct TestCroppingInfo
    {
        /// <summary>
        /// The output width of the cropping filter in pixels.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The output height of the cropping filter in pixels.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The pixel offset of the crop frame from the left of the image.
        /// </summary>
        public int XPos { get; set; }

        /// <summary>
        /// The pixel offset of the crop frame from the top of the image.
        /// </summary>
        public int YPos { get; set; }
    }
}
