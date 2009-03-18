using System;
using System.IO;
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
				string xmlFile = args[0];
				string xslFile = args[1];
				string output = args[2];

				// Load the XML
				XPathDocument xml = new XPathDocument(xmlFile);

				XslTransform xsl = new XslTransform();

				// Load the XSL 
				xsl.Load(xslFile);

				// Create the output stream
				XmlTextWriter outFile = new XmlTextWriter(output, null);

				// Transform of XML
				xsl.Transform(xml, null, outFile);

				outFile.Close();

				return 0;
			}
			catch (Exception e) {
				Console.WriteLine("Exception: {0}", e.ToString());
				return -1;
			}

		}
	}
}
