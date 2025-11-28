using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Speech.AudioFormat;

namespace System.Speech.Recognition;

public class SpeechRecognitionEngine : IDisposable
{
	public SpeechAudioFormatInfo AudioFormat
	{
		get
		{
			throw null;
		}
	}

	public int AudioLevel
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan AudioPosition
	{
		get
		{
			throw null;
		}
	}

	public AudioState AudioState
	{
		get
		{
			throw null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public TimeSpan BabbleTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public TimeSpan EndSilenceTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public TimeSpan EndSilenceTimeoutAmbiguous
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ReadOnlyCollection<Grammar> Grammars
	{
		get
		{
			throw null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public TimeSpan InitialSilenceTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int MaxAlternates
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan RecognizerAudioPosition
	{
		get
		{
			throw null;
		}
	}

	public RecognizerInfo RecognizerInfo
	{
		get
		{
			throw null;
		}
	}

	public event EventHandler<AudioLevelUpdatedEventArgs> AudioLevelUpdated
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<AudioSignalProblemOccurredEventArgs> AudioSignalProblemOccurred
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<AudioStateChangedEventArgs> AudioStateChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<EmulateRecognizeCompletedEventArgs> EmulateRecognizeCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<LoadGrammarCompletedEventArgs> LoadGrammarCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<RecognizeCompletedEventArgs> RecognizeCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<RecognizerUpdateReachedEventArgs> RecognizerUpdateReached
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<SpeechDetectedEventArgs> SpeechDetected
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<SpeechHypothesizedEventArgs> SpeechHypothesized
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<SpeechRecognitionRejectedEventArgs> SpeechRecognitionRejected
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized
	{
		add
		{
		}
		remove
		{
		}
	}

	public SpeechRecognitionEngine()
	{
	}

	public SpeechRecognitionEngine(CultureInfo culture)
	{
	}

	public SpeechRecognitionEngine(RecognizerInfo recognizerInfo)
	{
	}

	public SpeechRecognitionEngine(string recognizerId)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public RecognitionResult EmulateRecognize(RecognizedWordUnit[] wordUnits, CompareOptions compareOptions)
	{
		throw null;
	}

	public RecognitionResult EmulateRecognize(string inputText)
	{
		throw null;
	}

	public RecognitionResult EmulateRecognize(string inputText, CompareOptions compareOptions)
	{
		throw null;
	}

	public void EmulateRecognizeAsync(RecognizedWordUnit[] wordUnits, CompareOptions compareOptions)
	{
	}

	public void EmulateRecognizeAsync(string inputText)
	{
	}

	public void EmulateRecognizeAsync(string inputText, CompareOptions compareOptions)
	{
	}

	public static ReadOnlyCollection<RecognizerInfo> InstalledRecognizers()
	{
		throw null;
	}

	public void LoadGrammar(Grammar grammar)
	{
	}

	public void LoadGrammarAsync(Grammar grammar)
	{
	}

	public object QueryRecognizerSetting(string settingName)
	{
		throw null;
	}

	public RecognitionResult Recognize()
	{
		throw null;
	}

	public RecognitionResult Recognize(TimeSpan initialSilenceTimeout)
	{
		throw null;
	}

	public void RecognizeAsync()
	{
	}

	public void RecognizeAsync(RecognizeMode mode)
	{
	}

	public void RecognizeAsyncCancel()
	{
	}

	public void RecognizeAsyncStop()
	{
	}

	public void RequestRecognizerUpdate()
	{
	}

	public void RequestRecognizerUpdate(object userToken)
	{
	}

	public void RequestRecognizerUpdate(object userToken, TimeSpan audioPositionAheadToRaiseUpdate)
	{
	}

	public void SetInputToAudioStream(Stream audioSource, SpeechAudioFormatInfo audioFormat)
	{
	}

	public void SetInputToDefaultAudioDevice()
	{
	}

	public void SetInputToNull()
	{
	}

	public void SetInputToWaveFile(string path)
	{
	}

	public void SetInputToWaveStream(Stream audioSource)
	{
	}

	public void UnloadAllGrammars()
	{
	}

	public void UnloadGrammar(Grammar grammar)
	{
	}

	public void UpdateRecognizerSetting(string settingName, int updatedValue)
	{
	}

	public void UpdateRecognizerSetting(string settingName, string updatedValue)
	{
	}
}
