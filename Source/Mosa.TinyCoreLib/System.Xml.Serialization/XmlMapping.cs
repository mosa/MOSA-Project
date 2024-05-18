namespace System.Xml.Serialization;

public abstract class XmlMapping
{
	public string ElementName
	{
		get
		{
			throw null;
		}
	}

	public string? Namespace
	{
		get
		{
			throw null;
		}
	}

	public string XsdElementName
	{
		get
		{
			throw null;
		}
	}

	internal XmlMapping()
	{
	}

	public void SetKey(string? key)
	{
	}
}
