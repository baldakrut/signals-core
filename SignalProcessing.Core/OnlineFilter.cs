using System;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Потоковый (онлайн) фильтр скользящего среднего на базе циклического буфера.
    /// </summary>
    public class OnlineFilter
    {
        private readonly int _windowSize;
        private readonly CircularBuffer<float> _buffer;
        private double _sum;
        private bool _isInitialized;

        /// <summary>
        /// Создаёт экземпляр онлайн-фильтра с заданным размером окна.
        /// </summary>
        /// <param name="windowSize">Размер окна фильтрации в отсчётах (должен быть больше 0).</param>
        /// <exception cref="ArgumentOutOfRangeException">Размер окна меньше или равен 0.</exception>
        public OnlineFilter(int windowSize)
        {
            if (windowSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(windowSize), "Размер окна должен быть положительным.");

            _windowSize = windowSize;
            _buffer = new CircularBuffer<float>(windowSize);
            _sum = 0.0;
            _isInitialized = false;
        }

        /// <summary>
        /// Фильтрует очередной фрагмент сигнала с сохранением состояния между вызовами.
        /// </summary>
        /// <param name="data">Входной фрагмент отсчётов сигнала.</param>
        /// <returns>Новый массив отфильтрованных отсчётов той же длины.</returns>
        /// <exception cref="ArgumentNullException">Входной массив равен null.</exception>
        public float[] Filter(float[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Массив данных не может быть null.");
            if (data.Length == 0)
                return Array.Empty<float>();

            float[] result = new float[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                float sample = data[i];

                if (!_isInitialized)
                {
                    for (int j = 0; j < _windowSize - 1; j++)
                        _buffer.Add(sample);

                    _sum = (_windowSize - 1) * (double)sample;
                    _isInitialized = true;
                }

                if (_buffer.IsFull)
                {
                    _sum -= _buffer[0];
                }

                _buffer.Add(sample);
                _sum += sample;

                result[i] = (float)(_sum / _windowSize);
            }

            return result;
        }

        /// <summary>
        /// Сбрасывает состояние фильтра: следующий вызов <see cref="Filter"/> начинает новый сигнал.
        /// </summary>
        public void Reset()
        {
            _buffer.Clear();
            _sum = 0.0;
            _isInitialized = false;
        }
    }
}