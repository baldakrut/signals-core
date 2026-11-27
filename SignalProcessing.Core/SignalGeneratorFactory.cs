using System;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Простая фабрика для создания генераторов сигналов по объекту параметров (паттерн Simple Factory).
    /// </summary>
    public static class SignalGeneratorFactory
    {
        /// <summary>
        /// Создаёт генератор сигнала, соответствующий переданному типу параметров.
        /// </summary>
        /// <param name="parameters">Объект параметров конкретного типа сигнала.</param>
        /// <returns>Экземпляр генератора, реализующий <see cref="ISignalGenerator"/>.</returns>
        /// <exception cref="ArgumentNullException">Параметр <paramref name="parameters"/> равен null.</exception>
        /// <exception cref="NotSupportedException">Передан неподдерживаемый тип параметров.</exception>
        public static ISignalGenerator Create(GeneratorParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters), "Параметры генератора не могут быть null.");

            switch (parameters)
            {
                case SineParameters sine:
                    return new SineGenerator(sine.Period, sine.Phase, sine.Amplitude);

                case LineParameters line:
                    return new LineGenerator(line.K, line.B);

                case NoiseParameters noise:
                    return new NoiseGenerator(noise.Range);

                default:
                    throw new NotSupportedException(
                        $"Неподдерживаемый тип параметров: {parameters.GetType().Name}.");
            }
        }
    }
}