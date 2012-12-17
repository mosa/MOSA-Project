/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.RegisterAllocator
{

	/// <summary>
	/// 
	/// </summary>
	public class Interval
	{
		public int Start { get; set; }
		public int End { get; set; }
		public int Size { get { return End - Start; } }

		public Interval(int start, int end)
		{
			Debug.Assert(end > start);
			Start = start;
			End = end;
		}

		public override string ToString()
		{
			return "[" + Start.ToString() + ", " + End.ToString() + "]";
		}

		public bool IsInside(int location)
		{
			return (location >= Start && location < End);
		}

		public bool IsSame(int start, int end)
		{
			return (Start == start && End == end);
		}

		public bool Intersects(int start, int end)
		{
			return (start <= End - 1) && (end - 1 >= Start);
		}

		public bool Intersects(Interval liveRange)
		{
			return Intersects(liveRange.Start, liveRange.End);
		}

		public bool IsAdjacent(int start, int end)
		{
			return (start == End) || (end == Start);
		}

		public bool IsAdjacent(Interval liveRange)
		{
			return IsAdjacent(liveRange.Start, liveRange.End);
		}

		public void Merge(int start, int end)
		{
			Debug.Assert(IsAdjacent(start, end));

			this.Start = Math.Min(this.Start, start);
			this.End = Math.Max(this.End, end);
		}

		public void Merge(Interval liveRange)
		{
			Merge(liveRange.Start, liveRange.End);
		}

	}

}

