namespace System.Speech.Recognition;

public class RecognizedWordUnit
{
	public float Confidence
	{
		get
		{
			throw null;
		}
	}

	public DisplayAttributes DisplayAttributes
	{
		get
		{
			throw null;
		}
	}

	public string LexicalForm
	{
		get
		{
			throw null;
		}
	}

	public string Pronunciation
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

	public RecognizedWordUnit(string text, float confidence, string pronunciation, string lexicalForm, DisplayAttributes displayAttributes, TimeSpan audioPosition, TimeSpan audioDuration)
	{
	}
}
