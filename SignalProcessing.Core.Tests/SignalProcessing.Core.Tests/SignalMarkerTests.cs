using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SignalMarkerTests
    {
        // Тест 1
        [Fact]
        public void Constructor_ValidArguments_SetsProperties()
        {
            var marker = new SignalMarker(3, CrossingDirection.Up);

            Assert.Equal(3, marker.SampleIndex);
            Assert.Equal(CrossingDirection.Up, marker.Direction);
        }

        // Тест 2 (отрицательный индекс)
        [Fact]
        public void Constructor_NegativeSampleIndex_ThrowsArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => new SignalMarker(-1, CrossingDirection.Up));

            Assert.Equal("sampleIndex", ex.ParamName);
        }

        // Тест 2 (недопустимое направление)
        [Fact]
        public void Constructor_InvalidDirection_ThrowsArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => new SignalMarker(0, (CrossingDirection)42));

            Assert.Equal("direction", ex.ParamName);
        }
    }
}