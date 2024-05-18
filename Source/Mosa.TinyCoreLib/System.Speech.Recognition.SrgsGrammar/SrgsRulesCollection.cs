using System.Collections.ObjectModel;

namespace System.Speech.Recognition.SrgsGrammar;

public sealed class SrgsRulesCollection : KeyedCollection<string, SrgsRule>
{
	public void Add(params SrgsRule[] rules)
	{
	}

	protected override string GetKeyForItem(SrgsRule rule)
	{
		throw null;
	}
}
