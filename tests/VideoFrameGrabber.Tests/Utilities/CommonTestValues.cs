using System.Collections;

namespace VideoFrameGrabber.Tests.Utilities;

/// <summary>
/// Exposes classes that provide common test values for xUnit theory tests.
/// </summary>
public static class CommonTestValues
{
    /// <summary>
    /// Exposes classes that provide single-integer test values.
    /// </summary>
    public static class SingleInts
    {
        /// <summary>
        /// Gets a list of common positive <see cref="int"/> test values (not including zero).
        /// </summary>
        public static List<int> PositiveIntDataSet { get; } = [
            1,
            2,
            10,
            1000,
            9999999,
            int.MaxValue - 1,
            int.MaxValue
        ];

        /// <summary>
        /// Gets a list of common negative <see cref="int"/> test values (not including zero).
        /// </summary>
        public static List<int> NegativeIntDataSet { get; } = [
            1,
            2,
            10,
            1000,
            9999999,
            int.MaxValue - 1,
            int.MaxValue
        ];

        /// <summary>
        /// Represents test data consisting of only positive integer values. This does not include
        /// the zero value. Values are provided by <see cref="PositiveIntDataSet"/>.
        /// </summary>
        public class AllPositive : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                foreach (int value in PositiveIntDataSet)
                {
                    yield return new object[] { value };
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// Represents test data consisting of negative integer values. Values are provided by
        /// <see cref="NegativeIntDataSet"/>.
        /// </summary>
        public class AllNegative : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                foreach (int value in NegativeIntDataSet)
                {
                    yield return new object[] { value };
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }

    /// <summary>
    /// Exposes classes that provide integer pair test values.
    /// </summary>
    public static class IntPairs
    {
        /// <summary>
        /// Represents test data consisting of only positive integer pairs (including non-zero).
        /// </summary>
        public class AllPositive : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 1, 1 };
                yield return new object[] { 1, 2 };
                yield return new object[] { 1, 10 };
                yield return new object[] { 1, 1000 };
                yield return new object[] { 1, 99999999 };
                yield return new object[] { 1, int.MaxValue - 1 };
                yield return new object[] { 1, int.MaxValue };
                yield return new object[] { 2, 1 };
                yield return new object[] { 10, 1 };
                yield return new object[] { 1000, 1 };
                yield return new object[] { 99999999, 1 };
                yield return new object[] { int.MaxValue - 1, 1 };
                yield return new object[] { int.MaxValue, 1 };
                yield return new object[] { 2, 2 };
                yield return new object[] { 10, 10 };
                yield return new object[] { 1000, 1000 };
                yield return new object[] { 99999999, 99999999 };
                yield return new object[] { int.MaxValue - 1, int.MaxValue - 1 };
                yield return new object[] { int.MaxValue, int.MaxValue };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// Represents a smaller set of test data (compared to <see cref="AllPositive"/>) consisting
        /// of only positive integer pairs (including non-zero).
        /// </summary>
        public class SmallerAllPositive : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 1, 1 };
                yield return new object[] { 1, 99999999 };
                yield return new object[] { 1, int.MaxValue };
                yield return new object[] { 99999999, 1 };
                yield return new object[] { int.MaxValue, 1 };
                yield return new object[] { 99999999, 99999999 };
                yield return new object[] { int.MaxValue, int.MaxValue };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// Represents test data consisting of interger pairs with one or more zero values.
        /// </summary>
        public class ContainsZero : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 0, 0 };
                yield return new object[] { 0, 1 };
                yield return new object[] { 0, 2 };
                yield return new object[] { 0, 10 };
                yield return new object[] { 0, 1000 };
                yield return new object[] { 0, 99999999 };
                yield return new object[] { 0, int.MaxValue - 1 };
                yield return new object[] { 0, int.MaxValue };
                yield return new object[] { 2, 0 };
                yield return new object[] { 10, 0 };
                yield return new object[] { 1000, 0 };
                yield return new object[] { 99999999, 0 };
                yield return new object[] { int.MaxValue - 1, 0 };
                yield return new object[] { int.MaxValue, 0 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// Represents a smaller set of test data (compared to <see cref="ContainsZero"/>)
        /// consisting of interger pairs with one or more zero values.
        /// </summary>
        public class SmallerContainsZero : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 0, 0 };
                yield return new object[] { 0, 1 };
                yield return new object[] { 0, 99999999 };
                yield return new object[] { 0, int.MaxValue };
                yield return new object[] { 1, 0 };
                yield return new object[] { 99999999, 0 };
                yield return new object[] { int.MaxValue, 0 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// Represents test data consisting of integer pairs with one or more negative values.
        /// </summary>
        public class ContainsNegatives : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 1, -1 };
                yield return new object[] { 1, -2 };
                yield return new object[] { 1, -10 };
                yield return new object[] { 1, -1000 };
                yield return new object[] { 1, -99999999 };
                yield return new object[] { 1, int.MinValue + 1 };
                yield return new object[] { 1, int.MinValue };
                yield return new object[] { -2, 1 };
                yield return new object[] { -10, 1 };
                yield return new object[] { -1000, 1 };
                yield return new object[] { -99999999, 1 };
                yield return new object[] { int.MinValue + 1, 1 };
                yield return new object[] { int.MinValue, 1 };
                yield return new object[] { -2, -2 };
                yield return new object[] { -10, -10 };
                yield return new object[] { -1000, -1000 };
                yield return new object[] { -99999999, -99999999 };
                yield return new object[] { int.MinValue + 1, int.MinValue + 1 };
                yield return new object[] { int.MinValue, int.MinValue };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// Represents a smaller set of test data (compared to <see cref="ContainsNegatives"/>)
        /// consisting of integer pairs with one or more negative values.
        /// </summary>
        public class SmallerContainsNegatives : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 1, -1 };
                yield return new object[] { 1, -99999999 };
                yield return new object[] { 1, int.MinValue };
                yield return new object[] { -1, 1 };
                yield return new object[] { -99999999, 1 };
                yield return new object[] { int.MinValue, 1 };
                yield return new object[] { -1, -1 };
                yield return new object[] { -99999999, -99999999 };
                yield return new object[] { int.MinValue, int.MinValue };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }

    /// <summary>
    /// Exposes methods that append pre-defined test values to a given value set.
    /// </summary>
    public static class Append
    {
        public static IEnumerable<object[]> PositiveIntPairs(object[] originalSet)
        {
            foreach (object[] positivePair in new IntPairs.SmallerAllPositive())
            {
                List<object> objectList = [.. originalSet, .. positivePair];
                yield return objectList.ToArray();
            }
        }

        public static IEnumerable<object[]> IntPairsContainingZero(object[] originalSet)
        {
            foreach (object[] zeroPairs in new IntPairs.SmallerContainsZero())
            {
                List<object> objectList = [.. originalSet, .. zeroPairs];
                yield return objectList.ToArray();
            }
        }

        public static IEnumerable<object[]> IntPairsContainingNegatives(object[] originalSet)
        {
            foreach (object[] negativePairs in new IntPairs.SmallerContainsNegatives())
            {
                List<object> objectList = [.. originalSet, .. negativePairs];
                yield return objectList.ToArray();
            }
        }
    }
}
