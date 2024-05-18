namespace System.Speech.Recognition;

public class SemanticResultKey
{
	public SemanticResultKey(string semanticResultKey, params GrammarBuilder[] builders)
	{
	}

	public SemanticResultKey(string semanticResultKey, params string[] phrases)
	{
	}

	public GrammarBuilder ToGrammarBuilder()
	{
		throw null;
	}
}
