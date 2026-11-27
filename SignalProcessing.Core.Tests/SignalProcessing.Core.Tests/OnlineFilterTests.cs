using System;
using System.Collections.Generic;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class OnlineFilterTests
    {
        // Тест 10
        [Fact]
        public void Filter_SingleCall_ReturnsExpectedValues()
        {
            float[] data = { 1f, 2f, 3f, 4f, 5f };
            float[] expected = { 1f, 4f / 3f, 2f, 3f, 4f };
            var filter = new OnlineFilter(3);

            float[] result = filter.Filter(data);

            AssertSignal.Equal(expected, result);
        }

        // Тест 11
        [Fact]
        public void Filter_TwoCalls_MatchesSingleCall()
        {
            float[] expected = { 1f, 4f / 3f, 2f, 3f, 4f };
            var filter = new OnlineFilter(3);

            float[] part1 = filter.Filter(new float[] { 1f, 2f });
            float[] part2 = filter.Filter(new float[] { 3f, 4f, 5f });

            var combined = new List<float>(part1);
            combined.AddRange(part2);

            AssertSignal.Equal(expected, combined.ToArray());
        }

        // Тест 12
        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        [InlineData(100)]
        public void Filter_ChunkedInput_MatchesWholeInput(int chunkSize)
        {
            float[] data = GenerateRandomSignal(1000, seed: 42);
            float[] expected = new OnlineFilter(7).Filter(data);

            var chunkedFilter = new OnlineFilter(7);
            var actual = new List<float>(data.Length);

            for (int i = 0; i < data.Length; i += chunkSize)
            {
                int len = Math.Min(chunkSize, data.Length - i);
                float[] chunk = new float[len];
                Array.Copy(data, i, chunk, 0, len);
                actual.AddRange(chunkedFilter.Filter(chunk));
            }

            AssertSignal.Equal(expected, actual.ToArray());
        }

        // Тест 13
        [Fact]
        public void Filter_MatchesOfflineFilterWithDelay()
        {
            const int windowSize = 7;
            const int delay = (windowSize - 1) / 2; // 3
            float[] data = GenerateRandomSignal(1000, seed: 42);

            float[] offline = OfflineFilter.Filter(data, windowSize);
            float[] online = new OnlineFilter(windowSize).Filter(data);

            for (int i = delay; i < data.Length; i++)
            {
                Assert.Equal(offline[i - delay], online[i], 1e-5f);
            }
        }

        // Тест 14
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_InvalidWindowSize_ThrowsArgumentOutOfRangeException(int windowSize)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new OnlineFilter(windowSize));
        }

        private static float[] GenerateRandomSignal(int length, int seed)
        {
            var rnd = new Random(seed);
            float[] data = new float[length];
            for (int i = 0; i < length; i++)
                data[i] = (float)(rnd.NextDouble() * 20.0 - 10.0);
            return data;
        }
        // Тест 33
        [Fact]
        public void Reset_AfterSignal_MatchesNewFilter()
        {
            float[] firstSignal = { 100f, 200f, 300f, 400f, 500f };
            float[] secondSignal = { 1f, 2f, 3f, 4f, 5f };

            var reusedFilter = new OnlineFilter(3);
            reusedFilter.Filter(firstSignal);
            reusedFilter.Reset();

            float[] actual = reusedFilter.Filter(secondSignal);
            float[] expected = new OnlineFilter(3).Filter(secondSignal);

            AssertSignal.Equal(expected, actual);
        }

        // Тест 34
        [Fact]
        public void Filter_WindowSizeOne_ReturnsInputValues()
        {
            float[] data = { -3.5f, 0f, 1.25f, 42f, 7.75f };
            var filter = new OnlineFilter(1);

            float[] result = filter.Filter(data);

            Assert.NotSame(data, result);
            AssertSignal.Equal(data, result);
        }

        // Тест 35
        [Fact]
        public void Filter_LongSignalWithLargeOffset_MatchesDirectDoubleCalculation()
        {
            const int length = 200_000;
            const int windowSize = 7;
            var rnd = new Random(42);
            float[] data = new float[length];
            for (int i = 0; i < length; i++)
                data[i] = (float)(900.0 + rnd.NextDouble() * 200.0);

            float[] expected = new float[length];
            for (int i = 0; i < length; i++)
            {
                double windowSum = 0.0;
                for (int k = 0; k < windowSize; k++)
                {
                    int idx = i - k;
                    windowSum += idx < 0 ? data[0] : data[idx];
                }
                expected[i] = (float)(windowSum / windowSize);
            }

            float[] actual = new OnlineFilter(windowSize).Filter(data);

            AssertSignal.Equal(expected, actual, 1e-3f);
        }
    }
}