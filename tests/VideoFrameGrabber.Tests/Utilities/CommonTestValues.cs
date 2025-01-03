using System.Collections;

namespace VideoFrameGrabber.Tests.Utilities;

/// <summary>
/// Exposes classes that provide common test values.
/// </summary>
public static class CommonTestValues
{
    /// <summary>
    /// Exposes classes that provide single-integer test values.
    /// </summary>
    public static class SingleInts
    {
        /// <summary>
        /// Represents test data consisting of only positive integer values. This does not include
        /// the zero value.
        /// </summary>
        public class AllPositive : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 1 };
                yield return new object[] { 2 };
                yield return new object[] { 10 };
                yield return new object[] { 1000 };
                yield return new object[] { 99999999 };
                yield return new object[] { int.MaxValue - 1 };
                yield return new object[] { int.MaxValue };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// Represents test data consisting of negative integer values.
        /// </summary>
        public class AllNegative : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { -1 };
                yield return new object[] { -2 };
                yield return new object[] { -10 };
                yield return new object[] { -1000 };
                yield return new object[] { -99999999 };
                yield return new object[] { int.MinValue + 1 };
                yield return new object[] { int.MinValue };
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
    }
}
