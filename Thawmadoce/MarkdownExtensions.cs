using System;
using System.Text;
using System.Text.RegularExpressions;
using MarkdownSharp;

namespace Thawmadoce
{
    public static class MarkdownExtensions
    {
        private const string Start = @"
<html>
<head>
<style>
html, body {
    margin: 10px 100px 0px 50px;
    padding: 0;
    font-family:  Arial, sans-serif;
	font-size: 16px;
	line-height: 1.5em;
    color: #224400;
  }
 </style>
</head>
<body>
";
        private const string End = "</body></html>";
        private static readonly Regex _nonEmptyLine = new Regex(@"^(.*?\S.*)$+", RegexOptions.RightToLeft | RegexOptions.Multiline);
        private static readonly Markdown _markdown = new Markdown();
        private static readonly object _markdownDoesNotFeelThreadSafeMeThinks = new object();

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
            string tx;
            lock (_markdownDoesNotFeelThreadSafeMeThinks)
                tx = _markdown.Transform(markdowntext);
            var sb = new StringBuilder();
            sb.AppendLine(Start);
            sb.AppendLine(tx);
            sb.AppendLine(End);
            return sb.ToString();
        }
    }
}