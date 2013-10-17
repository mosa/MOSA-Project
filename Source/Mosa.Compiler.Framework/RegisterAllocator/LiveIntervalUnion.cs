/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	// TODO: Use data structures which are faster at finding intersetions, and add & evicting intervals.
	public class LiveIntervalUnion
	{
		protected List<LiveInterval> liveIntervals = new List<LiveInterval>();
		protected Register register;
		protected bool reserved;

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
					if (intersections == null)
						intersections = new List<LiveInterval>();

					intersections.Add(interval);
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
			return register.ToString();
		}
	}
}