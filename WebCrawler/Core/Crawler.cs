using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp;
using System.Linq;

namespace WebCrawler.Core
{
	class Crawler
	{
		private Uri root;

		public bool IncludeImgTag { get; set; }

		public bool IncludeFtp { get; set; }

		public int NestingLevel { get; set; }

		public Crawler(Uri root)
		{
			this.root = root ?? throw new ArgumentNullException(nameof(root));
		}

		public async Task<ICollection<Uri>> RunAsync()
		{	
			var context = new CrawlerContext();

			await DoCrawl(this.root.AbsoluteUri, context, this.NestingLevel);

			return context.UrlList.ToList();
		}

		private async Task DoCrawl(string url, CrawlerContext context, int nestingLevel)
		{
			// Create a new context for evaluating webpages with the given config.

			var config = Configuration.Default.WithDefaultLoader();
						
			var browsingContext = BrowsingContext.New(config);

			// Just get the DOM representation

			var document = await browsingContext.OpenAsync(url);

			if (document != null)
			{
				foreach (var anchor in document.QuerySelectorAll("a").OfType<AngleSharp.Html.Dom.IHtmlAnchorElement>())
				{
					if (String.IsNullOrWhiteSpace(anchor.Href))
						continue;

					if (Uri.TryCreate(anchor.Href.TrimEnd('/'), UriKind.RelativeOrAbsolute, out Uri uri))
					{
						var abs = uri;

						if (!abs.IsAbsoluteUri)
						{
							abs = new Uri(new Uri(anchor.BaseUri), uri);
						}

						if (abs.Scheme == Uri.UriSchemeHttp || abs.Scheme == Uri.UriSchemeHttps || (abs.Scheme == Uri.UriSchemeFtp && this.IncludeFtp))
						{
							if (!context.UrlList.Contains(abs))
							{
								context.UrlList.Add(abs);

								if (abs.Scheme != Uri.UriSchemeFtp)
								{
									if (nestingLevel > 0)
									{
										await DoCrawl(abs.AbsoluteUri, context, nestingLevel - 1);
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
