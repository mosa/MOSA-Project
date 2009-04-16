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
			// Load file into string array
			string[] lines = File.ReadAllLines(Path.Combine(root, filename));

			SortedDictionary<int, int> methods = new SortedDictionary<int, int>();
			Dictionary<int, int> method2class = new Dictionary<int, int>();
			List<int> classes = new List<int>();
			List<int> namespaces = new List<int>();
			List<int> usings = new List<int>();

			// Analysis File
			for (int l = 0; l < lines.Length; l++) {
				string line = lines[l];

				int comment = line.IndexOf("//");
				if (comment > 0)
					line = line.Substring(0, comment + 2);

				int colon = lines[l].IndexOf(":");

				if ((line.Contains(" extern ") || (line.Contains("\textern ")) || (line.StartsWith("extern "))) && !line.Contains(" alias ")) {
					AddPartialToClassName(ref lines, classes[classes.Count - 1]);
					int cnt = SplitLines(lines, l, ";", true);
					methods.Add(l, cnt + 1);
					method2class.Add(l, classes[classes.Count - 1]);
					l += cnt;
				}
				else if (line.Contains(" DllImport ") && (colon > 0)) {
					AddPartialToClassName(ref lines, classes[classes.Count - 1]);
					int cnt = SplitLines(lines, l, "]", false);
					methods.Add(l, cnt + 1);
					method2class.Add(l, classes[classes.Count - 1]);
					l += cnt;
				}
				else if (line.Contains("[DllImport")) {
					AddPartialToClassName(ref lines, classes[classes.Count - 1]);
					int cnt = SplitLines(lines, l, "]", false);
					methods.Add(l, cnt + 1);
					method2class.Add(l, classes[classes.Count - 1]);
					l += cnt;
				}
				else if (line.Contains("MethodImplOptions.InternalCall")) {
					AddPartialToClassName(ref lines, classes[classes.Count - 1]);
					int cnt = SplitLines(lines, l, "]", false);
					methods.Add(l, cnt + 1);
					method2class.Add(l, classes[classes.Count - 1]);
					l += cnt;
				}
				else if (line.StartsWith("using ") && line.Contains(";")) {
					usings.Add(l);
				}
				else if (line.StartsWith("namespace ")) {
					namespaces.Add(l);
				}
				else if (line.Contains(" class ") || (line.Contains("\tclass ")) || (line.StartsWith("class ")) ||
						line.Contains(" struct ") || (line.Contains("\tstruct ")) || (line.StartsWith("struct "))) {

					if (line.Contains("\""))
						continue;

					classes.Add(l);
				}
			}

			// Create all directories
			CreateSubDirectories(dest, Path.GetDirectoryName(filename));

			// Create partial file
			if (methods.Count != 0) {
				string partialFile = Path.Combine(dest, filename.Insert(filename.Length - 2, "Partial."));
				List<string> other = new List<string>();

				Console.WriteLine(partialFile);

				// write using lines
				foreach (int i in usings)
					other.Add(lines[i].Trim(new char[] { '\t', ';', ' ' }) + ";");

				other.Add(string.Empty);

				// write namespace
				if (namespaces.Count != 1)
					return; // problem

				other.Add(lines[namespaces[0]].Trim(new char[] { '\t', ';', ' ', '{' }));
				other.Add("{");

				int lastClassline = -1;

				// write methods stubs
				foreach (KeyValuePair<int, int> section in methods) {
					string tabs = "\t\t";

					for (int i = 0; i < section.Value; i++) {
						string line = lines[section.Key + i];

						if (line.Contains("MethodImplOptions.InternalCall"))
							continue;

						if (line.Contains("[DllImport"))
							continue;

						if (lastClassline != method2class[section.Key]) {
							if (lastClassline > 0)
								other.Add("\t}");

							lastClassline = method2class[section.Key];

							other.Add("\t" + GetDeclaration(lines, lastClassline));

							other.Add("\t{");
						}

						line = line.Replace(" extern ", " ");
						line = line.Trim(new char[] { '\t', ' ' });

						bool semicolon = line.Contains(";");

						if (semicolon)
							line = line.Replace(";", string.Empty);

						if ((line == "get") || (line == "set"))
							tabs = "\t\t\t";

						if (!string.IsNullOrEmpty(line)) {
							// Strip internal and extern method attributes
							line = line.Replace(" internal ", " ").Replace(" extern ", " ");

							if (line.StartsWith("internal "))
								line = line.Substring(9);

							if (line.StartsWith("extern "))
								line = line.Substring(7);

							other.Add(tabs + line);
						}

						if (semicolon) {
							other.Add(tabs + "{");
							other.Add(tabs + "\tthrow new System.Exception(\"The method or operation is not implemented.\");");
							other.Add(tabs + "}");

							tabs = "\t\t";
						}
					}

					if (!string.IsNullOrEmpty(other[other.Count - 1]))
						other.Add(string.Empty);
				}

				if (lastClassline > 0)
					other.Add("\t}");

				other.Add("}");

				using (TextWriter writer = new StreamWriter(Path.Combine(dest, partialFile))) {
					foreach (string line in other)
						if (line != null)
							writer.WriteLine(line);
					writer.Close();
				}
			}

			// Insert partial
			foreach (int i in classes)
				AddPartialToClassName(ref lines, i);

			// Insert comments
			foreach (KeyValuePair<int, int> section in methods)
				for (int i = 0; i < section.Value; i++)
					lines[section.Key + i] = @"//" + lines[section.Key + i];

			// Write modified source files
			using (TextWriter writer = new StreamWriter(Path.Combine(dest, filename))) {
				foreach (string line in lines)
					if (line != null)
						writer.WriteLine(line);
				writer.Close();
			}

			return;
		}

		static int SplitLines(string[] lines, int at, string end, bool bracket)
		{
			if (bracket)
				if (lines[at].Contains("{"))
					end = "}";

			if (lines[at].Contains(end))
				return 0;

			return 1 + SplitLines(lines, at + 1, end, false);
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

			lines[line] = lines[line].Insert(insert, "partial ");
		}

		/// <summary>
		/// Gets the declaration.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="line">The line.</param>
		/// <returns></returns>
		static string GetDeclaration(string[] lines, int line)
		{
			// yes, poor parsing approach but it works
			string declare = lines[line].Trim(new char[] { '\t', ' ' });

			// Determine class or struct
			int insert = declare.IndexOf("class ");
			string obj = "class";

			if (insert < 0) {
				insert = declare.IndexOf("struct ");
				obj = "struct";
			}

			// Determine attribute (public, private, protected)
			string attribute = string.Empty;

			if (declare.IndexOf("public ") >= 0)
				attribute = "public";
			else if (declare.IndexOf("protected ") >= 0)
				attribute = "protected";
			else if (declare.IndexOf("private ") >= 0)
				attribute = "private";

			// Determine if abstract
			if ((declare.Contains(" abstract ")) || (declare.StartsWith("abstract ")))
				obj = obj + " abstract";

			// Get class/struct name
			string name = declare.Substring(insert + obj.Length + 1);
			int end = name.Length;
			int at = name.IndexOf(":");
			if (at > 0) end = at;
			at = name.IndexOf(" ");
			if ((at > 0) && (at < end)) end = at;
			name = name.Substring(0, end);

			return attribute + " partial " + obj + " " + name;
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
