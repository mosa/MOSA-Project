/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public class LiveInterval : LiveRange
	{
		public enum AllocationStage
		{
			Initial = 0,
			SplitLevel1 = 1,
			SplitLevel2 = 2,
			Spillable = 3,
			Max = 4,
		}

		public VirtualRegister VirtualRegister { get; private set; }

		public int SpillValue { get; set; }

		public int SpillCost { get { return NeverSpill ? Int32.MaxValue : (SpillValue / (Length + 1)); } }

		public LiveIntervalTrack LiveIntervalTrack { get; set; }

		public AllocationStage Stage { get; set; }

		public bool IsPhysicalRegister { get { return VirtualRegister.IsPhysicalRegister; } }

		public Register AssignedPhysicalRegister { get { return LiveIntervalTrack == null ? null : LiveIntervalTrack.Register; } }

		public Operand AssignedPhysicalOperand { get; set; }

		public Operand AssignedOperand { get { return (AssignedPhysicalRegister != null) ? AssignedPhysicalOperand : VirtualRegister.SpillSlotOperand; } }

		public bool ForceSpilled { get; set; }

		public bool NeverSpill { get; set; }

		public bool IsSplit { get; set; }

		private LiveInterval(VirtualRegister virtualRegister, SlotIndex start, SlotIndex end, IList<SlotIndex> uses, IList<SlotIndex> defs)
			: base(start, end, uses, defs)
		{
			this.VirtualRegister = virtualRegister;
			this.SpillValue = 0;
			this.Stage = AllocationStage.Initial;
			this.ForceSpilled = false;
			this.NeverSpill = false;
			this.IsSplit = false;
		}

		public LiveInterval(VirtualRegister virtualRegister, SlotIndex start, SlotIndex end)
			: this(virtualRegister, start, end, virtualRegister.UsePositions, virtualRegister.DefPositions)
		{
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

			return new LiveInterval(this.VirtualRegister, mergedStart, mergedEnd);
		}

		public override string ToString()
		{
			return VirtualRegister.ToString() + " between " + base.ToString();
		}

		public void Evict()
		{
			this.LiveIntervalTrack.Evict(this);
		}

		public LiveInterval Split(SlotIndex start, SlotIndex end)
		{
			return new LiveInterval(VirtualRegister, start, end, UsePositions, DefPositions);
		}
	}
}