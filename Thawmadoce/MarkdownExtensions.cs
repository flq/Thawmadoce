using System;
using System.Text;
using System.Text.RegularExpressions;
using MarkdownSharp;

namespace Thawmadoce
{
    public static class MarkdownExtensions
    {
        private const string StartPreview = @"
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

        private const string StartSaveFile = @"
<html>
<head>
	<link href=""markdown.css"" rel=""stylesheet""></link>
	<script src=""http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"" type=""text/javascript""></script>
	<script src=""jquery.tableofcontents.min.js"" type=""text/javascript"" charset=""utf-8""></script>
	<script type=""text/javascript"" charset=""utf-8"">
	  $(function(){ $(""#toc"").tableOfContents(); })
	</script>
</head>
<body>
<ul id=""toc""></ul>
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

        public static string ToHtml(this string markdowntext, bool forPreview = true)
        {
            string tx;
            lock (_markdownDoesNotFeelThreadSafeMeThinks)
                tx = _markdown.Transform(markdowntext);

            return CreateHtml(forPreview ? StartPreview : StartSaveFile, tx);
        }

        private static string CreateHtml(string startPreview, string text)
        {
            var sb = new StringBuilder();
            sb.AppendLine(startPreview);
            sb.AppendLine(text);
            sb.AppendLine(End);
            return sb.ToString();
        }
    }
}