using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar;

public class SrgsToken : SrgsElement
{
	public string Display
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Pronunciation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public SrgsToken(string text)
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
