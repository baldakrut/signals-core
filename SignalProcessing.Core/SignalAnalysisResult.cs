using System;
using System.Collections.Generic;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Неизменяемый результат анализа сигнала, связывающий сигнал и коллекцию найденных маркеров.
    /// </summary>
    public sealed class SignalAnalysisResult
    {
        /// <summary>
        /// Проанализированный одноканальный сигнал.
        /// </summary>
        public SingleChannelSignal Signal { get; }

        /// <summary>
        /// Коллекция найденных маркеров, доступная только для чтения.
        /// </summary>
        public IReadOnlyList<SignalMarker> Markers { get; }

        /// <summary>
        /// Создаёт результат анализа сигнала.
        /// </summary>
        /// <param name="signal">Анализируемый сигнал.</param>
        /// <param name="markers">Коллекция маркеров, найденных в сигнале.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="signal"/> или <paramref name="markers"/> равен null, либо <paramref name="markers"/> содержит элемент null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Индекс одного из маркеров выходит за пределы длины массива данных сигнала.
        /// </exception>
        public SignalAnalysisResult(SingleChannelSignal signal, IEnumerable<SignalMarker> markers)
        {
            if (signal == null)
                throw new ArgumentNullException(nameof(signal), "Сигнал не может быть null.");

            if (markers == null)
                throw new ArgumentNullException(nameof(markers), "Коллекция маркеров не может быть null.");

            var copy = new List<SignalMarker>();
            int dataLength = signal.Data.Length;

            foreach (SignalMarker marker in markers)
            {
                if (marker == null)
                    throw new ArgumentNullException(nameof(markers), "Коллекция маркеров не может содержать элемент null.");

                if (marker.SampleIndex >= dataLength)
                    throw new ArgumentOutOfRangeException(
                        nameof(markers),
                        $"Индекс маркера ({marker.SampleIndex}) выходит за пределы длины сигнала ({dataLength}).");

                copy.Add(marker);
            }

            Signal = signal;
            Markers = copy.AsReadOnly();
        }
    }
}