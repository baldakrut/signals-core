using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SingleChannelSignalTests
    {
        // Тест 1
        [Fact]
        public void Constructor_NullData_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SingleChannelSignal(null, 100f));
        }

        // Тест 2
        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Constructor_NonPositiveFrequency_ThrowsArgumentOutOfRangeException(float frequency)
        {
            float[] data = { 1f, 2f, 3f };
            Assert.Throws<ArgumentOutOfRangeException>(() => new SingleChannelSignal(data, frequency));
        }
    }
}