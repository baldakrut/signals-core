using System;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Неизменяемые параметры дискретизации сигнала.
    /// </summary>
    public sealed class SamplingParameters
    {
        /// <summary>
        /// Частота квантования, Гц.
        /// </summary>
        public float SamplingRate { get; }

        /// <summary>
        /// Длительность сигнала, с.
        /// </summary>
        public float Duration { get; }

        /// <summary>
        /// Общее число отсчётов сигнала.
        /// </summary>
        public int SampleCount { get; }

        /// <summary>
        /// Создаёт экземпляр параметров дискретизации.
        /// </summary>
        /// <param name="samplingRate">Частота квантования, Гц (должна быть больше 0).</param>
        /// <param name="duration">Длительность сигнала, с (должна быть больше 0).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Частота квантования или длительность меньше либо равны 0.
        /// </exception>
        public SamplingParameters(float samplingRate, float duration)
        {
            if (samplingRate <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(samplingRate),
                    "Частота квантования должна быть положительной.");

            if (duration <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(duration),
                    "Длительность сигнала должна быть положительной.");

            SamplingRate = samplingRate;
            Duration = duration;
            SampleCount = (int)Math.Round(samplingRate * (double)duration);
        }

        /// <summary>
        /// Возвращает момент времени для отсчёта с указанным индексом.
        /// </summary>
        /// <param name="index">Индекс отсчёта (начиная с 0).</param>
        /// <returns>Момент времени отсчёта, с.</returns>
        public float TimeAt(int index)
        {
            return (float)(index / (double)SamplingRate);
        }
    }
}