/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.InternalTrace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	/// <summary>
	///
	/// </summary>
	public class BaseLiveRangeRegisterAllocator
	{
		protected BasicBlocks basicBlocks;
		protected InstructionSet instructionSet;
		protected StackLayout stackLayout;
		protected BaseArchitecture architecture;

		protected int virtualRegisterCount;
		protected int physicalRegisterCount;
		protected int registerCount;
		protected List<ExtendedBlock> extendedBlocks;
		protected List<VirtualRegister> virtualRegisters;

		protected SimpleKeyPriorityQueue<LiveInterval> priorityQueue;
		protected List<LiveIntervalTrack> liveIntervalTracks;

		protected Register stackFrameRegister;
		protected Register stackPointerRegister;
		protected Register programCounter;

		protected List<LiveInterval> spilledIntervals;

		protected List<SlotIndex> callSlots;

		protected Dictionary<SlotIndex, MoveHint> moveHints;

		protected CompilerTrace trace;

		public BaseLiveRangeRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters compilerVirtualRegisters, InstructionSet instructionSet, StackLayout stackLayout, BaseArchitecture architecture, CompilerTrace trace)
		{
			this.trace = trace;

			this.basicBlocks = basicBlocks;
			this.instructionSet = instructionSet;
			this.stackLayout = stackLayout;
			this.architecture = architecture;

			this.virtualRegisterCount = compilerVirtualRegisters.Count;
			this.physicalRegisterCount = architecture.RegisterSet.Length;
			this.registerCount = virtualRegisterCount + physicalRegisterCount;

			this.liveIntervalTracks = new List<LiveIntervalTrack>(physicalRegisterCount);
			this.virtualRegisters = new List<VirtualRegister>(registerCount);
			this.extendedBlocks = new List<ExtendedBlock>(basicBlocks.Count);

			stackFrameRegister = architecture.StackFrameRegister;
			stackPointerRegister = architecture.StackPointerRegister;
			programCounter = architecture.ProgramCounter;

			// Setup extended physical registers
			foreach (var physicalRegister in architecture.RegisterSet)
			{
				Debug.Assert(physicalRegister.Index == virtualRegisters.Count);
				Debug.Assert(physicalRegister.Index == liveIntervalTracks.Count);

				bool reserved = (physicalRegister == stackFrameRegister
					|| physicalRegister == stackPointerRegister
					|| (programCounter != null && physicalRegister == programCounter));

				this.virtualRegisters.Add(new VirtualRegister(physicalRegister, reserved));
				this.liveIntervalTracks.Add(new LiveIntervalTrack(physicalRegister, reserved));
			}

			// Setup extended virtual registers
			foreach (var virtualRegister in compilerVirtualRegisters)
			{
				Debug.Assert(virtualRegister.Index == virtualRegisters.Count - physicalRegisterCount + 1);

				this.virtualRegisters.Add(new VirtualRegister(virtualRegister));
			}

			priorityQueue = new SimpleKeyPriorityQueue<LiveInterval>();
			spilledIntervals = new List<LiveInterval>();

			callSlots = new List<SlotIndex>();

			moveHints = new Dictionary<SlotIndex, MoveHint>();

			Start();
		}

		protected void Start()
		{
			// Order all the blocks
			CreateExtendedBlocks();

			// Number all the instructions in block order
			NumberInstructions();

			// Generate trace information for instruction numbering
			TraceNumberInstructions();

			// Computer local live sets
			ComputeLocalLiveSets();

			// Computer global live sets
			ComputeGlobalLiveSets();

			// Build the live intervals
			BuildLiveIntervals();

			// Generate trace information for blocks
			TraceBlocks();

			// Generate trace information for live intervals
			TraceLiveIntervals("InitialLiveIntervals", false);

			// Collect move locations
			CollectMoveHints();

			// Generate trace information for move hints
			TraceMoveHints();

			// Split intervals at call sites
			SplitIntervalsAtCallSites();

			// Calculate spill costs for live intervals
			CalculateSpillCosts();

			// Populate priority queue
			PopulatePriorityQueue();

			// Process priority queue
			ProcessPriorityQueue();

			// Create spill slots operands
			CreateSpillSlotOperands();

			// Create physical register operands
			CreatePhysicalRegisterOperands();

			// Insert spill moves
			InsertSpillMoves();

			// Assign physical registers
			AssignRegisters();

			// Insert register moves
			InsertRegisterMoves();

			// Resolve data flow
			ResolveDataFlow();

			// Generate trace information for live intervals
			TraceLiveIntervals("PostLiveIntervals", true);
		}

		protected static string ToString(BitArray bitArray)
		{
			var builder = new StringBuilder();

			foreach (bool bit in bitArray)
			{
				builder.Append(bit ? "X" : ".");
			}

			return builder.ToString();
		}

		protected void TraceBlocks()
		{
			if (!trace.Active)
				return;

			var sectionTrace = new CompilerTrace(trace, "Extended Blocks");

			foreach (var block in extendedBlocks)
			{
				sectionTrace.Log("Block # " + block.BasicBlock.Sequence.ToString() + " (" + block.Start + " to " + block.End + ")");
				sectionTrace.Log(" LiveIn:   " + ToString(block.LiveIn));
				sectionTrace.Log(" LiveGen:  " + ToString(block.LiveGen));
				sectionTrace.Log(" LiveKill: " + ToString(block.LiveKill));
				sectionTrace.Log(" LiveOut:  " + ToString(block.LiveOut));
			}
		}

		protected void TraceLiveIntervals(string stage, bool operand)
		{
			if (!trace.Active)
				return;

			var registerTrace = new CompilerTrace(trace, stage);

			foreach (var virtualRegister in virtualRegisters)
			{
				if (virtualRegister.IsPhysicalRegister)
				{
					registerTrace.Log("Physical Register # " + virtualRegister.PhysicalRegister.ToString());
				}
				else
				{
					registerTrace.Log("Virtual Register # " + virtualRegister.VirtualRegisterOperand.Index.ToString());
				}

				registerTrace.Log("Live Intervals (" + virtualRegister.LiveIntervals.Count.ToString() + "): " + LiveIntervalsToString(virtualRegister.LiveIntervals, operand));

				registerTrace.Log("Def Positions (" + virtualRegister.DefPositions.Count.ToString() + "): " + SlotsToString(virtualRegister.DefPositions));
				registerTrace.Log("Use Positions (" + virtualRegister.UsePositions.Count.ToString() + "): " + SlotsToString(virtualRegister.UsePositions));
			}
		}

		protected String LiveIntervalsToString(List<LiveInterval> liveIntervals)
		{
			return LiveIntervalsToString(liveIntervals, false);
		}

		protected String LiveIntervalsToString(List<LiveInterval> liveIntervals, bool operand)
		{
			if (liveIntervals.Count == 0)
				return string.Empty;

			StringBuilder sb = new StringBuilder();

			foreach (var liveInterval in liveIntervals)
			{
				if (operand && !liveInterval.IsPhysicalRegister)
					sb.Append("[" + liveInterval.Start.ToString() + ", " + liveInterval.End.ToString() + "]/" + liveInterval.AssignedOperand.ToString() + ",");
				else
					sb.Append("[" + liveInterval.Start.ToString() + ", " + liveInterval.End.ToString() + "],");
			}

			if (sb[sb.Length - 1] == ',')
				sb.Length = sb.Length - 1;

			return sb.ToString();
		}

		protected string SlotsToString(IList<SlotIndex> slots)
		{
			if (slots.Count == 0)
				return string.Empty;

			StringBuilder sb = new StringBuilder();

			foreach (var use in slots)
			{
				sb.Append(use.ToString() + ",");
			}

			if (sb[sb.Length - 1] == ',')
				sb.Length = sb.Length - 1;

			return sb.ToString();
		}

		protected int GetIndex(Operand operand)
		{
			return (operand.IsCPURegister) ? (operand.Register.Index) : (operand.Index + physicalRegisterCount - 1);
		}

		protected void CreateExtendedBlocks()
		{
			var blockOrder = new LoopAwareBlockOrder(this.basicBlocks);

			// The re-ordering is not strictly necessary; however, it reduces "holes" in live ranges.
			// Less "holes" increase readability of the debug logs.
			//basicBlocks.ReorderBlocks(loopAwareBlockOrder.NewBlockOrder);

			// Allocate and setup extended blocks
			for (int i = 0; i < basicBlocks.Count; i++)
			{
				extendedBlocks.Add(new ExtendedBlock(basicBlocks[i], registerCount, blockOrder.GetLoopDepth(basicBlocks[i])));
			}
		}

		public static void NumberInstructions(BasicBlocks basicBlocks, InstructionSet instructionSet)
		{
			int index = SlotIndex.Increment;

			foreach (var block in basicBlocks)
			{
				for (var context = new Context(instructionSet, block); ; context.GotoNext())
				{
					if (!context.IsEmpty)
					{
						context.SlotNumber = index;
						index = index + SlotIndex.Increment;
					}

					if (context.IsBlockEndInstruction)
						break;
				}
			}
		}

		protected void NumberInstructions()
		{
			NumberInstructions(basicBlocks, instructionSet);

			foreach (BasicBlock block in basicBlocks)
			{
				SlotIndex start = new SlotIndex(instructionSet, block.StartIndex);
				SlotIndex end = new SlotIndex(instructionSet, block.EndIndex);
				extendedBlocks[block.Sequence].Interval = new Interval(start, end);
			}
		}

		protected void TraceNumberInstructions()
		{
			if (!trace.Active)
				return;

			var number = new CompilerTrace(trace, "InstructionNumber");

			int index = SlotIndex.Increment;

			foreach (BasicBlock block in basicBlocks)
			{
				for (Context context = new Context(instructionSet, block); ; context.GotoNext())
				{
					if (context.IsEmpty)
						continue;

					if (context.IsBlockStartInstruction)
					{
						number.Log(context.SlotNumber.ToString() + " = " + context.ToString() + " # " + block.ToString());
					}
					else
					{
						number.Log(context.SlotNumber.ToString() + " = " + context.ToString());
					}

					if (context.IsBlockEndInstruction)
						break;
				}
			}
		}

		protected void ComputeLocalLiveSets()
		{
			var liveSetTrace = new CompilerTrace(trace, "ComputeLocalLiveSets");

			foreach (var block in extendedBlocks)
			{
				if (liveSetTrace.Active)
					liveSetTrace.Log("Block # " + block.BasicBlock.Sequence.ToString());

				BitArray liveGen = new BitArray(registerCount, false);
				BitArray liveKill = new BitArray(registerCount, false);

				liveGen.Set(stackFrameRegister.Index, true);
				liveGen.Set(stackPointerRegister.Index, true);

				if (programCounter != null)
					liveGen.Set(programCounter.Index, true);

				for (Context context = new Context(instructionSet, block.BasicBlock); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty)
						continue;

					if (liveSetTrace.Active)
						liveSetTrace.Log(context.ToString());

					OperandVisitor visitor = new OperandVisitor(context);

					foreach (var ops in visitor.Input)
					{
						if (liveSetTrace.Active)
							liveSetTrace.Log("INPUT:  " + ops.ToString());

						int index = GetIndex(ops);
						if (!liveKill.Get(index))
						{
							liveGen.Set(index, true);

							if (liveSetTrace.Active)
								liveSetTrace.Log("GEN:  " + index.ToString() + " " + ops.ToString());
						}
					}

					if (context.Instruction.FlowControl == FlowControl.Call)
					{
						for (int s = 0; s < physicalRegisterCount; s++)
						{
							liveKill.Set(s, true);
						}

						if (liveSetTrace.Active)
							liveSetTrace.Log("KILL ALL PHYSICAL");
					}

					foreach (var ops in visitor.Output)
					{
						if (liveSetTrace.Active)
							liveSetTrace.Log("OUTPUT: " + ops.ToString());

						int index = GetIndex(ops);
						liveKill.Set(index, true);

						if (liveSetTrace.Active)
							liveSetTrace.Log("KILL: " + index.ToString() + " " + ops.ToString());
					}
				}

				block.LiveGen = liveGen;
				block.LiveKill = liveKill;
				block.LiveKillNot = ((BitArray)liveKill.Clone()).Not();

				if (liveSetTrace.Active)
				{
					liveSetTrace.Log("GEN:     " + ToString(block.LiveGen));
					liveSetTrace.Log("KILL:    " + ToString(block.LiveKill));
					liveSetTrace.Log("KILLNOT: " + ToString(block.LiveKillNot));
					liveSetTrace.Log(string.Empty);
				}
			}
		}

		protected void ComputeGlobalLiveSets()
		{
			bool changed = true;
			while (changed)
			{
				changed = false;

				for (int i = basicBlocks.Count - 1; i >= 0; i--)
				{
					var block = extendedBlocks[i];

					BitArray liveOut = new BitArray(registerCount);

					foreach (var next in block.BasicBlock.NextBlocks)
					{
						liveOut.Or(extendedBlocks[next.Sequence].LiveIn);
					}

					BitArray liveIn = (BitArray)block.LiveOut.Clone();
					liveIn.And(block.LiveKillNot);
					liveIn.Or(block.LiveGen);

					// compare them for any changes
					if (!changed)
						if (!block.LiveOut.AreSame(liveOut) || !block.LiveIn.AreSame(liveIn))
							changed = true;

					block.LiveOut = liveOut;
					block.LiveIn = liveIn;
				}
			}
		}

		protected void BuildLiveIntervals()
		{
			var intervalTrace = new CompilerTrace(trace, "BuildLiveIntervals");

			for (int b = basicBlocks.Count - 1; b >= 0; b--)
			{
				var block = extendedBlocks[b];

				for (int r = 0; r < registerCount; r++)
				{
					if (!block.LiveOut.Get(r))
						continue;

					var register = virtualRegisters[r];

					if (b + 1 != basicBlocks.Count && extendedBlocks[b + 1].LiveIn.Get(r))
					{
						if (intervalTrace.Active) intervalTrace.Log("Add (LiveOut) " + register.ToString() + " : " + block.Start + " to " + extendedBlocks[b + 1].Start);
						if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
						register.AddLiveInterval(block.Start, extendedBlocks[b + 1].Start);
						if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
					}
					else
					{
						if (intervalTrace.Active) intervalTrace.Log("Add (!LiveOut) " + register.ToString() + " : " + block.Interval.Start + " to " + block.Interval.End);
						if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
						register.AddLiveInterval(block.Interval);
						if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
					}
				}

				Context context = new Context(instructionSet, block.BasicBlock, block.BasicBlock.EndIndex);

				while (!context.IsBlockStartInstruction)
				{
					if (!context.IsEmpty)
					{
						SlotIndex slotIndex = new SlotIndex(context);

						OperandVisitor visitor = new OperandVisitor(context);

						if (context.Instruction.FlowControl == FlowControl.Call)
						{
							for (int s = 0; s < physicalRegisterCount; s++)
							{
								var register = virtualRegisters[s];
								if (intervalTrace.Active) intervalTrace.Log("Add (Call) " + register.ToString() + " : " + slotIndex + " to " + slotIndex.HalfStepForward);
								if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
								register.AddLiveInterval(slotIndex, slotIndex.HalfStepForward);
								if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
							}

							callSlots.Add(slotIndex);
						}

						foreach (var result in visitor.Output)
						{
							var register = virtualRegisters[GetIndex(result)];

							if (register.IsReserved)
								continue;

							var first = register.FirstRange;

							if (!register.IsPhysicalRegister)
							{
								register.AddDefPosition(slotIndex);
							}

							if (first != null)
							{
								if (intervalTrace.Active) intervalTrace.Log("Replace First " + register.ToString() + " : " + slotIndex + " to " + first.End);
								if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
								register.FirstRange = new LiveInterval(register, slotIndex, first.End);
								if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
							}
							else
							{
								// This is necesary to handled a result that is never used!
								// Common with instructions which more than one result
								if (intervalTrace.Active) intervalTrace.Log("Add (Unused) " + register.ToString() + " : " + slotIndex + " to " + slotIndex);
								if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
								register.AddLiveInterval(slotIndex, slotIndex.HalfStepForward);
								if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
							}
						}

						foreach (var result in visitor.Input)
						{
							var register = virtualRegisters[GetIndex(result)];

							if (register.IsReserved)
								continue;

							if (!register.IsPhysicalRegister)
							{
								register.AddUsePosition(slotIndex);
							}

							if (intervalTrace.Active) intervalTrace.Log("Add (normal) " + register.ToString() + " : " + block.Start + " to " + slotIndex);
							if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
							register.AddLiveInterval(block.Start, slotIndex);
							if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
						}
					}

					context.GotoPrevious();
				}
			}
		}

		protected ExtendedBlock GetContainingBlock(SlotIndex slotIndex)
		{
			foreach (var block in extendedBlocks)
			{
				if (block.Contains(slotIndex))
				{
					return block;
				}
			}

			Debug.Assert(false, "GetContainingBlock");
			return null;
		}

		protected int GetLoopDepth(SlotIndex slotIndex)
		{
			return GetContainingBlock(slotIndex).LoopDepth + 1;
		}

		protected SlotIndex GetBlockEnd(SlotIndex slotIndex)
		{
			return GetContainingBlock(slotIndex).End;
		}

		protected SlotIndex GetBlockStart(SlotIndex slotIndex)
		{
			return GetContainingBlock(slotIndex).Start;
		}

		protected int GetSpillCost(SlotIndex use, int factor)
		{
			return factor * GetLoopDepth(use) * 100;
		}

		protected void CalculateSpillCost(LiveInterval liveInterval)
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

		protected void CalculateSpillCosts()
		{
			foreach (var virtualRegister in virtualRegisters)
			{
				foreach (var liveInterval in virtualRegister.LiveIntervals)
				{
					// Skip adding live intervals for physical registers to priority queue
					if (liveInterval.VirtualRegister.IsPhysicalRegister)
					{
						// fixed and not spillable
						liveInterval.NeverSpill = true;
					}
					else if (liveInterval.VirtualRegister.VirtualRegisterOperand.Type.IsUserValueType)
					{
						// Value types will never be spilled (they are already allocated in stack anyways)
						liveInterval.NeverSpill = true;
					}
					else
					{
						// Calculate spill costs for live interval
						CalculateSpillCost(liveInterval);
					}
				}
			}
		}

		protected void AddPriorityQueue(LiveInterval liveInterval)
		{
			Debug.Assert(!liveInterval.IsSplit);
			Debug.Assert(liveInterval.LiveIntervalTrack == null);

			// priority is based on allocation stage (primary, lower first) and interval size (secondary, higher first)
			priorityQueue.Enqueue(liveInterval.Length | ((int)(((int)LiveInterval.AllocationStage.Max - liveInterval.Stage)) << 28), liveInterval);
		}

		protected void PopulatePriorityQueue()
		{
			foreach (var virtualRegister in virtualRegisters)
			{
				foreach (var liveInterval in virtualRegister.LiveIntervals)
				{
					// Skip adding live intervals for physical registers to priority queue
					if (liveInterval.VirtualRegister.IsPhysicalRegister)
					{
						liveIntervalTracks[liveInterval.VirtualRegister.PhysicalRegister.Index].Add(liveInterval);

						continue;
					}

					liveInterval.Stage = LiveInterval.AllocationStage.Initial;

					// Add live intervals for virtual registers to priority queue
					AddPriorityQueue(liveInterval);
				}
			}
		}

		protected void ProcessPriorityQueue()
		{
			while (!priorityQueue.IsEmpty)
			{
				var liveInterval = priorityQueue.Dequeue();

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

			if (trace.Active) trace.Log("  Assigned live interval to: " + track.ToString());

			track.Add(liveInterval);

			return true;
		}

		protected bool PlaceLiveIntervalOnAnyAvailableTrack(LiveInterval liveInterval)
		{
			foreach (var track in liveIntervalTracks)
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
			foreach (var track in liveIntervalTracks)
			{
				if (track.IsReserved)
					continue;

				if (track.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
					continue;

				var intersections = track.GetIntersections(liveInterval);

				bool evict = true;

				foreach (var intersection in intersections)
				{
					if (intersection.SpillCost >= liveInterval.SpillCost || intersection.SpillCost == Int32.MaxValue || intersection.VirtualRegister.IsPhysicalRegister || intersection.IsPhysicalRegister)
					{
						evict = false;
						break;
					}
				}

				if (evict)
				{
					if (intersections.Count != 0)
					{
						if (trace.Active) trace.Log("  Evicting live intervals");

						track.Evict(intersections);

						foreach (var intersection in intersections)
						{
							if (trace.Active) trace.Log("  Evicted: " + intersection.ToString());

							liveInterval.Stage = LiveInterval.AllocationStage.Initial;
							AddPriorityQueue(intersection);
						};
					}

					track.Add(liveInterval);

					if (trace.Active) trace.Log("  Assigned live interval to: " + track.ToString());

					return true;
				}
			}

			return false;
		}

		protected bool PlaceLiveIntervalOnTrack(LiveInterval liveInterval, MoveHint[] hints)
		{
			if (hints == null)
				return false;

			foreach (var moveHint in hints)
			{
				LiveIntervalTrack track = null;

				var register = (liveInterval.Start == moveHint.Slot) ? moveHint.FromRegister : moveHint.ToRegister;

				if (register == null)
					continue;	// no usable hint

				if (trace.Active) trace.Log("  Trying move hint: " + register.ToString() + "  [ " + moveHint.ToString() + " ]");

				if (PlaceLiveIntervalOnTrack(liveInterval, liveIntervalTracks[register.Index]))
				{
					return true;
				}
			}

			return false;
		}

		protected MoveHint[] GetMoveHints(LiveInterval liveInterval)
		{
			MoveHint startMoveHint = null;
			MoveHint endMoveHint = null;
			moveHints.TryGetValue(liveInterval.Start, out startMoveHint);

			if (!liveInterval.End.IsBlockStartInstruction)
				moveHints.TryGetValue(liveInterval.End, out endMoveHint);

			int cnt = (startMoveHint == null ? 0 : 1) + (endMoveHint == null ? 0 : 1);

			if (cnt == 0)
				return null;

			var hints = new MoveHint[cnt];

			if (startMoveHint != null && endMoveHint != null)
			{
				// sorted by bonus
				if (startMoveHint.Bonus > endMoveHint.Bonus)
				{
					hints[0] = startMoveHint;
					hints[1] = endMoveHint;
				}
				else
				{
					hints[0] = endMoveHint;
					hints[1] = startMoveHint;
				}
			}
			else
			{
				if (startMoveHint != null)
				{
					hints[0] = startMoveHint;
				}
				else
				{
					hints[0] = endMoveHint;
				}
			}

			return hints;
		}

		protected void UpdateMoveHints(LiveInterval liveInterval, MoveHint[] moveHints)
		{
			if (moveHints == null)
				return;

			if (moveHints.Length >= 1)
				moveHints[0].Update(liveInterval);
			else
				if (moveHints.Length >= 2)
					moveHints[1].Update(liveInterval);
		}

		protected void UpdateMoveHints(LiveInterval liveInterval)
		{
			MoveHint MoveHint = null;

			if (moveHints.TryGetValue(liveInterval.Start, out MoveHint))
			{
				MoveHint.Update(liveInterval);
			}

			if (moveHints.TryGetValue(liveInterval.End, out MoveHint))
			{
				MoveHint.Update(liveInterval);
			}
		}

		protected void ProcessLiveInterval(LiveInterval liveInterval)
		{
			Debug.Assert(liveInterval.LiveIntervalTrack == null);
			Debug.Assert(!liveInterval.IsSplit);

			if (trace.Active)
			{
				trace.Log("Processing Interval: " + liveInterval.ToString() + " / Length: " + liveInterval.Length.ToString() + " / Spill Cost: " + liveInterval.SpillCost.ToString() + " / Stage: " + liveInterval.Stage.ToString());
				trace.Log("  Defs (" + liveInterval.DefPositions.Count.ToString() + "): " + SlotsToString(liveInterval.DefPositions));
				trace.Log("  Uses (" + liveInterval.UsePositions.Count.ToString() + "): " + SlotsToString(liveInterval.UsePositions));
			}

			// For now, empty intervals will stay spilled
			if (liveInterval.IsEmpty)
			{
				if (trace.Active) trace.Log("  Spilled");

				liveInterval.VirtualRegister.IsSpilled = true;
				spilledIntervals.Add(liveInterval);

				return;
			}

			var moveHints = GetMoveHints(liveInterval);

			// Try to place using move hints first
			if (PlaceLiveIntervalOnTrack(liveInterval, moveHints))
			{
				UpdateMoveHints(liveInterval, moveHints);
				return;
			}

			// TODO - try move hints first, allow evictions

			// Find any available track and place interval there
			if (PlaceLiveIntervalOnAnyAvailableTrack(liveInterval))
			{
				UpdateMoveHints(liveInterval, moveHints);
				return;
			}

			if (trace.Active) trace.Log("  No free register available");

			// No place for live interval; find live interval(s) to evict based on spill costs
			if (PlaceLiveIntervalOnTrackAllowEvictions(liveInterval))
			{
				UpdateMoveHints(liveInterval, moveHints);
				return;
			}

			if (trace.Active) trace.Log("  No live intervals to evicts");

			// No live intervals to evict!

			// prepare to split live interval
			if (liveInterval.Stage == LiveInterval.AllocationStage.Initial)
			{
				if (trace.Active) trace.Log("  Re-queued for split level 1 stage");
				liveInterval.Stage = LiveInterval.AllocationStage.SplitLevel1;
				AddPriorityQueue(liveInterval);
				return;
			}

			// split live interval - level 1
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitLevel1)
			{
				if (trace.Active) trace.Log("  Attempting to split interval - level 1");

				if (TrySplitInterval(liveInterval, 1))
				{
					return;
				}

				// Move to split level 2 stage
				if (trace.Active) trace.Log("  Re-queued for split interval - level 2");

				liveInterval.Stage = LiveInterval.AllocationStage.SplitLevel2;
				AddPriorityQueue(liveInterval);
				return;
			}

			// split live interval - level 2
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitLevel2)
			{
				if (trace.Active) trace.Log("  Attempting to split interval - level 2");

				if (TrySplitInterval(liveInterval, 2))
				{
					return;
				}

				// Move to spill stage
				if (trace.Active) trace.Log("  Re-queued for spillable stage");

				liveInterval.Stage = LiveInterval.AllocationStage.Spillable;
				AddPriorityQueue(liveInterval);
				return;
			}

			// spill interval to stack slot
			if (liveInterval.Stage == LiveInterval.AllocationStage.Spillable)
			{
				if (trace.Active) trace.Log("  Spilled interval");

				//liveInterval.Stage = LiveInterval.AllocationStage.Spilled
				liveInterval.VirtualRegister.IsSpilled = true;
				spilledIntervals.Add(liveInterval);
				return;
			}

			return;
		}

		protected bool TrySplitInterval(LiveInterval liveInterval, int level)
		{
			if (liveInterval.IsEmpty)
				return false;

			if (TrySimplePartialFreeIntervalSplit(liveInterval))
				return true;

			if (level <= 1)
				return false;

			if (IntervalSplitAtFirstUseOrDef(liveInterval))
				return true;

			return false;
		}

		protected bool SplitInterval(LiveInterval liveInterval, SlotIndex at, bool addToQueue)
		{
			// can not split on use position
			if (liveInterval.UsePositions.Contains(at))
				return false;

			var low = GetLowerOptimalSplitLocation(liveInterval, at);

			var high = GetUpperOptimalSplitLocation(liveInterval, at);

			if (high == liveInterval.End)
			{
				high = low;
			}

			var first = liveInterval.Split(liveInterval.Start, low);
			var last = liveInterval.Split(high, liveInterval.End);

			var middle = (low != high) ? liveInterval.Split(low, high) : null;

			if (trace.Active) trace.Log("   Low Split   : " + first.ToString());
			if (trace.Active) trace.Log("   Middle Split: " + (middle == null ? "n/a" : middle.ToString()));
			if (trace.Active) trace.Log("   High Split  : " + last.ToString());

			CalculateSpillCost(first);
			CalculateSpillCost(last);

			liveInterval.IsSplit = true;

			var virtualRegister = liveInterval.VirtualRegister;

			virtualRegister.Remove(liveInterval);

			virtualRegister.Add(first);
			virtualRegister.Add(last);

			if (addToQueue)
			{
				AddPriorityQueue(first);
				AddPriorityQueue(last);
			}

			if (middle != null)
			{
				//middle.ForceSpilled = true;
				CalculateSpillCost(middle);
				virtualRegister.Add(middle);
				if (addToQueue)
				{
					AddPriorityQueue(middle);
				}
			}

			return true;
		}

		protected bool TrySimplePartialFreeIntervalSplit(LiveInterval liveInterval)
		{
			SlotIndex furthestUsed = null;

			foreach (var track in liveIntervalTracks)
			{
				if (track.IsReserved)
					continue;

				if (track.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
					continue;

				var next = track.GetNextLiveRange(liveInterval.Start);

				if (next == null)
					continue;

				Debug.Assert(next > liveInterval.Start);

				if (trace.Active) trace.Log("  Register " + track.ToString() + " free up to " + next.ToString());

				if (furthestUsed == null || furthestUsed < next)
					furthestUsed = next;
			}

			if (furthestUsed == null)
			{
				if (trace.Active) trace.Log("  No partial free space available");
				return false;
			}

			if (furthestUsed < liveInterval.Minimum)
			{
				if (trace.Active) trace.Log("  No partial free space available");
				return false;
			}

			if (furthestUsed.IsBlockStartInstruction)
			{
				return false;
			}

			if (furthestUsed <= liveInterval.Start)
			{
				if (trace.Active) trace.Log("  No partial free space available");
				return false;
			}

			if (trace.Active) trace.Log("  Partial free up to: " + furthestUsed.ToString());

			return SplitInterval(liveInterval, furthestUsed, true);
		}

		protected bool IntervalSplitAtFirstUseOrDef(LiveInterval liveInterval)
		{
			if (liveInterval.IsEmpty)
				return false;

			SlotIndex at = null;

			var firstUse = liveInterval.UsePositions.Count != 0 ? liveInterval.UsePositions[0] : null;
			var firstDef = liveInterval.DefPositions.Count != 0 ? liveInterval.DefPositions[0] : null;

			if (at == null)
			{
				at = firstUse;
			}

			if (at != null && firstDef != null && firstDef < at)
			{
				at = firstDef;
			}

			if (at >= liveInterval.End)
				return false;

			if (at <= liveInterval.Start)
				return false;

			if (trace.Active) trace.Log(" Splitting around first use/def");

			return SplitInterval(liveInterval, at, true);
		}

		protected IEnumerable<VirtualRegister> GetVirtualRegisters(BitArray array)
		{
			for (int i = 0; i < array.Count; i++)
			{
				if (array.Get(i))
				{
					var virtualRegister = virtualRegisters[i];
					if (!virtualRegister.IsPhysicalRegister)
						yield return virtualRegister;
				}
			}
		}

		protected SlotIndex GetMaximum(SlotIndex[] slots)
		{
			SlotIndex max = null;

			foreach (var slot in slots)
			{
				if (slot == null)
					continue;

				if (max == null || slot > max)
				{
					max = slot;
				}
			}

			return max;
		}

		protected SlotIndex GetMinimum(SlotIndex[] slots)
		{
			SlotIndex min = null;

			foreach (var slot in slots)
			{
				if (slot == null)
					continue;

				if (min == null || slot < min)
				{
					min = slot;
				}
			}

			return min;
		}

		protected SlotIndex GetLowerOptimalSplitLocation(LiveInterval liveInterval, SlotIndex at)
		{
			if (trace.Active) trace.Log("--Low Splitting: " + liveInterval.ToString() + " move: " + at.ToString());

			var blockStart = GetBlockStart(at);

			var slots = new SlotIndex[4];

			slots[0] = liveInterval.Start.IsOnHalfStep ? liveInterval.Start : liveInterval.Start.HalfStepForward;

			slots[1] = blockStart > liveInterval.Start ? blockStart : null;
			if (trace.Active) trace.Log("   Block Start : " + (slots[1] != null ? slots[1].ToString() : "null"));

			slots[2] = liveInterval.LiveRange.GetPreviousUsePosition(at);
			if (trace.Active) trace.Log("  Previous Use : " + (slots[2] != null ? slots[2].ToString() : "null"));

			slots[3] = liveInterval.LiveRange.GetPreviousDefPosition(at);
			if (trace.Active) trace.Log("  Previous Def : " + (slots[3] != null ? slots[3].ToString() : "null"));

			var max = GetMaximum(slots);

			if (trace.Active) trace.Log("   Low Optimal : " + max.ToString());

			return max;
		}

		protected SlotIndex GetUpperOptimalSplitLocation(LiveInterval liveInterval, SlotIndex at)
		{
			if (trace.Active) trace.Log("-High Splitting: " + liveInterval.ToString() + " move: " + at.ToString());

			var next = liveInterval.LiveRange.GetNextUsePosition(at);

			if (next == null)
			{
				return liveInterval.End;
			}

			var blockEnd = GetBlockEnd(at);

			var slots = new SlotIndex[4];

			slots[0] = liveInterval.End.HalfStepBack;

			slots[1] = blockEnd > liveInterval.End ? blockEnd : null;
			if (trace.Active) trace.Log("     Block End : " + (slots[1] != null ? slots[1].ToString() : "null"));

			slots[2] = next != null ? next.HalfStepBack : null;
			if (trace.Active) trace.Log("      Next Use : " + (slots[2] != null ? slots[2].ToString() : "null"));

			slots[3] = liveInterval.LiveRange.GetNextDefPosition(at);
			if (trace.Active) trace.Log("      Next Def : " + (slots[3] != null ? slots[3].ToString() : "null"));

			var min = GetMinimum(slots);

			if (trace.Active) trace.Log("  High Optimal : " + min.ToString());

			return min;
		}

		protected SlotIndex FindCallSiteInInterval(LiveInterval liveInterval)
		{
			foreach (SlotIndex slot in callSlots)
			{
				if (liveInterval.Contains(slot))
					return slot;
			}
			return null;
		}

		protected void CollectMoveHints()
		{
			foreach (var block in basicBlocks)
			{
				for (Context context = new Context(instructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty || context.IsBlockStartInstruction || context.IsBlockEndInstruction)
						continue;

					if (!architecture.IsInstructionMove(context.Instruction))
						continue;

					if (!((context.Result.IsVirtualRegister && context.Operand1.IsVirtualRegister)
						|| (context.Result.IsVirtualRegister && context.Operand1.IsCPURegister)
						|| (context.Result.IsCPURegister && context.Operand1.IsVirtualRegister)))
						continue;

					var from = virtualRegisters[GetIndex(context.Operand1)];
					var to = virtualRegisters[GetIndex(context.Result)];

					var slot = new SlotIndex(context);

					int factor = (from.IsPhysicalRegister || to.IsPhysicalRegister) ? 150 : 125;

					int bonus = GetLoopDepth(slot);

					moveHints.Add(slot, new MoveHint(slot, from, to, bonus));
				}
			}
		}

		protected void TraceMoveHints()
		{
			if (!trace.Active)
				return;

			var moveHintTrace = new CompilerTrace(trace, "MoveHints");

			foreach (var moveHint in moveHints)
			{
				moveHintTrace.Log(moveHint.Value.ToString());
			}
		}

		protected void SplitIntervalsAtCallSites()
		{
			foreach (var virtualRegister in virtualRegisters)
			{
				if (virtualRegister.IsPhysicalRegister)
					continue;

				for (int i = 0; i < virtualRegister.LiveIntervals.Count; i++)
				{
					LiveInterval liveInterval = virtualRegister.LiveIntervals[i];

					if (liveInterval.ForceSpilled)
						continue;

					if (liveInterval.IsEmpty)
						continue;

					var callSite = FindCallSiteInInterval(liveInterval);

					if (callSite == null)
						continue;

					SplitInterval(liveInterval, callSite, false);

					i = 0; // list was modified
				}
			}
		}

		protected void CreateSpillSlotOperands()
		{
			foreach (var register in virtualRegisters)
			{
				if (!register.IsSpilled)
					continue;

				Debug.Assert(register.IsVirtualRegister);
				register.SpillSlotOperand = stackLayout.AddStackLocal(register.VirtualRegisterOperand.Type);
			}
		}

		protected void CreatePhysicalRegisterOperands()
		{
			foreach (var register in virtualRegisters)
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
			foreach (var register in virtualRegisters)
			{
				if (!register.IsUsed || register.IsPhysicalRegister || !register.IsSpilled)
					continue;

				foreach (var liveInterval in register.LiveIntervals)
				{
					foreach (var def in liveInterval.DefPositions)
					{
						Context context = new Context(instructionSet, def.Index);

						architecture.InsertMoveInstruction(context, register.SpillSlotOperand, liveInterval.AssignedPhysicalOperand);

						context.Marked = true;
					}
				}
			}
		}

		protected void AssignRegisters()
		{
			foreach (var register in virtualRegisters)
			{
				if (!register.IsUsed || register.IsPhysicalRegister)
					continue;

				foreach (var liveInterval in register.LiveIntervals)
				{
					foreach (var use in liveInterval.UsePositions)
					{
						Context context = new Context(instructionSet, use.Index);
						AssignPhysicalRegistersToInstructions(context, register.VirtualRegisterOperand, liveInterval.AssignedPhysicalOperand == null ? liveInterval.VirtualRegister.SpillSlotOperand : liveInterval.AssignedPhysicalOperand);
					}

					foreach (var def in liveInterval.DefPositions)
					{
						Context context = new Context(instructionSet, def.Index);
						AssignPhysicalRegistersToInstructions(context, register.VirtualRegisterOperand, liveInterval.AssignedPhysicalOperand == null ? liveInterval.VirtualRegister.SpillSlotOperand : liveInterval.AssignedPhysicalOperand);
					}
				}
			}
		}

		protected void AssignPhysicalRegistersToInstructions(Context context, Operand old, Operand replacement)
		{
			for (int i = 0; i < context.OperandCount; i++)
			{
				var operand = context.GetOperand(i);

				if (operand.IsVirtualRegister)
				{
					if (operand == old)
					{
						context.SetOperand(i, replacement);
						continue;
					}
				}
				else if (operand.IsMemoryAddress && operand.OffsetBase != null)
				{
					if (operand.OffsetBase == old)
					{
						// FIXME: Creates a lot of duplicate single operands
						context.SetOperand(i, Operand.CreateMemoryAddress(operand.Type, replacement, operand.Displacement));
					}
				}
			}

			for (int i = 0; i < context.ResultCount; i++)
			{
				var operand = context.GetResult(i);

				if (operand.IsVirtualRegister)
				{
					if (operand == old)
					{
						context.SetResult(i, replacement);
						continue;
					}
				}
				else if (operand.IsMemoryAddress && operand.OffsetBase != null)
				{
					if (operand.OffsetBase == old)
					{
						// FIXME: Creates a lot of duplicate single operands
						context.SetResult(i, Operand.CreateMemoryAddress(operand.Type, replacement, operand.Displacement));
					}
				}
			}
		}

		protected void ResolveDataFlow()
		{
			var resolverTrace = new CompilerTrace(trace, "ResolveDataFlow");

			MoveResolver[,] moveResolvers = new MoveResolver[2, basicBlocks.Count];

			foreach (var from in extendedBlocks)
			{
				foreach (var nextBlock in from.BasicBlock.NextBlocks)
				{
					var to = extendedBlocks[nextBlock.Sequence];

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
							if (resolverTrace.Active)
							{
								resolverTrace.Log("REGISTER: " + fromLiveInterval.VirtualRegister.ToString());
								resolverTrace.Log("    FROM: " + from.ToString().PadRight(7) + " " + fromLiveInterval.AssignedOperand.ToString());
								resolverTrace.Log("      TO: " + to.ToString().PadRight(7) + " " + toLiveInterval.AssignedOperand.ToString());

								resolverTrace.Log("  INSERT: " + (fromAnchorFlag ? "FROM (bottom)" : "TO (top)") + ((toLiveInterval.AssignedPhysicalOperand == null) ? "  ****SKIPPED***" : string.Empty));
								resolverTrace.Log("");
							}

							// interval was spilled (spill moves are inserted elsewhere)
							if (toLiveInterval.AssignedPhysicalOperand == null)
								continue;

							Debug.Assert(from.BasicBlock.NextBlocks.Count == 1 || to.BasicBlock.PreviousBlocks.Count == 1);

							moveResolver.AddMove(fromLiveInterval.AssignedOperand, toLiveInterval.AssignedOperand);
						}
					}
				}
			}

			for (int b = 0; b < basicBlocks.Count; b++)
			{
				for (int fromTag = 0; fromTag < 2; fromTag++)
				{
					MoveResolver moveResolver = moveResolvers[fromTag, b];

					if (moveResolver == null)
						continue;

					moveResolver.InsertResolvingMoves(architecture, instructionSet);
				}
			}
		}

		protected void InsertRegisterMoves()
		{
			var insertTrace = new CompilerTrace(trace, "InsertRegisterMoves");

			// collect edge slot indexes
			Dictionary<SlotIndex, ExtendedBlock> blockEdges = new Dictionary<SlotIndex, ExtendedBlock>();

			foreach (var block in extendedBlocks)
			{
				blockEdges.Add(block.Start, block);
				blockEdges.Add(block.End, block);
			}

			foreach (var virtualRegister in virtualRegisters)
			{
				if (virtualRegister.IsPhysicalRegister)
					continue;

				if (virtualRegister.LiveIntervals.Count <= 1)
					continue;

				foreach (var currentInterval in virtualRegister.LiveIntervals)
				{
					if (blockEdges.ContainsKey(currentInterval.End))
						continue;

					// List is not sorted, so scan thru each one
					foreach (var nextInterval in virtualRegister.LiveIntervals)
					{
						if (nextInterval.Start != currentInterval.End)
							continue;

						// next interval is stack - stores to stack are done elsewhere
						if (nextInterval.AssignedPhysicalOperand == null)
							break;

						// check if source and destination operands of the move are the same
						if (nextInterval.AssignedOperand == currentInterval.AssignedOperand ||
							nextInterval.AssignedOperand.Register == currentInterval.AssignedOperand.Register)
							break;

						Context context = new Context(instructionSet, currentInterval.End.Index);

						if (!currentInterval.End.IsOnHalfStepForward)
						{
							context.GotoPrevious();

							while (context.IsEmpty || context.Instruction.FlowControl == FlowControl.UnconditionalBranch || context.Instruction.FlowControl == FlowControl.ConditionalBranch || context.Instruction.FlowControl == FlowControl.Return)
							{
								context.GotoPrevious();
							}
						}

						architecture.InsertMoveInstruction(context,
							nextInterval.AssignedOperand,
							currentInterval.AssignedOperand
						);

						context.Marked = true;

						if (insertTrace.Active)
						{
							insertTrace.Log("REGISTER: " + virtualRegister.ToString());
							insertTrace.Log("POSITION: " + currentInterval.End.ToString());
							insertTrace.Log("    FROM: " + currentInterval.AssignedOperand.ToString());
							insertTrace.Log("      TO: " + nextInterval.AssignedOperand.ToString());

							insertTrace.Log("");
						}

						break;
					}
				}
			}
		}
	}
}