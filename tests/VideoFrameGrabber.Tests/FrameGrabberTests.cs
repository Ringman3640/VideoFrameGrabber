using FluentAssertions;
using VideoFrameGrabber.Tests.CollectionFixtures;

namespace VideoFrameGrabber.Tests
{
    [Collection("FFmpegDependency collection")]
    public class FrameGrabberTests
    {
        private FFmpegDependencyFixture ffmpegDependency;

        public FrameGrabberTests(FFmpegDependencyFixture ffmpegFixture)
        {
            ffmpegDependency = ffmpegFixture;
        }

        [Fact]
        public void Constructor_EnvPathArgument_Succeeds()
        {
            FrameGrabber? grabber = null;

            try
            {
                grabber = new("ffmpeg");
            }
            catch { }

            grabber.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_NullArgument_Fails()
        {
            FrameGrabber? grabber = null;

            try
            {
                grabber = new(null);
            }
            catch { }

            grabber.Should().BeNull();
        }
    }
}
