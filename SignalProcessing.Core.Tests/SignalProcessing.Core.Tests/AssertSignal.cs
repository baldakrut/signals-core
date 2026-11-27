using System;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    internal static class AssertSignal
    {
        public static void Equal(float[] expected, float[] actual, float tolerance = 1e-5f)
        {
            Assert.NotNull(actual);
            Assert.Equal(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                float diff = Math.Abs(expected[i] - actual[i]);
                Assert.True(
                    diff <= tolerance,
                    $"Расхождение на индексе {i}: ожидалось {expected[i]}, получено {actual[i]} (разница {diff} > допуска {tolerance}).");
            }
        }
    }
}