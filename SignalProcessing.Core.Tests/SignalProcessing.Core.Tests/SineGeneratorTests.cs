using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SineGeneratorTests
    {
        // Тест 7
        [Fact]
        public void Generate_UnitSine_ReturnsExpectedValuesAtKeyPoints()
        {
            var generator = new SineGenerator(period: 1f, phase: 0f, amplitude: 1f);
            var sampling = new SamplingParameters(samplingRate: 100f, duration: 1f);

            SingleChannelSignal signal = generator.Generate(sampling);

            // t = 0 с (индекс 0), t = 0.25 с (индекс 25), t = 0.5 с (индекс 50)
            Assert.Equal(0f, signal.Data[0], 1e-6f);
            Assert.Equal(1f, signal.Data[25], 1e-6f);
            Assert.Equal(0f, signal.Data[50], 1e-6f);
        }

        // Тест 10
        [Theory]
        [InlineData(0f)]
        [InlineData(-2f)]
        public void Constructor_NonPositivePeriod_ThrowsArgumentOutOfRangeException(float period)
        {
            Assert.Throws<ArgumentOutOfRangeException>("period", () => new SineGenerator(period, 0f, 1f));
        }
    }
}