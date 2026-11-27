using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SamplingParametersTests
    {
        // Тест 1
        [Fact]
        public void Constructor_100HzAnd2Seconds_SampleCountIs200()
        {
            var sampling = new SamplingParameters(100f, 2f);

            Assert.Equal(200, sampling.SampleCount);
            Assert.Equal(100f, sampling.SamplingRate, 1e-6f);
            Assert.Equal(2f, sampling.Duration, 1e-6f);
        }

        // Тест 2
        [Theory]
        [InlineData(0f)]
        [InlineData(-10f)]
        public void Constructor_NonPositiveSamplingRate_ThrowsArgumentOutOfRangeException(float samplingRate)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SamplingParameters(samplingRate, 2f));
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Constructor_NonPositiveDuration_ThrowsArgumentOutOfRangeException(float duration)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SamplingParameters(100f, duration));
        }
    }
}