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
		class ClassNode
		{
			public int Start;
			public int End;
			public List<ClassNode> Children = new List<ClassNode>();
			public List<MethodNode> Methods = new List<MethodNode>();
			public ClassNode Parent;
			public bool Partial = false;
			public string Name = string.Empty;

			public ClassNode()
			{
				this.Parent = this; // trick!
				this.Start = int.MinValue;
				this.End = int.MaxValue;
			}

			public ClassNode(ClassNode parent, int start, int end)
			{
				this.Parent = parent;
				this.Start = start;
				this.End = end;
			}
		}

		class MethodNode
		{
			public ClassNode ClassNode;
			public int Start;
			public int Count;

			public MethodNode(ClassNode classNode, int start, int count)
			{
				this.ClassNode = classNode;
				this.Start = start;
				this.Count = count;
			}
		}

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

			ClassNode rootNode = new ClassNode();
			List<ClassNode> classNodes = new List<ClassNode>();
			List<MethodNode> methodNodes = new List<MethodNode>();

			List<int> namespaces = new List<int>();
			List<int> usings = new List<int>();

			ClassNode currentNode = rootNode;

			// TODO: handle #if/#else/#endif in source

			// Analyze File
			for (int l = 0; l < lines.Length; l++) {
				string line = lines[l];

				int comment = line.IndexOf("//");
				if (comment > 0)
					line = line.Substring(0, comment + 2);

				int colon = lines[l].IndexOf(":");

				if ((line.Contains(" extern ") || (line.Contains("\textern ")) || (line.StartsWith("extern "))) && !line.Contains(" alias ")) {
					int cnt = GetNumberOfMethodDeclarationLines(lines, l, ";", true);
					MethodNode node = new MethodNode(currentNode, l, cnt + 1);
					methodNodes.Add(node);
					currentNode.Methods.Add(node);
					l += cnt;
				}
				else if (line.Contains(" DllImport ") && (colon > 0)) {
					int cnt = GetNumberOfMethodDeclarationLines(lines, l, "]", false);
					MethodNode node = new MethodNode(currentNode, l, cnt + 1);
					methodNodes.Add(node);
					currentNode.Methods.Add(node);
					l += cnt;
				}
				else if (line.Contains("[DllImport")) {
					int cnt = GetNumberOfMethodDeclarationLines(lines, l, "]", false);
					MethodNode node = new MethodNode(currentNode, l, cnt + 1);
					methodNodes.Add(node);
					currentNode.Methods.Add(node);
					l += cnt;
				}
				else if (line.Contains("MethodImplOptions.InternalCall")) {
					int cnt = GetNumberOfMethodDeclarationLines(lines, l, "]", false);
					MethodNode node = new MethodNode(currentNode, l, cnt + 1);
					methodNodes.Add(node);
					currentNode.Methods.Add(node);
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
					// Attempt to include keywords in quotes
					if (line.Contains("\""))
						continue;

					// TODO: Search backwards for the start of the class definition (might not be on the same line as class keyword)
					// TODO: Search forwards for the end of the class definition (again, might be on another line)
					// TODO: Handle #if NET_2_0 / #else / #endif 
					// NOW: Assuming only one line

					// Find the last line of the class
					int end = GetEndOfScope(lines, l);

					// Go up to parent
					while (l > currentNode.Parent.End)
						currentNode = currentNode.Parent;

					// Child
					ClassNode child = new ClassNode(currentNode, l, end);
					classNodes.Add(child);
					currentNode.Children.Add(child);
					currentNode = child;
				}
			}

			// Mark all partial nodes
			foreach (ClassNode node in classNodes)
				if (node.Methods.Count != 0) {
					ClassNode upNode = node;
					do {
						upNode.Parent.Partial = true;
						upNode = upNode.Parent;
					}
					while (upNode != upNode.Parent);
				}

			// Create all directories
			CreateSubDirectories(dest, Path.GetDirectoryName(filename));

			// Create partial file
			if (methodNodes.Count != 0) {
				string partialFile = Path.Combine(dest, filename.Insert(filename.Length - 2, "Partial."));
				Console.WriteLine(partialFile);
				CreatePartialFile(lines, rootNode, usings, namespaces, Path.Combine(dest, partialFile));
			}

			// Modify source file
			CreateModifiedFile(lines, classNodes, methodNodes, Path.Combine(dest, filename));
		}

		static void CreateModifiedFile(string[] lines, List<ClassNode> classNodes, List<MethodNode> methodNodes, string filename)
		{
			// Insert partial
			foreach (ClassNode classNode in classNodes)
				AddPartialToClassName(ref lines, classNode.Start);	// TODO: Pass start/end lines numbers

			// Insert comments
			foreach (MethodNode method in methodNodes)
				for (int i = 0; i < method.Count; i++)
					lines[method.Start + i] = @"//" + lines[method.Start + i];

			// Write modified source files
			using (TextWriter writer = new StreamWriter(filename)) {
				foreach (string line in lines)
					if (line != null)
						writer.WriteLine(line);

				writer.Close();
			}
		}

		static void CreatePartialFile(string[] lines, ClassNode rootNode, List<int> usings, List<int> namespaces, string filename)
		{
			List<string> output = new List<string>();

			// Write "using" lines
			foreach (int i in usings)
				output.Add(lines[i].Trim(new char[] { '\t', ';', ' ' }) + ";");

			output.Add(string.Empty);

			// Write "namespace" lines
			if (namespaces.Count != 1)
				return; // problem, more than one namespace

			output.Add(lines[namespaces[0]].Trim(new char[] { '\t', ';', ' ', '{' }));
			output.Add("{");

			foreach (ClassNode child in rootNode.Children)
				WriteClass(lines, child, output, 0);

			output.Add("}");

			using (TextWriter writer = new StreamWriter(filename)) {
				foreach (string line in output)
					if (line != null)
						writer.WriteLine(line);
				writer.Close();
			}
		}

		static void WriteClass(string[] lines, ClassNode currentNode, List<string> output, int depth)
		{
			if (!currentNode.Partial)
				return;

			string tabs = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t".Substring(0, depth + 1);

			// Write class declaration
			output.Add(tabs + GetDeclaration(lines, currentNode.Start));
			output.Add(tabs + "{");

			// Write method declarations
			foreach (MethodNode method in currentNode.Methods) {
				string extra = string.Empty;

				for (int i = 0; i < method.Count; i++) {
					string line = lines[method.Start + i];

					if (line.Contains("MethodImplOptions.InternalCall"))
						continue;

					if (line.Contains("[DllImport"))
						continue;

					line = line.Replace(" extern ", " ");
					line = line.Trim(new char[] { '\t', ' ' });

					bool semicolon = line.Contains(";");

					if (semicolon)
						line = line.Replace(";", string.Empty);

					if ((line == "get") || (line == "set"))
						extra = "\t";

					if (!string.IsNullOrEmpty(line)) {
						// Strip internal and extern method attributes
						line = line.Replace(" internal ", " ").Replace(" extern ", " ");

						if (line.StartsWith("internal "))
							line = line.Substring(9);

						if (line.StartsWith("extern "))
							line = line.Substring(7);

						output.Add(tabs + "\t" + line);
					}

					if (semicolon) {
						output.Add(tabs + extra + "\t{");
						output.Add(tabs + extra + "\t\tthrow new System.Exception(\"The method or operation is not implemented.\");");
						output.Add(tabs + extra + "\t}");
						output.Add(string.Empty);
						extra = string.Empty;
					}
				}
			}

			output.Add(string.Empty);

			foreach (ClassNode child in currentNode.Children)
				WriteClass(lines, child, output, depth + 1);

			output.Add(tabs + "}");
		}

		static int GetNumberOfMethodDeclarationLines(string[] lines, int at, string end, bool bracket)
		{
			if (bracket)
				if (lines[at].Contains("{"))
					end = "}";

			if (lines[at].Contains(end))
				return 0;

			return 1 + GetNumberOfMethodDeclarationLines(lines, at + 1, end, false);
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
				attribute = "public ";
			else if (declare.IndexOf("protected ") >= 0)
				attribute = "protected ";
			else if (declare.IndexOf("private ") >= 0)
				attribute = "private ";

			// Get class/struct name
			string name = declare.Substring(insert + obj.Length + 1);
			int end = name.Length;
			int at = name.IndexOf(":");
			if (at > 0) end = at;
			at = name.IndexOf(" ");
			if ((at > 0) && (at < end)) end = at;
			name = name.Substring(0, end);

			// Determine if abstract
			if ((declare.Contains(" abstract ")) || (declare.StartsWith("abstract ")))
				obj = obj + " abstract";

			return attribute + "partial " + obj + " " + name;
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

		static int GetEndOfScope(string[] lines, int at)
		{
			bool first = false;

			int open = 0;
			int close = 0;

			for (; at < lines.Length; at++) {
				foreach (char c in lines[at])
					if (c == '{')
						open++;
					else if (c == '}')
						close++;

				if ((open > 0) & (!first))
					first = true;

				if ((first) && (open <= close))
					return at;
			}

			return at;
		}

	}
}
