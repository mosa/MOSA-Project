using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar;

public class SrgsSemanticInterpretationTag : SrgsElement
{
	public string Script
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SrgsSemanticInterpretationTag()
	{
	}

	public SrgsSemanticInterpretationTag(string script)
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
