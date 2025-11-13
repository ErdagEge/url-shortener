using System.Text.RegularExpressions;
using FluentAssertions;


namespace ShortUrl.Tests;


public class LinkTests
{
    [Fact]
    public void ShortCode_GeneratesAlphaNumeric()
    {
        const string charset = "^[a-zA-Z0-9]+$";
        var rx = new Regex(charset);
        for (var i = 0; i < 100; i++)
        {
            var code = Generate(6);
            code).Should().HaveLength(6);
        rx.IsMatch(code).Should().BeTrue();
    }
}


private static string Generate(int len)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var rng = Random.Shared;
        return new string(Enumerable.Range(0, len).Select(_ => chars[rng.Next(chars.Length)]).ToArray());
    }
}