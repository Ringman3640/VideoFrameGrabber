using FluentAssertions;
using VideoFrameGrabber.Cropping;
using VideoFrameGrabber.UnitTests.UnitTests.ScalingTests;
using VideoFrameGrabber.UnitTests.Utilities;

namespace VideoFrameGrabber.UnitTests.CroppingTests;

public class CropProviderUnitTests
{
    // T-1
    [Fact]
    public void GetCropOffsetFromAlign_AllCropAlignValues_DoesNotThrowException()
    {
        CropAlign[] alignValues = (CropAlign[])Enum.GetValues(typeof(CropAlign));
        foreach (CropAlign align in alignValues)
        {
            int cropWidth = 0;
            int cropHeight = 0;
            int inputWidth = 0;
            int inputHeight = 0;
            Exception? exception = null;

            try
            {
                _ = CropProvider.GetCropOffsetFromAlign(cropWidth, cropHeight, inputWidth, inputHeight, align);
            }
            catch (Exception except)
            {
                exception = except;
            }

            exception.Should().BeNull();
        }
    }

    // T-2
    [Theory]
    [MemberData(nameof(GetFormattedTestData))]
    public void GetCropOffsetFromAlign_CropAlignAndCropAndInputValues_OffsetValuesMatchExpected(
        CropAlign align,
        int cropWidth,
        int cropHeight,
        int inputWidth,
        int inputHeight,
        int expectedOffsetX,
        int expectedOffsetY
    ) {
        CommonTests.CropProvider.GetsCorrectOffsetValuesFromAlign(
            align: align,
            cropWidth: cropWidth,
            cropHeight: cropHeight,
            inputWidth: inputWidth,
            inputHeight: inputHeight,
            expectedX: expectedOffsetX,
            expectedY: expectedOffsetY
        );
    }

    public static IEnumerable<object[]> GetFormattedTestData()
    {
        foreach (TestData.TestUnit testUnit in TestData.Instance.TestUnits)
        {
            CropAlign[] alignValues = (CropAlign[])Enum.GetValues(typeof(CropAlign));

            foreach (CropAlign align in alignValues)
            {
                yield return new object[]
                {
                    align, 
                    testUnit.CropSize.Width,
                    testUnit.CropSize.Height,
                    testUnit.ImageSize.Width,
                    testUnit.ImageSize.Height,
                    testUnit.GetExpectedResults(align).OffsetX,
                    testUnit.GetExpectedResults(align).OffsetY
                };
            }
        }
    }

    /// <summary>
    /// Represents a set of test data for testing the
    /// <see cref="CropProvider.GetCropOffsetFromAlign(int, int, int, int, CropAlign)"/> method.
    /// Consists of a list of <see cref="TestUnit"/> tests.
    /// </summary>
    /// <remarks>
    /// This is a singleton class. Use the <see cref="Instance"/> property to get an
    /// instance of the singleton.
    /// </remarks>
    public class TestData
    {
        private static TestData? instance = null;
        private static object instanceLock = new();

