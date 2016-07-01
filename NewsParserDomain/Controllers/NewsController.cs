using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using NewsParserDomain.Models;

namespace NewsParserDomain.Controllers
{
    public class NewsController
    {
        public static List<NewsItem> GetNews(String feedUrl)
        {
            var lista = new List<NewsItem>();
            var url = feedUrl;
            var reader = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(reader);
            reader.Close();
            if (feed != null)
                foreach (var item in feed.Items)
                {
                    lista.Add(new NewsItem()
                    {
                        Title =  item.Title.Text,
                        Summary = UnHtml(item.Summary.Text)
                    
                    });
                }
            return lista;
        }

        public static string CleanInvalidXmlChars(string text)
        {
            string re = @"[^\x09\x0A\xA0\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
            var temp = Regex.Replace(text, re, "");
            string re2 = @"<a [^>]*>(.*?)<\/a>";
            return Regex.Replace(temp, re2, "");
        }

        private static readonly Regex _tags_ = new Regex(@"<[^>]+?>", RegexOptions.Multiline | RegexOptions.Compiled);

        //add characters that are should not be removed to this regex
        private static readonly Regex _notOkCharacter_ = new Regex(@"[^\w;&#@.:/\\?=|%!() -]", RegexOptions.Compiled);

        public static String UnHtml(String html)
        {
            html = HttpUtility.UrlDecode(html);
            html = HttpUtility.HtmlDecode(html);

            html = RemoveTag(html, "<!--", "-->");
            html = RemoveTag(html, "<script", "</script>");
            html = RemoveTag(html, "<style", "</style>");

            //replace matches of these regexes with space
            html = _tags_.Replace(html, " ");
            html = _notOkCharacter_.Replace(html, " ");
            html = SingleSpacedTrim(html);

            return html;
        }

        private static String RemoveTag(String html, String startTag, String endTag)
        {
            Boolean bAgain;
            do
            {
                bAgain = false;
                Int32 startTagPos = html.IndexOf(startTag, 0, StringComparison.CurrentCultureIgnoreCase);
                if (startTagPos < 0)
                    continue;
                Int32 endTagPos = html.IndexOf(endTag, startTagPos + 1, StringComparison.CurrentCultureIgnoreCase);
                if (endTagPos <= startTagPos)
                    continue;
                html = html.Remove(startTagPos, endTagPos - startTagPos + endTag.Length);
                bAgain = true;
            } while (bAgain);
            return html;
        }

        private static String SingleSpacedTrim(String inString)
        {
            StringBuilder sb = new StringBuilder();
            Boolean inBlanks = false;
            foreach (Char c in inString)
            {
                switch (c)
                {
                    case '\r':
                    case '\n':
                    case '\t':
                    case ' ':
                        if (!inBlanks)
                        {
                            inBlanks = true;
                            sb.Append(' ');
                        }
                        continue;
                    default:
                        inBlanks = false;
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString().Trim();
        }

    }
}
