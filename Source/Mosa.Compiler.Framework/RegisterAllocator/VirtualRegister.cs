// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class VirtualRegister // alternative name is LiveInterval
	{
		private readonly SortedList<SlotIndex, SlotIndex> usePositions = new SortedList<SlotIndex, SlotIndex>();

		private readonly SortedList<SlotIndex, SlotIndex> defPositions = new SortedList<SlotIndex, SlotIndex>();

		public Operand VirtualRegisterOperand { get; }

		public PhysicalRegister PhysicalRegister { get; }

		public bool IsPhysicalRegister { get { return VirtualRegisterOperand == null; } }

		public bool IsVirtualRegister { get { return VirtualRegisterOperand != null; } }

		public List<LiveInterval> LiveIntervals { get; } = new List<LiveInterval>(1);

		public int Count { get { return LiveIntervals.Count; } }

		public IList<SlotIndex> UsePositions { get { return usePositions.Keys; } }

		public IList<SlotIndex> DefPositions { get { return defPositions.Keys; } }

		public LiveInterval LastRange { get { return LiveIntervals.Count == 0 ? null : LiveIntervals[LiveIntervals.Count - 1]; } }

		public LiveInterval FirstRange { get { return LiveIntervals.Count == 0 ? null : LiveIntervals[0]; } set { LiveIntervals[0] = value; } }

		public Operand SpillSlotOperand { get; set; }

		public bool IsFloatingPoint { get { return VirtualRegisterOperand.IsR; } }

		public bool IsReserved { get; }

		public bool IsSpilled { get; set; }

		public bool IsUsed { get { return Count != 0; } }

		public VirtualRegister(Operand virtualRegister)
		{
			VirtualRegisterOperand = virtualRegister;
			IsReserved = false;
			IsSpilled = false;
		}

		public VirtualRegister(PhysicalRegister physicalRegister, bool reserved)
		{
			PhysicalRegister = physicalRegister;
			IsReserved = reserved;
			IsSpilled = false;
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
			LiveIntervals.Add(liveInterval);
		}

		public void Remove(LiveInterval liveInterval)
		{
			LiveIntervals.Remove(liveInterval);
		}

		public void AddLiveInterval(SlotInterval interval)
		{
			AddLiveInterval(interval.Start, interval.End);
		}

		public void AddLiveInterval(SlotIndex start, SlotIndex end)
		{
			if (LiveIntervals.Count == 0)
			{
				LiveIntervals.Add(new LiveInterval(this, start, end));
				return;
			}

			for (int i = 0; i < LiveIntervals.Count; i++)
			{
				var liveRange = LiveIntervals[i];

				if (liveRange.StartSlot == start && liveRange.EndSlot == end)
					return;

				if (liveRange.IsAdjacent(start, end) || liveRange.Intersects(start, end))
				{
					liveRange = liveRange.CreateExpandedLiveRange(start, end);
					LiveIntervals[i] = liveRange;

					for (int z = i + 1; z < LiveIntervals.Count; z++)
					{
						var nextLiveRange = LiveIntervals[z];
						if (liveRange.IsAdjacent(nextLiveRange) || liveRange.Intersects(nextLiveRange))
						{
							liveRange = liveRange.CreateExpandedLiveInterval(nextLiveRange);
							LiveIntervals[i] = liveRange;
							LiveIntervals.RemoveAt(z);

							continue;
						}

						return;
					}

					return;
				}

				if (liveRange.StartSlot > end)
				{
					// new range is before the current range (so insert before)
					LiveIntervals.Insert(i, new LiveInterval(this, start, end));
					return;
				}
			}

			// new range is after the last range
			LiveIntervals.Add(new LiveInterval(this, start, end));
		}

		/// <summary>
		/// Gets the interval at.
		/// </summary>
		/// <param name="at">At.</param>
		/// <returns></returns>
		public LiveInterval GetIntervalAt(SlotIndex at)
		{
			foreach (var liveInterval in LiveIntervals)
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
			foreach (var liveInterval in LiveIntervals)
			{
				if (liveInterval.Contains(at) || at == liveInterval.EndSlot)
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
