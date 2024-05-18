using System.Collections.ObjectModel;
using System.Xml.XPath;

namespace System.Speech.Recognition;

public class RecognizedPhrase
{
	public float Confidence
	{
		get
		{
			throw null;
		}
	}

	public Grammar Grammar
	{
		get
		{
			throw null;
		}
	}

	public int HomophoneGroupId
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<RecognizedPhrase> Homophones
	{
		get
		{
			throw null;
		}
	}

	public Collection<ReplacementText> ReplacementWordUnits
	{
		get
		{
			throw null;
		}
	}

	public SemanticValue Semantics
	{
		get
		{
			throw null;
		}
	}

	public string Text
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<RecognizedWordUnit> Words
	{
		get
		{
			throw null;
		}
	}

	internal RecognizedPhrase()
	{
	}

	public IXPathNavigable ConstructSmlFromSemantics()
	{
		throw null;
	}
}
