using System;
using System.Collections.Generic;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class ThresholdAnalyzerTests
    {
        // Тест 6
        [Fact]
        public void Analyze_WithoutSubscribers_FindsAllCrossingsAndDoesNotModifySignal()
        {
            float[] data = { -1f, 1f, 2f, -1f, -2f, 1f };
            float[] originalCopy = (float[])data.Clone();
            var signal = new SingleChannelSignal(data, 10f);
            var analyzer = new ThresholdAnalyzer();

            SignalAnalysisResult result = analyzer.Analyze(signal, 0f);

            Assert.Same(signal, result.Signal);
            Assert.Equal(3, result.Markers.Count);

            Assert.Equal(1, result.Markers[0].SampleIndex);
            Assert.Equal(CrossingDirection.Up, result.Markers[0].Direction);

            Assert.Equal(3, result.Markers[1].SampleIndex);
            Assert.Equal(CrossingDirection.Down, result.Markers[1].Direction);

            Assert.Equal(5, result.Markers[2].SampleIndex);
            Assert.Equal(CrossingDirection.Up, result.Markers[2].Direction);

            AssertSignal.Equal(originalCopy, signal.Data, 1e-6f);
        }

        // Тест 7
        [Fact]
        public void Analyze_ThresholdPlateau_ReturnsTwoMarkers()
        {
            var signal = new SingleChannelSignal(new float[] { -1f, 0f, 0f, -1f }, 10f);
            var analyzer = new ThresholdAnalyzer();

            SignalAnalysisResult result = analyzer.Analyze(signal, 0f);

            Assert.Equal(2, result.Markers.Count);
            Assert.Equal(1, result.Markers[0].SampleIndex);
            Assert.Equal(CrossingDirection.Up, result.Markers[0].Direction);
            Assert.Equal(3, result.Markers[1].SampleIndex);
            Assert.Equal(CrossingDirection.Down, result.Markers[1].Direction);
        }

        // Тест 8
        public static IEnumerable<object[]> NoCrossingData()
        {
            yield return new object[] { Array.Empty<float>() };
            yield return new object[] { new float[] { 1f } };
            yield return new object[] { new float[] { 1f, 2f, 3f } };
        }

        [Theory]
        [MemberData(nameof(NoCrossingData))]
        public void Analyze_NoCrossings_ReturnsEmptyAndRaisesNoEvents(float[] data)
        {
            var signal = new SingleChannelSignal(data, 10f);
            var analyzer = new ThresholdAnalyzer();
            int eventCount = 0;
            analyzer.MarkerDetected += (sender, e) => eventCount++;

            SignalAnalysisResult result = analyzer.Analyze(signal, 0f);

            Assert.Empty(result.Markers);
            Assert.Equal(0, eventCount);
        }

        // Тест 9
        [Fact]
        public void Analyze_TwoSubscribersAndUnsubscribe_NotifiesCorrectly()
        {
            var signal = new SingleChannelSignal(new float[] { -1f, 1f }, 10f);
            var analyzer = new ThresholdAnalyzer();

            int firstCount = 0;
            int secondCount = 0;
            object capturedSender = null;
            MarkerDetectedEventArgs capturedArgs = null;

            EventHandler<MarkerDetectedEventArgs> firstHandler = (sender, e) =>
            {
                firstCount++;
                capturedSender = sender;
                capturedArgs = e;
            };
            EventHandler<MarkerDetectedEventArgs> secondHandler = (sender, e) => secondCount++;

            analyzer.MarkerDetected += firstHandler;
            analyzer.MarkerDetected += secondHandler;

            SignalAnalysisResult firstResult = analyzer.Analyze(signal, 0f);

            Assert.Single(firstResult.Markers);
            Assert.Equal(1, firstCount);
            Assert.Equal(1, secondCount);
            Assert.Same(analyzer, capturedSender);
            Assert.Same(signal, capturedArgs.Signal);
            Assert.Same(firstResult.Markers[0], capturedArgs.Marker);

            analyzer.MarkerDetected -= secondHandler;

            SignalAnalysisResult secondResult = analyzer.Analyze(signal, 0f);

            Assert.Single(secondResult.Markers);
            Assert.Equal(2, firstCount);
            Assert.Equal(1, secondCount);
            Assert.Same(secondResult.Markers[0], capturedArgs.Marker);
        }

        // Тест 10 (signal == null)
        [Fact]
        public void Analyze_NullSignal_ThrowsArgumentNullExceptionAndRaisesNoEvents()
        {
            var analyzer = new ThresholdAnalyzer();
            int eventCount = 0;
            analyzer.MarkerDetected += (sender, e) => eventCount++;

            Assert.Throws<ArgumentNullException>("signal", () => analyzer.Analyze(null, 0f));
            Assert.Equal(0, eventCount);
        }

        // Тест 10 (некорректный порог)
        [Theory]
        [InlineData(float.NaN)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.NegativeInfinity)]
        public void Analyze_InvalidThreshold_ThrowsArgumentOutOfRangeExceptionAndRaisesNoEvents(float threshold)
        {
            var signal = new SingleChannelSignal(new float[] { -1f, 1f }, 10f);
            var analyzer = new ThresholdAnalyzer();
            int eventCount = 0;
            analyzer.MarkerDetected += (sender, e) => eventCount++;

            Assert.Throws<ArgumentOutOfRangeException>("threshold", () => analyzer.Analyze(signal, threshold));
            Assert.Equal(0, eventCount);
        }

        // Тест 11 (NaN или бесконечность среди отсчётов, в том числе после пересечения)
        public static IEnumerable<object[]> InvalidSignalData()
        {
            yield return new object[] { new float[] { -1f, 1f, float.NaN } };
            yield return new object[] { new float[] { -1f, 1f, float.PositiveInfinity } };
            yield return new object[] { new float[] { -1f, 1f, float.NegativeInfinity } };
        }

        [Theory]
        [MemberData(nameof(InvalidSignalData))]
        public void Analyze_SignalWithInvalidSamples_ThrowsArgumentExceptionAndRaisesNoEvents(float[] data)
        {
            var signal = new SingleChannelSignal(data, 10f);
            var analyzer = new ThresholdAnalyzer();
            int eventCount = 0;
            analyzer.MarkerDetected += (sender, e) => eventCount++;

            var ex = Assert.Throws<ArgumentException>(() => analyzer.Analyze(signal, 0f));

            Assert.Equal("signal", ex.ParamName);
            Assert.Equal(0, eventCount);
        }
    }
}