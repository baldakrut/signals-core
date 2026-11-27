using System;
using System.Collections.Generic;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SignalAnalysisResultTests
    {
        // Тест 3 (signal == null)
        [Fact]
        public void Constructor_NullSignal_ThrowsArgumentNullException()
        {
            var markers = new List<SignalMarker>();

            Assert.Throws<ArgumentNullException>("signal", () => new SignalAnalysisResult(null, markers));
        }

        // Тест 3 (markers == null)
        [Fact]
        public void Constructor_NullMarkers_ThrowsArgumentNullException()
        {
            var signal = new SingleChannelSignal(new float[] { 1f, 2f }, 10f);

            Assert.Throws<ArgumentNullException>("markers", () => new SignalAnalysisResult(signal, null));
        }

        // Тест 3 (индекс маркера >= длины данных)
        [Fact]
        public void Constructor_MarkerIndexOutOfBounds_ThrowsArgumentOutOfRangeException()
        {
            var signal = new SingleChannelSignal(new float[] { 1f, 2f }, 10f);
            var markers = new List<SignalMarker> { new SignalMarker(2, CrossingDirection.Up) };

            Assert.Throws<ArgumentOutOfRangeException>("markers", () => new SignalAnalysisResult(signal, markers));
        }

        // Тест 4 (неизменяемость и защита от изменения исходного списка)
        [Fact]
        public void Constructor_SourceListModifiedAfterCreation_DoesNotAffectResult()
        {
            var signal = new SingleChannelSignal(new float[] { 0f, 1f, 2f, 3f }, 10f);
            var sourceList = new List<SignalMarker>
            {
                new SignalMarker(1, CrossingDirection.Up)
            };

            var result = new SignalAnalysisResult(signal, sourceList);
            sourceList.Add(new SignalMarker(2, CrossingDirection.Down));

            Assert.Single(result.Markers);
            Assert.Equal(1, result.Markers[0].SampleIndex);
            Assert.IsNotType<List<SignalMarker>>(result.Markers);
        }
    }
}