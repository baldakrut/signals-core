using System;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Неизменяемый маркер события пересечения порога в конкретном отсчёте сигнала.
    /// </summary>
    public sealed class SignalMarker
    {
        /// <summary>
        /// Индекс отсчёта сигнала (начиная с 0).
        /// </summary>
        public int SampleIndex { get; }

        /// <summary>
        /// Направление пересечения порога.
        /// </summary>
        public CrossingDirection Direction { get; }

        /// <summary>
        /// Создаёт маркер пересечения порога.
        /// </summary>
        /// <param name="sampleIndex">Индекс отсчёта сигнала (должен быть больше или равен 0).</param>
        /// <param name="direction">Направление пересечения порога.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="sampleIndex"/> отрицателен либо <paramref name="direction"/> не является допустимым значением <see cref="CrossingDirection"/>.
        /// </exception>
        public SignalMarker(int sampleIndex, CrossingDirection direction)
        {
            if (sampleIndex < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(sampleIndex),
                    "Индекс отсчёта не может быть отрицательным.");

            if (!Enum.IsDefined(typeof(CrossingDirection), direction))
                throw new ArgumentOutOfRangeException(
                    nameof(direction),
                    "Недопустимое значение направления пересечения.");

            SampleIndex = sampleIndex;
            Direction = direction;
        }
    }
}