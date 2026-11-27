using System;
using System.Globalization;
using System.IO;
using System.Threading;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SignalFileTests : IDisposable
    {
        private readonly string filePath;

        public SignalFileTests()
        {
            filePath = Path.GetTempFileName();
        }

        public void Dispose()
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        // Тест 15
        [Fact]
        public void WriteAndRead_RoundTrip_PreservesFrequencyAndData()
        {
            float[] data = { 0.5f, -1.25f, 3f, 0.1f };
            var signal = new SingleChannelSignal(data, 250f);

            SingleChannelSignalWriter.Write(signal, filePath);
            SingleChannelSignal loaded = SingleChannelSignalReader.Read(filePath);

            Assert.Equal(250f, loaded.Frequency, 1e-5f);
            AssertSignal.Equal(data, loaded.Data);
        }

        // Тест 16
        [Fact]
        public void Write_UnderRussianCulture_UsesInvariantDotSeparator()
        {
            var signal = new SingleChannelSignal(new float[] { 0.5f, -1.25f, 3f }, 250f);
            CultureInfo previous = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            try
            {
                SingleChannelSignalWriter.Write(signal, filePath);
                string[] lines = File.ReadAllLines(filePath);

                Assert.Equal(new[] { "250", "0.5", "-1.25", "3" }, lines);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = previous;
            }
        }

        // Тест 17
        [Fact]
        public void Read_UnderRussianCulture_ParsesInvariantDotSeparator()
        {
            File.WriteAllLines(filePath, new[] { "100", "0.5", "-1.5" });
            CultureInfo previous = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            try
            {
                SingleChannelSignal loaded = SingleChannelSignalReader.Read(filePath);

                Assert.Equal(100f, loaded.Frequency, 1e-5f);
                AssertSignal.Equal(new float[] { 0.5f, -1.5f }, loaded.Data);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = previous;
            }
        }

        // Тест 18
        [Fact]
        public void Read_OnlyFrequencyLine_ReturnsEmptySignal()
        {
            File.WriteAllLines(filePath, new[] { "100" });

            SingleChannelSignal loaded = SingleChannelSignalReader.Read(filePath);

            Assert.Equal(100f, loaded.Frequency, 1e-5f);
            Assert.Empty(loaded.Data);
        }

        // Тест 19 (пустой файл)
        [Fact]
        public void Read_EmptyFile_ThrowsInvalidDataException()
        {
            File.WriteAllText(filePath, string.Empty);

            Assert.Throws<InvalidDataException>(() => SingleChannelSignalReader.Read(filePath));
        }

        // Тест 19 (некорректное значение)
        [Fact]
        public void Read_InvalidValueLine_ThrowsInvalidDataException()
        {
            File.WriteAllLines(filePath, new[] { "100", "1.0", "abc", "2.0" });

            Assert.Throws<InvalidDataException>(() => SingleChannelSignalReader.Read(filePath));
        }

        // Тест 19 (нулевая частота)
        [Fact]
        public void Read_ZeroFrequency_ThrowsInvalidDataException()
        {
            File.WriteAllLines(filePath, new[] { "0", "1.0" });

            Assert.Throws<InvalidDataException>(() => SingleChannelSignalReader.Read(filePath));
        }
    }
}