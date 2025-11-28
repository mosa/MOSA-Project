using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Speech.AudioFormat;

namespace System.Speech.Synthesis;

public class VoiceInfo
{
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IDictionary<string, string> AdditionalInfo
	{
		get
		{
			throw null;
		}
	}

	public VoiceAge Age
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

	public VoiceGender Gender
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

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public ReadOnlyCollection<SpeechAudioFormatInfo> SupportedAudioFormats
	{
		get
		{
			throw null;
		}
	}

	internal VoiceInfo()
	{
	}

	public override bool Equals(object obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
