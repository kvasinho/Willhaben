using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;
using Willhaben.Scraper.Products;
using Xunit;

namespace Willhaben.Tests.UnitTests;

public class KeywordUnitTests
{
    [Fact]
    public void Constructor_Should_CreateKeywords()
    {
        var term = "test";
        Keyword k1 = new FuzzyKeyword(term);
        Keyword k2 = new ExactKeyword(term);
        Keyword k3 = new OmitKeyword(term);


        Assert.Equal("test", k1.Value);
        Assert.Equal("%22test%22", k2.Value);
        Assert.Equal("-test", k3.Value);

    }
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void Constructor_Should_FailOnEmptyKeyword(string @base)
    {
        Assert.Throws<KeywordTooShortException>(() => new FuzzyKeyword(@base));
        Assert.Throws<KeywordTooShortException>(() => new ExactKeyword(@base));
        Assert.Throws<KeywordTooShortException>(() => new OmitKeyword(@base));

    }
    
    [Theory]
    [InlineData("a*")]
    [InlineData(".")]
    [InlineData("_")]
    [InlineData("test*")]
    [InlineData("test.")]
    [InlineData("test,")]
    [InlineData("test*")]
    [InlineData("test*")]
    [InlineData(" .")]
    [InlineData("  .")]
    [InlineData(". ")]
    [InlineData(".  ")]
    [InlineData("hello.there")]
    [InlineData("hello there")]
    [InlineData("  hello there")]
    [InlineData("  hello   there")]
    public void Constructor_Should_FailOnInvalidCharacter(string @base)
    {
        Assert.Throws<InvalidCharacterException>(() => new FuzzyKeyword(@base));
        Assert.Throws<InvalidCharacterException>(() => new ExactKeyword(@base));
        Assert.Throws<InvalidCharacterException>(() => new OmitKeyword(@base));

    }

    


    
    [Fact]
    public void Equals_Should_ReturnTrueForEqualKeywords()
    {
        // Arrange
        var keyword1 = new FuzzyKeyword("test");
        var keyword2 = new ExactKeyword("test");

        // Act & Assert
        Assert.True(keyword1.Equals(keyword2));
        Assert.True(keyword1 == keyword2);
        Assert.False(keyword1 != keyword2);
    }

    [Fact]
    public void Equals_Should_ReturnFalseForDifferentKeywords()
    {
        // Arrange
        var keyword1 = new FuzzyKeyword("test");
        var keyword2 = new ExactKeyword("different");

        // Act & Assert
        Assert.False(keyword1.Equals(keyword2));
        Assert.False(keyword1 == keyword2);
        Assert.True(keyword1 != keyword2);
    }

    

    
}