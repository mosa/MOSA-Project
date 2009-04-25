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
using System.Xml.Xsl;
using System.Xml.XPath;

namespace Mosa.Tools.XmlTo
{
	class Program
	{
		static int Main(string[] args)
		{
			Console.WriteLine("XMLTo v0.1 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2009. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();
			Console.WriteLine("Usage: XMLTo <xml file> <xsl file> <output file>");
			Console.WriteLine();

			if (args.Length < 3)
			 {
				Console.WriteLine("ERROR: Missing arguments");
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
				Console.WriteLine("Exception: {0}", e.ToString());
				return -1;
			}

		}
	}
}
