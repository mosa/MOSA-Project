// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Trace;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	/// <summary>
	///
	/// </summary>
	public class BasicRegisterAllocator : BaseRegisterAllocator
	{
		public BasicRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters compilerVirtualRegisters, StackLayout stackLayout, BaseArchitecture architecture, ITraceFactory traceFactory)
			: base(basicBlocks, compilerVirtualRegisters, stackLayout, architecture, traceFactory)
		{
		}

		protected override void AdditionalSetup()
		{
			//SplitIntervalsAtCallSites();
		}

		private int GetSpillCost(SlotIndex use, int factor)
		{
			return factor * GetLoopDepth(use) * 100;
		}

		protected override void CalculateSpillCost(LiveInterval liveInterval)
		{
			int spillvalue = 0;

			foreach (var use in liveInterval.UsePositions)
			{
				spillvalue += GetSpillCost(use, 100);
			}

			foreach (var use in liveInterval.DefPositions)
			{
				spillvalue += GetSpillCost(use, 115);
			}

			liveInterval.SpillValue = spillvalue;
		}

		protected override int CalculatePriorityValue(LiveInterval liveInterval)
		{
			return liveInterval.Length | ((int)(((int)LiveInterval.AllocationStage.Max - liveInterval.Stage)) << 28);
		}

		protected void SplitIntervalsAtCallSites()
		{
			foreach (var virtualRegister in VirtualRegisters)
			{
				if (virtualRegister.IsPhysicalRegister)
					continue;

				for (int i = 0; i < virtualRegister.LiveIntervals.Count; i++)
				{
					var liveInterval = virtualRegister.LiveIntervals[i];

					if (liveInterval.ForceSpilled)
						continue;

					if (liveInterval.IsEmpty)
						continue;

					var callSite = FindCallSiteInInterval(liveInterval);

					if (callSite == null)
						continue;

					SplitIntervalAtCallSite(liveInterval, callSite);

					i = 0; // list was modified
				}
			}
		}

		protected override bool TrySplitInterval(LiveInterval liveInterval, int level)
		{
			return false;
		}

		protected virtual void SplitIntervalAtCallSite(LiveInterval liveInterval, SlotIndex callSite)
		{
		}
	}
}