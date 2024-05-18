namespace System.Xml.Serialization;

public class SoapAttributeOverrides
{
	public SoapAttributes? this[Type type]
	{
		get
		{
			throw null;
		}
	}

	public SoapAttributes? this[Type type, string member]
	{
		get
		{
			throw null;
		}
	}

	public void Add(Type type, string member, SoapAttributes? attributes)
	{
	}

	public void Add(Type type, SoapAttributes? attributes)
	{
	}
}
