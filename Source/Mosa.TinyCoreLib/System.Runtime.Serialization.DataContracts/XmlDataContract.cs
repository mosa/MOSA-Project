using System.Xml.Schema;

namespace System.Runtime.Serialization.DataContracts;

public sealed class XmlDataContract : DataContract
{
	public bool HasRoot
	{
		get
		{
			throw null;
		}
	}

	public bool IsAnonymous
	{
		get
		{
			throw null;
		}
	}

	public bool IsTopLevelElementNullable
	{
		get
		{
			throw null;
		}
	}

	public bool IsTypeDefinedOnImport
	{
		get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public new bool IsValueType
	{
		get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public XmlSchemaType? XsdType
	{
		get
		{
			throw null;
		}
	}

	internal XmlDataContract(Type type)
		: base(null)
	{
	}
}
