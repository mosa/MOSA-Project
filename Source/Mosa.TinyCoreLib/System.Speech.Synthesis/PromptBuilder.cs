using System.ComponentModel;
using System.Globalization;
using System.Xml;

namespace System.Speech.Synthesis;

public class PromptBuilder
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

	public bool IsEmpty
	{
		get
		{
			throw null;
		}
	}

	public PromptBuilder()
	{
	}

	public PromptBuilder(CultureInfo culture)
	{
	}

	public void AppendAudio(string path)
	{
	}

	public void AppendAudio(Uri audioFile)
	{
	}

	public void AppendAudio(Uri audioFile, string alternateText)
	{
	}

	public void AppendBookmark(string bookmarkName)
	{
	}

	public void AppendBreak()
	{
	}

	public void AppendBreak(PromptBreak strength)
	{
	}

	public void AppendBreak(TimeSpan duration)
	{
	}

	public void AppendPromptBuilder(PromptBuilder promptBuilder)
	{
	}

	public void AppendSsml(string path)
	{
	}

	public void AppendSsml(Uri ssmlFile)
	{
	}

	public void AppendSsml(XmlReader ssmlFile)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void AppendSsmlMarkup(string ssmlMarkup)
	{
	}

	public void AppendText(string textToSpeak)
	{
	}

	public void AppendText(string textToSpeak, PromptEmphasis emphasis)
	{
	}

	public void AppendText(string textToSpeak, PromptRate rate)
	{
	}

	public void AppendText(string textToSpeak, PromptVolume volume)
	{
	}

	public void AppendTextWithAlias(string textToSpeak, string substitute)
	{
	}

	public void AppendTextWithHint(string textToSpeak, SayAs sayAs)
	{
	}

	public void AppendTextWithHint(string textToSpeak, string sayAs)
	{
	}

	public void AppendTextWithPronunciation(string textToSpeak, string pronunciation)
	{
	}

	public void ClearContent()
	{
	}

	public void EndParagraph()
	{
	}

	public void EndSentence()
	{
	}

	public void EndStyle()
	{
	}

	public void EndVoice()
	{
	}

	public void StartParagraph()
	{
	}

	public void StartParagraph(CultureInfo culture)
	{
	}

	public void StartSentence()
	{
	}

	public void StartSentence(CultureInfo culture)
	{
	}

	public void StartStyle(PromptStyle style)
	{
	}

	public void StartVoice(CultureInfo culture)
	{
	}

	public void StartVoice(VoiceGender gender)
	{
	}

	public void StartVoice(VoiceGender gender, VoiceAge age)
	{
	}

	public void StartVoice(VoiceGender gender, VoiceAge age, int voiceAlternate)
	{
	}

	public void StartVoice(VoiceInfo voice)
	{
	}

	public void StartVoice(string name)
	{
	}

	public string ToXml()
	{
		throw null;
	}
}
