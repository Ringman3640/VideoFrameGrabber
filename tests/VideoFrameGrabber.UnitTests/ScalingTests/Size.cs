namespace VideoFrameGrabber.UnitTests.UnitTests.ScalingTests;

/// <summary>
/// Represents a width and height size.
/// </summary>
/// <remarks>
/// This struct serves as syntax above raw width and height values. It makes the test code
/// easier to understand by grouping corresponding width and height values.
/// </remarks>
public readonly struct Size
{
    public int Width { get; init; }

    public int Height { get; init; }

    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }
}
