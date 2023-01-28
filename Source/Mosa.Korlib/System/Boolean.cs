// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System;

/// <summary>
///
/// </summary>
[Serializable]
public struct Boolean: IComparable, IComparable<bool>, IEquatable<bool>
{
	//
	// Member Variables
	//
	internal bool m_value;

	// The true value.
	//
	internal const int True = 1;

	// The false value.
	//
	internal const int False = 0;

	//
	// Internal Constants are real consts for performance.
	//

	// The internal string representation of true.
	//
	internal const String TrueLiteral = "True";

	// The internal string representation of false.
	//
	internal const String FalseLiteral = "False";

	//
	// Public Constants
	//

	// The public string representation of true.
	//
	public static readonly String TrueString = TrueLiteral;

	// The public string representation of false.
	//
	public static readonly String FalseString = FalseLiteral;

	public override int GetHashCode()
	{
		return (m_value) ? True : False;
	}

	public override string ToString()
	{
		return m_value ? "True" : "False";
	}

	// IComparable interface
	//
	public override bool Equals(object obj)
	{
		if (obj is Boolean)
		{
			return (this.m_value == ((Boolean)obj).m_value);
		}
		else
		{
			return false;
		}
	}

	public bool Equals(bool obj)
	{
		return (this.m_value == obj.m_value);
	}

	public int CompareTo(object obj)
	{
		if (obj == null) { return 1; }

		if (!(obj is bool)) { throw new ArgumentException("Argument Type Must Be Boolean", "obj"); }

		if (this.m_value == ((bool)obj).m_value) { return 0; }

		if (this.m_value == false) { return -1; }

		return 1;	// this.m_value == true;
	}

	public int CompareTo(bool value)
	{
		if (this.m_value == value)	{ return 0;	}

		if (this.m_value == false) { return -1; }

		return 1;
	}
}
