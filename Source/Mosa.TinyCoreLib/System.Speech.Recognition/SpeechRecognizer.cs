using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.AudioFormat;

namespace System.Speech.Recognition;

public class SpeechRecognizer : IDisposable
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

	public bool Enabled
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

	public bool PauseRecognizerOnRecognition
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

	public RecognizerState State
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

	public event EventHandler<StateChangedEventArgs> StateChanged
	{
		add
		{
		}
		remove
		{
		}
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

	public void LoadGrammar(Grammar grammar)
	{
	}

	public void LoadGrammarAsync(Grammar grammar)
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

	public void UnloadAllGrammars()
	{
	}

	public void UnloadGrammar(Grammar grammar)
	{
	}
}
