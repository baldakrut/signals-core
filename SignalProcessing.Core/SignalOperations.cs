using System;
using System.Collections.Generic;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Операции объединения одноканальных сигналов.
    /// </summary>
    public static class SignalOperations
    {
        private const float FrequencyTolerance = 1e-5f;

        /// <summary>
        /// Вычисляет поэлементную сумму списка сигналов.
        /// Если сигналы имеют разную длину, результат имеет длину наибольшего,
        /// а короткие сигналы дополняются нулями.
        /// </summary>
        /// <param name="signals">Список исходных сигналов.</param>
        /// <returns>Новый сигнал с суммой отсчётов и исходной частотой дискретизации.</returns>
        /// <exception cref="ArgumentNullException">Список равен null или содержит элемент null.</exception>
        /// <exception cref="ArgumentException">Список пуст или частоты сигналов различаются более чем на 1e-5 Гц.</exception>
        public static SingleChannelSignal Sum(IReadOnlyList<SingleChannelSignal> signals)
        {
            ValidateSignals(signals);

            float frequency = signals[0].Frequency;
            int maxLength = 0;

            for (int i = 0; i < signals.Count; i++)
            {
                if (signals[i].Data.Length > maxLength)
                    maxLength = signals[i].Data.Length;
            }

            float[] result = new float[maxLength];

            for (int i = 0; i < signals.Count; i++)
            {
                float[] data = signals[i].Data;
                for (int j = 0; j < data.Length; j++)
                {
                    result[j] += data[j];
                }
            }

            return new SingleChannelSignal(result, frequency);
        }

        /// <summary>
        /// Последовательно склеивает сигналы в порядке их следования.
        /// </summary>
        /// <param name="signals">Список исходных сигналов.</param>
        /// <returns>Новый сигнал длиной, равной сумме длин исходных сигналов.</returns>
        /// <exception cref="ArgumentNullException">Список равен null или содержит элемент null.</exception>
        /// <exception cref="ArgumentException">Список пуст или частоты сигналов различаются более чем на 1e-5 Гц.</exception>
        public static SingleChannelSignal Concat(IReadOnlyList<SingleChannelSignal> signals)
        {
            ValidateSignals(signals);

            float frequency = signals[0].Frequency;
            int totalLength = 0;

            for (int i = 0; i < signals.Count; i++)
            {
                totalLength += signals[i].Data.Length;
            }

            float[] result = new float[totalLength];
            int offset = 0;

            for (int i = 0; i < signals.Count; i++)
            {
                float[] data = signals[i].Data;
                Array.Copy(data, 0, result, offset, data.Length);
                offset += data.Length;
            }

            return new SingleChannelSignal(result, frequency);
        }

        private static void ValidateSignals(IReadOnlyList<SingleChannelSignal> signals)
        {
            if (signals == null)
                throw new ArgumentNullException(nameof(signals), "Список сигналов не может быть равен null.");

            if (signals.Count == 0)
                throw new ArgumentException("Список сигналов не может быть пустым.", nameof(signals));

            for (int i = 0; i < signals.Count; i++)
            {
                if (signals[i] == null)
                    throw new ArgumentNullException(nameof(signals), $"Сигнал с индексом {i} равен null.");
            }

            float baseFreq = signals[0].Frequency;
            for (int i = 1; i < signals.Count; i++)
            {
                float currentFreq = signals[i].Frequency;
                if (Math.Abs(currentFreq - baseFreq) > FrequencyTolerance)
                {
                    throw new ArgumentException(
                        $"Частоты сигналов различаются: {baseFreq} Гц и {currentFreq} Гц.",
                        nameof(signals));
                }
            }
        }
    }
}