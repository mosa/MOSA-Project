/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mosa.Tool.XmlTo
{
	class Program
	{
		static int Main(string[] args)
		{
			Console.WriteLine();
			Console.WriteLine("XMLTo v0.1 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2010. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();

			if (args.Length < 3) {
				Console.WriteLine("Usage: XMLTo <xml file> <xsl file> <output file>");
				Console.Error.WriteLine("ERROR: Missing arguments");
				return -1;
			}

			try {
				XPathDocument myXPathDoc = new XPathDocument(args[0]);
				XslCompiledTransform myXslTrans = new XslCompiledTransform();
				myXslTrans.Load(args[1]);
				XmlTextWriter myWriter = new XmlTextWriter(args[2], null);
				myXslTrans.Transform(myXPathDoc, null, myWriter);

				return 0;
			}
			catch (Exception e) {
				Console.Error.WriteLine("Exception: {0}", e.ToString());
				return -1;
			}

		}
	}
}