        /// <summary>
        /// Gets the <see cref="TestData"/> singleton instance.
        /// </summary>
        public static TestData Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance is null)
                    {
                        instance = new TestData();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// Gets a list of <see cref="TestUnit"/> tests for testing
        /// <see cref="CropProvider.GetCropOffsetFromAlign(int, int, int, int, CropAlign)"/>.
        /// </summary>
        public List<TestUnit> TestUnits { get; } = CreateTestUnits();

        /// <summary>
        /// Creates the list of <see cref="TestUnit"/> tests. 
        /// </summary>
        /// <remarks>
        /// This method is responsible for declaring all the test values. Modify this method to add
        /// more tests.
        /// </remarks>
        private static List<TestUnit> CreateTestUnits()
        {
            return
            [
                new TestUnit(
                    imageSize: new(500, 500),
                    cropSize: new(100, 100),
                    expectedResultsForAlign: new Dictionary<CropAlign, ExpectedOffsetResults>
                    {
                        [CropAlign.TopLeft] = new(0, 0),
                        [CropAlign.TopCenter] = new(200, 0),
                        [CropAlign.TopRight] = new(400, 0),
                        [CropAlign.CenterLeft] = new(0, 200),
                        [CropAlign.Center] = new(200, 200),
                        [CropAlign.CenterRight] = new(400, 200),
                        [CropAlign.BottomLeft] = new(0, 400),
                        [CropAlign.BottomCenter] = new(200, 400),
                        [CropAlign.BottomRight] = new(400, 400)
                    }),

                new TestUnit(//
                    imageSize: new(500, 500),
                    cropSize: new(1, 1),
                    expectedResultsForAlign: new Dictionary<CropAlign, ExpectedOffsetResults>
                    {
                        [CropAlign.TopLeft] = new(0, 0),
                        [CropAlign.TopCenter] = new(249, 0),
                        [CropAlign.TopRight] = new(499, 0),
                        [CropAlign.CenterLeft] = new(0, 249),
                        [CropAlign.Center] = new(249, 249),
                        [CropAlign.CenterRight] = new(499, 249),
                        [CropAlign.BottomLeft] = new(0, 499),
                        [CropAlign.BottomCenter] = new(249, 499),
                        [CropAlign.BottomRight] = new(499, 499)
                    }),

                new TestUnit(
                    imageSize: new(500, 500),
                    cropSize: new(1, 500),
                    expectedResultsForAlign: new Dictionary<CropAlign, ExpectedOffsetResults>
                    {
                        [CropAlign.TopLeft] = new(0, 0),
                        [CropAlign.TopCenter] = new(249, 0),
                        [CropAlign.TopRight] = new(499, 0),
                        [CropAlign.CenterLeft] = new(0, 0),
                        [CropAlign.Center] = new(249, 0),
                        [CropAlign.CenterRight] = new(499, 0),
                        [CropAlign.BottomLeft] = new(0, 0),
                        [CropAlign.BottomCenter] = new(249, 0),
                        [CropAlign.BottomRight] = new(499, 0)
                    }),

                new TestUnit(
                    imageSize: new(500, 500),
                    cropSize: new(500, 1),
                    expectedResultsForAlign: new Dictionary<CropAlign, ExpectedOffsetResults>
                    {
                        [CropAlign.TopLeft] = new(0, 0),
                        [CropAlign.TopCenter] = new(0, 0),
                        [CropAlign.TopRight] = new(0, 0),
                        [CropAlign.CenterLeft] = new(0, 249),
                        [CropAlign.Center] = new(0, 249),
                        [CropAlign.CenterRight] = new(0, 249),
                        [CropAlign.BottomLeft] = new(0, 499),
                        [CropAlign.BottomCenter] = new(0, 499),
                        [CropAlign.BottomRight] = new(0, 499)
                    }),

                new TestUnit(
                    imageSize: new(500, 500),
                    cropSize: new(500, 500),
                    expectedResultsForAlign: new Dictionary<CropAlign, ExpectedOffsetResults>
                    {
                        [CropAlign.TopLeft] = new(0, 0),
                        [CropAlign.TopCenter] = new(0, 0),
                        [CropAlign.TopRight] = new(0, 0),
                        [CropAlign.CenterLeft] = new(0, 0),
                        [CropAlign.Center] = new(0, 0),
                        [CropAlign.CenterRight] = new(0, 0),
                        [CropAlign.BottomLeft] = new(0, 0),
                        [CropAlign.BottomCenter] = new(0, 0),
                        [CropAlign.BottomRight] = new(0, 0)
                    }),

                new TestUnit(
                    imageSize: new(500, 500),
                    cropSize: new(1, 1000),
                    expectedResultsForAlign: new Dictionary<CropAlign, ExpectedOffsetResults>
                    {
                        [CropAlign.TopLeft] = new(0, 0),
                        [CropAlign.TopCenter] = new(249, 0),
                        [CropAlign.TopRight] = new(499, 0),
                        [CropAlign.CenterLeft] = new(0, 0),
                        [CropAlign.Center] = new(249, 0),
                        [CropAlign.CenterRight] = new(499, 0),
                        [CropAlign.BottomLeft] = new(0, 0),
                        [CropAlign.BottomCenter] = new(249, 0),
                        [CropAlign.BottomRight] = new(499, 0)
                    }),

                new TestUnit(
                    imageSize: new(500, 500),
                    cropSize: new(1000, 1),
                    expectedResultsForAlign: new Dictionary<CropAlign, ExpectedOffsetResults>
                    {
                        [CropAlign.TopLeft] = new(0, 0),
                        [CropAlign.TopCenter] = new(0, 0),
                        [CropAlign.TopRight] = new(0, 0),
                        [CropAlign.CenterLeft] = new(0, 249),
                        [CropAlign.Center] = new(0, 249),
                        [CropAlign.CenterRight] = new(0, 249),
                        [CropAlign.BottomLeft] = new(0, 499),
                        [CropAlign.BottomCenter] = new(0, 499),
                        [CropAlign.BottomRight] = new(0, 499)
                    }),

                new TestUnit(
                    imageSize: new(500, 500),
                    cropSize: new(1000, 1000),
                    expectedResultsForAlign: new Dictionary<CropAlign, ExpectedOffsetResults>
                    {
                        [CropAlign.TopLeft] = new(0, 0),
                        [CropAlign.TopCenter] = new(0, 0),
                        [CropAlign.TopRight] = new(0, 0),
                        [CropAlign.CenterLeft] = new(0, 0),
                        [CropAlign.Center] = new(0, 0),
                        [CropAlign.CenterRight] = new(0, 0),
                        [CropAlign.BottomLeft] = new(0, 0),
                        [CropAlign.BottomCenter] = new(0, 0),
                        [CropAlign.BottomRight] = new(0, 0)
                    }),

                new TestUnit(
                    imageSize: new(0, 0),
                    cropSize: new(0, 0),
                    expectedResultsForAlign: new Dictionary<CropAlign, ExpectedOffsetResults>
                    {
                        [CropAlign.TopLeft] = new(0, 0),
                        [CropAlign.TopCenter] = new(0, 0),
                        [CropAlign.TopRight] = new(0, 0),
                        [CropAlign.CenterLeft] = new(0, 0),
                        [CropAlign.Center] = new(0, 0),
                        [CropAlign.CenterRight] = new(0, 0),
                        [CropAlign.BottomLeft] = new(0, 0),
                        [CropAlign.BottomCenter] = new(0, 0),
                        [CropAlign.BottomRight] = new(0, 0)
                    }),


                // Template:

                //new TestUnit(
                //    imageSize: new(0, 0),
                //    cropSize: new(0, 0),
                //    expectedResultsForAlign: new Dictionary<CropAlign, ExpectedOffsetResults>
                //    {
                //        [CropAlign.TopLeft] = new(0, 0),
                //        [CropAlign.TopCenter] = new(0, 0),
                //        [CropAlign.TopRight] = new(0, 0),
                //        [CropAlign.CenterLeft] = new(0, 0),
                //        [CropAlign.Center] = new(0, 0),
                //        [CropAlign.CenterRight] = new(0, 0),
                //        [CropAlign.BottomLeft] = new(0, 0),
                //        [CropAlign.BottomCenter] = new(0, 0),
                //        [CropAlign.BottomRight] = new(0, 0)
                //    }),
            ];
        }

        /// <summary>
        /// Represents the expected offset values for a corresponding
        /// <see cref="CropProvider.GetCropOffsetFromAlign(int, int, int, int, CropAlign)"/> call.
        /// </summary>
        public readonly struct ExpectedOffsetResults
        {
            public int OffsetX { get; init; }

            public int OffsetY { get; init; }

            public ExpectedOffsetResults(int offsetX, int offsetY)
            {
                OffsetX = offsetX;
                OffsetY = offsetY;
            }
        }

        /// <summary>
        /// Represents a single test of <see cref="TestData"/>.
        /// </summary>
        /// <remarks>
        /// Each <see cref="TestUnit"/> contains all the arguments needed to call
        /// <see cref="CropProvider.GetCropOffsetFromAlign(int, int, int, int, CropAlign)"/> along
        /// with the expected <see cref="ExpectedOffsetResults"/> for each <see cref="CropAlign"/>
        /// value.
        /// </remarks>
        public class TestUnit
        {
            public Size ImageSize { get; init; }

            public Size CropSize { get; init; }

            public Dictionary<CropAlign, ExpectedOffsetResults> ExpectedResultsForAlign { get; init; }

            public TestUnit(
                Size imageSize,
                Size cropSize,
                Dictionary<CropAlign, ExpectedOffsetResults> expectedResultsForAlign
            ) {
                ImageSize = imageSize;
                CropSize = cropSize;
                ExpectedResultsForAlign = expectedResultsForAlign;
            }

            /// <summary>
            /// Gets the <see cref="ExpectedOffsetResults"/> corresponding to a specific
            /// <see cref="CropAlign"/> value.
            /// </summary>
            /// <exception cref="NotImplementedException">
            /// The value of <paramref name="align"/> does not have an implemented
            /// <see cref="ExpectedOffsetResults"/>.
            /// </exception>
            /// <remarks>
            /// <para>
            /// This method effectively acts as a wrapper for <see cref="ExpectedResultsForAlign"/>
            /// while providing formatted exception throwing in case a <see cref="CropAlign"/> value
            /// does not have a corresponding expected value. This makes it more obvious to testers
            /// in xUnit what the problem is (that they need to add the expected value).
            /// </para>
            /// <para>
            /// If <paramref name="align"/> is <see cref="CropAlign.None"/>, this method will return
            /// an <see cref="ExpectedOffsetResults"/> instnace with values (0, 0). It will not
            /// throw an error.
            /// </para>
            /// </remarks>
            public ExpectedOffsetResults GetExpectedResults(CropAlign align)
            {
                try
                {
                    ExpectedResultsForAlign.TryGetValue(align, out ExpectedOffsetResults results);
                    return results;
                }
                catch
                {
                    if (align == CropAlign.None)
                    {
                        return new ExpectedOffsetResults(0, 0);
                    }
                    throw new NotImplementedException($"The {Enum.GetName(align.GetType(), align)}" +
                        $" CropAlign value does not have a corresponding expected result. (Add it. NOW.)");
                }
            }
        }
    }
}


