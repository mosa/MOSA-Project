namespace System.Xml.Schema;

public sealed class XmlSchemaInference
{
	public enum InferenceOption
	{
		Restricted,
		Relaxed
	}

	public InferenceOption Occurrence
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public InferenceOption TypeInference
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlSchemaSet InferSchema(XmlReader instanceDocument)
	{
		throw null;
	}

	public XmlSchemaSet InferSchema(XmlReader instanceDocument, XmlSchemaSet schemas)
	{
		throw null;
	}
}
