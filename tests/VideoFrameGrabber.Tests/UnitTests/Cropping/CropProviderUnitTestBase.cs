using VideoFrameGrabber.Cropping;
using VideoFrameGrabber.Tests.Utilities;

namespace VideoFrameGrabber.Tests.UnitTests.Cropping;

/// <summary>
/// Represents a base class for <see cref="CropProvider"/> unit tests that provides common helper
/// methods.
/// </summary>
/// <remarks>
/// This class is needed to support methods from <see cref="CommonTestValues.Append"/> due to
/// limitations of the <see cref="MemberDataAttribute"/> in xUnit. Specifically, the attribute does
/// not allow parameters to be passed to methods that exist outside of the current class.
/// <see cref="CropProviderUnitTestBase"/> acts as a wrapper for those methods so that
/// <see cref="MemberDataAttribute"/> sees the method as part of the current class. 
/// </remarks>
public class CropProviderUnitTestBase
{
    public static IEnumerable<object[]> AppendPositiveIntPairs(int[] originalSet)
    {
        return CommonTestValues.Append.PositiveIntPairs([.. originalSet]);
    }

    public static IEnumerable<object[]> AppendIntPairsContainingZero(int[] originalSet)
    {
        return CommonTestValues.Append.IntPairsContainingZero([.. originalSet]);
    }

    public static IEnumerable<object[]> AppendIntPairsContainingNegatives(int[] originalSet)
    {
        return CommonTestValues.Append.IntPairsContainingNegatives([.. originalSet]);
    }

    public static IEnumerable<object[]> GetVariablePositiveInts(int count)
    {
        return CommonTestValues.VariableInts.AllPositive(count);
    }

    public static IEnumerable<object[]> GetVariablePositiveWithZeroInts(int count)
    {
        return CommonTestValues.VariableInts.AllPositiveWithZero(count);
    }

    public static IEnumerable<object[]> GetVariableNegativeInts(int count)
    {
        return CommonTestValues.VariableInts.AllNegative(count);
    }

    public static IEnumerable<object[]> GetVariableNegativeWithZeroInts(int count)
    {
        return CommonTestValues.VariableInts.AllNegativeWithZero(count);
    }

    public static IEnumerable<object[]> GetVariableMixedInts(int count)
    {
        return CommonTestValues.VariableInts.Mixed(count);
    }

    public static IEnumerable<object[]> GetVariableMixedWithZeroInts(int count)
    {
        return CommonTestValues.VariableInts.MixedWithZero(count);
    }
}
