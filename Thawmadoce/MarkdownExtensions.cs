using System;
using System.Text.RegularExpressions;
using MarkdownSharp;

namespace Thawmadoce
{
    public static class MarkdownExtensions
    {
        private static readonly Regex _nonEmptyLine = new Regex(@"^(.*?\S.*)$+", RegexOptions.RightToLeft | RegexOptions.Multiline);
        private static readonly Markdown _markdown = new Markdown();

        public static bool EndsOnNewLine(this string text)
        {
            return text.EndsWith(Environment.NewLine);
        }

        public static string FindLastNonEmptyLine(this string text)
        {
            var m = _nonEmptyLine.Match(text);
            return m.Captures.Count > 0 ? m.Groups[0].Value.EndsWith("\r") ? m.Groups[0].Value.Replace("\r", "") : m.Groups[0].Value : null;
        }

        public static string ToHtml(this string markdowntext)
        {
            return _markdown.Transform(markdowntext);
        }
    }
}