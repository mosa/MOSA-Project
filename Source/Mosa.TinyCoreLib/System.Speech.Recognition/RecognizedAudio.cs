using System.IO;
using System.Speech.AudioFormat;

namespace System.Speech.Recognition;

public class RecognizedAudio
{
	public TimeSpan AudioPosition
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan Duration
	{
		get
		{
			throw null;
		}
	}

	public SpeechAudioFormatInfo Format
	{
		get
		{
			throw null;
		}
	}

	public DateTime StartTime
	{
		get
		{
			throw null;
		}
	}

	internal RecognizedAudio()
	{
	}

	public RecognizedAudio GetRange(TimeSpan audioPosition, TimeSpan duration)
	{
		throw null;
	}

	public void WriteToAudioStream(Stream outputStream)
	{
	}

	public void WriteToWaveStream(Stream outputStream)
	{
	}
}
