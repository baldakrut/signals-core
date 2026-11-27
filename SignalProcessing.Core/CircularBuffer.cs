using System;

namespace SignalProcessing.Core
{
    /// <summary>
    /// Циклический буфер фиксированной ёмкости, хранящий последние добавленные элементы.
    /// </summary>
    /// <typeparam name="T">Тип хранимых элементов.</typeparam>
    public sealed class CircularBuffer<T>
    {
        private readonly T[] _buffer;
        private int _start;
        private int _count;

        /// <summary>
        /// Создаёт циклический буфер заданной ёмкости.
        /// </summary>
        /// <param name="capacity">Максимальное число хранимых элементов (должно быть больше 0).</param>
        /// <exception cref="ArgumentOutOfRangeException">Ёмкость меньше или равна 0.</exception>
        public CircularBuffer(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Ёмкость буфера должна быть положительной.");

            _buffer = new T[capacity];
            _start = 0;
            _count = 0;
        }

        /// <summary>
        /// Ёмкость буфера.
        /// </summary>
        public int Capacity => _buffer.Length;

        /// <summary>
        /// Текущее число хранимых элементов.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Признак того, что буфер полностью заполнен.
        /// </summary>
        public bool IsFull => _count == _buffer.Length;

        /// <summary>
        /// Добавляет элемент в буфер. Если буфер заполнен, вытесняет самый старый элемент.
        /// </summary>
        /// <param name="item">Добавляемый элемент.</param>
        public void Add(T item)
        {
            if (_count < _buffer.Length)
            {
                _buffer[(_start + _count) % _buffer.Length] = item;
                _count++;
            }
            else
            {
                _buffer[_start] = item;
                _start = (_start + 1) % _buffer.Length;
            }
        }

        /// <summary>
        /// Возвращает элемент по логическому индексу (0 — самый старый, Count - 1 — самый новый).
        /// </summary>
        /// <param name="index">Индекс элемента.</param>
        /// <returns>Элемент буфера.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Индекс меньше 0 либо больше или равен Count.</exception>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException(nameof(index), "Индекс выходит за пределы числа элементов буфера.");

                return _buffer[(_start + index) % _buffer.Length];
            }
        }

        /// <summary>
        /// Возвращает копию содержимого буфера в порядке от самого старого элемента к самому новому.
        /// </summary>
        /// <returns>Новый массив с элементами буфера.</returns>
        public T[] ToArray()
        {
            T[] result = new T[_count];
            for (int i = 0; i < _count; i++)
            {
                result[i] = _buffer[(_start + i) % _buffer.Length];
            }

            return result;
        }

        /// <summary>
        /// Удаляет все элементы из буфера, сохраняя его ёмкость.
        /// </summary>
        public void Clear()
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            _start = 0;
            _count = 0;
        }
    }
}