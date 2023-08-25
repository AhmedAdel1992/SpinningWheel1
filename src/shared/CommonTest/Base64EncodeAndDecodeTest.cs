using Common.Extensions;
using FluentAssertions;
using Xunit;

namespace CommonTest;

public class Base64EncodeAndDecodeTest
{
    public Base64EncodeAndDecodeTest()
    {
    }

    [Fact]
    public void DecodeBase64_ShouldSuccess_WhenStringNotEmptyOrNull()
    {
      var base64Path=  Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"sample", "invoice-base64.txt");
        var base64 = File.ReadAllText(base64Path);
        var result = base64.Base64Decode();
        result.Should().StartWith("<?xml");
    }
    
    [Fact]
    public void EncodeBase64_ShouldSuccess_WhenStringNotEmptyOrNull()
    {
        var base64Path=  Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"sample", "invoice-base64.txt");
        var base64 = File.ReadAllText(base64Path.Trim());

      var xmlFilePath=  Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"sample", "invoice.xml");
        var xmlString = File.ReadAllText(xmlFilePath.Trim());
        var result = xmlString.Base64Encode();
        // result.Base64Decode().Should().Be(base64.Base64Decode());
        result.Should().StartWith("PD94bWwgdmVyc2lvbj0iMS4wIiBlbm");
    }
}