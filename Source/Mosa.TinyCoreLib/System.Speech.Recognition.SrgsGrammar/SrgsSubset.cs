using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar;

public class SrgsSubset : SrgsElement
{
	public SubsetMatchingMode MatchingMode
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

	public SrgsSubset(string text)
	{
	}

	public SrgsSubset(string text, SubsetMatchingMode matchingMode)
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
