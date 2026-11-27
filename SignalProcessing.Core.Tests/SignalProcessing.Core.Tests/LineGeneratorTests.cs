using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class LineGeneratorTests
    {
        // Тест 8
        [Fact]
        public void Generate_K2B1_MatchesLinearFormulaAtAllPoints()
        {
            var generator = new LineGenerator(k: 2f, b: 1f);
            var sampling = new SamplingParameters(samplingRate: 10f, duration: 2f);

            SingleChannelSignal signal = generator.Generate(sampling);

            float[] expected = new float[sampling.SampleCount];
            for (int i = 0; i < expected.Length; i++)
            {
                float t = sampling.TimeAt(i);
                expected[i] = 2f * t + 1f;
            }

            AssertSignal.Equal(expected, signal.Data, 1e-6f);
        }
    }
}