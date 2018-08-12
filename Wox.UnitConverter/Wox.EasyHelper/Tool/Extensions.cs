using System;
using System.Collections.Generic;
using System.Text;

namespace Wox.UnitConverter.Wox.EasyHelper.Tool
{
    public static class Extensions
    {
        public static string FormatWith(this string self, params object[] args) => string.Format(self, args);

        public static bool MatchPattern(this string command, string pattern) => string.IsNullOrEmpty(pattern) || command.Contains(pattern);
    }
}