// Copyright (c) MOSA Project. Licensed under the New BSD License.

using static Mosa.Compiler.Framework.BaseMethodCompilerStage;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	/// <summary>
	/// Basic Register Allocator
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.RegisterAllocator.BaseRegisterAllocator" />
	public class BasicRegisterAllocator : BaseRegisterAllocator
	{
		public BasicRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters virtualRegisters, BaseArchitecture architecture, AddStackLocalDelegate addStackLocal, Operand stackFrame, CreateTraceHandler createTrace)
			: base(basicBlocks, virtualRegisters, architecture, addStackLocal, stackFrame, createTrace)
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
			var value = liveInterval.Length | ((int)((int)LiveInterval.AllocationStage.Max - liveInterval.Stage) << 20);

			return value;
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

					if (callSite.IsNull)
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
