using System.Xml.Schema;

namespace System.Xml;

public sealed class XmlReaderSettings
{
	public bool Async
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool CheckCharacters
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool CloseInput
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ConformanceLevel ConformanceLevel
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DtdProcessing DtdProcessing
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IgnoreComments
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IgnoreProcessingInstructions
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IgnoreWhitespace
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int LineNumberOffset
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int LinePositionOffset
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long MaxCharactersFromEntities
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long MaxCharactersInDocument
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

	[Obsolete("XmlReaderSettings.ProhibitDtd has been deprecated. Use DtdProcessing instead.")]
	public bool ProhibitDtd
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlSchemaSet Schemas
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlSchemaValidationFlags ValidationFlags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ValidationType ValidationType
	{
		get
		{
			throw null;
		}
		set
		{
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

	public XmlReaderSettings Clone()
	{
		throw null;
	}

	public void Reset()
	{
	}
}
