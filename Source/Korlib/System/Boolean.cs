// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	public struct Boolean
	{
		internal bool _value;

		public override string ToString()
		{
			return _value ? "True" : "False";
		}
	}
}