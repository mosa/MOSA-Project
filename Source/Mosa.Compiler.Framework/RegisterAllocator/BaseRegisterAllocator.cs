// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Common;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using Priority_Queue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static Mosa.Compiler.Framework.BaseMethodCompilerStage;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	/// <summary>
	/// Base Register Allocator
	/// </summary>
	public abstract class BaseRegisterAllocator
	{
		public delegate Operand AddStackLocalDelegate(MosaType type);

		protected readonly BasicBlocks BasicBlocks;
		protected readonly BaseArchitecture Architecture;
		protected readonly AddStackLocalDelegate AddStackLocal;
		protected readonly Operand StackFrame;

		private readonly int VirtualRegisterCount;
		private readonly int PhysicalRegisterCount;
		private readonly int RegisterCount;

		protected readonly List<ExtendedBlock> ExtendedBlocks;
		protected readonly List<VirtualRegister> VirtualRegisters;

		private readonly SimplePriorityQueue<LiveInterval, int> PriorityQueue;
		protected readonly List<LiveIntervalTrack> LiveIntervalTracks;

		private readonly PhysicalRegister StackFrameRegister;
		private readonly PhysicalRegister StackPointerRegister;
		private readonly PhysicalRegister ProgramCounter;

		private readonly List<LiveInterval> SpilledIntervals;

		private readonly List<SlotIndex> KillAll;

		protected readonly CreateTraceHandler CreateTrace;

		protected readonly TraceLog Trace;

		private readonly List<InstructionNode> SlotsToNodes;

		protected BaseRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters virtualRegisters, BaseArchitecture architecture, AddStackLocalDelegate addStackLocal, Operand stackFrame, CreateTraceHandler createTrace)
		{
			CreateTrace = createTrace;

			BasicBlocks = basicBlocks;
			Architecture = architecture;
			AddStackLocal = addStackLocal;
			StackFrame = stackFrame;

			VirtualRegisterCount = virtualRegisters.Count;
			PhysicalRegisterCount = architecture.RegisterSet.Length;
			RegisterCount = VirtualRegisterCount + PhysicalRegisterCount;

			LiveIntervalTracks = new List<LiveIntervalTrack>(PhysicalRegisterCount);
			VirtualRegisters = new List<VirtualRegister>(RegisterCount);
			ExtendedBlocks = new List<ExtendedBlock>(basicBlocks.Count);

			Trace = CreateTrace("Main", 7);

			StackFrameRegister = architecture.StackFrameRegister;
			StackPointerRegister = architecture.StackPointerRegister;
			ProgramCounter = architecture.ProgramCounter;

			// Setup extended physical registers
			foreach (var physicalRegister in architecture.RegisterSet)
			{
				bool reserved = (physicalRegister == StackFrameRegister
					|| physicalRegister == StackPointerRegister
					|| (ProgramCounter != null && physicalRegister == ProgramCounter));

				VirtualRegisters.Add(new VirtualRegister(physicalRegister, reserved));
				LiveIntervalTracks.Add(new LiveIntervalTrack(physicalRegister, reserved));
			}

			// Setup extended virtual registers
			foreach (var virtualRegister in virtualRegisters)
			{
				Debug.Assert(virtualRegister.Index == VirtualRegisters.Count - PhysicalRegisterCount + 1);

				VirtualRegisters.Add(new VirtualRegister(virtualRegister));
			}

			PriorityQueue = new SimplePriorityQueue<LiveInterval, int>();
			SpilledIntervals = new List<LiveInterval>();

			KillAll = new List<SlotIndex>();

			SlotsToNodes = new List<InstructionNode>(512);
		}

		public virtual void Start()
		{
			// Order all the blocks
			CreateExtendedBlocks();

			// Number all the instructions in block order
			NumberInstructions();

			// Update virtual register with index numbers
			UpdateVirtualRegisterPositions();

			// Generate trace information for instruction numbering
			TraceNumberInstructions();
			TraceDefAndUseLocations();

			// Computer local live sets
			ComputeLocalLiveSets();

			// Computer global live sets
			ComputeGlobalLiveSets();

			// Build the live intervals
			BuildLiveIntervals();

			// Generate trace information for blocks
			//TraceBlocks();

			TraceUsageMap("Initial");

			// Generate trace information for live intervals
			TraceLiveIntervals("InitialLiveIntervals", false);

			AdditionalSetup();

			// Calculate spill costs for live intervals
			AssignSpillCosts();

			// Populate priority queue
			PopulatePriorityQueue();

			// Process priority queue
			ProcessPriorityQueue();

			// Create spill slots operands
			CreateSpillSlotOperands();

			// Create physical register operands
			CreatePhysicalRegisterOperands();

			TraceUsageMap("Post");

			// Assign physical registers
			AssignRegisters();

			// Insert register moves
			InsertRegisterMoves();

			// Insert spill moves
			InsertSpillMoves();

			// Resolve data flow
			ResolveDataFlow();

			// Generate trace information for live intervals
			TraceLiveIntervals("PostLiveIntervals", true);
		}

		private void UpdateVirtualRegisterPositions()
		{
			foreach (var register in VirtualRegisters)
			{
				if (register.IsVirtualRegister)
				{
					register.UpdatePositions();
				}
			}
		}

		protected abstract void AdditionalSetup();

		protected InstructionNode GetNode(SlotIndex slot)
		{
			return SlotsToNodes[slot.Index >> 1];
		}

		private void TraceBlocks()
		{
			var extendedBlockTrace = CreateTrace("Extended Blocks", 9);

			if (extendedBlockTrace == null)
				return;

			foreach (var block in ExtendedBlocks)
			{
				extendedBlockTrace.Log($"Block # {block.BasicBlock} [{block.BasicBlock.Sequence.ToString()}] ({block.Start} destination {block.End})");
				extendedBlockTrace.Log($" LiveIn:   {block.LiveIn.ToString2()}");
				extendedBlockTrace.Log($" LiveGen:  {block.LiveGen.ToString2()}");
				extendedBlockTrace.Log($" LiveKill: {block.LiveKill.ToString2()}");
				extendedBlockTrace.Log($" LiveOut:  {block.LiveOut.ToString2()}");
			}
		}

		private void TraceLiveIntervals(string stage, bool operand)
		{
			var registerTrace = CreateTrace(stage, 9);

			if (registerTrace == null)
				return;

			foreach (var virtualRegister in VirtualRegisters)
			{
				if (virtualRegister.IsPhysicalRegister)
				{
					registerTrace.Log($"Physical Register # {virtualRegister.PhysicalRegister}");
				}
				else
				{
					registerTrace.Log($"Virtual Register # {virtualRegister.VirtualRegisterOperand.Index}");
				}

				registerTrace.Log($"Live Intervals ({virtualRegister.LiveIntervals.Count}): {LiveIntervalsToString(virtualRegister.LiveIntervals, operand)}");

				if (virtualRegister.IsVirtualRegister)
				{
					registerTrace.Log($"Def Positions ({virtualRegister.DefPositions.Count}): {SlotsToString(virtualRegister.DefPositions)}");
					registerTrace.Log($"Use Positions ({virtualRegister.UsePositions.Count}): {SlotsToString(virtualRegister.UsePositions)}");
				}
			}
		}

		private String LiveIntervalsToString(List<LiveInterval> liveIntervals)
		{
			return LiveIntervalsToString(liveIntervals, false);
		}

		private String LiveIntervalsToString(List<LiveInterval> liveIntervals, bool operand)
		{
			if (liveIntervals.Count == 0)
				return string.Empty;

			var sb = new StringBuilder();

			foreach (var liveInterval in liveIntervals)
			{
				if (operand && !liveInterval.IsPhysicalRegister)
					sb.Append("[").Append(liveInterval.Start).Append(",").Append(liveInterval.End).Append("]/").Append(liveInterval.AssignedOperand).Append(",");
				else
					sb.Append("[").Append(liveInterval.Start).Append(",").Append(liveInterval.End).Append("],");
			}

			if (sb[sb.Length - 1] == ',')
				sb.Length--;

			return sb.ToString();
		}

		protected string SlotsToString(IEnumerable<SlotIndex> slots)
		{
			var sb = new StringBuilder();

			foreach (var use in slots)
			{
				sb.Append(use);
				sb.Append(',');
			}

			if (sb.Length == 0)
				return string.Empty;

			if (sb[sb.Length - 1] == ',')
				sb.Length--;

			return sb.ToString();
		}

		protected int GetIndex(Operand operand)
		{
			//FUTURE: Make private by refactoring
			return (operand.IsCPURegister) ? (operand.Register.Index) : (operand.Index + PhysicalRegisterCount - 1);
		}

		private void CreateExtendedBlocks()
		{
			var blockOrder = new LoopAwareBlockOrder(BasicBlocks);

			// The re-ordering is not strictly necessary; however, it reduces "holes" in live ranges.
			// And less "holes" improves the readability of the debug logs.
			//basicBlocks.ReorderBlocks(loopAwareBlockOrder.NewBlockOrder);

			// Allocate and setup extended blocks
			for (int i = 0; i < BasicBlocks.Count; i++)
			{
				ExtendedBlocks.Add(new ExtendedBlock(BasicBlocks[i], RegisterCount, blockOrder.GetLoopDepth(BasicBlocks[i])));
			}
		}

		protected void AddSpilledInterval(LiveInterval liveInterval)
		{
			SpilledIntervals.Add(liveInterval);
		}

		public void NumberInstructions(BasicBlocks basicBlocks)
		{
			SlotsToNodes.Add(null);

			foreach (var block in basicBlocks)
			{
				for (var node = block.First; ; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					node.Offset = SlotsToNodes.Count << 1;

					SlotsToNodes.Add(node);

					if (node.IsBlockEndInstruction)
						break;
				}

				Debug.Assert(block.Last.Offset != 0);
			}
		}

		private void NumberInstructions()
		{
			NumberInstructions(BasicBlocks);

			foreach (var block in BasicBlocks)
			{
				ExtendedBlocks[block.Sequence].Start = new SlotIndex(block.First);
				ExtendedBlocks[block.Sequence].End = new SlotIndex(block.Last);
			}
		}

		private void TraceNumberInstructions()
		{
			var numberTrace = CreateTrace("InstructionNumber", 9);

			if (numberTrace == null)
				return;

			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; ; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					string log = node.Offset.ToString() + " = " + node;

					if (node.IsBlockStartInstruction)
					{
						log = log + " # " + block;
					}

					numberTrace.Log(log);

					if (node.IsBlockEndInstruction)
						break;
				}
			}
		}

		private void TraceDefAndUseLocations()
		{
			var locationTrace = CreateTrace("DefAndUseLocations", 9);

			if (locationTrace == null)
				return;

			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; ; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					var visitor = new OperandVisitor(node);
					var sb = new StringBuilder();

					var def = new List<Operand>();
					var use = new List<Operand>();

					foreach (var op in visitor.Output)
					{
						def.AddIfNew(op);
					}

					foreach (var op in visitor.Input)
					{
						use.AddIfNew(op);
					}

					//if (output.Count == 0 && input.Count == 0)
					//	continue;

					sb.Append(node.Offset.ToString());
					sb.Append(" - ");

					if (def.Count > 0)
					{
						sb.Append("DEF: ");
						foreach (var op in def)
						{
							sb.Append(op.ToString(false));
							sb.Append(' ');
						}
					}

					if (use.Count > 0)
					{
						sb.Append("USE: ");
						foreach (var op in use)
						{
							sb.Append(op.ToString(false));
							sb.Append(' ');
						}
					}

					locationTrace.Log(sb.ToString());

					if (node.IsBlockEndInstruction)
						break;
				}
			}
		}

		private void TraceUsageMap(string stage)
		{
			var usageMap = CreateTrace("TraceUsageMap-" + stage, 9);

			if (usageMap == null)
				return;

			var map = new Dictionary<int, string[]>();
			var slots = new List<int>();
			var blockStarts = new List<int>();
			var blockEnds = new List<int>();

			var header = new StringBuilder();
			header.Append('\t');

			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; ; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					var slot = new SlotIndex(node);

					if (node.IsBlockStartInstruction)
					{
						blockStarts.Add(slot.Value);
					}
					else if (node.IsBlockEndInstruction)
					{
						blockEnds.Add(slot.Value);
					}

					for (int step = 0; step < 2; step++)
					{
						var innerslot = (step == 0) ? slot.Before : slot;

						var row = new string[RegisterCount];
						map.Add(innerslot.Value, row);

						header.Append(innerslot.Value.ToString());
						header.Append("\t");

						slots.Add(innerslot.Value);

						if (!innerslot.IsOnSlot)
						{
							var visitor = new OperandVisitor(node);

							foreach (var op in visitor.Output)
							{
								if (op.IsCPURegister && op.Register.IsSpecial)
									continue;

								var s = row[GetIndex(op)] ?? string.Empty;

								row[GetIndex(op)] = s + "+D";
							}

							foreach (var op in visitor.Input)
							{
								if (op.IsCPURegister && op.Register.IsSpecial)
									continue;

								var s = row[GetIndex(op)] ?? string.Empty;

								row[GetIndex(op)] = s + "+U";
							}
						}

						for (int index = 0; index < RegisterCount; index++)
						{
							var vr = VirtualRegisters[index];

							foreach (var r in vr.LiveIntervals)
							{
								if (r.Contains(innerslot))
								{
									var s = row[index] ?? string.Empty;

									if (vr.IsPhysicalRegister)
									{
										s = "X";
									}
									else if (r.AssignedPhysicalRegister == null)
									{
										if (vr.SpillSlotOperand == null)
											s = "x";
										else
											s = "T_" + vr.SpillSlotOperand.Index.ToString(); //vr.SpillSlotOperand.ToString(false);
									}
									else
									{
										s = r.AssignedPhysicalRegister + s;
									}

									if (r.Start == innerslot)
										s = "(" + s;

									row[index] = s;

									if (!vr.IsPhysicalRegister)
									{
										if (r.AssignedPhysicalRegister != null)
										{
											int index2 = r.AssignedPhysicalRegister.Index;

											var s2 = row[index2] ?? string.Empty;

											s2 += vr.ToString();

											row[index2] = s2;
										}
									}
								}
								else if (r.End == innerslot)
								{
									var s = row[index] ?? string.Empty;
									s += ")";
									row[index] = s;
								}
							}
						}
					}

					if (node.IsBlockEndInstruction)
						break;
				}
			}

			usageMap.Log(header.ToString());

			for (int index = 0; index < RegisterCount; index++)
			{
				var vr = VirtualRegisters[index];
				if (vr.LiveIntervals.Count == 0)
					continue;

				var sb = new StringBuilder();
				sb.Append(vr.ToString());
				sb.Append('\t');

				foreach (var s in slots)
				{
					var row = map[s];
					var value = row[index];

					if (blockStarts.Contains(s))
						sb.Append("|");

					sb.Append(value);

					if (blockEnds.Contains(s))
						sb.Append("!");

					sb.Append('\t');
				}

				usageMap.Log(sb.ToString());
			}
		}

		private void ComputeLocalLiveSets()
		{
			var liveSetTrace = CreateTrace("ComputeLocalLiveSets", 9);

			foreach (var block in ExtendedBlocks)
			{
				liveSetTrace?.Log($"Block # {block.BasicBlock.Sequence}");

				var liveGen = new BitArray(RegisterCount, false);
				var liveKill = new BitArray(RegisterCount, false);

				liveGen.Set(StackFrameRegister.Index, true);
				liveGen.Set(StackPointerRegister.Index, true);

				if (ProgramCounter != null)
				{
					liveGen.Set(ProgramCounter.Index, true);
				}

				if (block.BasicBlock.IsHeadBlock)
				{
					for (int s = 0; s < PhysicalRegisterCount; s++)
					{
						liveKill.Set(s, true);
					}

					liveSetTrace?.Log("KILL ALL PHYSICAL");
				}

				for (var node = block.BasicBlock.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					liveSetTrace?.Log(node.ToString());

					var visitor = new OperandVisitor(node);

					foreach (var op in visitor.Input)
					{
						if (op.IsCPURegister && op.Register.IsSpecial)
							continue;

						liveSetTrace?.Log($"INPUT:  {op}");

						int index = GetIndex(op);
						if (!liveKill.Get(index))
						{
							liveGen.Set(index, true);

							liveSetTrace?.Log($"GEN:  {index} {op}");
						}
					}

					if (node.Instruction.FlowControl == FlowControl.Call || node.Instruction == IRInstruction.KillAll)
					{
						for (int reg = 0; reg < PhysicalRegisterCount; reg++)
						{
							liveKill.Set(reg, true);
						}

						liveSetTrace?.Log("KILL ALL PHYSICAL");
					}
					else if (node.Instruction == IRInstruction.KillAllExcept)
					{
						var except = node.Operand1.Register.Index;

						for (int reg = 0; reg < PhysicalRegisterCount; reg++)
						{
							if (reg != except)
							{
								liveKill.Set(reg, true);
							}
						}

						liveSetTrace?.Log($"KILL EXCEPT PHYSICAL: {node.Operand1}");
					}

					foreach (var op in visitor.Output)
					{
						if (op.IsCPURegister && op.Register.IsSpecial)
							continue;

						liveSetTrace?.Log($"OUTPUT: {op}");

						int index = GetIndex(op);
						liveKill.Set(index, true);

						liveSetTrace?.Log($"KILL: {index} {op}");
					}
				}

				block.LiveGen = liveGen;
				block.LiveKill = liveKill;
				block.LiveKillNot = new BitArray(liveKill).Not();

				liveSetTrace?.Log($"GEN:     {block.LiveGen.ToString2()}");
				liveSetTrace?.Log($"KILL:    {block.LiveKill.ToString2()}");
				liveSetTrace?.Log($"KILLNOT: {block.LiveKillNot.ToString2()}");
				liveSetTrace?.Log();
			}
		}

		private void ComputeGlobalLiveSets()
		{
			bool changed = true;
			while (changed)
			{
				changed = false;

				for (int i = BasicBlocks.Count - 1; i >= 0; i--)
				{
					var block = ExtendedBlocks[i];

					var liveOut = new BitArray(RegisterCount);

					foreach (var next in block.BasicBlock.NextBlocks)
					{
						liveOut.Or(ExtendedBlocks[next.Sequence].LiveIn);
					}

					var liveIn = new BitArray(block.LiveOut);

					if (block.LiveKillNot != null)
						liveIn.And(block.LiveKillNot);
					else
						liveIn.SetAll(false);

					liveIn.Or(block.LiveGen);

					// compare them for any changes
					if (!changed)
					{
						if (!block.LiveOut.AreSame(liveOut) || !block.LiveIn.AreSame(liveIn))
						{
							changed = true;
						}
					}

					block.LiveOut = liveOut;
					block.LiveIn = liveIn;
				}
			}
		}

		private void BuildLiveIntervals()
		{
			var intervalTrace = CreateTrace("BuildLiveIntervals", 9);

			for (int b = BasicBlocks.Count - 1; b >= 0; b--)
			{
				var block = ExtendedBlocks[b];

				intervalTrace?.Log($"Block # {block.BasicBlock.Sequence}");

				for (int r = 0; r < RegisterCount; r++)
				{
					if (!block.LiveOut.Get(r))
						continue;

					var register = VirtualRegisters[r];

					if (b + 1 != BasicBlocks.Count && ExtendedBlocks[b + 1].LiveIn.Get(r))
					{
						intervalTrace?.Log($"Add (LiveOut) {register} : {block.Start} destination {ExtendedBlocks[b + 1].Start}");
						intervalTrace?.Log($"   Before: {LiveIntervalsToString(register.LiveIntervals)}");
						register.AddLiveInterval(block.Start, ExtendedBlocks[b + 1].Start);
						intervalTrace?.Log($"    After: {LiveIntervalsToString(register.LiveIntervals)}");
					}
					else
					{
						intervalTrace?.Log($"Add (!LiveOut) {register} : {block.Start} destination {block.End}");
						intervalTrace?.Log($"   Before: {LiveIntervalsToString(register.LiveIntervals)}");
						register.AddLiveInterval(block.Start, block.End);
						intervalTrace?.Log($"    After: {LiveIntervalsToString(register.LiveIntervals)}");
					}
				}

				for (var node = block.BasicBlock.Last; !node.IsBlockStartInstruction; node = node.Previous)
				{
					if (node.IsEmpty)
						continue;

					var slot = new SlotIndex(node);
					var slotAfter = slot.After;

					if (node.Instruction.FlowControl == FlowControl.Call || node.Instruction == IRInstruction.KillAll)
					{
						for (int s = 0; s < PhysicalRegisterCount; s++)
						{
							var register = VirtualRegisters[s];

							intervalTrace?.Log($"Add (Call) {register} : {slot} destination {slotAfter}");
							intervalTrace?.Log($"   Before: {LiveIntervalsToString(register.LiveIntervals)}");
							register.AddLiveInterval(slot, slotAfter);
							intervalTrace?.Log($"    After: {LiveIntervalsToString(register.LiveIntervals)}");
						}

						KillAll.Add(slot);
					}
					else if (node.Instruction == IRInstruction.KillAllExcept)
					{
						for (int s = 0; s < PhysicalRegisterCount; s++)
						{
							var except = node.Operand1.Register.Index;

							if (s == except)
								continue;

							var register = VirtualRegisters[s];

							intervalTrace?.Log($"Add (Call) {register} : {slot} destination {slotAfter}");
							intervalTrace?.Log($"   Before: {LiveIntervalsToString(register.LiveIntervals)}");
							register.AddLiveInterval(slot, slotAfter);
							intervalTrace?.Log($"    After: {LiveIntervalsToString(register.LiveIntervals)}");
						}
					}

					var visitor = new OperandVisitor(node);

					foreach (var result in visitor.Output)
					{
						if (result.IsCPURegister && result.Register.IsSpecial)
							continue;

						var register = VirtualRegisters[GetIndex(result)];

						if (register.IsReserved)
							continue;

						var first = register.FirstRange;

						if (first != null)
						{
							intervalTrace?.Log($"Replace First {register} : {slot} destination {first.End}");
							intervalTrace?.Log($"   Before: {LiveIntervalsToString(register.LiveIntervals)}");
							register.FirstRange = new LiveInterval(register, slot, first.End);
							intervalTrace?.Log($"    After: {LiveIntervalsToString(register.LiveIntervals)}");
						}
						else
						{
							// This is necessary to handled a result that is never used!
							// This is common with instructions with more than one result.
							intervalTrace?.Log($"Add (Unused) {register} : {slot} destination {slotAfter}");
							intervalTrace?.Log($"   Before: {LiveIntervalsToString(register.LiveIntervals)}");
							register.AddLiveInterval(slot, slotAfter);
							intervalTrace?.Log($"    After: {LiveIntervalsToString(register.LiveIntervals)}");
						}
					}

					foreach (var result in visitor.Input)
					{
						if (result.IsCPURegister && result.Register.IsSpecial)
							continue;

						var register = VirtualRegisters[GetIndex(result)];

						if (register.IsReserved)
							continue;

						intervalTrace?.Log($"Add (normal) {register} : {block.Start} destination {slot}");
						intervalTrace?.Log($"   Before: {LiveIntervalsToString(register.LiveIntervals)}");
						register.AddLiveInterval(block.Start, slot);
						intervalTrace?.Log($"    After: {LiveIntervalsToString(register.LiveIntervals)}");
					}
				}
			}

			KillAll.Sort();
		}

		private ExtendedBlock GetContainingBlock(SlotIndex slot)
		{
			var node = GetNode(slot);
			var block = ExtendedBlocks[node.Block.Sequence];
			return block;
		}

		protected int GetLoopDepth(SlotIndex slot)
		{
			return GetContainingBlock(slot).LoopDepth + 1;
		}

		protected SlotIndex GetBlockEnd(SlotIndex slot)
		{
			return GetContainingBlock(slot).End;
		}

		protected SlotIndex GetBlockStart(SlotIndex slot)
		{
			return GetContainingBlock(slot).Start;
		}

		protected void CalculateSpillCosts(List<LiveInterval> liveIntervals)
		{
			foreach (var liveInterval in liveIntervals)
			{
				CalculateSpillCost(liveInterval);
			}
		}

		protected abstract void CalculateSpillCost(LiveInterval liveInterval);

		private void AssignSpillCosts()
		{
			foreach (var virtualRegister in VirtualRegisters)
			{
				foreach (var liveInterval in virtualRegister.LiveIntervals)
				{
					// Skip adding live intervals for physical registers to priority queue
					if (liveInterval.VirtualRegister.IsPhysicalRegister)
					{
						// fixed and not spillable
						liveInterval.NeverSpill = true;
					}

					//else if (liveInterval.VirtualRegister.VirtualRegisterOperand.Type.IsUserValueType)
					//{
					//	// Value types will never be spilled (they are already allocated in stack anyways)
					//	liveInterval.NeverSpill = true;
					//}
					else
					{
						// Calculate spill costs for live interval
						CalculateSpillCost(liveInterval);
					}
				}
			}
		}

		protected abstract int CalculatePriorityValue(LiveInterval liveInterval);

		protected void AddPriorityQueue(List<LiveInterval> liveIntervals)
		{
			foreach (var liveInterval in liveIntervals)
			{
				AddPriorityQueue(liveInterval);
			}
		}

		protected void AddPriorityQueue(LiveInterval liveInterval)
		{
			//Debug.Assert(!liveInterval.IsSplit);
			Debug.Assert(liveInterval.LiveIntervalTrack == null);

			// priority is based on allocation stage (primary, lower first) and interval size (secondary, higher first)
			int value = CalculatePriorityValue(liveInterval);

			PriorityQueue.Enqueue(liveInterval, int.MaxValue - value);
		}

		protected virtual void PopulatePriorityQueue()
		{
			foreach (var virtualRegister in VirtualRegisters)
			{
				foreach (var liveInterval in virtualRegister.LiveIntervals)
				{
					// Skip adding live intervals for physical registers to priority queue
					if (liveInterval.VirtualRegister.IsPhysicalRegister)
					{
						LiveIntervalTracks[liveInterval.VirtualRegister.PhysicalRegister.Index].Add(liveInterval);

						continue;
					}

					liveInterval.Stage = LiveInterval.AllocationStage.Initial;

					// Add live intervals for virtual registers to priority queue
					AddPriorityQueue(liveInterval);
				}
			}
		}

		private void ProcessPriorityQueue()
		{
			while (PriorityQueue.Count != 0)
			{
				var liveInterval = PriorityQueue.Dequeue();

				ProcessLiveInterval(liveInterval);
			}
		}

		protected bool PlaceLiveIntervalOnTrack(LiveInterval liveInterval, LiveIntervalTrack track)
		{
			if (track.IsReserved)
				return false;

			if (track.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
				return false;

			if (track.Intersects(liveInterval))
				return false;

			Trace?.Log($"  Assigned live interval destination: {track}");

			track.Add(liveInterval);

			//Trace?.Log("    Track: " + track.ToString2());

			return true;
		}

		protected bool PlaceLiveIntervalOnAnyAvailableTrack(LiveInterval liveInterval)
		{
			foreach (var track in LiveIntervalTracks)
			{
				if (PlaceLiveIntervalOnTrack(liveInterval, track))
				{
					return true;
				}
			}

			return false;
		}

		protected bool PlaceLiveIntervalOnTrackAllowEvictions(LiveInterval liveInterval)
		{
			// find live interval(s) to evict based on spill costs
			foreach (var track in LiveIntervalTracks)
			{
				if (track.IsReserved)
					continue;

				if (track.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
					continue;

				var intersections = track.GetIntersections(liveInterval);

				bool evict = true;

				foreach (var intersection in intersections)
				{
					if (intersection.SpillCost >= liveInterval.SpillCost || intersection.SpillCost == int.MaxValue || intersection.VirtualRegister.IsPhysicalRegister || intersection.IsPhysicalRegister)
					{
						evict = false;
						break;
					}
				}

				if (evict)
				{
					if (intersections.Count != 0)
					{
						Trace?.Log("  Evicting live intervals");

						track.Evict(intersections);

						foreach (var intersection in intersections)
						{
							Trace?.Log($"  Evicted: {intersection}");

							liveInterval.Stage = LiveInterval.AllocationStage.Initial;
							AddPriorityQueue(intersection);
						}
					}

					track.Add(liveInterval);

					Trace?.Log($"  Assigned live interval destination: {track}");

					return true;
				}
			}

			return false;
		}

		protected virtual bool PlaceLiveInterval(LiveInterval liveInterval)
		{
			// For now, empty intervals will stay spilled
			if (liveInterval.IsEmpty)
			{
				Trace?.Log("  Spilled");

				liveInterval.VirtualRegister.IsSpilled = true;
				AddSpilledInterval(liveInterval);

				return true;
			}

			// Find any available track and place interval there
			if (PlaceLiveIntervalOnAnyAvailableTrack(liveInterval))
			{
				return true;
			}

			Trace?.Log("  No free register available");

			// No place for live interval; find live interval(s) to evict based on spill costs
			return PlaceLiveIntervalOnTrackAllowEvictions(liveInterval);
		}

		protected void ProcessLiveInterval(LiveInterval liveInterval)
		{
			Debug.Assert(liveInterval.LiveIntervalTrack == null);

			//Debug.Assert(!liveInterval.IsSplit);

			Trace?.Log($"Processing Interval: {liveInterval} / Length: {liveInterval.Length} / Spill Cost: {liveInterval.SpillCost} / Stage: {liveInterval.Stage.ToString()}");
			Trace?.Log($"  Defs ({liveInterval.LiveRange.DefCount}): {SlotsToString(liveInterval.DefPositions)}");
			Trace?.Log($"  Uses ({liveInterval.LiveRange.UseCount}): {SlotsToString(liveInterval.UsePositions)}");

			if (PlaceLiveInterval(liveInterval))
			{
				return;
			}

			// No live intervals to evict!
			Trace?.Log("  No live intervals to evict");

			if (Trace != null)
			{
				foreach (var track in LiveIntervalTracks)
				{
					if (track.IsReserved)
						continue;

					if (track.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
						continue;

					var assignedList = track.GetIntersections(liveInterval);

					if (assignedList == null)
						continue;

					foreach (var assigned in assignedList)
					{
						Trace.Log($"     Track: {track} Assigned: {assigned} / Spill Cost: {assigned.SpillCost}");
					}
				}
			}

			// prepare to split live interval
			if (liveInterval.Stage == LiveInterval.AllocationStage.Initial)
			{
				Trace?.Log("  Re-queued for split level 1 stage");
				liveInterval.Stage = LiveInterval.AllocationStage.SplitLevel1;
				AddPriorityQueue(liveInterval);
				return;
			}

			// split live interval - level 1
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitLevel1)
			{
				Trace?.Log("  Attempting destination split interval - level 1");

				if (TrySplitInterval(liveInterval, 1))
				{
					return;
				}

				Trace?.Log("  Re-queued for split interval - level 2");

				liveInterval.Stage = LiveInterval.AllocationStage.SplitLevel2;
				AddPriorityQueue(liveInterval);
				return;
			}

			// split live interval - level 2
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitLevel2)
			{
				Trace?.Log("  Attempting destination split interval - level 2");

				if (TrySplitInterval(liveInterval, 2))
				{
					return;
				}

				Trace?.Log("  Re-queued for split interval - level 3");

				liveInterval.Stage = LiveInterval.AllocationStage.SplitLevel3;
				AddPriorityQueue(liveInterval);
				return;
			}

			// split live interval - level 3
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitLevel3)
			{
				Trace?.Log("  Attempting destination split interval - level 3");

				if (TrySplitInterval(liveInterval, 3))
				{
					return;
				}

				// Move to final split option stage
				Trace?.Log("  Re-queued for spillable stage");

				liveInterval.Stage = LiveInterval.AllocationStage.SplitFinal;
				AddPriorityQueue(liveInterval);
				return;
			}

			// Final split option
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitFinal)
			{
				Trace?.Log("  Final split option");

				SplitLastResort(liveInterval);
				return;
			}

			return;
		}

		protected abstract bool TrySplitInterval(LiveInterval liveInterval, int level);

		private void SplitLastResort(LiveInterval liveInterval)
		{
			// This is the last option when all other split options fail.
			// Split interval up into very small pieces that can always be placed.

			// 1. Find the first use or def
			// 2. Determine split location
			// 3. Split the range into two (or maybe more ranges)

			if (liveInterval.IsEmpty)
			{
				SpilledIntervals.Add(liveInterval);
				return;
			}

			Trace?.Log(" Splitting around first use/def");

			var liveRange = liveInterval.LiveRange;

			SlotIndex splitAt;

			if (liveRange.IsDefFirst)
			{
				if (liveRange.FirstDef == liveRange.Start || liveRange.FirstDef.Before == liveRange.Start)
				{
					// must split after def
					splitAt = liveRange.FirstDef.After;
				}
				else
				{
					// split before def
					splitAt = liveRange.FirstDef.Before;
				}
			}
			else
			{
				Debug.Assert(liveRange.FirstUse != liveRange.Start);

				if (liveRange.FirstUse == liveRange.End)
				{
					// must split before use
					splitAt = liveRange.FirstUse.Before;
				}
				else if (liveRange.FirstUse.Before == liveRange.Start)
				{
					splitAt = liveRange.FirstUse.After;
				}
				else
				{
					splitAt = liveRange.FirstUse.Before;
				}
			}

			var intervals = liveInterval.SplitAt(splitAt);

			ReplaceIntervals(liveInterval, intervals, true);
		}

		protected void ReplaceIntervals(LiveInterval replaceLiveInterval, List<LiveInterval> newIntervals, bool addToQueue)
		{
			CalculateSpillCosts(newIntervals);

			replaceLiveInterval.VirtualRegister.ReplaceWithSplit(replaceLiveInterval, newIntervals);

			if (Trace != null)
			{
				foreach (var interval in newIntervals)
				{
					Trace.Log($" New Split: {interval}");
				}
			}

			if (addToQueue)
			{
				AddPriorityQueue(newIntervals);
			}
		}

		private IEnumerable<VirtualRegister> GetVirtualRegisters(BitArray array)
		{
			for (int i = 0; i < array.Count; i++)
			{
				if (array.Get(i))
				{
					var virtualRegister = VirtualRegisters[i];

					if (!virtualRegister.IsPhysicalRegister)
					{
						yield return virtualRegister;
					}
				}
			}
		}

		protected SlotIndex FindCallSiteInInterval(LiveInterval liveInterval)
		{
			// FUTURE: Optimization - KillAll is sorted!
			foreach (var slot in KillAll)
			{
				if (liveInterval.Contains(slot))
				{
					return slot;
				}
			}
			return SlotIndex.NullSlot;
		}

		protected void CreateSpillSlotOperands()
		{
			foreach (var register in VirtualRegisters)
			{
				if (!register.IsSpilled)
					continue;

				Debug.Assert(register.IsVirtualRegister);
				register.SpillSlotOperand = AddStackLocal(register.VirtualRegisterOperand.Type);
			}
		}

		protected void CreatePhysicalRegisterOperands()
		{
			foreach (var register in VirtualRegisters)
			{
				if (!register.IsUsed || register.IsPhysicalRegister)
					continue;

				foreach (var liveInterval in register.LiveIntervals)
				{
					if (liveInterval.AssignedPhysicalRegister == null)
						continue;

					liveInterval.AssignedPhysicalOperand = Operand.CreateCPURegister(liveInterval.VirtualRegister.VirtualRegisterOperand.Type, liveInterval.AssignedPhysicalRegister);
				}
			}
		}

		protected void InsertSpillMoves()
		{
			foreach (var register in VirtualRegisters)
			{
				if (!register.IsUsed || register.IsPhysicalRegister || !register.IsSpilled)
					continue;

				foreach (var liveInterval in register.LiveIntervals)
				{
					foreach (var def in liveInterval.DefPositions)
					{
						var context = new Context(GetNode(def));

						Architecture.InsertStoreInstruction(context, StackFrame, register.SpillSlotOperand, liveInterval.AssignedPhysicalOperand);

						context.Marked = true;
					}
				}
			}
		}

		protected void AssignRegisters()
		{
			foreach (var register in VirtualRegisters)
			{
				if (!register.IsUsed || register.IsPhysicalRegister)
					continue;

				foreach (var liveInterval in register.LiveIntervals)
				{
					foreach (var use in liveInterval.UsePositions)
					{
						AssignPhysicalRegistersToInstructions(GetNode(use), register.VirtualRegisterOperand, liveInterval.AssignedPhysicalOperand ?? liveInterval.VirtualRegister.SpillSlotOperand);
					}

					foreach (var def in liveInterval.DefPositions)
					{
						AssignPhysicalRegistersToInstructions(GetNode(def), register.VirtualRegisterOperand, liveInterval.AssignedPhysicalOperand ?? liveInterval.VirtualRegister.SpillSlotOperand);
					}
				}
			}
		}

		protected void AssignPhysicalRegistersToInstructions(InstructionNode node, Operand old, Operand replacement)
		{
			for (int i = 0; i < node.OperandCount; i++)
			{
				var operand = node.GetOperand(i);

				if (operand == old)
				{
					node.SetOperand(i, replacement);
				}
			}

			for (int i = 0; i < node.ResultCount; i++)
			{
				var operand = node.GetResult(i);

				if (operand == old)
				{
					node.SetResult(i, replacement);
				}
			}
		}

		protected void ResolveDataFlow()
		{
			var resolverTrace = CreateTrace("ResolveDataFlow", 9);

			var moveResolvers = new MoveResolver[2, BasicBlocks.Count];

			foreach (var from in ExtendedBlocks)
			{
				foreach (var nextBlock in from.BasicBlock.NextBlocks)
				{
					var to = ExtendedBlocks[nextBlock.Sequence];

					// determine where to insert resolving moves
					bool fromAnchorFlag = (from.BasicBlock.NextBlocks.Count == 1);

					ExtendedBlock anchor = fromAnchorFlag ? from : to;

					MoveResolver moveResolver = moveResolvers[fromAnchorFlag ? 0 : 1, anchor.Sequence];

					if (moveResolver == null)
					{
						moveResolver = new MoveResolver(anchor.BasicBlock, from.BasicBlock, to.BasicBlock);
						moveResolvers[fromAnchorFlag ? 0 : 1, anchor.Sequence] = moveResolver;
					}

					foreach (var virtualRegister in GetVirtualRegisters(to.LiveIn))
					{
						//if (virtualRegister.IsPhysicalRegister)
						//continue;

						var fromLiveInterval = virtualRegister.GetIntervalAtOrEndsAt(from.End);
						var toLiveInterval = virtualRegister.GetIntervalAt(to.Start);

						Debug.Assert(fromLiveInterval != null);
						Debug.Assert(toLiveInterval != null);

						if (fromLiveInterval.AssignedPhysicalRegister != toLiveInterval.AssignedPhysicalRegister)
						{
							resolverTrace?.Log($"REGISTER: {fromLiveInterval.VirtualRegister}");
							resolverTrace?.Log($"    FROM: {from.ToString().PadRight(7)} {fromLiveInterval.AssignedOperand}");
							resolverTrace?.Log($"      TO: {to.ToString().PadRight(7)} {toLiveInterval.AssignedOperand}");

							resolverTrace?.Log($"  INSERT: {(fromAnchorFlag ? "FROM (bottom)" : "TO (Before)")}{((toLiveInterval.AssignedPhysicalOperand == null) ? "  ****SKIPPED***" : string.Empty)}");
							resolverTrace?.Log();

							// interval was spilled (spill moves are inserted elsewhere)
							if (toLiveInterval.AssignedPhysicalOperand == null)
								continue;

							Debug.Assert(from.BasicBlock.NextBlocks.Count == 1 || to.BasicBlock.PreviousBlocks.Count == 1);

							moveResolver.AddMove(fromLiveInterval.AssignedOperand, toLiveInterval.AssignedOperand);
						}
					}
				}
			}

			for (int b = 0; b < BasicBlocks.Count; b++)
			{
				for (int fromTag = 0; fromTag < 2; fromTag++)
				{
					var moveResolver = moveResolvers[fromTag, b];

					if (moveResolver == null)
						continue;

					moveResolver.InsertResolvingMoves(Architecture, StackFrame);
				}
			}
		}

		protected class MoveKeyedList : KeyedList<SlotIndex, Move>
		{
			public void Add(SlotIndex at, Operand source, Operand destination)
			{
				Add(at, new Move(source, destination));
			}
		}

		protected void InsertRegisterMoves()
		{
			var insertTrace = CreateTrace("InsertRegisterMoves", 9);

			var moves = GetRegisterMoves();

			foreach (var key in moves.Keys)
			{
				var moveResolver = new MoveResolver(GetNode(key), !key.IsAfterSlot, moves[key]);

				moveResolver.InsertResolvingMoves(Architecture, StackFrame);

				if (insertTrace != null)
				{
					foreach (var move in moves[key])
					{
						//insertTrace.Log("REGISTER: " + virtualRegister.ToString());
						insertTrace.Log($"  AT: {key}");
						insertTrace.Log($"FROM: {move.Source}");
						insertTrace.Log($"  TO: {move.Destination}");

						insertTrace.Log();
					}
				}
			}
		}

		protected MoveKeyedList GetRegisterMoves()
		{
			var keyedList = new MoveKeyedList();

			// collect edge slot indexes
			var blockEdges = new Dictionary<SlotIndex, ExtendedBlock>();

			foreach (var block in ExtendedBlocks)
			{
				blockEdges.Add(block.Start, block);
				blockEdges.Add(block.End, block);
			}

			foreach (var virtualRegister in VirtualRegisters)
			{
				if (virtualRegister.IsPhysicalRegister)
					continue;

				if (virtualRegister.LiveIntervals.Count <= 1)
					continue;

				foreach (var currentInterval in virtualRegister.LiveIntervals)
				{
					// No moves at block edges (these are done in the resolve move phase later)
					if (blockEdges.ContainsKey(currentInterval.End))
						continue;

					// List is not sorted, so scan thru each one
					foreach (var nextInterval in virtualRegister.LiveIntervals)
					{
						// same interval
						if (currentInterval == nextInterval)
							continue;

						if (nextInterval.Start != currentInterval.End)
							continue;

						// next interval is stack - stores to stack are done elsewhere
						if (nextInterval.AssignedPhysicalOperand == null)
							break;

						// check if source and destination operands of the move are the same
						if (nextInterval.AssignedOperand == currentInterval.AssignedOperand
							|| nextInterval.AssignedOperand.Register == currentInterval.AssignedOperand.Register)
						{
							break;
						}

						// don't load from slot if next live interval starts with a def before use
						if (nextInterval.LiveRange.DefCount != 0)
						{
							if (nextInterval.LiveRange.UseCount == 0)
							{
								continue;
							}
							else
							{
								if (nextInterval.LiveRange.FirstDef < nextInterval.LiveRange.FirstUse)
									continue;
							}
						}

						keyedList.Add(currentInterval.End, currentInterval.AssignedOperand, nextInterval.AssignedOperand);

						break;
					}
				}
			}

			return keyedList;
		}
	}
}
