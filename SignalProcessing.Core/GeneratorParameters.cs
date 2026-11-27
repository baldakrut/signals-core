namespace SignalProcessing.Core
{
    /// <summary>
    /// Базовый класс параметров генератора сигнала.
    /// Служит единым типом для передачи индивидуальных параметров в фабрику.
    /// </summary>
    public abstract class GeneratorParameters
    {
    }

    /// <summary>
    /// Параметры генератора синусоидального сигнала.
    /// </summary>
    public sealed class SineParameters : GeneratorParameters
    {
        /// <summary>
        /// Период колебаний, с.
        /// </summary>
        public float Period { get; set; }

        /// <summary>
        /// Фазовый сдвиг, с.
        /// </summary>
        public float Phase { get; set; }

        /// <summary>
        /// Амплитуда (масштабирующий коэффициент).
        /// </summary>
        public float Amplitude { get; set; }
    }

    /// <summary>
    /// Параметры генератора линейного сигнала (y = k * t + b).
    /// </summary>
    public sealed class LineParameters : GeneratorParameters
    {
        /// <summary>
        /// Угловой коэффициент наклона прямой (единиц / с).
        /// </summary>
        public float K { get; set; }

        /// <summary>
        /// Свободный член (смещение при t = 0).
        /// </summary>
        public float B { get; set; }
    }

    /// <summary>
    /// Параметры генератора равномерного белого шума.
    /// </summary>
    public sealed class NoiseParameters : GeneratorParameters
    {
        /// <summary>
        /// Размах шума R (значения генерируются на отрезке [-R, R]).
        /// </summary>
        public float Range { get; set; }
    }
}