// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Framework.Analysis.LiveVariableAnalysis
{
	public sealed class LiveRanges
	{
		public List<Range> Ranges { get; } = new List<Range>(1);

		public int Count { get { return Ranges.Count; } }

		public Range LastRange { get { return Ranges[Count - 1]; } }

		public Range FirstRange { get { return Ranges[0]; } set { Ranges[0] = value; } }

		public void Add(int start, int end)
		{
			Add(new Range(start, end));
		}

		public void Add(Range range)
		{
			if (Count == 0)
			{
				Ranges.Add(range);
				return;
			}

			if (range.End < FirstRange.Start)
			{
				Ranges.Insert(0, range);
				return;
			}

			if (range.Start > LastRange.End)
			{
				Ranges.Add(range);
				return;
			}

			for (int i = 0; i < Count; i++)
			{
				var liveRange = Ranges[i];

				if (liveRange.Start == range.Start && liveRange.End == range.End)
					return;

				if (liveRange.IsAdjacent(range) || liveRange.Intersects(range))
				{
					liveRange = new Range(Math.Min(range.Start, liveRange.Start), Math.Max(range.End, liveRange.End));
					Ranges[i] = liveRange;

					for (int z = i + 1; z < Count; z++)
					{
						var nextLiveRange = Ranges[z];
						if (liveRange.IsAdjacent(nextLiveRange) || liveRange.Intersects(nextLiveRange))
						{
							var newliveRange = new Range(Math.Min(liveRange.Start, nextLiveRange.Start), Math.Max(liveRange.End, nextLiveRange.End));
							Ranges[i] = liveRange;
							Ranges.RemoveAt(z);

							continue;
						}

						return;
					}

					return;
				}
			}
		}

		public override string ToString()
		{
			if (Ranges.Count == 0)
				return string.Empty;

			var sb = new StringBuilder();

			foreach (var range in Ranges)
			{
				sb.Append("[").Append(range.Start).Append(",").Append(range.End).Append("],");
			}

			if (sb[sb.Length - 1] == ',')
				sb.Length--;

			return sb.ToString();
		}
	}
}
