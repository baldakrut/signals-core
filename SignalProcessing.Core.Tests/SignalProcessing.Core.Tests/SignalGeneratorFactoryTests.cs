using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SignalGeneratorFactoryTests
    {
        private sealed class FakeParameters : GeneratorParameters { }

        // Тест 3
        [Fact]
        public void Create_SineParameters_ReturnsSineGenerator()
        {
            var parameters = new SineParameters { Period = 1f, Phase = 0f, Amplitude = 1f };

            ISignalGenerator generator = SignalGeneratorFactory.Create(parameters);

            Assert.IsType<SineGenerator>(generator);
        }

        // Тест 4 (LineParameters)
        [Fact]
        public void Create_LineParameters_ReturnsLineGenerator()
        {
            var parameters = new LineParameters { K = 2f, B = 1f };

            ISignalGenerator generator = SignalGeneratorFactory.Create(parameters);

            Assert.IsType<LineGenerator>(generator);
        }

        // Тест 4 (NoiseParameters)
        [Fact]
        public void Create_NoiseParameters_ReturnsNoiseGenerator()
        {
            var parameters = new NoiseParameters { Range = 1.5f };

            ISignalGenerator generator = SignalGeneratorFactory.Create(parameters);

            Assert.IsType<NoiseGenerator>(generator);
        }

        // Тест 5
        [Fact]
        public void Create_NullParameters_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => SignalGeneratorFactory.Create(null));
        }

        // Тест 6
        [Fact]
        public void Create_UnsupportedParameters_ThrowsNotSupportedException()
        {
            var fake = new FakeParameters();

            var ex = Assert.Throws<NotSupportedException>(() => SignalGeneratorFactory.Create(fake));
            Assert.Contains(nameof(FakeParameters), ex.Message);
        }
    }
}