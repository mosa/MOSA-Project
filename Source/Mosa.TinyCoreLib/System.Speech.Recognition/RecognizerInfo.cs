using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.AudioFormat;

namespace System.Speech.Recognition;

public class RecognizerInfo : IDisposable
{
	public IDictionary<string, string> AdditionalInfo
	{
		get
		{
			throw null;
		}
	}

	public CultureInfo Culture
	{
		get
		{
			throw null;
		}
	}

	public string Description
	{
		get
		{
			throw null;
		}
	}

	public string Id
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<SpeechAudioFormatInfo> SupportedAudioFormats
	{
		get
		{
			throw null;
		}
	}

	internal RecognizerInfo()
	{
	}

	public void Dispose()
	{
	}
}
