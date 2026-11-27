using System;
using System.Collections.Generic;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Анализатор одноканального сигнала, обнаруживающий пересечения заданного порога
    /// и уведомляющий подписчиков через событие <see cref="MarkerDetected"/>.
    /// </summary>
    public sealed class ThresholdAnalyzer
    {
        /// <summary>
        /// Возникает при обнаружении каждого пересечения порога в процессе анализа сигнала.
        /// </summary>
        public event EventHandler<MarkerDetectedEventArgs> MarkerDetected;

        /// <summary>
        /// Выполняет поиск пересечений заданного порога в сигнале.
        /// </summary>
        /// <param name="signal">Анализируемый одноканальный сигнал.</param>
        /// <param name="threshold">Пороговое значение сигнала (конечное число).</param>
        /// <returns>Неизменяемый результат анализа со всеми найденными маркерами.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="signal"/> равен null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="threshold"/> равен NaN или бесконечности.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Среди отсчётов <paramref name="signal"/> обнаружен NaN или бесконечность.
        /// </exception>
        public SignalAnalysisResult Analyze(SingleChannelSignal signal, float threshold)
        {
            if (signal == null)
                throw new ArgumentNullException(nameof(signal), "Сигнал не может быть null.");

            if (float.IsNaN(threshold) || float.IsInfinity(threshold))
                throw new ArgumentOutOfRangeException(
                    nameof(threshold),
                    "Пороговое значение должно быть конечным числом.");

            float[] data = signal.Data;
            for (int i = 0; i < data.Length; i++)
            {
                if (float.IsNaN(data[i]) || float.IsInfinity(data[i]))
                {
                    throw new ArgumentException(
                        $"Отсчёт сигнала с индексом {i} содержит недопустимое значение ({data[i]}).",
                        nameof(signal));
                }
            }

            var markers = new List<SignalMarker>();

            for (int i = 1; i < data.Length; i++)
            {
                float previous = data[i - 1];
                float current = data[i];

                SignalMarker marker = null;

                if (previous < threshold && current >= threshold)
                {
                    marker = new SignalMarker(i, CrossingDirection.Up);
                }
                else if (previous >= threshold && current < threshold)
                {
                    marker = new SignalMarker(i, CrossingDirection.Down);
                }

                if (marker != null)
                {
                    markers.Add(marker);
                    MarkerDetected?.Invoke(this, new MarkerDetectedEventArgs(signal, marker));
                }
            }

            return new SignalAnalysisResult(signal, markers);
        }
    }
}