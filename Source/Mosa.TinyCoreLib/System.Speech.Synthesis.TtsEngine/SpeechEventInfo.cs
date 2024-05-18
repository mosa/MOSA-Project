using System.ComponentModel;

namespace System.Speech.Synthesis.TtsEngine;

[ImmutableObject(true)]
public struct SpeechEventInfo : IEquatable<SpeechEventInfo>
{
	private object _dummy;

	private int _dummyPrimitive;

	public short EventId
	{
		get
		{
			throw null;
		}
	}

	public int Param1
	{
		get
		{
			throw null;
		}
	}

	public IntPtr Param2
	{
		get
		{
			throw null;
		}
	}

	public short ParameterType
	{
		get
		{
			throw null;
		}
	}

	public SpeechEventInfo(short eventId, short parameterType, int param1, IntPtr param2)
	{
		throw null;
	}

	public override bool Equals(object obj)
	{
		throw null;
	}

	public bool Equals(SpeechEventInfo other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(SpeechEventInfo event1, SpeechEventInfo event2)
	{
		throw null;
	}

	public static bool operator !=(SpeechEventInfo event1, SpeechEventInfo event2)
	{
		throw null;
	}
}
