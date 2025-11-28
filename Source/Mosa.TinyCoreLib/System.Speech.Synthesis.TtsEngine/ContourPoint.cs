using System.ComponentModel;

namespace System.Speech.Synthesis.TtsEngine;

[ImmutableObject(true)]
public struct ContourPoint : IEquatable<ContourPoint>
{
	private object _dummy;

	private int _dummyPrimitive;

	public float Change
	{
		get
		{
			throw null;
		}
	}

	public ContourPointChangeType ChangeType
	{
		get
		{
			throw null;
		}
	}

	public float Start
	{
		get
		{
			throw null;
		}
	}

	public ContourPoint(float start, float change, ContourPointChangeType changeType)
	{
		throw null;
	}

	public override bool Equals(object obj)
	{
		throw null;
	}

	public bool Equals(ContourPoint other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(ContourPoint point1, ContourPoint point2)
	{
		throw null;
	}

	public static bool operator !=(ContourPoint point1, ContourPoint point2)
	{
		throw null;
	}
}
