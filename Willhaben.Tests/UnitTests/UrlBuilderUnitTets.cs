using Willhaben.Domain.Exceptions;
using Willhaben.Scraper.Products;
using Xunit.Abstractions;

namespace Willhaben.Tests.UnitTests;

public class UrlBuilderUnitTets
{

    public Guid SfId = Guid.Parse("35042f29-d5c6-4080-b0ed-65a07e161896");
    
    private readonly ITestOutputHelper _output;

    public UrlBuilderUnitTets(ITestOutputHelper output)
    {
        _output = output;
    }
    [Fact] 
    public void UrlBuilder_Should_Return_BaseUrl()
    {
        // Arrange
        var builder = new UrlBuilder(SfId);
        // Assert
        Assert.Equal($"https://www.willhaben.at/webapi/iad/search/atz/seo/kaufen-und-verkaufen/marktplatz?sfId={SfId}&rows={builder.Rows}&isNavigation=true&keyword=",builder.Url);
    }
    
    [Theory]
    [InlineData("test")]
    [InlineData(" test")]
    [InlineData(" test ")]
    public void UrlBuilder_Should_HandleValidKeywordInput(string @base)
    {
        // Arrange
        var builder = new UrlBuilder(SfId);
        _output.WriteLine($"Testing with base: '{@base}'");

        // Act
        builder.AddKeyword(@base);
        builder.AddExactKeyword(@base);
        builder.AddOmitKeyword(@base);

        var keyword1 = builder.Keywords[0].Value;
        var keyword2 = builder.Keywords[1].Value;
        var keyword3 = builder.Keywords[2].Value;
    
        _output.WriteLine($"Keyword1: {keyword1}");
        _output.WriteLine($"Keyword2: {keyword2}");
        _output.WriteLine($"Keyword3: {keyword3}");
    
        // Assert
        Assert.Equal("test", keyword1);
        Assert.Equal("%22test%22", keyword2);
        Assert.Equal("-test", keyword3);
        Assert.Equal("test", builder.Keywords[0].ToString());
        Assert.Equal("%22test%22", builder.Keywords[1].ToString());
        Assert.Equal("-test", builder.Keywords[2].ToString());

        var expectedUrl = $"https://www.willhaben.at/webapi/iad/search/atz/seo/kaufen-und-verkaufen/marktplatz?sfId={SfId}&rows={builder.Rows}&isNavigation=true&keyword=" +
                          $"{keyword1}%20{keyword2}%20{keyword3}";
        _output.WriteLine($"Expected URL: {expectedUrl}");
        _output.WriteLine($"Actual URL: {builder.Url}");

        Assert.Equal(expectedUrl, builder.Url);
    }
    
    [Theory]
    [InlineData(".")]
    [InlineData("valid")]
    [InlineData(" test  test")]
    [InlineData("")]
    [InlineData(" valid")]
    [InlineData(" valid ")]
    [InlineData(",")]
    [InlineData(" ßvalid")]
    [InlineData(" üvalid ")]
    [InlineData(" ävalid")]
    [InlineData(" övalid")]
    [InlineData(" -test ")]
    [InlineData("a")]
    [InlineData(" ")] 
    [InlineData("    ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData("!")] 
    [InlineData("\"test\"")]
    [InlineData("こんにちは")] // Japanese characters
    [InlineData("😊test")] // Emoji
    [InlineData("' OR '1'='1")] // SQL injection-like input
    [InlineData("<script>alert('test')</script>")] // HTML/JS injection
    [InlineData("Valid")] // Mixed case
    [InlineData("123valid")] // Starts with numbers
    [InlineData(".test")] // Leading punctuation
    [InlineData("val!id")] // Mix of valid and invalid characters

    public void UrlBuilder_AddKeyword_ShouldHandleDIfferentInputs(string @base)
    {
        // Arrange
        var builder = new UrlBuilder(SfId);
        
        if (@base.ToLower().Contains("valid") || @base.Equals("a"))
        {
            builder.AddKeyword(@base);
            builder.AddExactKeyword(@base);
            builder.AddOmitKeyword(@base);

            var keyword = @base.Trim().ToLower();
            var comp1 = keyword;
            var comp2 = $"%22{keyword}%22";
            var comp3 = $"-{keyword}";
            
            Assert.Equal(builder.Keywords[0].Value, comp1);
            Assert.Equal(builder.Keywords[1].Value, comp2);
            Assert.Equal(builder.Keywords[2].Value, comp3);
        }
        else
        {
            Assert.ThrowsAny<KeywordException>(() => builder.AddKeyword(@base));
            Assert.ThrowsAny<KeywordException>(() => builder.AddExactKeyword(@base));
            Assert.ThrowsAny<KeywordException>(() => builder.AddOmitKeyword(@base));
        }
    }
    

}