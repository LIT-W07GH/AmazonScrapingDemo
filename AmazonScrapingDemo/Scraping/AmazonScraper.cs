using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace AmazonScrapingDemo.Scraping
{

    public class AmazonResult
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Price { get; set; }
    }

    public static class AmazonScraper
    {

        public static List<AmazonResult> Search(string searchTerm)
        {
            var result = new List<AmazonResult>();
            var html = GetAmazonHtml(searchTerm);
            var parser = new HtmlParser();
            IHtmlDocument htmlDocument = parser.ParseDocument(html);
            var divs = htmlDocument.QuerySelectorAll(".sg-row");
            foreach (var div in divs)
            {
                var amazonItem = ParseDiv(div);
                if (amazonItem != null)
                {
                    result.Add(amazonItem);
                }
            }

            return result;

        }

        private static AmazonResult ParseDiv(IElement div)
        {
            var titleSpan = div.QuerySelector("span.a-size-medium");
            if (titleSpan == null)
            {
                return null;
            }

            var result = new AmazonResult
            {
                Title = titleSpan.TextContent
            };
            var image = div.QuerySelector("div.a-section img");
            if (image == null)
            {
                return null;
            }
            var src = image.Attributes["src"].Value;
            result.ImageUrl = src;

            var aTag = div.QuerySelector("a.a-link-normal");
            if (aTag == null)
            {
                return null;
            }

            var href = aTag.Attributes["href"].Value;
            result.Url = $"https://amazon.com{href}";

            var priceSpan = div.QuerySelector("span.a-offscreen");
            if (priceSpan != null)
            {
                var price = priceSpan.TextContent;
                result.Price = price;
            }

            return result;
        }

        static string GetAmazonHtml(string search)
        {
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("user-agent", "amazon is terrible");
            var html = client.GetStringAsync($"https://www.amazon.com/s?k={search}").Result;
            return html;
        }
    }
}
