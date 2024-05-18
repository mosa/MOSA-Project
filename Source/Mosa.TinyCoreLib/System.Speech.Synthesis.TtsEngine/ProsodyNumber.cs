using System.ComponentModel;

namespace System.Speech.Synthesis.TtsEngine;

[ImmutableObject(true)]
public struct ProsodyNumber : IEquatable<ProsodyNumber>
{
	private object _dummy;

	private int _dummyPrimitive;

	public const int AbsoluteNumber = int.MaxValue;

	public bool IsNumberPercent
	{
		get
		{
			throw null;
		}
	}

	public float Number
	{
		get
		{
			throw null;
		}
	}

	public int SsmlAttributeId
	{
		get
		{
			throw null;
		}
	}

	public ProsodyUnit Unit
	{
		get
		{
			throw null;
		}
	}

	public ProsodyNumber(int ssmlAttributeId)
	{
		throw null;
	}

	public ProsodyNumber(float number)
	{
		throw null;
	}

	public override bool Equals(object obj)
	{
		throw null;
	}

	public bool Equals(ProsodyNumber other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(ProsodyNumber prosodyNumber1, ProsodyNumber prosodyNumber2)
	{
		throw null;
	}

	public static bool operator !=(ProsodyNumber prosodyNumber1, ProsodyNumber prosodyNumber2)
	{
		throw null;
	}
}
