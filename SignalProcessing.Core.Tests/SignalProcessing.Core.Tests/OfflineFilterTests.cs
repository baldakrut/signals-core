using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class OfflineFilterTests
    {
        [Fact]
        public void Filter_ConstantSignal_ReturnsSameConstant()
        {
            // Arrange
            float[] data = { 5, 5, 5, 5, 5 };

            // Act
            float[] result = OfflineFilter.Filter(data, 3);

            // Assert
            AssertSignal.Equal(data, result);
        }
    }
}