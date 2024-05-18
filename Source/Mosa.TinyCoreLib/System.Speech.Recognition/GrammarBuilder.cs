using System.Globalization;

namespace System.Speech.Recognition;

public class GrammarBuilder
{
	public CultureInfo Culture
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string DebugShowPhrases
	{
		get
		{
			throw null;
		}
	}

	public GrammarBuilder()
	{
	}

	public GrammarBuilder(Choices alternateChoices)
	{
	}

	public GrammarBuilder(GrammarBuilder builder, int minRepeat, int maxRepeat)
	{
	}

	public GrammarBuilder(SemanticResultKey key)
	{
	}

	public GrammarBuilder(SemanticResultValue value)
	{
	}

	public GrammarBuilder(string phrase)
	{
	}

	public GrammarBuilder(string phrase, int minRepeat, int maxRepeat)
	{
	}

	public GrammarBuilder(string phrase, SubsetMatchingMode subsetMatchingCriteria)
	{
	}

	public static GrammarBuilder Add(Choices choices, GrammarBuilder builder)
	{
		throw null;
	}

	public static GrammarBuilder Add(GrammarBuilder builder, Choices choices)
	{
		throw null;
	}

	public static GrammarBuilder Add(GrammarBuilder builder1, GrammarBuilder builder2)
	{
		throw null;
	}

	public static GrammarBuilder Add(GrammarBuilder builder, string phrase)
	{
		throw null;
	}

	public static GrammarBuilder Add(string phrase, GrammarBuilder builder)
	{
		throw null;
	}

	public void Append(Choices alternateChoices)
	{
	}

	public void Append(GrammarBuilder builder)
	{
	}

	public void Append(GrammarBuilder builder, int minRepeat, int maxRepeat)
	{
	}

	public void Append(SemanticResultKey key)
	{
	}

	public void Append(SemanticResultValue value)
	{
	}

	public void Append(string phrase)
	{
	}

	public void Append(string phrase, int minRepeat, int maxRepeat)
	{
	}

	public void Append(string phrase, SubsetMatchingMode subsetMatchingCriteria)
	{
	}

	public void AppendDictation()
	{
	}

	public void AppendDictation(string category)
	{
	}

	public void AppendRuleReference(string path)
	{
	}

	public void AppendRuleReference(string path, string rule)
	{
	}

	public void AppendWildcard()
	{
	}

	public static GrammarBuilder operator +(Choices choices, GrammarBuilder builder)
	{
		throw null;
	}

	public static GrammarBuilder operator +(GrammarBuilder builder, Choices choices)
	{
		throw null;
	}

	public static GrammarBuilder operator +(GrammarBuilder builder1, GrammarBuilder builder2)
	{
		throw null;
	}

	public static GrammarBuilder operator +(GrammarBuilder builder, string phrase)
	{
		throw null;
	}

	public static GrammarBuilder operator +(string phrase, GrammarBuilder builder)
	{
		throw null;
	}

	public static implicit operator GrammarBuilder(Choices choices)
	{
		throw null;
	}

	public static implicit operator GrammarBuilder(SemanticResultKey semanticKey)
	{
		throw null;
	}

	public static implicit operator GrammarBuilder(SemanticResultValue semanticValue)
	{
		throw null;
	}

	public static implicit operator GrammarBuilder(string phrase)
	{
		throw null;
	}
}
