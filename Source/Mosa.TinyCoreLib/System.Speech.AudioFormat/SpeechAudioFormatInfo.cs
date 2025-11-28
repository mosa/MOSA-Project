using System.ComponentModel;

namespace System.Speech.AudioFormat;

public class SpeechAudioFormatInfo
{
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int AverageBytesPerSecond
	{
		get
		{
			throw null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int BitsPerSample
	{
		get
		{
			throw null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int BlockAlign
	{
		get
		{
			throw null;
		}
	}

	public int ChannelCount
	{
		get
		{
			throw null;
		}
	}

	public EncodingFormat EncodingFormat
	{
		get
		{
			throw null;
		}
	}

	public int SamplesPerSecond
	{
		get
		{
			throw null;
		}
	}

	public SpeechAudioFormatInfo(int samplesPerSecond, AudioBitsPerSample bitsPerSample, AudioChannel channel)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public SpeechAudioFormatInfo(EncodingFormat encodingFormat, int samplesPerSecond, int bitsPerSample, int channelCount, int averageBytesPerSecond, int blockAlign, byte[] formatSpecificData)
	{
	}

	public override bool Equals(object obj)
	{
		throw null;
	}

	public byte[] FormatSpecificData()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
