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
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace Mosa.Tools.Mono.UpdateProject
{
	/// <summary>
	/// Program class for Mono.UpdateProject
	/// </summary>
	internal class Add
	{
		/// <summary>
		/// Processes the specified file.
		/// </summary>
		/// <param name="file">The source.</param>
		internal static void Process(string file)
		{
			string root = Path.GetDirectoryName(file);

			List<string> files = Transform.GetProjectFiles(file);

			// New nodes (new node, parent node)
			List<KeyValuePair<XmlNode, XmlNode>> newNodes = new List<KeyValuePair<XmlNode, XmlNode>>();

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(file);

			XmlNamespaceManager mgr = new XmlNamespaceManager(xmlDocument.NameTable);
			mgr.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

			XmlNodeList compileNodes = xmlDocument.SelectNodes("/x:Project/x:ItemGroup/x:Compile", mgr);
			foreach (XmlNode compileNode in compileNodes)
				foreach (XmlNode attribute in compileNode.Attributes)
					if (attribute.Name.Equals("Include"))
						if (attribute.Value.EndsWith(".cs") && (!attribute.Value.EndsWith(".Internal.cs") && (!attribute.Value.EndsWith(".Mosa.cs")))) {
							string partialfile = attribute.Value.Insert(attribute.Value.Length - 2, "Mosa."); ;

							if (File.Exists(Path.Combine(root, partialfile)))
								if (!files.Contains(partialfile)) {
									XmlNode newCompileNode = xmlDocument.CreateElement("Compile", string.Empty);
									XmlAttribute newCompileNodeAttribute = xmlDocument.CreateAttribute("Include");
									newCompileNodeAttribute.Value = partialfile;
									newCompileNode.Attributes.Append(newCompileNodeAttribute);
									newNodes.Add(new KeyValuePair<XmlNode, XmlNode>(newCompileNode, compileNode.ParentNode));
								}
						}

			if (newNodes.Count == 0)
				return;

			foreach (KeyValuePair<XmlNode, XmlNode> node in newNodes)
				node.Value.AppendChild(node.Key);

			MemoryStream mem = new MemoryStream();
			xmlDocument.Save(new StreamWriter(mem));
			mem.Seek(0, SeekOrigin.Begin);
			StreamReader reader = new StreamReader(mem);

			File.WriteAllText(file, reader.ReadToEnd().Replace("xmlns=\"\" ", string.Empty));
		}

	}
}