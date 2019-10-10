using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Core;

namespace WebCrawler
{
	class Program
	{
		static async Task<int> Main(string[] args)
		{
			var result = Parser.Default.ParseArguments<Options>(args);

			if (result.Tag == ParserResultType.Parsed)
			{
				Options opts = ((Parsed<Options>)result).Value;

				await RunWithOptions(opts);

				return 0;
			}
			else
			{
				return -1;
			}
		}

		private async static Task RunWithOptions(Options opts)
		{
			var uri = new Uri(opts.Root);

			// Do the scan asynchronously.

			var crawler = new Crawler(uri)
			{
				NestingLevel = opts.NestingLevel,
				ExcludePhrase = opts.ExcludePhrase,
				IncludeFtp = opts.IncludeFtp,
				IncludeImgTag = opts.Images
			};

			var list = await crawler.RunAsync();

			// Save urls to specified output file.

			await WriteResults(list, opts.Output);
		}

		private static async Task WriteResults(ICollection<Uri> list, string fileName)
		{
			await File.WriteAllLinesAsync(fileName, list.Select(x => x.AbsoluteUri));
		}
	}
}
