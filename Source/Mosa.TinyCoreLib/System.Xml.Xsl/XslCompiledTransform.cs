using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Xml.XPath;

namespace System.Xml.Xsl;

[RequiresDynamicCode("XslCompiledTransform requires dynamic code because it generates IL at runtime.")]
public sealed class XslCompiledTransform
{
	public XmlWriterSettings? OutputSettings
	{
		get
		{
			throw null;
		}
	}

	public XslCompiledTransform()
	{
	}

	public XslCompiledTransform(bool enableDebug)
	{
	}

	[RequiresUnreferencedCode("This method will call into constructors of the earlyBoundTypes array which cannot be statically analyzed.")]
	public void Load(MethodInfo executeMethod, byte[] queryData, Type[]? earlyBoundTypes)
	{
	}

	public void Load(string stylesheetUri)
	{
	}

	public void Load(string stylesheetUri, XsltSettings? settings, XmlResolver? stylesheetResolver)
	{
	}

	[RequiresUnreferencedCode("This method will get fields and types from the assembly of the passed in compiledStylesheet and call their constructors which cannot be statically analyzed")]
	public void Load(Type compiledStylesheet)
	{
	}

	public void Load(XmlReader stylesheet)
	{
	}

	public void Load(XmlReader stylesheet, XsltSettings? settings, XmlResolver? stylesheetResolver)
	{
	}

	public void Load(IXPathNavigable stylesheet)
	{
	}

	public void Load(IXPathNavigable stylesheet, XsltSettings? settings, XmlResolver? stylesheetResolver)
	{
	}

	public void Transform(string inputUri, string resultsFile)
	{
	}

	public void Transform(string inputUri, XmlWriter results)
	{
	}

	public void Transform(string inputUri, XsltArgumentList? arguments, Stream results)
	{
	}

	public void Transform(string inputUri, XsltArgumentList? arguments, TextWriter results)
	{
	}

	public void Transform(string inputUri, XsltArgumentList? arguments, XmlWriter results)
	{
	}

	public void Transform(XmlReader input, XmlWriter results)
	{
	}

	public void Transform(XmlReader input, XsltArgumentList? arguments, Stream results)
	{
	}

	public void Transform(XmlReader input, XsltArgumentList? arguments, TextWriter results)
	{
	}

	public void Transform(XmlReader input, XsltArgumentList? arguments, XmlWriter results)
	{
	}

	public void Transform(XmlReader input, XsltArgumentList? arguments, XmlWriter results, XmlResolver? documentResolver)
	{
	}

	public void Transform(IXPathNavigable input, XmlWriter results)
	{
	}

	public void Transform(IXPathNavigable input, XsltArgumentList? arguments, Stream results)
	{
	}

	public void Transform(IXPathNavigable input, XsltArgumentList? arguments, TextWriter results)
	{
	}

	public void Transform(IXPathNavigable input, XsltArgumentList? arguments, XmlWriter results)
	{
	}

	public void Transform(IXPathNavigable input, XsltArgumentList? arguments, XmlWriter results, XmlResolver? documentResolver)
	{
	}
}
