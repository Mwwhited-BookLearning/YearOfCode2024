using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OobDev.Search.Tests;


[TestClass]
public class StringToolsTests
{
    public required TestContext TestContext { get; set; }

    [DataTestMethod]
    [DataRow("abcdefghijklmnop", @"abcdefghij
klmnop")]
    [DataRow("abcde fghijklmnop", @"abcde
fghijklmno
p")]
    [DataRow("            ", @"
")]
    [DataRow("a            ", @"a
")]
    [DataRow("a            b", @"a
   b")]
    [DataRow("", "")]
    public void Test(string input, string expected)
    {
        var result = input.SplitBy(length: 10).WriteAsLines();
        Assert.AreEqual(expected, result);
    }
}
