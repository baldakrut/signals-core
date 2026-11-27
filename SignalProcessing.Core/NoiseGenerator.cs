using System;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Генератор равномерного белого шума на отрезке [-range, range].
    /// </summary>
    public sealed class NoiseGenerator : ISignalGenerator
    {
        private readonly float _range;
        private readonly Func<Random> _randomFactory;

        /// <summary>
        /// Создаёт генератор белого шума.
        /// </summary>
        /// <param name="range">Размах шума R (должен быть больше или равен 0).</param>
        /// <param name="randomFactory">
        /// Фабрика генератора случайных чисел; при null используется <c>() =&gt; new Random()</c>.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">Размах шума отрицателен.</exception>
        public NoiseGenerator(float range, Func<Random> randomFactory = null)
        {
            if (range < 0)
                throw new ArgumentOutOfRangeException(nameof(range), "Размах шума не может быть отрицательным.");

            _range = range;
            _randomFactory = randomFactory ?? (() => new Random());
        }

        /// <summary>
        /// Генерирует сигнал белого шума.
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
            Random random = _randomFactory();

            for (int i = 0; i < count; i++)
            {
                data[i] = (float)((random.NextDouble() * 2.0 - 1.0) * _range);
            }

            return new SingleChannelSignal(data, sampling.SamplingRate);
        }
    }
}