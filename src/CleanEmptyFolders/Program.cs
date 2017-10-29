using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;

namespace CleanEmptyFolders
{
	class MainClass
	{
		private static List<string> emptyDirs = new List<string>();

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

			emptyDirs = new List<string>();
			ProcessDir(options.StartingFolder);

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

		private static void ProcessDir(string dir)
		{
			var dirs = Directory.GetDirectories(dir);
			foreach (var item in dirs)
			{
				ProcessDir(item);
			}

			var files = Directory.GetFiles(dir);

			if (dirs.Length == 0 && (FilesAreUnimportant(files)))
				emptyDirs.Add(dir);

		}

		private static bool FilesAreUnimportant(string[] files)
		{
			return files.Any() && files.All(FileIsUnimportant);

		}

		static bool FileIsUnimportant(string item)
		{
			if (item.EndsWith(".AAE", StringComparison.InvariantCultureIgnoreCase))
				return true;

			var fileName = Path.GetFileName(item);
			if (fileName.StartsWith(".", StringComparison.CurrentCulture))
				return true;

			return false;
		}
}
}
