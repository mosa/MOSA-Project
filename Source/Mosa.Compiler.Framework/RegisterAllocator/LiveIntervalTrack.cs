﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	// TODO: Use data structures which are faster at finding intersetions, and add & evicting intervals.
	public class LiveIntervalTrack
	{
		public readonly List<LiveInterval> liveIntervals = new List<LiveInterval>();

		public readonly bool IsReserved;

		public readonly PhysicalRegister Register;

		public bool IsFloatingPoint { get { return Register.IsFloatingPoint; } }

		public bool IsInteger { get { return Register.IsInteger; } }

		public LiveIntervalTrack(PhysicalRegister register, bool reserved)
		{
			Register = register;
			IsReserved = reserved;
		}

		public void Add(LiveInterval liveInterval)
		{
			Debug.Assert(!Intersects(liveInterval));

			liveIntervals.Add(liveInterval);

			liveInterval.LiveIntervalTrack = this;
		}

		public void Evict(LiveInterval liveInterval)
		{
			liveIntervals.Remove(liveInterval);

			liveInterval.LiveIntervalTrack = null;
		}

		public void Evict(List<LiveInterval> liveIntervals)
		{
			foreach (var interval in liveIntervals)
			{
				Evict(interval);
			}
		}

		public bool Intersects(LiveInterval liveInterval)
		{
			foreach (var interval in liveIntervals)
			{
				if (interval.Intersects(liveInterval))
				{
					return true;
				}
			}

			return false;
		}

		public LiveInterval GetLiveIntervalAt(SlotIndex slotIndex)
		{
			foreach (var liveInterval in liveIntervals)
			{
				if (liveInterval.Contains(slotIndex))
					return liveInterval;
			}

			return null;
		}

		/// <summary>
		/// Gets the intersections.
		/// </summary>
		/// <param name="liveInterval">The live interval.</param>
		/// <returns></returns>
		public List<LiveInterval> GetIntersections(LiveInterval liveInterval)
		{
			List<LiveInterval> intersections = null;

			foreach (var interval in liveIntervals)
			{
				if (interval.Intersects(liveInterval))
				{
					(intersections ?? (intersections = new List<LiveInterval>())).Add(interval);
				}
			}

			return intersections;
		}

		/// <summary>
		/// Gets the next live range.
		/// </summary>
		/// <param name="after">Index of the slot.</param>
		/// <returns></returns>
		public SlotIndex GetNextLiveRange(SlotIndex after)
		{
			SlotIndex lastFree = null;

			foreach (var liveInterval in liveIntervals)
			{
				if (liveInterval.Contains(after))
					return null;

				if (liveInterval.End <= after)
					continue;

				if (lastFree == null || liveInterval.Start < lastFree)
				{
					Debug.Assert(liveInterval.Start > after);

					lastFree = liveInterval.Start;
				}
			}

			return lastFree;
		}

		public override string ToString()
		{
			return Register.ToString();
		}

		public string ToString2()
		{
			var sb = new StringBuilder();

			sb.Append(Register.ToString());
			sb.Append(' ');

			foreach (var interval in liveIntervals)
			{
				sb.Append(interval.ToString());
				sb.Append(", ");
			}

			sb.Length -= 2;

			return sb.ToString();
		}
	}
}
