using System.Collections.ObjectModel;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar;

public class SrgsOneOf : SrgsElement
{
	public Collection<SrgsItem> Items
	{
		get
		{
			throw null;
		}
	}

	public SrgsOneOf()
	{
	}

	public SrgsOneOf(params SrgsItem[] items)
	{
	}

	public SrgsOneOf(params string[] items)
	{
	}

	public void Add(SrgsItem item)
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
