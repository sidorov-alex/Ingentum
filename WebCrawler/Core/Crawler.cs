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

		public string ExcludePhrase { get; set; } = "";

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
							// Add URL if it is not added yet.

							if (!HasExcludedPhrase(abs) && !context.UrlList.Contains(abs))
							{
								context.UrlList.Add(abs);

								// Crawl this URL for nested links if possible.

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

				// If <img> parsing enabled search for images URLs.

				if (this.IncludeImgTag)
				{
					foreach (var img in document.QuerySelectorAll("img").OfType<AngleSharp.Html.Dom.IHtmlImageElement>())
					{
						if (String.IsNullOrWhiteSpace(img.Source))
							continue;

						if (Uri.TryCreate(img.Source, UriKind.RelativeOrAbsolute, out Uri uri))
						{
							if (!HasExcludedPhrase(uri) && !context.UrlList.Contains(uri))
							{
								context.UrlList.Add(uri);
							}
						}
					}
				}
			}
		}

		private bool HasExcludedPhrase(Uri uri) => !String.IsNullOrWhiteSpace(this.ExcludePhrase) && uri.AbsoluteUri.Contains(this.ExcludePhrase);
	}
}
