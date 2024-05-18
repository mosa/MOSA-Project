using System.Collections.ObjectModel;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar;

public class SrgsItem : SrgsElement
{
	public Collection<SrgsElement> Elements
	{
		get
		{
			throw null;
		}
	}

	public int MaxRepeat
	{
		get
		{
			throw null;
		}
	}

	public int MinRepeat
	{
		get
		{
			throw null;
		}
	}

	public float RepeatProbability
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public float Weight
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SrgsItem()
	{
	}

	public SrgsItem(int repeatCount)
	{
	}

	public SrgsItem(int min, int max)
	{
	}

	public SrgsItem(int min, int max, params SrgsElement[] elements)
	{
	}

	public SrgsItem(int min, int max, string text)
	{
	}

	public SrgsItem(params SrgsElement[] elements)
	{
	}

	public SrgsItem(string text)
	{
	}

	public void Add(SrgsElement element)
	{
	}

	public void SetRepeat(int count)
	{
	}

	public void SetRepeat(int minRepeat, int maxRepeat)
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
