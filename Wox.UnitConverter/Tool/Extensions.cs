using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wox.UnitConverter.Tool
{
    static public class Extensions
    {
        public static Tuple<string, string> Separate(this string value, string pattern)
        {
            var patternIndex = value.IndexOf(pattern);
            if (patternIndex < 0)
            {
                return null;
            }
            var left = value.Substring(0, patternIndex);
            var right = value.Substring(patternIndex + pattern.Length, value.Length - (patternIndex + pattern.Length));
            return new Tuple<string, string>(left, right);
        }

        public static Tuple<string, string> SeparateAndTrim(this string value, string pattern)
        {
            var result = value.Separate(pattern);
            if (result != null)
            {
                return new Tuple<string, string>(result.Item1.Trim(' ', '\t', '\r', '\n'), result.Item2.Trim(' ', '\t', '\r', '\n'));
            }
            return null;
        }
    }
}