using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class OfflineFilterTests
    {
        // Тест 3
        [Fact]
        public void Filter_ConstantSignal_ReturnsSameConstant()
        {
            float[] data = { 5f, 5f, 5f, 5f, 5f };

            float[] result = OfflineFilter.Filter(data, 3);

            AssertSignal.Equal(data, result);
        }

        // Тест 4
        [Fact]
        public void Filter_LinearRamp_ReturnsExpectedValues()
        {
            float[] data = { 1f, 2f, 3f, 4f, 5f };
            float[] expected = { 4f / 3f, 2f, 3f, 4f, 14f / 3f };

            float[] result = OfflineFilter.Filter(data, 3);

            AssertSignal.Equal(expected, result);
        }

        // Тест 5
        [Fact]
        public void Filter_Impulse_ReturnsSmoothedImpulse()
        {
            float[] data = { 0f, 0f, 0f, 1f, 0f, 0f, 0f };
            float[] expected = { 0f, 0f, 1f / 3f, 1f / 3f, 1f / 3f, 0f, 0f };

            float[] result = OfflineFilter.Filter(data, 3);

            AssertSignal.Equal(expected, result);
        }

        // Тест 6
        [Fact]
        public void Filter_WindowSizeOne_ReturnsCopyOfInput()
        {
            float[] data = { 1f, 2f, 3f, 4f };

            float[] result = OfflineFilter.Filter(data, 1);

            Assert.NotSame(data, result);
            AssertSignal.Equal(data, result);
        }

        // Тест 7
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(4)]
        public void Filter_InvalidWindowSize_ThrowsArgumentOutOfRangeException(int windowSize)
        {
            float[] data = { 1f, 2f, 3f };

            Assert.Throws<ArgumentOutOfRangeException>("windowSize",
                () => OfflineFilter.Filter(data, windowSize));
        }

        // Тест 8 (null)
        [Fact]
        public void Filter_NullData_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => OfflineFilter.Filter(null, 3));
        }

        // Тест 8 (пустой массив)
        [Fact]
        public void Filter_EmptyData_ReturnsEmptyArray()
        {
            float[] result = OfflineFilter.Filter(Array.Empty<float>(), 3);

            Assert.Empty(result);
        }

        // Тест 9
        [Fact]
        public void Filter_DoesNotModifyInputArray()
        {
            float[] data = { 1f, 5f, 2f, 8f, 3f };
            float[] originalCopy = (float[])data.Clone();

            OfflineFilter.Filter(data, 3);

            AssertSignal.Equal(originalCopy, data);
        }
    }
}