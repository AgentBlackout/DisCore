using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DisCore.Shared.Helpers
{
    public static class StringHelper
    {
        public static IEnumerable<string> ExtractString(char separator, string message)
        {
            string regex = $"\\{separator}[^\\{separator}]*\\{separator}";
            var matches = Regex.Matches(message, regex);

            var sections = new List<string>();
            foreach (var match in matches)
            {
                sections.Add(match.ToString());
            }
            return sections;
        }
    }
}
