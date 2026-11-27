using System;
using System.Linq;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class CircularBufferTests
    {
        [Fact]
        public void Add_BeyondCapacity_OldestElementsAreDropped()
        {
            var buffer = new CircularBuffer<int>(3);
            for (int i = 1; i <= 5; i++)
                buffer.Add(i);

            Assert.Equal(3, buffer.Count);
            Assert.Equal(new[] { 3, 4, 5 }, buffer.ToArray());
            Assert.Equal(3, buffer[0]);
            Assert.Equal(5, buffer[2]);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void Indexer_OutOfRange_Throws(int index)
        {
            var buffer = new CircularBuffer<int>(5);
            buffer.Add(10);
            buffer.Add(20);

            Assert.Throws<ArgumentOutOfRangeException>(() => buffer[index]);
        }

        // Тест 25
        [Fact]
        public void Constructor_NewBuffer_IsEmptyWithExpectedCapacity()
        {
            var buffer = new CircularBuffer<int>(4);

            Assert.Equal(4, buffer.Capacity);
            Assert.Equal(0, buffer.Count);
            Assert.False(buffer.IsFull);
            Assert.Empty(buffer.ToArray());
        }

        // Тест 26
        [Fact]
        public void Add_LessThanCapacity_PreservesOrderAndNotFull()
        {
            var buffer = new CircularBuffer<int>(4);
            buffer.Add(10);
            buffer.Add(20);

            Assert.Equal(2, buffer.Count);
            Assert.False(buffer.IsFull);
            Assert.Equal(new[] { 10, 20 }, buffer.ToArray());
            Assert.Equal(10, buffer[0]);
            Assert.Equal(20, buffer[1]);
        }

        // Тест 27
        [Fact]
        public void Add_ExactlyCapacityElements_IsFullTrue()
        {
            var buffer = new CircularBuffer<int>(3);
            buffer.Add(1);
            buffer.Add(2);
            buffer.Add(3);

            Assert.Equal(3, buffer.Count);
            Assert.True(buffer.IsFull);
        }

        // Тест 28
        [Fact]
        public void Add_CapacityOne_StoresOnlyLastElement()
        {
            var buffer = new CircularBuffer<int>(1);
            buffer.Add(10);
            buffer.Add(20);
            buffer.Add(30);

            Assert.Equal(1, buffer.Count);
            Assert.True(buffer.IsFull);
            Assert.Equal(30, buffer[0]);
            Assert.Equal(new[] { 30 }, buffer.ToArray());
        }

        // Тест 29
        [Fact]
        public void Add_ThousandElementsToCapacitySeven_StoresLastSeven()
        {
            var buffer = new CircularBuffer<int>(7);
            for (int i = 1; i <= 1000; i++)
                buffer.Add(i);

            int[] expected = Enumerable.Range(994, 7).ToArray();
            Assert.Equal(expected, buffer.ToArray());
        }

        // Тест 30
        [Fact]
        public void Clear_AfterOverflow_ResetsStateAndAllowsNewElements()
        {
            var buffer = new CircularBuffer<int>(3);
            for (int i = 1; i <= 5; i++)
                buffer.Add(i);

            buffer.Clear();
            buffer.Add(42);

            Assert.Equal(3, buffer.Capacity);
            Assert.Equal(1, buffer.Count);
            Assert.False(buffer.IsFull);
            Assert.Equal(42, buffer[0]);
        }

        // Тест 31
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_NonPositiveCapacity_ThrowsArgumentOutOfRangeException(int capacity)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CircularBuffer<int>(capacity));
        }

        // Тест 32
        [Fact]
        public void ToArray_ModifyingReturnedArray_DoesNotAffectBuffer()
        {
            var buffer = new CircularBuffer<int>(3);
            buffer.Add(1);
            buffer.Add(2);

            int[] snapshot = buffer.ToArray();
            snapshot[0] = 999;

            Assert.Equal(1, buffer[0]);
            Assert.Equal(new[] { 1, 2 }, buffer.ToArray());
        }
    }
}