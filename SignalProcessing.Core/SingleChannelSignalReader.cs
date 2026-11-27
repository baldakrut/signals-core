using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace SignalProcessing.Core
{
    public static class SingleChannelSignalReader
    {
        public static SingleChannelSignal Read(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
                throw new InvalidDataException("Файл сигнала пуст.");

            if (!float.TryParse(lines[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float frequency) || frequency <= 0)
                throw new InvalidDataException("Строка 1: некорректная частота квантования.");

            float[] data = new float[lines.Length - 1];
            for (int i = 1; i < lines.Length; i++)
            {
                if (!float.TryParse(lines[i], NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
                    throw new InvalidDataException($"Строка {i + 1}: некорректное значение сигнала '{lines[i]}'.");
                data[i - 1] = value;
            }

            return new SingleChannelSignal(data, frequency);
        }

    }
}
