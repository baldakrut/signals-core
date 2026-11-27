using System;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Генератор линейного сигнала.
    /// </summary>
    public sealed class LineGenerator : ISignalGenerator
    {
        private readonly float _k;
        private readonly float _b;

        /// <summary>
        /// Создаёт генератор прямой y = k * t + b.
        /// </summary>
        /// <param name="k">Угловой коэффициент наклона (единиц / с).</param>
        /// <param name="b">Свободный член (значение при t = 0).</param>
        public LineGenerator(float k, float b)
        {
            _k = k;
            _b = b;
        }

        /// <summary>
        /// Генерирует линейный сигнал.
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

            for (int i = 0; i < count; i++)
            {
                double t = sampling.TimeAt(i);
                data[i] = (float)(_k * t + _b);
            }

            return new SingleChannelSignal(data, sampling.SamplingRate);
        }
    }
}