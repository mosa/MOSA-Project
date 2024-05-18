namespace System.Xml;

public abstract class XmlLinkedNode : XmlNode
{
	public override XmlNode? NextSibling
	{
		get
		{
			throw null;
		}
	}

	public override XmlNode? PreviousSibling
	{
		get
		{
			throw null;
		}
	}

	internal XmlLinkedNode()
	{
	}
}
