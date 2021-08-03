using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimer.Models;
using Xunit;

namespace Dimer.Tests
{
    public class TimeParserTests
    {
        [Theory]
        [InlineData("10", "0:0:0:10")]
        [InlineData("11:10", "0:0:11:10")]
        [InlineData("10:12:12", "0:10:12:12")]
        [InlineData("5:5:05:3", "5:5:5:3")]
        [InlineData("3:3:3:03", "3:3:3:03")]
        [InlineData("0:8:3:5", "0:8:3:5")]
        [InlineData("0:0:0:5", "0:0:0:5")]
        [InlineData("0:0:5:0", "0:0:5:0")]
        [InlineData("0:7:0:0", "0:7:0:0")]
        [InlineData("9:0:0:0", "9:0:0:0")]
        [InlineData("10: 01     :10                :      50", "10:01:10:50")]
        public void ParseTest(string parseString, string expect)
        {
            var actualBool = TimeParser.TryParse(parseString, out var actualTime);

            var expectTime = TimeSpan.Parse(expect);

            actualTime.Ticks.Is(expectTime.Ticks);
            actualBool.Is(true);
        }

        [Theory]
        [InlineData(":")]
        [InlineData(":0")]
        [InlineData(":3:0")]
        [InlineData("1:1:1:1:1")]
        [InlineData(":::")]
        [InlineData(":1:1:")]
        [InlineData("10:$$:37:35")]
        [InlineData("X")]
        [InlineData("Can not")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n\r")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData("999999999999999999999999999999")]
        public void ReturnFalseWhenCantParseTest(string parseString)
        {
            var actual = TimeParser.TryParse(parseString, out var actualTime);

            actual.Is(false);
            actualTime.Ticks.Is(TimeSpan.Zero.Ticks);
        }
    }
}
