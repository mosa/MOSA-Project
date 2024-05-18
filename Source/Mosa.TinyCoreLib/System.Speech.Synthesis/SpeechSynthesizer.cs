using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Speech.AudioFormat;

namespace System.Speech.Synthesis;

public sealed class SpeechSynthesizer : IDisposable
{
	public int Rate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SynthesizerState State
	{
		get
		{
			throw null;
		}
	}

	public VoiceInfo Voice
	{
		get
		{
			throw null;
		}
	}

	public int Volume
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event EventHandler<BookmarkReachedEventArgs> BookmarkReached
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<PhonemeReachedEventArgs> PhonemeReached
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<SpeakCompletedEventArgs> SpeakCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<SpeakProgressEventArgs> SpeakProgress
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<SpeakStartedEventArgs> SpeakStarted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<StateChangedEventArgs> StateChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<VisemeReachedEventArgs> VisemeReached
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<VoiceChangeEventArgs> VoiceChange
	{
		add
		{
		}
		remove
		{
		}
	}

	public void AddLexicon(Uri uri, string mediaType)
	{
	}

	public void Dispose()
	{
	}

	~SpeechSynthesizer()
	{
	}

	public Prompt GetCurrentlySpokenPrompt()
	{
		throw null;
	}

	public ReadOnlyCollection<InstalledVoice> GetInstalledVoices()
	{
		throw null;
	}

	public ReadOnlyCollection<InstalledVoice> GetInstalledVoices(CultureInfo culture)
	{
		throw null;
	}

	public void Pause()
	{
	}

	public void RemoveLexicon(Uri uri)
	{
	}

	public void Resume()
	{
	}

	public void SelectVoice(string name)
	{
	}

	public void SelectVoiceByHints(VoiceGender gender)
	{
	}

	public void SelectVoiceByHints(VoiceGender gender, VoiceAge age)
	{
	}

	public void SelectVoiceByHints(VoiceGender gender, VoiceAge age, int voiceAlternate)
	{
	}

	public void SelectVoiceByHints(VoiceGender gender, VoiceAge age, int voiceAlternate, CultureInfo culture)
	{
	}

	public void SetOutputToAudioStream(Stream audioDestination, SpeechAudioFormatInfo formatInfo)
	{
	}

	public void SetOutputToDefaultAudioDevice()
	{
	}

	public void SetOutputToNull()
	{
	}

	public void SetOutputToWaveFile(string path)
	{
	}

	public void SetOutputToWaveFile(string path, SpeechAudioFormatInfo formatInfo)
	{
	}

	public void SetOutputToWaveStream(Stream audioDestination)
	{
	}

	public void Speak(Prompt prompt)
	{
	}

	public void Speak(PromptBuilder promptBuilder)
	{
	}

	public void Speak(string textToSpeak)
	{
	}

	public void SpeakAsync(Prompt prompt)
	{
	}

	public Prompt SpeakAsync(PromptBuilder promptBuilder)
	{
		throw null;
	}

	public Prompt SpeakAsync(string textToSpeak)
	{
		throw null;
	}

	public void SpeakAsyncCancel(Prompt prompt)
	{
	}

	public void SpeakAsyncCancelAll()
	{
	}

	public void SpeakSsml(string textToSpeak)
	{
	}

	public Prompt SpeakSsmlAsync(string textToSpeak)
	{
		throw null;
	}
}
