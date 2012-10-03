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

		public bool Overlaps(int start, int end)
		{
			return (start <= End - 1) && (end - 1 >= Start);
		}

		public bool Overlaps(LiveRange liveRange)
		{
			return Overlaps(liveRange.Start, liveRange.End);
		}

		public void Merge(int start, int end)
		{
			Debug.Assert(Overlaps(start, end));

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

			int active = -1;

			for (int i = 0; i < liveRanges.Count; i++)
			{
				var liveRange = liveRanges[i];

				if (liveRange.Overlaps(start, end))
				{
					liveRange.Merge(start, end);
					active = i;
					break;
				}

				if (liveRange.Start >= end)
				{
					// new range is before the current range (so insert before)
					liveRanges.Insert(i, new LiveRange(start, end));
					return;
				}
			}

			if (active == -1)
			{
				// new range is after the last range
				liveRanges.Add(new LiveRange(start, end));
				return;
			}

			LiveRange activeLiveRange = liveRanges[active];

			for (int i = active + 1; i < liveRanges.Count; i++)
			{
				var liveRange = liveRanges[i];

				if (!activeLiveRange.Overlaps(liveRange.Start, liveRange.End))
					break;

				activeLiveRange.Merge(liveRange.Start, liveRange.End);
				liveRanges.RemoveAt(i);
			}
		}
	}

}

