using System;
using System.Globalization;
using System.IO;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Записывает одноканальный сигнал в текстовый файл.
    /// </summary>
    public static class SingleChannelSignalWriter
    {
        /// <summary>
        /// Сохраняет частоту квантования и отсчёты сигнала в файл с инвариантным форматом чисел.
        /// </summary>
        /// <param name="signal">Записываемый сигнал.</param>
        /// <param name="filePath">Путь к выходному файлу.</param>
        /// <exception cref="ArgumentNullException">Сигнал равен null.</exception>
        /// <exception cref="ArgumentException">Путь к файлу пуст или состоит из пробелов.</exception>
        public static void Write(SingleChannelSignal signal, string filePath)
        {
            if (signal == null)
                throw new ArgumentNullException(nameof(signal), "Сигнал не может быть null.");
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым.", nameof(filePath));

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine(signal.Frequency.ToString("R", CultureInfo.InvariantCulture));
                foreach (float value in signal.Data)
                {
                    writer.WriteLine(value.ToString("R", CultureInfo.InvariantCulture));
                }
            }
        }
    }
}