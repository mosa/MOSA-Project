/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Mosa.Tools.TransformMonoSource
{
	class Program
	{

		/// <summary>
		/// Mains the specified args.
		/// </summary>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		static int Main(string[] args)
		{
			Console.WriteLine("TransformMonoSource v0.1 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2009. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();
			Console.WriteLine("Usage: TransformMonoSource <source directory> <destination directory>");
			Console.WriteLine();

			if (args.Length < 2) {
				Console.WriteLine("ERROR: Missing arguments");
				return -1;
			}

			try {
				List<string> files = new List<string>();

				FindFiles(args[0], string.Empty, ref files);

				foreach (string file in files)
					Process(args[0], file, args[1]);
			}
			catch (Exception e) {
				Console.WriteLine("Error: " + e.ToString());
				return -1;
			}

			return 0;
		}

		static void FindFiles(string root, string directory, ref List<string> files)
		{
			foreach (string file in Directory.GetFiles(Path.Combine(root, directory), "*.cs", SearchOption.TopDirectoryOnly)) {
				files.Add(Path.Combine(directory, Path.GetFileName(file)));
			}

			foreach (string dir in Directory.GetDirectories(Path.Combine(root, directory), "*.*", SearchOption.TopDirectoryOnly)) {
				if (dir.Contains(".svn"))
					continue;
				FindFiles(root, Path.Combine(directory, Path.GetFileName(dir)), ref files);
			}
		}

		static void Process(string root, string filename, string dest)
		{
			string[] lines = File.ReadAllLines(Path.Combine(root, filename));
			List<string> other = new List<string>();	
			string classname = string.Empty;
			int classline = -1;

			for (int l = 0; l < lines.Length; l++) {				
				string line = lines[l];

				int comment = lines[l].IndexOf("//");
				if (comment > 0)
					line = line.Substring(0, comment);

				int colon = lines[l].IndexOf(":");

				if (line.Contains(" extern ")) {
					l += SplitLines(ref lines, ref other, l, ";");
					AddPartialToClassName(ref lines, classline);
				}
				else if (line.Contains(" DllImport ") && (colon > 0)) {
					l += SplitLines(ref lines, ref other, l, "]");
					AddPartialToClassName(ref lines, classline);
				}
				else if (line.Contains("MethodImplOptions.InternalCall")) {
					l += SplitLines(ref lines, ref other, l, "]");
					AddPartialToClassName(ref lines, classline);
				}
				else if (line.Contains(" class ") || (line.Contains("\tclass ")) || (line.StartsWith("class ")) ||
						line.Contains(" struct ") || (line.Contains("\tstruct ")) || (line.StartsWith("struct "))) {
						classname = lines[l];
						classline = l;
				}
			}

			string partialFile = Path.Combine(dest, filename.Insert(filename.Length - 2, "Partial."));

			CreateSubDirectories(dest, Path.GetDirectoryName(filename));

			using (TextWriter writer = new StreamWriter(Path.Combine(dest, filename))) {
				foreach (string line in lines)
					if (line != null)
						writer.WriteLine(line);
				writer.Close();
			}

			if (other.Count != 0) {
				Console.WriteLine(partialFile);

				using (TextWriter writer = new StreamWriter(Path.Combine(dest, partialFile))) {
					foreach (string line in other)
						if (line != null)
							writer.WriteLine(line);
					writer.Close();
				}
			}

			return;
		}

		static int SplitLines(ref string[] lines, ref List<string> other, int at, string end)
		{
			string line = lines[at];

			other.Add(line);
			lines[at] = null;

			if (line.Contains(end))
				return 0;

			return 1 + SplitLines(ref lines, ref other, at + 1, end);
		}

		static void AddPartialToClassName(ref string[] lines, int line)
		{
			if (line < 0)
				return;

			if (lines[line].Contains(" partial "))
				return;

			int insert = lines[line].IndexOf("class ");
			if (insert < 0)
				insert = lines[line].IndexOf("struct ");

			lines[line] = lines[line].Insert(insert, " partial");
			return;
		}

		static void CreateSubDirectories(string root, string directory)
		{
			string current = Path.Combine(root, directory);

			if (Directory.Exists(current))
				return;

			string parent = Path.GetDirectoryName(directory);

			if (!string.IsNullOrEmpty(parent))
				CreateSubDirectories(root, parent);

			Directory.CreateDirectory(current);
		}
	}
}
