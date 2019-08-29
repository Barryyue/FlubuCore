using System.Text.RegularExpressions;

namespace FlubuCore.Packaging
{
    public class RegexFilter : IFilter
    {
        private readonly Regex _filterRegex;

        /// <summary>
        /// Filter files by regex expression.
        /// </summary>
        /// <param name="filterRegexValue">The regex expression.</param>
        public RegexFilter(string filterRegexValue)
        {
            _filterRegex = new Regex(filterRegexValue, RegexOptions.IgnoreCase);
        }

        public bool IsPassedThrough(string path)
        {
            return !_filterRegex.IsMatch(path);
        }
    }
}