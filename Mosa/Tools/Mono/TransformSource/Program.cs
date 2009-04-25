/*
* (c) 2009 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Mono.TransformSource
{
	/// <summary>
	/// Program class for Mono.TransformSource 
	/// </summary>
	internal class Program
	{
		private static readonly char[] trimchars = {' ', '\t'};
		private static List<string> notes = new List<string>();

		/// <summary>
		/// Mains the specified args.
		/// </summary>
		/// <param name="args">The command line arguments</param>
		/// <returns>Zero on success</returns>
		private static int Main(string[] args)
		{
			Console.WriteLine("TransformSource v0.1 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2009. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();
			Console.WriteLine("Usage: TransformSource <source directory> <destination directory>");
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
				Console.WriteLine("Error: " + e);
				return -1;
			}

			return 0;
		}

		/// <summary>
		/// Finds the files.
		/// </summary>
		/// <param name="root">The root.</param>
		/// <param name="directory">The directory.</param>
		/// <param name="files">The files.</param>
		private static void FindFiles(string root, string directory, ref List<string> files)
		{
			foreach (string file in Directory.GetFiles(Path.Combine(root, directory), "*.cs", SearchOption.TopDirectoryOnly))
				//if (file.Contains("\\Assembly.cs")) // DEBUG
					files.Add(Path.Combine(directory, Path.GetFileName(file)));

			foreach (string dir in Directory.GetDirectories(Path.Combine(root, directory), "*.*", SearchOption.TopDirectoryOnly)) {
				if (dir.Contains(".svn"))
					continue;
				FindFiles(root, Path.Combine(directory, Path.GetFileName(dir)), ref files);
			}
		}

		/// <summary>
		/// Processes the specified root.
		/// </summary>
		/// <param name="root">The root.</param>
		/// <param name="filename">The filename.</param>
		/// <param name="dest">The dest.</param>
		private static void Process(string root, string filename, string dest)
		{
			Console.WriteLine(filename);

			// Load file into string array
			string[] lines = File.ReadAllLines(Path.Combine(root, filename));

			ClassNode rootNode = new ClassNode();
			List<ClassNode> classNodes = new List<ClassNode>();
			List<MethodNode> methodNodes = new List<MethodNode>();

			List<int> namespaces = new List<int>();
			List<int> usings = new List<int>();

			ClassNode currentNode = rootNode;

			bool incomment = false;
			// Analyze File
			for (int linenbr = 0; linenbr < lines.Length; linenbr++) {
				string trim = GetLine(lines, linenbr, ref incomment).Replace('\t', ' ');

				if (incomment)
					continue;

				trim = StripWithInDoubleQuotes(trim);

				int skip = 0;

				if (string.IsNullOrEmpty(trim))
					continue;
				else if (trim.StartsWith("#"))
					continue;
				else if ((trim.StartsWith("extern ") || (trim.Contains(" extern "))) && !trim.Contains(" alias ")) {
					int start = GetPreviousBlockEnd(lines, linenbr);
					skip = GetNumberOfMethodDeclarationLines(lines, linenbr, false);
					MethodNode node = new MethodNode(currentNode, start, linenbr + skip, linenbr);
					methodNodes.Add(node);
					currentNode.Methods.Add(node);
				}
				else if (trim.StartsWith("using ") && trim.Contains(";")) {
					usings.Add(linenbr);
				}
				else if (trim.StartsWith("namespace ")) {
					namespaces.Add(linenbr);
				}
				else if (trim.Contains(" class ") || (trim.StartsWith("class ")) ||
				         trim.Contains(" struct ") || (trim.StartsWith("struct "))) {
					// Search backwards for the start of the class definition (might not be on the same line as class keyword)
					int start = GetPreviousBlockEnd(lines, linenbr);

					string className = GetClassName(lines, start, linenbr);

					// Attempt to handle #else class definitions
					if (className == currentNode.Name) {
						currentNode.OtherDeclare.Add(linenbr);
						continue;
					}

					// Find the last line of the class
					int end = GetEndOfScope(lines, linenbr);

					// Go up to parent
					while (linenbr > currentNode.End)
						currentNode = currentNode.Parent;

					// Child
					ClassNode child = new ClassNode(currentNode, start, end, linenbr);
					classNodes.Add(child);
					currentNode.Children.Add(child);
					currentNode = child;
					child.Name = className;
				}

				linenbr = linenbr + skip;

				// Go up to parent
				while (linenbr >= currentNode.End)
					currentNode = currentNode.Parent;
			}

			// Mark all partial nodes
			foreach (ClassNode node in classNodes)
				if (node.Methods.Count != 0) {
					node.Partial = true;
					ClassNode upNode = node;
					do {
						upNode.Parent.Partial = true;
						upNode = upNode.Parent;
					} while (upNode != upNode.Parent);
				}

			// Create all directories
			CreateSubDirectories(dest, Path.GetDirectoryName(filename));

			// Create partial file
			if (methodNodes.Count != 0) {
				string partialFile = Path.Combine(dest, filename.Insert(filename.Length - 2, "Partial."));
				//Console.WriteLine(partialFile);
				CreatePartialFile(lines, rootNode, usings, namespaces, Path.Combine(dest, partialFile));
			}

			// Modify source file
			CreateModifiedFile(lines, classNodes, methodNodes, Path.Combine(dest, filename));
		}

		/// <summary>
		/// Creates the modified file.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="classNodes">The class nodes.</param>
		/// <param name="methodNodes">The method nodes.</param>
		/// <param name="filename">The filename.</param>
		private static void CreateModifiedFile(string[] lines, List<ClassNode> classNodes, List<MethodNode> methodNodes,
		                                       string filename)
		{
			// Insert partial
			foreach (ClassNode classNode in classNodes) {
				AddPartialToClassName(ref lines, classNode.Declare);

				foreach (int line in classNode.OtherDeclare)
					AddPartialToClassName(ref lines, line);
			}

			foreach (MethodNode method in methodNodes) {
				int start = method.Declare;
				int cnt = 0;

				for (int at = method.Declare; at >= method.Start; at--) {
					string line = lines[at].Trim(trimchars);

					if (string.IsNullOrEmpty(line) || line.StartsWith("//"))
						continue;
					else if (line.StartsWith("#endif"))
						cnt++;
					else if (line.StartsWith("#if")) {
						if (cnt < 0)
							break;
						if (cnt > 0) start = at;
						cnt--;
					}
					else if (line.StartsWith("#endregion"))
						break; // should be the end
					else if (line.StartsWith("#region"))
						break; // should be the end
					else if (line.StartsWith("#"))
						continue;
					else
						start = at;
				}

				method.Start = start;

				while (true) {
					string line = lines[method.End].Trim(trimchars);

					if ((string.IsNullOrEmpty(line)) || line.StartsWith("#") || line.StartsWith("//")) {
						method.End--;

						if (method.Start == method.End)
							break;
					}

					break;
				}
			}

			// Insert comments
			foreach (MethodNode method in methodNodes) {
				lines[method.Start] = "#if !MOSAPROJECT\n" + lines[method.Start];
				lines[method.End] = lines[method.End] + "\n#endif";
			}

			// Write modified source files
			using (TextWriter writer = new StreamWriter(filename)) {
				foreach (string line in lines)
					if (line != null)
						writer.WriteLine(line);

				writer.Close();
			}
		}

		/// <summary>
		/// Creates the partial file.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="rootNode">The root node.</param>
		/// <param name="usings">The usings.</param>
		/// <param name="namespaces">The namespaces.</param>
		/// <param name="filename">The filename.</param>
		private static void CreatePartialFile(string[] lines, ClassNode rootNode, List<int> usings, List<int> namespaces,
		                                      string filename)
		{
			List<string> output = new List<string>();

			output.Add("#if MOSAPROJECT");
			output.Add(string.Empty);

			// Write "using" lines
			foreach (int i in usings)
			    output.Add(lines[i].Trim(new[] {'\t', ';', ' '}) + ";");
			output.Add(string.Empty);

			// Write "namespace" lines
			if (namespaces.Count != 1)
				return; // problem, more than one namespace

			output.Add(lines[namespaces[0]].Trim(new[] {'\t', ';', ' ', '{'}));
			output.Add("{");

			foreach (ClassNode child in rootNode.Children)
				WriteClass(lines, child, output, 0);

			output.Add("}");

			output.Add("");
			output.Add("#endif");

			using (TextWriter writer = new StreamWriter(filename)) {
				foreach (string line in output)
					if (line != null)
						writer.WriteLine(line);
				writer.Close();
			}
		}

		/// <summary>
		/// Writes the class.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="currentNode">The current node.</param>
		/// <param name="output">The output.</param>
		/// <param name="depth">The depth.</param>
		private static void WriteClass(string[] lines, ClassNode currentNode, List<string> output, int depth)
		{
			if (!currentNode.Partial)
				return;

			string tabs = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t".Substring(0, depth + 1);

			// Write class declaration
			output.Add(tabs + GetDeclaration(lines, currentNode.Start, currentNode.Declare));
			output.Add(tabs + "{");

			// Write method declarations
			foreach (MethodNode method in currentNode.Methods) {
				string extra = string.Empty;

				for (int i = method.Declare; i <= method.End; i++) {
					string line = lines[i];

					string trim = line.TrimStart(trimchars);

					if ((trim.StartsWith("[")) && (trim.EndsWith("]")))
						continue;

					if (trim.StartsWith("//"))
						continue;

					line = " " + line.Replace("\t", " ");
					// line = line.Replace(" virtual ", " ");
					line = line.Replace(" extern ", " ");
					line = line.Trim(new[] {'\t', ' '});

					bool semicolon = line.Contains(";");

					if (semicolon)
						line = line.Replace(";", string.Empty);

					if ((line == "get") || (line == "set"))
						extra = "\t";

					if (!string.IsNullOrEmpty(line)) {
						output.Add(tabs + extra + "\t" + line);
					}

					if (semicolon) {
						output.Add(tabs + extra + "\t{");
						output.Add(tabs + extra + "\t\tthrow new System.NotImplementedException();");
						output.Add(tabs + extra + "\t}");
						extra = string.Empty;
					}
				}
			}

			output.Add(string.Empty);

			foreach (ClassNode child in currentNode.Children)
				WriteClass(lines, child, output, depth + 1);

			output.Add(tabs + "}");
		}

		/// <summary>
		/// Gets the number of method declaration lines.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="at">At.</param>
		/// <param name="bracket">if set to <c>true</c> [bracket].</param>
		/// <returns></returns>
		private static int GetNumberOfMethodDeclarationLines(string[] lines, int at, bool bracket)
		{
			if (lines[at].Contains("{"))
				bracket = true;

			if (bracket)
				if (lines[at].Contains("}"))
					return 0;

			if (!bracket)
				if (lines[at].Contains(";"))
					return 0;

			return 1 + GetNumberOfMethodDeclarationLines(lines, at + 1, bracket);
		}

		/// <summary>
		/// Adds the partial name of to class.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="line">The line.</param>
		private static void AddPartialToClassName(ref string[] lines, int line)
		{
			if (line < 0)
				return;

			if (lines[line].Contains(" partial "))
				return;

			int insert = lines[line].IndexOf("class ");

			if (insert < 0)
				insert = lines[line].IndexOf("struct ");

			lines[line] = lines[line].Insert(insert, "\n#if MOSAPROJECT\n\tpartial\n#endif\n");
		}

		/// <summary>
		/// Gets the declaration tokens.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="start">The start.</param>
		/// <param name="declare">The declare.</param>
		/// <returns></returns>
		private static List<string> GetDeclarationTokens(string[] lines, int start, int declare)
		{
			List<string> tokens = new List<string>();
			bool incomment = false;

			for (int i = start; i <= declare; i++) {
				string line = GetLine(lines, i, ref incomment);

				if (incomment)
					continue;

				if (line.StartsWith("#"))
					continue;

				// strip []
				while (true) {
					int bracketstart = line.IndexOf("[");

					if (bracketstart < 0)
						break;

					int bracketend = line.IndexOf("]");

					if (bracketend < 0) {
						line = string.Empty;
						break;
					}

					line = line.Substring(0, bracketstart) + line.Substring(bracketend + 1);
				}

				if (line.Length == 0)
					continue;

				string[] parsed = line.Split(new[] {'\t', ' ', ':'});

				foreach (string token in parsed)
					if (!string.IsNullOrEmpty(token))
						tokens.Add(token);
			}

			return tokens;
		}

		/// <summary>
		/// Gets the name of the class.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="start">The start.</param>
		/// <param name="declare">The declare.</param>
		/// <returns></returns>
		private static string GetClassName(string[] lines, int start, int declare)
		{
			List<string> tokens = GetDeclarationTokens(lines, start, declare);

			return tokens[GetClassOrStructLocation(tokens) + 1];
		}

		/// <summary>
		/// Gets the declaration.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="start">The start.</param>
		/// <param name="declare">The declare.</param>
		/// <returns></returns>
		private static string GetDeclaration(string[] lines, int start, int declare)
		{
			List<string> tokens = GetDeclarationTokens(lines, start, declare);

			int line = GetClassOrStructLocation(tokens);

			// Determine attribute (public, private, protected)
			string attribute = string.Empty;

			for (int i = 0; i < line; i++) {
				string token = tokens[i];

				bool accept = false;

				switch (token) {
					case "public":
						accept = true;
						break;
					case "private":
						accept = true;
						break;
					case "protected":
						accept = true;
						break;
					case "unsafe":
						accept = true;
						break;
					default:
						break;
				}

				if (accept)
					if (!attribute.Contains(token))
						attribute = attribute + " " + token;
			}

			return attribute.Trim() + " partial " + tokens[line] + " " + tokens[line + 1];
		}

		/// <summary>
		/// Gets the class or struct location.
		/// </summary>
		/// <param name="tokens">The tokens.</param>
		/// <returns></returns>
		private static int GetClassOrStructLocation(List<string> tokens)
		{
			for (int i = 0; i < tokens.Count; i++)
				if ((tokens[i].Equals("class")) || (tokens[i].Equals("struct")))
					return i;

			return -1;
		}

		/// <summary>
		/// Creates the sub directories.
		/// </summary>
		/// <param name="root">The root.</param>
		/// <param name="directory">The directory.</param>
		private static void CreateSubDirectories(string root, string directory)
		{
			string current = Path.Combine(root, directory);

			if (Directory.Exists(current))
				return;

			string parent = Path.GetDirectoryName(directory);

			if (!string.IsNullOrEmpty(parent))
				CreateSubDirectories(root, parent);

			Directory.CreateDirectory(current);
		}

		/// <summary>
		/// Gets the end of scope.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="at">At.</param>
		/// <returns></returns>
		private static int GetEndOfScope(string[] lines, int at)
		{
			bool first = false;

			int open = 0;
			int close = 0;

			bool incomment = false;

			for (; at < lines.Length; at++) {
				string line = GetLine(lines, at, ref incomment).Replace("'{'", string.Empty).Replace("'}'", string.Empty);

				line = StripWithInDoubleQuotes(line);

				if (incomment)
					continue;

				foreach (char c in line)
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

		/// <summary>
		/// Gets the previous block end.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="at">At.</param>
		/// <returns></returns>
		private static int GetPreviousBlockEnd(string[] lines, int at)
		{
			for (at--; at >= 0; at--) {
				if ((lines[at].Contains("#endregion")) || (lines[at].Contains("#region")))
					return at + 1;

				foreach (char c in lines[at])
					if ((c == '{') || (c == ';') || (c == '}'))
						return at + 1;
			}

			return 0;
		}

		/// <summary>
		/// Gets the line.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="linenbr">The linenbr.</param>
		/// <param name="incomment">if set to <c>true</c> [incomment].</param>
		/// <returns></returns>
		private static string GetLine(string[] lines, int linenbr, ref bool incomment)
		{
			string line = StripDoubleComment(lines[linenbr]);
			int comment = -1;

			if (incomment) {
				comment = line.IndexOf("*/");

				if (comment < 0)
					return string.Empty;

				incomment = false;
				line = line.Substring(comment + 2);
			}

			// strip /* comments */
			while (true) {
				comment = line.IndexOf("/*");

				if (comment < 0)
					break;

				int endcomment = line.IndexOf("*/");

				if (endcomment < 0) {
					incomment = true;
					line = string.Empty;
					break;
				}

				line = line.Substring(0, comment) + line.Substring(endcomment + 2);
			}

			if (incomment)
				return string.Empty;

			return line.Trim(trimchars);
		}

		/// <summary>
		/// Strips the double comment.
		/// </summary>
		/// <param name="line">The line.</param>
		/// <returns></returns>
		private static string StripDoubleComment(string line)
		{
			if (!line.Contains("//"))
				return line;

			bool inquotes = false;
			bool inescape = false;
			bool first = false;

			for (int i = 0; i < line.Length; i++) {
				char c = line[i];

				if (inescape) {
					inescape = false;
					continue;
				}

				if (c == '"')
					if (!inquotes)
						inquotes = true;
					else
						inquotes = false;

				if ((inquotes) && (c == '\\'))
					inescape = true;

				if (!inquotes)
					if (c == '/')
						if (first)
							return line.Substring(0, i - 1);
						else {
							first = true;
							continue;
						}

				first = false;
			}

			return line;
		}

		/// <summary>
		/// Strips the with in double quotes.
		/// </summary>
		/// <param name="line">The line.</param>
		/// <returns></returns>
		private static string StripWithInDoubleQuotes(string line)
		{
			if (!line.Contains("\""))
				return line;

			StringBuilder newline = new StringBuilder(line.Length);

			bool inquotes = false;
			bool inescape = false;

			foreach (char c in line) {
				if (inescape) {
					inescape = false;
					continue;
				}

				if (c == '"')
					if (!inquotes)
						inquotes = true;
					else {
						newline.Append(c);
						inquotes = false;
					}

				if ((inquotes) && (c == '\\'))
					inescape = true;

				if (!inquotes)
					newline.Append(c);
			}

			return newline.ToString();
		}

		#region Nested type: ClassNode

		/// <summary>
		/// ClassNode
		/// </summary>
		private class ClassNode
		{
			public readonly List<ClassNode> Children = new List<ClassNode>();
			public readonly int Declare;
			public readonly int End;
			public readonly List<MethodNode> Methods = new List<MethodNode>();
			public readonly List<int> OtherDeclare = new List<int>();
			public readonly ClassNode Parent;
			public readonly int Start;
			public string Name = string.Empty;
			public bool Partial;

			/// <summary>
			/// Initializes a new instance of the <see cref="ClassNode"/> class.
			/// </summary>
			public ClassNode()
			{
				Parent = this; // trick!
				Start = int.MinValue;
				End = int.MaxValue;
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="ClassNode"/> class.
			/// </summary>
			/// <param name="parent">The parent.</param>
			/// <param name="start">The start.</param>
			/// <param name="end">The end.</param>
			/// <param name="declare">The declare.</param>
			public ClassNode(ClassNode parent, int start, int end, int declare)
			{
				Parent = parent;
				Start = start;
				End = end;
				Declare = declare;
			}
		}

		#endregion

		#region Nested type: MethodNode

		/// <summary>
		/// Method Node
		/// </summary>
		private class MethodNode
		{
			public readonly int Declare;
			public ClassNode ClassNode;
			public int End;
			public int Start;

			public MethodNode(ClassNode classNode, int start, int end, int declare)
			{
				ClassNode = classNode;
				Declare = declare;
				Start = start;
				End = end;
			}
		}

		#endregion
	}
}