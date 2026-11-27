using System;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Пересчёт (ресамплинг) одноканального сигнала на новую частоту квантования.
    /// </summary>
    public static class Resampler
    {
        /// <summary>
        /// Выполняет ресамплинг сигнала на заданную частоту методом линейной интерполяции.
        /// </summary>
        /// <param name="signal">Исходный сигнал.</param>
        /// <param name="newFrequency">Новая частота квантования, Гц (должна быть больше 0).</param>
        /// <returns>Новый сигнал с пересчитанными отсчётами и частотой <paramref name="newFrequency"/>.</returns>
        /// <exception cref="ArgumentNullException">Сигнал равен null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Новая частота меньше или равна 0.</exception>
        public static SingleChannelSignal Resample(SingleChannelSignal signal, float newFrequency)
        {
            if (signal == null)
                throw new ArgumentNullException(nameof(signal), "Сигнал не может быть null.");
            if (newFrequency <= 0)
                throw new ArgumentOutOfRangeException(nameof(newFrequency), "Новая частота должна быть положительной.");

            float[] source = signal.Data;
            int n = source.Length;

            if (n == 0)
                return new SingleChannelSignal(Array.Empty<float>(), newFrequency);

            double oldFreq = signal.Frequency;
            double newFreq = newFrequency;

            int newLength = (int)Math.Floor((n - 1) * newFreq / oldFreq + 1e-6) + 1;
            float[] result = new float[newLength];

            for (int j = 0; j < newLength; j++)
            {
                double p = j * oldFreq / newFreq;
                int k = (int)Math.Floor(p);

                if (k >= n - 1)
                {
                    result[j] = source[n - 1];
                }
                else
                {
                    double alpha = p - k;
                    result[j] = (float)(source[k] + alpha * (source[k + 1] - source[k]));
                }
            }

            return new SingleChannelSignal(result, newFrequency);
        }
    }
}