using System.Diagnostics.CodeAnalysis;

namespace System.Xml;

public abstract class XmlCharacterData : XmlLinkedNode
{
	public virtual string Data
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

	public override string InnerText
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual int Length
	{
		get
		{
			throw null;
		}
	}

	public override string? Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected internal XmlCharacterData(string? data, XmlDocument doc)
	{
	}

	public virtual void AppendData(string? strData)
	{
	}

	public virtual void DeleteData(int offset, int count)
	{
	}

	public virtual void InsertData(int offset, string? strData)
	{
	}

	public virtual void ReplaceData(int offset, int count, string? strData)
	{
	}

	public virtual string Substring(int offset, int count)
	{
		throw null;
	}
}
