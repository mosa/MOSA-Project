// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Label Region
	/// </summary>
	public struct LabelRegion
	{
		public int Label;
		public int Start;
		public int Length;

		public int End { get { return Start + Length; } }
	}
}
