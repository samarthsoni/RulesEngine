using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine
{
    static class StringExtensions
    {
        public static bool StartsWithAny(this string source, IEnumerable<string> characters)
        {
            foreach (var valueToCheck in characters)
            {
                if (source.StartsWith(valueToCheck))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
