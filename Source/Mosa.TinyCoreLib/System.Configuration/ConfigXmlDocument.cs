using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration;

public sealed class ConfigXmlDocument : XmlDocument, IConfigErrorInfo
{
	public string Filename
	{
		get
		{
			throw null;
		}
	}

	public int LineNumber
	{
		get
		{
			throw null;
		}
	}

	string IConfigErrorInfo.Filename
	{
		get
		{
			throw null;
		}
	}

	int IConfigErrorInfo.LineNumber
	{
		get
		{
			throw null;
		}
	}

	public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceUri)
	{
		throw null;
	}

	public override XmlCDataSection CreateCDataSection(string data)
	{
		throw null;
	}

	public override XmlComment CreateComment(string data)
	{
		throw null;
	}

	public override XmlElement CreateElement(string prefix, string localName, string namespaceUri)
	{
		throw null;
	}

	public override XmlSignificantWhitespace CreateSignificantWhitespace(string data)
	{
		throw null;
	}

	public override XmlText CreateTextNode(string text)
	{
		throw null;
	}

	public override XmlWhitespace CreateWhitespace(string data)
	{
		throw null;
	}

	public override void Load(string filename)
	{
	}

	public void LoadSingleElement(string filename, XmlTextReader sourceReader)
	{
	}
}
