namespace System.Xml.Serialization;

public struct XmlDeserializationEvents
{
	private object _dummy;

	private int _dummyPrimitive;

	public XmlAttributeEventHandler? OnUnknownAttribute
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlElementEventHandler? OnUnknownElement
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlNodeEventHandler? OnUnknownNode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public UnreferencedObjectEventHandler? OnUnreferencedObject
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}
}
