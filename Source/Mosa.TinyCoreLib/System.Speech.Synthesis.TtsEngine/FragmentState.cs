using System.ComponentModel;

namespace System.Speech.Synthesis.TtsEngine;

[ImmutableObject(true)]
public struct FragmentState : IEquatable<FragmentState>
{
	private object _dummy;

	private int _dummyPrimitive;

	public TtsEngineAction Action
	{
		get
		{
			throw null;
		}
	}

	public int Duration
	{
		get
		{
			throw null;
		}
	}

	public int Emphasis
	{
		get
		{
			throw null;
		}
	}

	public int LangId
	{
		get
		{
			throw null;
		}
	}

	public char[] Phoneme
	{
		get
		{
			throw null;
		}
	}

	public Prosody Prosody
	{
		get
		{
			throw null;
		}
	}

	public SayAs SayAs
	{
		get
		{
			throw null;
		}
	}

	public FragmentState(TtsEngineAction action, int langId, int emphasis, int duration, SayAs sayAs, Prosody prosody, char[] phonemes)
	{
		throw null;
	}

	public override bool Equals(object obj)
	{
		throw null;
	}

	public bool Equals(FragmentState other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(FragmentState state1, FragmentState state2)
	{
		throw null;
	}

	public static bool operator !=(FragmentState state1, FragmentState state2)
	{
		throw null;
	}
}
