using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;
using Xunit;

namespace Willhaben.Tests.UnitTests
{
    public class PriceRangeTests
    {
        [Fact]
        public void SetPriceFrom_Should_SetPriceFrom_When_Valid()
        {
            // Arrange
            var priceRange = new PriceRange();

            // Act
            priceRange.SetPriceFrom(100);

            // Assert
            Assert.Equal(100, priceRange.PriceFrom);
        }

        [Fact]
        public void SetPriceTo_Should_SetPriceTo_When_Valid()
        {
            // Arrange
            var priceRange = new PriceRange();

            // Act
            priceRange.SetPriceTo(200);

            // Assert
            Assert.Equal(200, priceRange.PriceTo);
        }

        [Fact]
        public void SetPriceFrom_Should_ThrowException_When_NegativeValue()
        {
            // Arrange
            var priceRange = new PriceRange();

            // Act & Assert
            Assert.Throws<NegativePriceException>(() => priceRange.SetPriceFrom(-100));
        }

        [Fact]
        public void SetPriceTo_Should_ThrowException_When_NegativeValue()
        {
            // Arrange
            var priceRange = new PriceRange();

            // Act & Assert
            Assert.Throws<NegativePriceException>(() => priceRange.SetPriceTo(-200));
        }

        [Fact]
        public void SetPriceFrom_Should_ThrowException_When_PriceFromGreaterThanPriceTo()
        {
            // Arrange
            var priceRange = new PriceRange();
            priceRange.SetPriceTo(200);

            // Act & Assert
            Assert.Throws<MaximumPriceLowerThanMinimumPriceException>(() => priceRange.SetPriceFrom(300));
        }

        [Fact]
        public void SetPriceTo_Should_ThrowException_When_PriceToLessThanPriceFrom()
        {
            // Arrange
            var priceRange = new PriceRange();
            priceRange.SetPriceFrom(200);

            // Act & Assert
            Assert.Throws<MaximumPriceLowerThanMinimumPriceException>(() => priceRange.SetPriceTo(100));
        }

        [Fact]
        public void SetPriceTo_Should_Work_When_PriceToIsGreaterThanOrEqualPriceFrom()
        {
            // Arrange
            var priceRange = new PriceRange();
            priceRange.SetPriceFrom(100);

            // Act
            priceRange.SetPriceTo(200);

            // Assert
            Assert.Equal(200, priceRange.PriceTo);
        }

        [Fact]
        public void SetPriceFrom_Should_Work_When_PriceFromIsLessThanOrEqualPriceTo()
        {
            // Arrange
            var priceRange = new PriceRange();
            priceRange.SetPriceTo(300);

            // Act
            priceRange.SetPriceFrom(200);

            // Assert
            Assert.Equal(200, priceRange.PriceFrom);
        }
        
        [Fact]
        public void SetPriceTo_ThenPriceFrom_BothToZero_Should_WorkCorrectly()
        {
            // Arrange
            var priceRange = new PriceRange();

            // Act
            priceRange.SetPriceTo(0);
            priceRange.SetPriceFrom(0);

            // Assert
            Assert.Equal(0, priceRange.PriceTo);
            Assert.Equal(0, priceRange.PriceFrom);
        }

        [Fact]
        public void SetPriceFrom_ThenPriceTo_BothToZero_Should_WorkCorrectly()
        {
            // Arrange
            var priceRange = new PriceRange();

            // Act
            priceRange.SetPriceFrom(0);
            priceRange.SetPriceTo(0);

            // Assert
            Assert.Equal(0, priceRange.PriceFrom);
            Assert.Equal(0, priceRange.PriceTo);
        }


        [Fact]
        public void SetPriceFromZero_AfterSettingPriceTo_Should_WorkCorrectly()
        {
            // Arrange
            var priceRange = new PriceRange();
            priceRange.SetPriceTo(100);

            // Act
            priceRange.SetPriceFrom(0);

            // Assert
            Assert.Equal(0, priceRange.PriceFrom);
            Assert.Equal(100, priceRange.PriceTo);
        }

        [Fact]
        public void SetPriceFromToZero_WithPriceToAlreadySet_Should_WorkCorrectly()
        {
            // Arrange
            var priceRange = new PriceRange();
            priceRange.SetPriceTo(100);

            // Act
            priceRange.SetPriceFrom(0);

            // Assert
            Assert.Equal(0, priceRange.PriceFrom);
            Assert.Equal(100, priceRange.PriceTo);
        }

        [Fact]
        public void SetPriceToToZero_WithPriceFromAlreadySet_Should_Throw_PriceException()
        {
            // Arrange
            var priceRange = new PriceRange();
            priceRange.SetPriceFrom(100);

            // Act
            Assert.Throws<MaximumPriceLowerThanMinimumPriceException>(()=>priceRange.SetPriceTo(0));

        }
    }
}
