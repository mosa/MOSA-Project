namespace System.Xml.Serialization;

public class XmlElementEventArgs : EventArgs
{
	public XmlElement Element
	{
		get
		{
			throw null;
		}
	}

	public string ExpectedElements
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

	internal XmlElementEventArgs()
	{
	}
}
