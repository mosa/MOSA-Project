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
using System.Text;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	// TODO: Use data structures which are faster at finding intersetions, and add & evicting intervals.
	public class LiveIntervalTrack
	{

		public readonly List<LiveInterval> liveIntervals = new List<LiveInterval>();

		public readonly bool IsReserved;

		public readonly Register Register;

		public bool IsFloatingPoint { get { return Register.IsFloatingPoint; } }

		public bool IsInteger { get { return Register.IsInteger; } }

		public LiveIntervalTrack(Register register, bool reserved)
		{
			this.Register = register;
			this.IsReserved = reserved;
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
			return Register.ToString();
		}

		public string ToString2()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(Register.ToString());
			sb.Append(' ');

			foreach (var interval in liveIntervals)
			{
				sb.Append(interval.ToString());
				sb.Append(", ");
			}

			sb.Length = sb.Length - 2;

			return sb.ToString();
		}
	}
}