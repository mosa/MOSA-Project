namespace System.Xml.Schema;

public class ValidationEventArgs : EventArgs
{
	public XmlSchemaException Exception
	{
		get
		{
			throw null;
		}
	}

	public string Message
	{
		get
		{
			throw null;
		}
	}

	public XmlSeverityType Severity
	{
		get
		{
			throw null;
		}
	}

	internal ValidationEventArgs()
	{
	}
}
