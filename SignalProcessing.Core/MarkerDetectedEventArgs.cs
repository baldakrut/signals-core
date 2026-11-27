using System;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Данные события обнаружения маркера пересечения порога.
    /// </summary>
    public sealed class MarkerDetectedEventArgs : EventArgs
    {
        /// <summary>
        /// Анализируемый одноканальный сигнал.
        /// </summary>
        public SingleChannelSignal Signal { get; }

        /// <summary>
        /// Обнаруженный маркер пересечения порога.
        /// </summary>
        public SignalMarker Marker { get; }

        /// <summary>
        /// Момент времени отсчёта от начала сигнала, с.
        /// </summary>
        public float Time { get; }

        /// <summary>
        /// Значение сигнала в отсчёте маркера.
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Создаёт аргументы события обнаружения маркера.
        /// </summary>
        /// <param name="signal">Анализируемый сигнал.</param>
        /// <param name="marker">Найденный маркер.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="signal"/> или <paramref name="marker"/> равен null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Индекс маркера выходит за пределы длины массива данных сигнала.
        /// </exception>
        public MarkerDetectedEventArgs(SingleChannelSignal signal, SignalMarker marker)
        {
            if (signal == null)
                throw new ArgumentNullException(nameof(signal), "Сигнал не может быть null.");

            if (marker == null)
                throw new ArgumentNullException(nameof(marker), "Маркер не может быть null.");

            if (marker.SampleIndex >= signal.Data.Length)
                throw new ArgumentOutOfRangeException(
                    nameof(marker),
                    $"Индекс маркера ({marker.SampleIndex}) выходит за пределы длины сигнала ({signal.Data.Length}).");

            Signal = signal;
            Marker = marker;
            Time = marker.SampleIndex / signal.Frequency;
            Value = signal.Data[marker.SampleIndex];
        }
    }
}