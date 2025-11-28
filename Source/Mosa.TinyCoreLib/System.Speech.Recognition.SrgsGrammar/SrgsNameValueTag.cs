using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar;

public class SrgsNameValueTag : SrgsElement
{
	public string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SrgsNameValueTag()
	{
	}

	public SrgsNameValueTag(object value)
	{
	}

	public SrgsNameValueTag(string name, object value)
	{
	}

	internal override string DebuggerDisplayString()
	{
		throw null;
	}

	internal override void WriteSrgs(XmlWriter writer)
	{
		throw null;
	}
}
