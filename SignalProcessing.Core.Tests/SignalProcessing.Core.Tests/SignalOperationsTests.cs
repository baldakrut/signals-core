using System;
using System.Collections.Generic;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SignalOperationsTests
    {
        // Тест 20
        [Fact]
        public void Sum_DifferentLengths_PadsShorterWithZeros()
        {
            var s1 = new SingleChannelSignal(new float[] { 1f, 2f, 3f }, 100f);
            var s2 = new SingleChannelSignal(new float[] { 10f, 20f }, 100f);

            SingleChannelSignal result = SignalOperations.Sum(new[] { s1, s2 });

            Assert.Equal(100f, result.Frequency, 1e-5f);
            AssertSignal.Equal(new float[] { 11f, 22f, 3f }, result.Data);
        }

        // Тест 21
        [Fact]
        public void Concat_TwoSignals_ReturnsConcatenatedData()
        {
            var s1 = new SingleChannelSignal(new float[] { 1f, 2f }, 100f);
            var s2 = new SingleChannelSignal(new float[] { 3f }, 100f);

            SingleChannelSignal result = SignalOperations.Concat(new[] { s1, s2 });

            Assert.Equal(100f, result.Frequency, 1e-5f);
            AssertSignal.Equal(new float[] { 1f, 2f, 3f }, result.Data);
        }

        // Тест 22
        [Fact]
        public void SumAndConcat_DifferentFrequencies_ThrowArgumentException()
        {
            var s1 = new SingleChannelSignal(new float[] { 1f, 2f }, 100f);
            var s2 = new SingleChannelSignal(new float[] { 3f, 4f }, 200f);

            Assert.Throws<ArgumentException>(() => SignalOperations.Sum(new[] { s1, s2 }));
            Assert.Throws<ArgumentException>(() => SignalOperations.Concat(new[] { s1, s2 }));
        }

        // Тест 23
        [Fact]
        public void SumAndConcat_InvalidCollections_ThrowExpectedExceptions()
        {
            var valid = new SingleChannelSignal(new float[] { 1f }, 100f);
            var empty = new List<SingleChannelSignal>();
            var withNull = new List<SingleChannelSignal> { valid, null };

            Assert.Throws<ArgumentException>(() => SignalOperations.Sum(empty));
            Assert.Throws<ArgumentException>(() => SignalOperations.Concat(empty));

            Assert.Throws<ArgumentNullException>(() => SignalOperations.Sum(null));
            Assert.Throws<ArgumentNullException>(() => SignalOperations.Concat(null));

            Assert.Throws<ArgumentNullException>(() => SignalOperations.Sum(withNull));
            Assert.Throws<ArgumentNullException>(() => SignalOperations.Concat(withNull));
        }

        // Тест 24
        [Fact]
        public void Sum_DoesNotModifyInputSignals()
        {
            float[] d1 = { 1f, 2f, 3f };
            float[] d2 = { 10f, 20f };
            var s1 = new SingleChannelSignal(d1, 100f);
            var s2 = new SingleChannelSignal(d2, 100f);

            SignalOperations.Sum(new[] { s1, s2 });

            AssertSignal.Equal(new float[] { 1f, 2f, 3f }, s1.Data);
            AssertSignal.Equal(new float[] { 10f, 20f }, s2.Data);
        }
    }
}