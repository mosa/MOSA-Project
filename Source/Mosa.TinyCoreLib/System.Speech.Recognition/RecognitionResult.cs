using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace System.Speech.Recognition;

public sealed class RecognitionResult : RecognizedPhrase, ISerializable
{
	public ReadOnlyCollection<RecognizedPhrase> Alternates
	{
		get
		{
			throw null;
		}
	}

	public RecognizedAudio Audio
	{
		get
		{
			throw null;
		}
	}

	internal RecognitionResult()
	{
	}

	public RecognizedAudio GetAudioForWordRange(RecognizedWordUnit firstWord, RecognizedWordUnit lastWord)
	{
		throw null;
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
