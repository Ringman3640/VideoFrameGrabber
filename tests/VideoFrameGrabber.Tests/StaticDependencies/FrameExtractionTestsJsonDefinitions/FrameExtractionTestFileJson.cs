namespace VideoFrameGrabber.Tests.StaticDependencies.FrameExtractionTestsJsonDefinitions;

public class TestCollectionDefinition
{
    public List<TestDefinition> Tests { get; set; }
}

public class TestDefinition
{
    public string TestName { get; set; }
    public string SeekTime { get; set; }
    public ScaleInfoDefinition? ScaleInfo { get; set; }
    public CropInfoDefinition? CropInfo { get; set; }
    public bool? ScaleFirst { get; set; }
}

public class ScaleInfoDefinition
{
    public int Width { get; set; }
    public int Height { get; set; }
}

public class CropInfoDefinition
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int XPos { get; set; }
    public int YPos { get; set; }
}