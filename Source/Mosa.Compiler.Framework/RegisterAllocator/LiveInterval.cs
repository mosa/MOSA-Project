/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
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

	public class LiveInterval : Interval
	{

		public enum AllocationStage
		{
			Initial = 0,
			PreSpill = 1,
			Spillable = 2,

			Max = 2,
		}

		public VirtualRegister VirtualRegister { get; private set; }

		public int SpillCost { get; set; }
		public LiveIntervalUnion LiveIntervalUnion { get; set; }

		public AllocationStage Stage { get; set; }

		public Register Register { get { return this.LiveIntervalUnion.Register; } }

		public LiveInterval(VirtualRegister virtualRegister, SlotIndex start, SlotIndex end)
			: base(start, end)
		{
			this.VirtualRegister = virtualRegister;
			this.SpillCost = 0;
			this.Stage = AllocationStage.Initial;
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
			return VirtualRegister.ToString() + " at " + base.ToString();
		}
	}
}