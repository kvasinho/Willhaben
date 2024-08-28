using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;

namespace Willhaben.Tests.UnitTests;

public class CategoryUnitTests
{
    [Fact]
    public void SetSelectedCategory_To_InvalidKey_ShouldThrowException()
    {
        // Arrange
        var category = new Category();
        // Assert
        Assert.Throws<InvalidCategoryException>(()=>category.SelectedCategory = "asd");
    }
    [Fact]
    public void SetSelectedCategory_To_EmptyKey_ShouldThrowException()
    {
        // Arrange
        var category = new Category();
        // Assert
        Assert.Throws<InvalidCategoryException>(()=>category.SelectedCategory = String.Empty);
    }
    
    [Fact]
    public void SetSelectedCategory_To_ValidKey_ShouldSucceed()
    {
        // Arrange
        var category = new Category();
        category.SelectedCategory = "ballsport";
        // Assert
        Assert.Equal("ballsportarten-4408",category.SelectedCategory );
    }
}