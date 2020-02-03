// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class VirtualRegister
	{
		public readonly List<SlotIndex> UsePositions;

		public readonly List<SlotIndex> DefPositions;

		public readonly Operand VirtualRegisterOperand;

		public readonly PhysicalRegister PhysicalRegister;

		public bool IsPhysicalRegister { get { return VirtualRegisterOperand == null; } }

		public bool IsVirtualRegister { get { return VirtualRegisterOperand != null; } }

		public List<LiveInterval> LiveIntervals { get; } = new List<LiveInterval>(1);

		public int Count { get { return LiveIntervals.Count; } }

		public LiveInterval LastRange { get { return LiveIntervals.Count == 0 ? null : LiveIntervals[LiveIntervals.Count - 1]; } }

		public LiveInterval FirstRange { get { return LiveIntervals.Count == 0 ? null : LiveIntervals[0]; } set { LiveIntervals[0] = value; } }

		public Operand SpillSlotOperand;

		public bool IsFloatingPoint { get { return VirtualRegisterOperand.IsR; } }

		public bool IsReserved { get; }

		public bool IsSpilled;

		public bool IsUsed { get { return Count != 0; } }

		public VirtualRegister(Operand virtualRegister)
		{
			VirtualRegisterOperand = virtualRegister;
			IsReserved = false;
			IsSpilled = false;

			if (virtualRegister.IsVirtualRegister)
			{
				UsePositions = new List<SlotIndex>(VirtualRegisterOperand.Uses.Count);
				DefPositions = new List<SlotIndex>(VirtualRegisterOperand.Definitions.Count);
			}
		}

		public void UpdatePositions()
		{
			var usePositions = UsePositions;

			foreach (var use in VirtualRegisterOperand.Uses)
			{
				usePositions.AddIfNew(new SlotIndex(use));
			}

			usePositions.Sort();

			var defPositions = DefPositions;
			foreach (var def in VirtualRegisterOperand.Definitions)
			{
				defPositions.AddIfNew(new SlotIndex(def));
			}

			defPositions.Sort();
		}

		public VirtualRegister(PhysicalRegister physicalRegister, bool reserved)
		{
			PhysicalRegister = physicalRegister;
			IsReserved = reserved;
			IsSpilled = false;
		}

		public void Add(LiveInterval liveInterval)
		{
			LiveIntervals.Add(liveInterval);
		}

		public void Remove(LiveInterval liveInterval)
		{
			LiveIntervals.Remove(liveInterval);
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

				if (liveRange.Start == start && liveRange.End == end)
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

				if (liveRange.Start > end)
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
				if (liveInterval.Contains(at) || at == liveInterval.End)
					return liveInterval;
			}

			return null;
		}

		public void ReplaceWithSplit(LiveInterval source, List<LiveInterval> liveIntervals)
		{
			Remove(source);

			foreach (var liveInterval in liveIntervals)
			{
				Add(liveInterval);
			}
		}

		public override string ToString()
		{
			if (IsPhysicalRegister)
			{
				return PhysicalRegister.ToString();
			}
			else
			{
				return $"V_{VirtualRegisterOperand.Index}";
			}
		}
	}
}
