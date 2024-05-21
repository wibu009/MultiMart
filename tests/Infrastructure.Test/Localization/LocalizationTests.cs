using System.Globalization;
using Microsoft.Extensions.Localization;
using Xunit;

namespace Infrastructure.Test.Localization;

public class LocalizationTests
{
    private const string TestString = "testString";
    private const string TestStringInDutch = "testString in dutch";
    private const string TestStringInBelgianDutch = "testString in belgian dutch";
    private const string TestString2 = "testString2";
    private const string TestString2InDutch = "testString2 in dutch";

    private readonly IStringLocalizer _t;

    public LocalizationTests(IStringLocalizer<LocalizationTests> t) => _t = t;

    // there's no "en-US" folder
    // "nl-BE/test.po" only contains testString
    // "nl/test.po" contains both testString and testString2
    [Theory]
    [InlineData("en-US", TestString, TestString)]
    [InlineData("nl-NL", TestString, TestStringInDutch)]
    [InlineData("nl-BE", TestString, TestStringInBelgianDutch)]
    [InlineData("nl-NL", TestString2, TestString2InDutch)]
    [InlineData("nl-BE", TestString2, TestString2InDutch)]
    public void TranslateToCultureTest(string culture, string testString, string translatedString)
    {
        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);

        var result = _t[testString];

        Assert.Equal(translatedString, result);
    }
}