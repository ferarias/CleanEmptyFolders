using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace CleanEmptyFolders
{
	class MainClass
	{
		public static int Main(string[] args)
		{
			Console.WriteLine("Clean Empty folders");

			var result = Parser.Default.ParseArguments<Options>(args);
			var exitCode = result.MapResult(options =>
											DoIt(options), errors =>
											{

												Console.WriteLine(errors);
												return 1;
											});
			return exitCode;
		}

		static int DoIt(Options options)
		{
			if (options.Verbose) Console.WriteLine("Starting folder: {0}", options.StartingFolder);

			if (!Directory.Exists(options.StartingFolder))
			{
				Console.WriteLine("Folder '{0}' does not exist.", options.StartingFolder);
				return 1;
			}
			else
			{
				Console.WriteLine("Starting in '{0}'.", options.StartingFolder);
			}


			var emptyDirs = ProcessDir(options.StartingFolder);

			foreach (var item in emptyDirs)
			{
				Console.Write(item);
				if (options.Execute)
				{
					Directory.Delete(item, true);
					Console.Write(" deleted!");
				}
				else
				{
					Console.WriteLine(" would be deleted");
				}
				Console.WriteLine();
			}
			return 0;

		}

		private static IEnumerable<string> ProcessDir(string dir)
		{
			var emptyDirs = new List<string>();

			var dirs = Directory.GetDirectories(dir);
			foreach (var item in dirs)
			{
				ProcessDir(item);
			}

			var files = Directory.GetFiles(dir);

			if (dirs.Length == 0 && (FilesAreUnimportant(files)))
				emptyDirs.Add(dir);

			return emptyDirs;
		}

		private static bool FilesAreUnimportant(string[] files)
		{
			if (files.Length == 0)
				return true;

			foreach (var item in files)
			{
				if (!Path.GetFileName(item).StartsWith(".", StringComparison.CurrentCulture))
					return false;
			}
			return true;

		}
	}
}
