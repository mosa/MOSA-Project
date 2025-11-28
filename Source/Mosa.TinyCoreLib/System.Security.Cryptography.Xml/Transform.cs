using System.Collections;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public abstract class Transform
{
	public string? Algorithm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlElement? Context
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public abstract Type[] InputTypes { get; }

	public abstract Type[] OutputTypes { get; }

	public Hashtable PropagatedNamespaces
	{
		get
		{
			throw null;
		}
	}

	public XmlResolver? Resolver
	{
		set
		{
		}
	}

	public virtual byte[] GetDigestedOutput(HashAlgorithm hash)
	{
		throw null;
	}

	protected abstract XmlNodeList? GetInnerXml();

	public abstract object GetOutput();

	public abstract object GetOutput(Type type);

	public XmlElement GetXml()
	{
		throw null;
	}

	public abstract void LoadInnerXml(XmlNodeList nodeList);

	public abstract void LoadInput(object obj);
}
