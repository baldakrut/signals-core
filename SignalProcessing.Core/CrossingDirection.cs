namespace SignalProcessing.Core
{
    /// <summary>
    /// Направление пересечения порогового значения сигналом.
    /// </summary>
    public enum CrossingDirection
    {
        /// <summary>
        /// Пересечение порога снизу вверх (от значений ниже порога к значениям не ниже порога).
        /// </summary>
        Up,

        /// <summary>
        /// Пересечение порога сверху вниз (от значений не ниже порога к значениям ниже порога).
        /// </summary>
        Down
    }
}