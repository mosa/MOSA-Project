using System.Collections;

namespace System.Xml.Serialization;

public abstract class XmlSerializerImplementation
{
	public virtual XmlSerializationReader Reader
	{
		get
		{
			throw null;
		}
	}

	public virtual Hashtable ReadMethods
	{
		get
		{
			throw null;
		}
	}

	public virtual Hashtable TypedSerializers
	{
		get
		{
			throw null;
		}
	}

	public virtual Hashtable WriteMethods
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlSerializationWriter Writer
	{
		get
		{
			throw null;
		}
	}

	public virtual bool CanSerialize(Type type)
	{
		throw null;
	}

	public virtual XmlSerializer GetSerializer(Type type)
	{
		throw null;
	}
}
