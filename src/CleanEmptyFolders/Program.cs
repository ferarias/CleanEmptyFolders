using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;

namespace CleanEmptyFolders
{
	static class MainClass
	{
		private static List<string> emptyDirs = [];

		public static int Main(string[] args)
		{
			Console.WriteLine("Clean Empty folders");

			var result = Parser.Default.ParseArguments<Options>(args);
			var exitCode = result.MapResult(DoIt, errors => 
													{
														Console.WriteLine(errors);
														return 1;
													});
			Console.WriteLine("Clean Empty folders finished with exit code " + exitCode);
			return exitCode;
		}

		static int DoIt(Options options)
		{


			emptyDirs = [];
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

			if (dirs.Length == 0 && FilesAreUnimportant(files))
				emptyDirs.Add(dir);

		}

        private static bool FilesAreUnimportant(string[] files) => files.Length == 0 || files.Length != 0 && files.All(FileIsUnimportant);

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
