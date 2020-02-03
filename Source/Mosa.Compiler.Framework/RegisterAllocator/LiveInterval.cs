// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class LiveInterval
	{
		public enum AllocationStage
		{
			Initial = 0,
			SplitLevel1 = 1,
			SplitLevel2 = 2,
			SplitLevel3 = 3,
			SplitFinal = 4,
			Max = 5,
		}

		public readonly VirtualRegister VirtualRegister;

		public readonly SlotIndex Start;
		public readonly SlotIndex End;

		public int Length { get { return End - Start; } }

		public readonly LiveRange LiveRange;

		public int StartValue { get { return LiveRange.Start.Value; } }
		public int EndValue { get { return LiveRange.End.Value; } }

		public int SpillValue;

		public int SpillCost { get { return NeverSpill || TooSmallToSplit ? int.MaxValue : (SpillValue / (Length + 1)); } }

		public LiveIntervalTrack LiveIntervalTrack;

		public AllocationStage Stage;

		public bool IsPhysicalRegister { get { return VirtualRegister.IsPhysicalRegister; } }

		public PhysicalRegister AssignedPhysicalRegister { get { return LiveIntervalTrack?.Register; } }

		public Operand AssignedPhysicalOperand;

		public Operand AssignedOperand { get { return (AssignedPhysicalRegister != null) ? AssignedPhysicalOperand : VirtualRegister.SpillSlotOperand; } }

		public bool ForceSpilled;

		public bool NeverSpill;

		public bool TooSmallToSplit { get; }

		#region Short Cuts

		public IEnumerable<SlotIndex> UsePositions { get { return LiveRange.UsePositions; } }

		public IEnumerable<SlotIndex> DefPositions { get { return LiveRange.DefPositions; } }

		public bool IsEmpty { get { return LiveRange.IsEmpty; } }

		public SlotIndex First { get { return LiveRange.First; } }

		public SlotIndex Last { get { return LiveRange.Last; } }

		public bool IsAdjacent(SlotIndex start, SlotIndex end)
		{
			return start == End || end == Start;
		}

		public bool Intersects(SlotIndex start, SlotIndex end)
		{
			return (Start <= start && End > start) || (start <= Start && end > Start);
		}

		public bool Contains(SlotIndex at)
		{
			return at >= Start && at < End;
		}

		public bool IsAdjacent(LiveInterval other)
		{
			return IsAdjacent(other.Start, other.End);
		}

		public bool Intersects(LiveInterval other)
		{
			return Intersects(other.Start, other.End);
		}

		#endregion Short Cuts

		public LiveInterval(VirtualRegister virtualRegister, SlotIndex start, SlotIndex end)
		{
			VirtualRegister = virtualRegister;
			Start = start;
			End = end;

			LiveRange = new LiveRange(start, end, virtualRegister);

			SpillValue = 0;
			Stage = AllocationStage.Initial;
			ForceSpilled = false;
			NeverSpill = false;

			TooSmallToSplit = IsTooSmallToSplit();
		}

		public LiveInterval CreateExpandedLiveInterval(LiveInterval interval)
		{
			Debug.Assert(VirtualRegister == interval.VirtualRegister);

			var start = Start < interval.Start ? Start : interval.Start;
			var end = End > interval.End ? End : interval.End;

			return new LiveInterval(VirtualRegister, start, end);
		}

		public LiveInterval CreateExpandedLiveRange(SlotIndex start, SlotIndex end)
		{
			var mergedStart = Start < start ? Start : start;
			var mergedEnd = End > end ? End : end;

			return new LiveInterval(VirtualRegister, mergedStart, mergedEnd);
		}

		private bool IsTooSmallToSplit()
		{
			if (LiveRange.UseCount == 1)
			{
				var firstUse = LiveRange.FirstUse;

				Debug.Assert(firstUse.IsNotNull);

				if (firstUse.Before == Start && firstUse.After == End)
					return true;
			}

			return false;
		}

		public override string ToString()
		{
			return $"{VirtualRegister} between {LiveRange}";
		}

		public void Evict()
		{
			LiveIntervalTrack.Evict(this);
		}

		private LiveInterval CreateSplit(LiveRange liveRange)
		{
			return new LiveInterval(VirtualRegister, liveRange.Start, liveRange.End);
		}

		public List<LiveInterval> SplitAt(SlotIndex at)
		{
			var liveRanges = LiveRange.SplitAt(at);

			var intervals = new List<LiveInterval>(liveRanges.Count);

			foreach (var liveRange in liveRanges)
			{
				intervals.Add(CreateSplit(liveRange));
			}

			return intervals;
		}

		public List<LiveInterval> SplitAt(SlotIndex low, SlotIndex high)
		{
			var liveRanges = LiveRange.SplitAt(low, high);

			var intervals = new List<LiveInterval>(liveRanges.Count);

			foreach (var liveRange in liveRanges)
			{
				intervals.Add(CreateSplit(liveRange));
			}

			return intervals;
		}
	}
}
