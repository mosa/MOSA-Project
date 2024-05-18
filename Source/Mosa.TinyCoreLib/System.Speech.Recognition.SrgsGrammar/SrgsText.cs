using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar;

public class SrgsText : SrgsElement
{
	public string Text
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SrgsText()
	{
	}

	public SrgsText(string text)
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
