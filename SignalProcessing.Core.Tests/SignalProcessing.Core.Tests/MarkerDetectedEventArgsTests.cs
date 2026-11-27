using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class MarkerDetectedEventArgsTests
    {
        // Тест 5
        [Fact]
        public void Constructor_ValidSignalAndMarker_ComputesTimeAndValue()
        {
            var signal = new SingleChannelSignal(new float[] { 0f, 2f, -3f }, 10f);
            var marker = new SignalMarker(2, CrossingDirection.Down);

            var args = new MarkerDetectedEventArgs(signal, marker);

            Assert.Same(signal, args.Signal);
            Assert.Same(marker, args.Marker);
            Assert.Equal(0.2f, args.Time, 1e-6f);
            Assert.Equal(-3f, args.Value, 1e-6f);
        }
    }
}