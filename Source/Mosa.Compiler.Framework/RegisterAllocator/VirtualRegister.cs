/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class VirtualRegister	// alternative name is LiveInterval
	{
		private List<LiveInterval> liveIntervals = new List<LiveInterval>(1);

		private SortedList<SlotIndex, SlotIndex> usePositions = new SortedList<SlotIndex, SlotIndex>();

		private SortedList<SlotIndex, SlotIndex> defPositions = new SortedList<SlotIndex, SlotIndex>();

		public Operand VirtualRegisterOperand { get; private set; }

		public Register PhysicalRegister { get; private set; }

		public bool IsPhysicalRegister { get { return VirtualRegisterOperand == null; } }

		public bool IsVirtualRegister { get { return VirtualRegisterOperand != null; } }

		public List<LiveInterval> LiveIntervals { get { return liveIntervals; } }

		public int Count { get { return liveIntervals.Count; } }

		public IList<SlotIndex> UsePositions { get { return usePositions.Keys; } }

		public IList<SlotIndex> DefPositions { get { return defPositions.Keys; } }

		public LiveInterval LastRange { get { return liveIntervals.Count == 0 ? null : liveIntervals[liveIntervals.Count - 1]; } }

		public LiveInterval FirstRange { get { return liveIntervals.Count == 0 ? null : liveIntervals[0]; } set { liveIntervals[0] = value; } }

		public Operand SpillSlotOperand { get; set; }

		public bool IsFloatingPoint { get { return VirtualRegisterOperand.IsR; } }

		public bool IsReserved { get; private set; }

		public bool IsSpilled { get; set; }

		public bool IsUsed { get { return Count != 0; } }

		public VirtualRegister(Operand virtualRegister)
		{
			this.VirtualRegisterOperand = virtualRegister;
			this.IsReserved = false;
			this.IsSpilled = false;
		}

		public VirtualRegister(Register physicalRegister, bool reserved)
		{
			this.PhysicalRegister = physicalRegister;
			this.IsReserved = reserved;
			this.IsSpilled = false;
		}

		public void AddUsePosition(SlotIndex position)
		{
			if (!usePositions.ContainsKey(position))
			{
				usePositions.Add(position, position);
			}
		}

		public void AddDefPosition(SlotIndex position)
		{
			if (!defPositions.ContainsKey(position))
			{
				defPositions.Add(position, position);
			}
		}

		public void Add(LiveInterval liveInterval)
		{
			liveIntervals.Add(liveInterval);
		}

		public void Remove(LiveInterval liveInterval)
		{
			liveIntervals.Remove(liveInterval);
		}

		public void AddLiveInterval(Interval interval)
		{
			AddLiveInterval(interval.Start, interval.End);
		}

		public void AddLiveInterval(SlotIndex start, SlotIndex end)
		{
			if (liveIntervals.Count == 0)
			{
				liveIntervals.Add(new LiveInterval(this, start, end));
				return;
			}

			for (int i = 0; i < liveIntervals.Count; i++)
			{
				var liveRange = liveIntervals[i];

				if (liveRange.Start == start && liveRange.End == end)
					return;

				if (liveRange.IsAdjacent(start, end) || liveRange.Intersects(start, end))
				{
					liveRange = liveRange.CreateExpandedLiveRange(start, end);
					liveIntervals[i] = liveRange;

					for (int z = i + 1; z < liveIntervals.Count; z++)
					{
						var nextLiveRange = liveIntervals[z];
						if (liveRange.IsAdjacent(nextLiveRange) || liveRange.Intersects(nextLiveRange))
						{
							liveRange = liveRange.CreateExpandedLiveInterval(nextLiveRange);
							liveIntervals[i] = liveRange;
							liveIntervals.RemoveAt(z);

							continue;
						}

						return;
					}

					return;
				}

				if (liveRange.Start > end)
				{
					// new range is before the current range (so insert before)
					liveIntervals.Insert(i, new LiveInterval(this, start, end));
					return;
				}
			}

			// new range is after the last range
			liveIntervals.Add(new LiveInterval(this, start, end));
		}

		/// <summary>
		/// Gets the interval at.
		/// </summary>
		/// <param name="at">At.</param>
		/// <returns></returns>
		public LiveInterval GetIntervalAt(SlotIndex at)
		{
			foreach (var liveInterval in liveIntervals)
			{
				if (liveInterval.Contains(at))
					return liveInterval;
			}

			return null;
		}

		/// <summary>
		/// Gets the interval at or ends at.
		/// </summary>
		/// <param name="at">At.</param>
		/// <returns></returns>
		public LiveInterval GetIntervalAtOrEndsAt(SlotIndex at)
		{
			foreach (var liveInterval in liveIntervals)
			{
				if (liveInterval.Contains(at) || at == liveInterval.End)
					return liveInterval;
			}

			return null;
		}

		public void ReplaceWithSplit(LiveInterval source, IList<LiveInterval> liveIntervals)
		{
			Remove(source);

			foreach (var liveInterval in liveIntervals)
			{
				Add(liveInterval);
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (IsPhysicalRegister)
			{
				return PhysicalRegister.ToString();
			}
			else
			{
				return string.Format("V_{0}", VirtualRegisterOperand.Index);
			}
		}
	}
}