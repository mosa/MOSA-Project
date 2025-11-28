using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace System.Xml;

public class XmlParserContext
{
	public string BaseURI
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public string DocTypeName
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public Encoding? Encoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string InternalSubset
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public XmlNamespaceManager? NamespaceManager
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlNameTable? NameTable
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string PublicId
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public string SystemId
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public string XmlLang
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public XmlSpace XmlSpace
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlParserContext(XmlNameTable? nt, XmlNamespaceManager? nsMgr, string? docTypeName, string? pubId, string? sysId, string? internalSubset, string? baseURI, string? xmlLang, XmlSpace xmlSpace)
	{
	}

	public XmlParserContext(XmlNameTable? nt, XmlNamespaceManager? nsMgr, string? docTypeName, string? pubId, string? sysId, string? internalSubset, string? baseURI, string? xmlLang, XmlSpace xmlSpace, Encoding? enc)
	{
	}

	public XmlParserContext(XmlNameTable? nt, XmlNamespaceManager? nsMgr, string? xmlLang, XmlSpace xmlSpace)
	{
	}

	public XmlParserContext(XmlNameTable? nt, XmlNamespaceManager? nsMgr, string? xmlLang, XmlSpace xmlSpace, Encoding? enc)
	{
	}
}
