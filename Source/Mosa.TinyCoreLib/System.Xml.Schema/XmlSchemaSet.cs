using System.Collections;

namespace System.Xml.Schema;

public class XmlSchemaSet
{
	public XmlSchemaCompilationSettings CompilationSettings
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public XmlSchemaObjectTable GlobalAttributes
	{
		get
		{
			throw null;
		}
	}

	public XmlSchemaObjectTable GlobalElements
	{
		get
		{
			throw null;
		}
	}

	public XmlSchemaObjectTable GlobalTypes
	{
		get
		{
			throw null;
		}
	}

	public bool IsCompiled
	{
		get
		{
			throw null;
		}
	}

	public XmlNameTable NameTable
	{
		get
		{
			throw null;
		}
	}

	public XmlResolver? XmlResolver
	{
		set
		{
		}
	}

	public event ValidationEventHandler ValidationEventHandler
	{
		add
		{
		}
		remove
		{
		}
	}

	public XmlSchemaSet()
	{
	}

	public XmlSchemaSet(XmlNameTable nameTable)
	{
	}

	public XmlSchema? Add(string? targetNamespace, string schemaUri)
	{
		throw null;
	}

	public XmlSchema? Add(string? targetNamespace, XmlReader schemaDocument)
	{
		throw null;
	}

	public XmlSchema? Add(XmlSchema schema)
	{
		throw null;
	}

	public void Add(XmlSchemaSet schemas)
	{
	}

	public void Compile()
	{
	}

	public bool Contains(string? targetNamespace)
	{
		throw null;
	}

	public bool Contains(XmlSchema schema)
	{
		throw null;
	}

	public void CopyTo(XmlSchema[] schemas, int index)
	{
	}

	public XmlSchema? Remove(XmlSchema schema)
	{
		throw null;
	}

	public bool RemoveRecursive(XmlSchema schemaToRemove)
	{
		throw null;
	}

	public XmlSchema Reprocess(XmlSchema schema)
	{
		throw null;
	}

	public ICollection Schemas()
	{
		throw null;
	}

	public ICollection Schemas(string? targetNamespace)
	{
		throw null;
	}
}
