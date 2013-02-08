/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	// TODO: Use data structures which are faster at finding intersetions, and add & evicting intervals.
	public class LiveIntervalUnion
	{
		protected List<LiveInterval> liveIntervals = new List<LiveInterval>();
		protected Register register;
		protected bool reserved;

		//public IList<LiveInterval> LiveIntervals { get { return liveIntervals.AsReadOnly(); } }

		public Register Register { get { return register; } }

		public bool IsFloatingPoint { get { return register.IsFloatingPoint; } }

		public bool IsInteger { get { return register.IsInteger; } }

		public bool IsReserved { get { return reserved; } }

		public LiveIntervalUnion(Register register, bool reserved)
		{
			this.register = register;
			this.reserved = reserved;
		}

		public void Add(LiveInterval liveInterval)
		{
			liveIntervals.Add(liveInterval);

			liveInterval.LiveIntervalUnion = this;
		}

		public void Evict(LiveInterval liveInterval)
		{
			liveIntervals.Remove(liveInterval);

			liveInterval.LiveIntervalUnion = null;
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

		public LiveInterval GetNextLiveIntervalAt(SlotIndex slotIndex)
		{
			LiveInterval nextLiveInterval = null;

			foreach (var liveInterval in liveIntervals)
			{
				if (liveInterval.Contains(slotIndex))
					return null;

				if (liveInterval.End < slotIndex)
					continue;

				if (nextLiveInterval == null || liveInterval.Start < nextLiveInterval.Start)
					nextLiveInterval = liveInterval;
			}

			return nextLiveInterval;
		}

		public SlotIndex GetMaximunFreeSlotAfter(SlotIndex slotIndex)
		{
			SlotIndex lastFree = null;

			foreach (var liveInterval in liveIntervals)
			{
				if (liveInterval.Contains(slotIndex))
					return null;

				if (liveInterval.End <= slotIndex)
					continue;

				if (lastFree == null || liveInterval.Start < lastFree)
				{
					Debug.Assert(liveInterval.Start > slotIndex);

					lastFree = liveInterval.Start;
				}
			}

			return lastFree;
		}

		public List<LiveInterval> GetIntersections(LiveInterval liveInterval)
		{
			List<LiveInterval> intersections = null;

			foreach (var interval in liveIntervals)
			{
				if (interval.Intersects(liveInterval))
				{
					if (intersections == null)
						intersections = new List<LiveInterval>();

					intersections.Add(interval);
				}
			}

			return intersections;
		}

		public override string ToString()
		{
			return register.ToString();
		}
	}
}