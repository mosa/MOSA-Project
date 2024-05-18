namespace System.Xml.Serialization;

public class XmlAttributeEventArgs : EventArgs
{
	public XmlAttribute Attr
	{
		get
		{
			throw null;
		}
	}

	public string ExpectedAttributes
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

	public int LinePosition
	{
		get
		{
			throw null;
		}
	}

	public object? ObjectBeingDeserialized
	{
		get
		{
			throw null;
		}
	}

	internal XmlAttributeEventArgs()
	{
	}
}
