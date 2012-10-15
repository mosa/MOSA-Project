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

namespace Mosa.Compiler.Framework
{

	/// <summary>
	/// 
	/// </summary>
	public class LiveRange
	{
		public int Start { get; set; }
		public int End { get; set; }

		public LiveRange(int start, int end)
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

		public bool Intersects(LiveRange liveRange)
		{
			return Intersects(liveRange.Start, liveRange.End);
		}

		public bool IsAdjacent(int start, int end)
		{
			return (start == End) || (end == Start);
		}

		public bool IsAdjacent(LiveRange liveRange)
		{
			return IsAdjacent(liveRange.Start, liveRange.End);
		}

		public void Merge(int start, int end)
		{
			Debug.Assert(IsAdjacent(start, end));

			this.Start = Math.Min(this.Start, start);
			this.End = Math.Max(this.End, end);
		}

		public void Merge(LiveRange liveRange)
		{
			Merge(liveRange.Start, liveRange.End);
		}

		public static void AddRangeToList(List<LiveRange> liveRanges, LiveRange liveRange)
		{
			AddRangeToList(liveRanges, liveRange.Start, liveRange.End);
		}

		public static void AddRangeToList(List<LiveRange> liveRanges, int start, int end)
		{
			if (liveRanges.Count == 0)
			{
				liveRanges.Add(new LiveRange(start, end));
				return;
			}

			for (int i = 0; i < liveRanges.Count; i++)
			{
				var liveRange = liveRanges[i];

				if (liveRange.IsSame(start, end))
					return;

				if (liveRange.IsAdjacent(start, end))
				{
					liveRange.Merge(start, end);

					if (i + 1 < liveRanges.Count)
					{
						var nextLiveRange = liveRanges[i + 1];
						if (liveRange.IsAdjacent(nextLiveRange))
						{
							liveRange.Merge(nextLiveRange);
							liveRanges.RemoveAt(i + 1);
						}
					}

					return;
				}

				if (liveRange.Start > end)
				{
					// new range is before the current range (so insert before)
					liveRanges.Insert(i, new LiveRange(start, end));
					return;
				}

			}

			// new range is after the last range
			liveRanges.Add(new LiveRange(start, end));
		}
	}

}

