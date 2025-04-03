namespace VideoFrameGrabber.IntegrationTests.StaticDependencies;

/// <summary>
/// Indicates that a property for a dependency test class is a test group property that should be
/// initialized during a reflection search.
/// </summary>
/// <remarks>
/// <para>
/// This attribute indicates to a static resource class that a specific property of that class
/// will refer to a specific test case.
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Property)]
class TestGroupPropertyAttribute : Attribute { }
