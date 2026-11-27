using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class NoiseGeneratorTests
    {
        // Тест 9
        [Fact]
        public void Generate_FixedSeed_ValuesWithinRangeAndReproducible()
        {
            const float range = 2.5f;
            var generator = new NoiseGenerator(range, () => new Random(42));
            var sampling = new SamplingParameters(samplingRate: 100f, duration: 2f);

            SingleChannelSignal first = generator.Generate(sampling);
            SingleChannelSignal second = generator.Generate(sampling);

            for (int i = 0; i < first.Data.Length; i++)
            {
                Assert.InRange(first.Data[i], -range, range);
            }

            AssertSignal.Equal(first.Data, second.Data, 1e-6f);
        }

        [Fact]
        public void Constructor_NegativeRange_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>("range", () => new NoiseGenerator(-1f));
        }
    }
}