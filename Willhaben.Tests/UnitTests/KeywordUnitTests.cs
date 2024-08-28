using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;
using Willhaben.Scraper.Products;
using Xunit;

namespace Willhaben.Tests.UnitTests;

public class KeywordUnitTests
{
    [Fact]
    public void Constructor_Should_CreateKeyword()
    {
        var term = "test";
        Keyword keyword = new Keyword(term);

        Assert.False(keyword.IsExactKeyword);
        Assert.False(keyword.IsOmitKeyword);
        Assert.Equal("test", keyword.Value);
    }
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void Constructor_Should_FailOnEmptyKeyword(string @base)
    {
        Assert.Throws<KeywordTooShortException>(() => new Keyword(@base));
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
        Assert.Throws<InvalidCharacterException>(() => new Keyword(@base));
    }
    
    [Fact]
    public void Constructor_Should_ThrowOmitAndExactException_WhenBothFlagsAreTrue()
    {
        var term = "test";
        Assert.Throws<OmitAndExactException>(() => new Keyword(term, isOmitKeyword: true, isExactKeyword: true));
    }
    
    [Fact]
    public void ToString_Should_ReturnExactKeywordString_WhenIsExactKeywordIsTrue()
    {
        // Arrange
        var keyword = new Keyword("test", isExactKeyword: true);

        // Act
        var result = keyword.ToString();

        // Assert
        Assert.Equal("%22test%22", result);
    }

    [Fact]
    public void ToString_Should_ReturnOmitKeywordString_WhenIsOmitKeywordIsTrue()
    {
        // Arrange
        var keyword = new Keyword("test", isOmitKeyword: true);

        // Act
        var result = keyword.ToString();

        // Assert
        Assert.Equal("-test", result);
    }

    [Fact]
    public void ToString_Should_ReturnNormalKeywordString_WhenNoFlagsAreSet()
    {
        // Arrange
        var keyword = new Keyword("test");

        // Act
        var result = keyword.ToString();

        // Assert
        Assert.Equal("test", result);
    }
    
    [Fact]
    public void Equals_Should_ReturnTrueForEqualKeywords()
    {
        // Arrange
        var keyword1 = new Keyword("test");
        var keyword2 = new Keyword("test");

        // Act & Assert
        Assert.True(keyword1.Equals(keyword2));
        Assert.True(keyword1 == keyword2);
        Assert.False(keyword1 != keyword2);
    }

    [Fact]
    public void Equals_Should_ReturnFalseForDifferentKeywords()
    {
        // Arrange
        var keyword1 = new Keyword("test");
        var keyword2 = new Keyword("different");

        // Act & Assert
        Assert.False(keyword1.Equals(keyword2));
        Assert.False(keyword1 == keyword2);
        Assert.True(keyword1 != keyword2);
    }
    
    [Fact]
    public void Constructor_Should_CreateKeyword_WithValidSpecialCharacters()
    {
        // Arrange
        var term = "test123äöüß";

        // Act
        var keyword = new Keyword(term);

        // Assert
        Assert.Equal(term, keyword.Value);
        Assert.False(keyword.IsExactKeyword);
        Assert.False(keyword.IsOmitKeyword);
    }
    
    
    [Fact]
    public void DisplayKeywordList_Should_JoinKeywordsWithUrlSeparator()
    {
        // Arrange
        var keywords = new List<Keyword>
        {
            new Keyword("test1"),
            new Keyword("test2"),
            new Keyword("test3")
        };

        // Act
        var result = Keyword.DisplayKeywordList(keywords);

        // Assert
        Assert.Equal("test1%20test2%20test3", result);
    }



    
}