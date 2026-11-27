using System;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Генератор синусоидального сигнала.
    /// </summary>
    public sealed class SineGenerator : ISignalGenerator
    {
        private readonly float _period;
        private readonly float _phase;
        private readonly float _amplitude;

        /// <summary>
        /// Создаёт генератор синусоиды.
        /// </summary>
        /// <param name="period">Период колебаний, с (должен быть строго больше 0).</param>
        /// <param name="phase">Фазовый сдвиг, с.</param>
        /// <param name="amplitude">Амплитуда сигнала.</param>
        /// <exception cref="ArgumentOutOfRangeException">Период меньше или равен 0.</exception>
        public SineGenerator(float period, float phase, float amplitude)
        {
            if (period <= 0)
                throw new ArgumentOutOfRangeException(nameof(period), "Период синусоиды должен быть положительным.");

            _period = period;
            _phase = phase;
            _amplitude = amplitude;
        }

        /// <summary>
        /// Генерирует синусоидальный сигнал.
        /// </summary>
        /// <param name="sampling">Параметры дискретизации (частота квантования, Гц и длительность, с).</param>
        /// <returns>Сгенерированный одноканальный сигнал.</returns>
        /// <exception cref="ArgumentNullException">Параметры дискретизации равны null.</exception>
        public SingleChannelSignal Generate(SamplingParameters sampling)
        {
            if (sampling == null)
                throw new ArgumentNullException(nameof(sampling), "Параметры дискретизации не могут быть null.");

            int count = sampling.SampleCount;
            float[] data = new float[count];
            double omega = 2.0 * Math.PI / _period;

            for (int i = 0; i < count; i++)
            {
                double t = sampling.TimeAt(i);
                data[i] = (float)(_amplitude * Math.Sin(omega * (t - _phase)));
            }

            return new SingleChannelSignal(data, sampling.SamplingRate);
        }
    }
}