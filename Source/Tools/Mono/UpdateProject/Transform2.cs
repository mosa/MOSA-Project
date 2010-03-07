/*
* (c) 2009 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Mosa.Tools.Mono.UpdateProject
{
	/// <summary>
	/// Transforms the Mono source code
	/// </summary>
	internal static class Transform2
	{
		private static readonly char[] trimchars = { ' ', '\t' };
		private static readonly char[] trimchars2 = { '\t', ';', ' ' };
		private static readonly char[] trimchars3 = { '\t', ';', ' ', '{' };
		private static readonly char[] trimchars4 = { ' ', '\t', ':' };

		/// <summary>
		/// Processes the specified file.
		/// </summary>
		/// <param name="file">The file.</param>
		internal static void ProcessProject(string project)
		{
			List<string> files = Transform.GetProjectFiles(project);

			string root = Path.GetDirectoryName(project);

			foreach (string file in files)
				Process(Path.GetFileName(file), Path.GetDirectoryName(Path.Combine(root, file)));
		}

		/// Processes the specified file.
		/// </summary>
		/// <param name="file">The file.</param>
		internal static void ProcessFile(string file)
		{
			Process(Path.GetFileName(file), Path.GetDirectoryName(file));
		}

		/// <summary>
		/// Gets the project filenames.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <returns></returns>
		static internal List<string> GetProjectFiles(string file)
		{
			List<string> list = new List<string>();

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(file);

			XmlNamespaceManager mgr = new XmlNamespaceManager(xmlDocument.NameTable);
			mgr.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

			XmlNodeList compileNodes = xmlDocument.SelectNodes("/x:Project/x:ItemGroup/x:Compile", mgr);
			foreach (XmlNode compileNode in compileNodes)
				foreach (XmlNode attribute in compileNode.Attributes)
					if (attribute.Name.Equals("Include"))
						if ((attribute.Value.EndsWith(".cs")) && ((!attribute.Value.Contains(".Partial."))))
							list.Add(attribute.Value);

			return list;
		}

		/// <summary>
		/// Processes the specified root.
		/// </summary>
		/// <param name="file">The filename.</param>
		/// <param name="root">The root.</param>
		internal static void Process(string file, string root)
		{
			Console.WriteLine(file);

			if (!File.Exists(Path.Combine(root, file)))
				return;

			// Load file into string array
			string[] lines = File.ReadAllLines(Path.Combine(root, file));

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

			// Create partial file
			if (methodNodes.Count != 0) {
				string partialMosaFile = Path.Combine(root, file.Insert(file.Length - 2, "Mosa.Partial."));
				Console.WriteLine(">" + Path.GetFileName(partialMosaFile));
				CreatePartialFileForMosa(lines, rootNode, usings, namespaces, partialMosaFile);

				string partialMonoFile = Path.Combine(root, file.Insert(file.Length - 2, "Partial."));
				Console.WriteLine(">" + Path.GetFileName(partialMonoFile));
				CreatePartialFileForMono(lines, rootNode, usings, namespaces, partialMonoFile);
			}

			// Modify source file
			CreateModifiedFile(lines, classNodes, methodNodes, Path.Combine(root, file));
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
				if (classNode.Partial) {
					AddPartialToClassName(ref lines, classNode.Declare);

					foreach (int line in classNode.OtherDeclare)
						AddPartialToClassName(ref lines, line);
				}
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
					else if (line.StartsWith("#else") && (cnt == 0))
						break;
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

			// Insert conditions
			foreach (MethodNode method in methodNodes) {
				for (int i = method.Start; i <= method.End; i++)
					lines[i] = null;
			}

			// Remove consequentive blank lines
			bool blank = false;
			for (int i = 0; i < lines.Length; i++)
				if (lines[i] != null)
					if (lines[i].Trim(trimchars).Length > 0)
						blank = false;
					else
						if (!blank)
							blank = true;
						else
							lines[i] = null;

			// Write modified source files
			using (TextWriter writer = new StreamWriter(filename)) {
				foreach (string line in lines)
					if (line != null)
						writer.WriteLine(line);

				writer.Close();
			}
		}

		/// <summary>
		/// Creates the partial file for MOSA.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="rootNode">The root node.</param>
		/// <param name="usings">The usings.</param>
		/// <param name="namespaces">The namespaces.</param>
		/// <param name="filename">The filename.</param>
		private static void CreatePartialFileForMosa(string[] lines, ClassNode rootNode, List<int> usings, List<int> namespaces,
											  string filename)
		{
			List<string> output = new List<string>();

			// Write "using" lines
			foreach (int i in usings)
				output.Add(lines[i].Trim(trimchars2) + ";");
			output.Add(string.Empty);

			// Write "namespace" lines
			if (namespaces.Count != 1)
				return; // problem, more than one namespace

			output.Add(lines[namespaces[0]].Trim(trimchars3));
			output.Add("{");

			foreach (ClassNode child in rootNode.Children)
				WriteClassForMosa(lines, child, output, 0);

			output.Add("}");

			using (TextWriter writer = new StreamWriter(filename)) {
				foreach (string line in output)
					if (line != null)
						writer.WriteLine(line);
				writer.Close();
			}
		}

		/// <summary>
		/// Creates the partial file for mono.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="rootNode">The root node.</param>
		/// <param name="usings">The usings.</param>
		/// <param name="namespaces">The namespaces.</param>
		/// <param name="filename">The filename.</param>
		private static void CreatePartialFileForMono(string[] lines, ClassNode rootNode, List<int> usings, List<int> namespaces,
											  string filename)
		{
			List<string> output = new List<string>();

			// Write "using" lines
			foreach (int i in usings)
				output.Add(lines[i].Trim(trimchars2) + ";");
			output.Add(string.Empty);

			// Write "namespace" lines
			if (namespaces.Count != 1)
				return; // problem, more than one namespace

			output.Add(lines[namespaces[0]].Trim(trimchars3));
			output.Add("{");

			foreach (ClassNode child in rootNode.Children)
				WriteClassForMono(lines, child, output, 0);

			output.Add("}");

			using (TextWriter writer = new StreamWriter(filename)) {
				foreach (string line in output)
					if (line != null)
						writer.WriteLine(line);
				writer.Close();
			}
		}

		/// <summary>
		/// Writes the class for Mosa.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="currentNode">The current node.</param>
		/// <param name="output">The output.</param>
		/// <param name="depth">The depth.</param>
		private static void WriteClassForMosa(string[] lines, ClassNode currentNode, List<string> output, int depth)
		{
			if (!currentNode.Partial)
				return;

			string tabs = "".PadLeft(20, '\t').Substring(0, depth + 1);

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

					line = StripDLLBrackets(line);
					line = " " + line.Replace("\t", " ");
					line = line.Replace(" extern ", " ");
					line = line.Trim(trimchars);

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
				WriteClassForMosa(lines, child, output, depth + 1);

			output.Add(tabs + "}");
		}

		/// <summary>
		/// Writes the class for mono.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="currentNode">The current node.</param>
		/// <param name="output">The output.</param>
		/// <param name="depth">The depth.</param>
		private static void WriteClassForMono(string[] lines, ClassNode currentNode, List<string> output, int depth)
		{
			if (!currentNode.Partial)
				return;

			string tabs = "".PadLeft(20, '\t').Substring(0, depth + 1);

			// Write class declaration
			output.Add(tabs + GetDeclaration(lines, currentNode.Start, currentNode.Declare));
			output.Add(tabs + "{");

			// Write method declarations
			foreach (MethodNode method in currentNode.Methods) {
				string extra = string.Empty;
				bool endif = false;

				for (int i = method.Start; i <= method.End; i++) {
					string line = lines[i];

					string trim = line.TrimStart(trimchars);

					if (trim.StartsWith("//"))
						continue;

					if (trim.StartsWith("#")) {
						if (trim.Contains("#endif"))
							endif = false;
						else
							if (trim.Contains("#if"))
								endif = true;
						output.Add(trim);
					}
					else
						output.Add(tabs + '\t' + trim);
				}
				//if (endif)
				//    output.Add("#endif");
			}

			output.Add(string.Empty);

			foreach (ClassNode child in currentNode.Children)
				WriteClassForMono(lines, child, output, depth + 1);

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

			if ((lines[line].Contains(" partial ")) || (lines[line].Contains("\tpartial ")) || (lines[line].Contains("\tpartial\t")))
				return;

			int insert = lines[line].Replace('\t', ' ').IndexOf("class ");

			if (insert < 0)
				insert = lines[line].Replace('\t', ' ').IndexOf("struct ");

			lines[line] = lines[line].Insert(insert, "partial ");
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

				line = StripBrackets(line);

				if (string.IsNullOrEmpty(line))
					continue;

				string[] parsed = line.Split(trimchars4);

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

		/// <summary>
		/// Strips the brackets.
		/// </summary>
		/// <param name="line">The line.</param>
		/// <returns></returns>
		private static string StripBrackets(string line)
		{
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

			return line;
		}

		/// <summary>
		/// Strips the DLL brackets.
		/// </summary>
		/// <param name="line">The line.</param>
		/// <returns></returns>
		private static string StripDLLBrackets(string line)
		{
			while (true) {
				int bracketstart = line.IndexOf("[DllImport");

				if (bracketstart < 0)
					break;

				int bracketend = line.IndexOf("]");

				if (bracketend < 0) {
					line = string.Empty;
					break;
				}

				line = line.Substring(0, bracketstart) + line.Substring(bracketend + 1);
			}

			return line;
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
