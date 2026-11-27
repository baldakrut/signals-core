using System;
using System.Collections.Generic;

namespace SignalProcessing.Core
{
    public class OnlineFilter
    {
        private readonly int _windowSize;
        private readonly Queue<float> _buffer;
        private bool _isInitialized;

        public OnlineFilter(int windowSize)
        {
            if (windowSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(windowSize), "Размер окна должен быть положительным.");

            _windowSize = windowSize;
            _buffer = new Queue<float>(windowSize);
            _isInitialized = false;
        }

        public float[] Filter(float[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Массив данных не может быть null.");
            if (data.Length == 0)
                return Array.Empty<float>();

            float[] result = new float[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                if (!_isInitialized)
                {
                    for (int j = 0; j < _windowSize - 1; j++)
                        _buffer.Enqueue(data[i]);
                    _isInitialized = true;
                }

                _buffer.Enqueue(data[i]);
                if (_buffer.Count > _windowSize)
                    _buffer.Dequeue();

                double sum = 0;
                foreach (float val in _buffer)
                    sum += val;

                result[i] = (float)(sum / _windowSize);
            }

            return result;
        }
    }
}