using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace CleanEmptyFolders
{
	public class Options
	{
		[Option('f', "folder", Required = true, HelpText = "Starting folder.")]
		public string StartingFolder { get; set; }

		[Option('x', "execute", Required = false, Default = false, HelpText = "Actually execute changes. Otherwise, just simulate.")]
		public bool Execute { get; set; }

		// Omitting long name, default --verbose
		[Option(HelpText = "Prints all messages to standard output.")]
		public bool Verbose { get; set; }

		[Usage(ApplicationAlias = "CleanEmptyFolders")]
		public static IEnumerable<Example> Examples
		{
			get
			{
				return new List<Example>()
				{
					new Example("Check which empty folders will be deleted (dry run)", new Options { StartingFolder = "D:\\photo", Execute = false })
				};
			}
		}
	}
}
