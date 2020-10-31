// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	public struct Boolean
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
	}
}
