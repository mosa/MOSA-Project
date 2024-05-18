namespace System.Speech.Recognition;

public class Choices
{
	public Choices()
	{
	}

	public Choices(params GrammarBuilder[] alternateChoices)
	{
	}

	public Choices(params string[] phrases)
	{
	}

	public void Add(params GrammarBuilder[] alternateChoices)
	{
	}

	public void Add(params string[] phrases)
	{
	}

	public GrammarBuilder ToGrammarBuilder()
	{
		throw null;
	}
}
