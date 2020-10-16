// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Analysis.LiveVariableAnalysis
{
	public struct Range
	{
		public int Start { get; }

		public int End { get; }

		public Range(int start, int end)
		{
			Debug.Assert(start <= end);

			Start = start;
			End = end;
		}

		public int Length { get { return End - Start; } }

		public bool Intersects(int start, int end)
		{
			return (Start <= start && End > start) || (start <= Start && end > Start);
		}

		public bool Intersects(Range other)
		{
			return Intersects(other.Start, other.End);
		}

		public bool IsAdjacent(int start, int end)
		{
			return start == End || end == Start;
		}

		public bool IsAdjacent(Range other)
		{
			return IsAdjacent(other.Start, other.End);
		}

		public bool Contains(int index)
		{
			return index >= Start && index < End;
		}

		public override string ToString()
		{
			return $"[{Start}, {End}]";
		}
	}
}
