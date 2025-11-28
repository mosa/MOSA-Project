using System.Collections.ObjectModel;

namespace System.Speech.Recognition.SrgsGrammar;

public class SrgsRule
{
	public string BaseClass
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Collection<SrgsElement> Elements
	{
		get
		{
			throw null;
		}
	}

	public string Id
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string OnError
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string OnInit
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string OnParse
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string OnRecognition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SrgsRuleScope Scope
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public SrgsRule(string id)
	{
	}

	public SrgsRule(string id, params SrgsElement[] elements)
	{
	}

	public void Add(SrgsElement element)
	{
	}
}
