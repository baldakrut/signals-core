namespace SignalProcessing.Core
{
    /// <summary>
    /// Общий интерфейс генератора одноканального сигнала (паттерн Strategy).
    /// </summary>
    public interface ISignalGenerator
    {
        /// <summary>
        /// Генерирует одноканальный сигнал с заданными параметрами дискретизации.
        /// </summary>
        /// <param name="sampling">Параметры дискретизации (частота квантования, Гц и длительность, с).</param>
        /// <returns>Сгенерированный одноканальный сигнал.</returns>
        SingleChannelSignal Generate(SamplingParameters sampling);
    }
}