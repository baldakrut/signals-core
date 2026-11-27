using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class ResamplerTests
    {
        // Тест 36
        [Fact]
        public void Resample_SameFrequency_ReturnsEqualData()
        {
            float[] data = { 1f, 3.5f, -2f, 4.25f, 0f };
            var signal = new SingleChannelSignal(data, 100f);

            SingleChannelSignal result = Resampler.Resample(signal, 100f);

            Assert.Equal(100f, result.Frequency, 1e-5f);
            AssertSignal.Equal(data, result.Data);
        }

        // Тест 37
        [Fact]
        public void Resample_UpsampleByTwo_InterpolatesMidpoints()
        {
            var signal = new SingleChannelSignal(new float[] { 0f, 2f, 4f, 8f }, 1f);

            SingleChannelSignal result = Resampler.Resample(signal, 2f);

            Assert.Equal(2f, result.Frequency, 1e-5f);
            AssertSignal.Equal(new float[] { 0f, 1f, 2f, 3f, 4f, 6f, 8f }, result.Data);
        }

        // Тест 38
        [Fact]
        public void Resample_DownsampleByTwo_TakesEverySecondSample()
        {
            var signal = new SingleChannelSignal(new float[] { 0f, 1f, 2f, 3f, 4f }, 2f);

            SingleChannelSignal result = Resampler.Resample(signal, 1f);

            Assert.Equal(1f, result.Frequency, 1e-5f);
            AssertSignal.Equal(new float[] { 0f, 2f, 4f }, result.Data);
        }

        // Тест 39
        [Fact]
        public void Resample_NonIntegerRatio_ReturnsInterpolatedValues()
        {
            var signal = new SingleChannelSignal(new float[] { 0f, 3f, 6f, 9f }, 3f);

            SingleChannelSignal result = Resampler.Resample(signal, 2f);

            Assert.Equal(2f, result.Frequency, 1e-5f);
            AssertSignal.Equal(new float[] { 0f, 4.5f, 9f }, result.Data);
        }

        // Тест 40
        [Fact]
        public void Resample_LinearFunction_PreservesExactLine()
        {
            const int n = 100;
            const float oldFreq = 100f;
            const float newFreq = 37f;

            float[] data = new float[n];
            for (int i = 0; i < n; i++)
            {
                double t = i / (double)oldFreq;
                data[i] = (float)(2.0 * t + 1.0);
            }

            var signal = new SingleChannelSignal(data, oldFreq);
            SingleChannelSignal result = Resampler.Resample(signal, newFreq);

            Assert.Equal(newFreq, result.Frequency, 1e-5f);
            for (int j = 0; j < result.Data.Length; j++)
            {
                double t = j / (double)newFreq;
                float expected = (float)(2.0 * t + 1.0);
                Assert.Equal(expected, result.Data[j], 1e-4f);
            }
        }

        // Тест 41 (один отсчёт)
        [Fact]
        public void Resample_SingleSample_ReturnsSameSample()
        {
            var signal = new SingleChannelSignal(new float[] { 7.5f }, 50f);

            SingleChannelSignal result = Resampler.Resample(signal, 120f);

            Assert.Equal(120f, result.Frequency, 1e-5f);
            AssertSignal.Equal(new float[] { 7.5f }, result.Data);
        }

        // Тест 41 (пустой сигнал)
        [Fact]
        public void Resample_EmptySignal_ReturnsEmptySignalWithNewFrequency()
        {
            var signal = new SingleChannelSignal(Array.Empty<float>(), 50f);

            SingleChannelSignal result = Resampler.Resample(signal, 120f);

            Assert.Equal(120f, result.Frequency, 1e-5f);
            Assert.Empty(result.Data);
        }

        // Тест 42 (некорректная частота)
        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Resample_NonPositiveFrequency_ThrowsArgumentOutOfRangeException(float newFrequency)
        {
            var signal = new SingleChannelSignal(new float[] { 1f, 2f }, 100f);

            Assert.Throws<ArgumentOutOfRangeException>(() => Resampler.Resample(signal, newFrequency));
        }

        // Тест 42 (null-сигнал)
        [Fact]
        public void Resample_NullSignal_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Resampler.Resample(null, 100f));
        }
    }
}