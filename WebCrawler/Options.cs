using CommandLine;

namespace WebCrawler
{
	public sealed class Options
	{
		[Value(0, Required = true, HelpText = "Root url from wich to begin crawling.")]
		public string Root { get; set; }

		[Option('o', "output", Default = "results.txt", HelpText = "Output file name that will contain links found (one per line).")]
		public string Output { get; set; }

		[Option('n', "nesting", Default = 0, HelpText = "Crawl nesting level.")]
		public int NestingLevel { get; set; }

		[Option("ftp", Default = false, HelpText = "Include FTP urls.")]
		public bool IncludeFtp { get; set; }

		[Option("img", Default = false, HelpText = "Include <img src=\"...\"> url links.")]
		public bool Images { get; set; }
	}
}
