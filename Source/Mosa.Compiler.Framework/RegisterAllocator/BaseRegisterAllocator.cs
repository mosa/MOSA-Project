// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Trace;

namespace Mosa.Compiler.Framework.RegisterAllocator;

/// <summary>
/// Base Register Allocator
/// </summary>
public abstract class BaseRegisterAllocator
{
	protected readonly BasicBlocks BasicBlocks;
	protected readonly BaseArchitecture Architecture;
	protected readonly LocalStack LocalStack;

	protected readonly Dictionary<SlotIndex, MoveHint> MoveHints = new();

	protected readonly Operand StackFrame;

	private readonly int VirtualRegisterCount;
	private readonly int PhysicalRegisterCount;
	private readonly int RegisterCount;

	protected readonly List<ExtendedBlock> ExtendedBlocks;
	protected readonly List<VirtualRegister> VirtualRegisters;
	protected readonly List<RegisterTrack> Tracks;

	private readonly PriorityQueue<LiveInterval, int> PriorityQueue = new();

	private readonly PhysicalRegister StackFrameRegister;
	private readonly PhysicalRegister StackPointerRegister;
	private readonly PhysicalRegister ProgramCounter;
	private readonly PhysicalRegister LinkRegister;

	private readonly List<LiveInterval> SpilledIntervals = new();

	private readonly List<InstructionNode> SlotsToNodes = new(512);

	protected readonly List<SlotIndex> KillSite = new();

	protected readonly BaseMethodCompilerStage.CreateTraceHandler CreateTrace;

	protected readonly TraceLog Trace;

	public int SpillMoves = 0;
	public int DataFlowMoves = 0;
	public int ResolvingMoves = 0;

	protected BaseRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters virtualRegisters, BaseArchitecture architecture, LocalStack localStack, Operand stackFrame, BaseMethodCompilerStage.CreateTraceHandler createTrace)
	{
		CreateTrace = createTrace;

		BasicBlocks = basicBlocks;
		Architecture = architecture;
		LocalStack = localStack;
		StackFrame = stackFrame;

		VirtualRegisterCount = virtualRegisters.Count;
		PhysicalRegisterCount = architecture.RegisterSet.Length;
		RegisterCount = VirtualRegisterCount + PhysicalRegisterCount;

		Tracks = new List<RegisterTrack>(PhysicalRegisterCount);
		VirtualRegisters = new List<VirtualRegister>(RegisterCount);
		ExtendedBlocks = new List<ExtendedBlock>(basicBlocks.Count);

		Trace = CreateTrace("Main", 7);

		StackFrameRegister = architecture.StackFrameRegister;
		StackPointerRegister = architecture.StackPointerRegister;
		ProgramCounter = architecture.ProgramCounter;
		LinkRegister = architecture.LinkRegister;

		// Setup extended physical registers
		foreach (var physicalRegister in architecture.RegisterSet)
		{
			var reserved = physicalRegister == StackFrameRegister
				|| physicalRegister == StackPointerRegister
				|| (LinkRegister != null && physicalRegister == LinkRegister)
				|| (ProgramCounter != null && physicalRegister == ProgramCounter);

			VirtualRegisters.Add(new VirtualRegister(physicalRegister, reserved));
			Tracks.Add(new RegisterTrack(physicalRegister, reserved));
		}

		// Setup extended virtual registers
		foreach (var virtualRegister in virtualRegisters)
		{
			Debug.Assert(virtualRegister.Index == VirtualRegisters.Count - PhysicalRegisterCount + 1);

			VirtualRegisters.Add(new VirtualRegister(virtualRegister));
		}
	}

	public virtual void Start()
	{
		// Order all the blocks
		CreateExtendedBlocks();

		// Collect instruction data and Number all the instructions in block order
		CollectInstructionData();

		// Collect block information
		CollectBlockInformation();

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

		// Generate trace information for live intervals
		TraceLiveIntervals("InitialLiveIntervals", false);

		// Split intervals at call sites
		SplitIntervalsAtCallSites();

		// Collect move locations
		CollectMoveHints();

		// Generate trace information for move hints
		TraceMoveHints();

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

	protected InstructionNode GetNode(SlotIndex slot) => SlotsToNodes[slot.Index >> 1];

	private void TraceBlocks()
	{
		var extendedBlockTrace = CreateTrace("Extended Blocks", 9);

		if (extendedBlockTrace == null)
			return;

		foreach (var block in ExtendedBlocks)
		{
			extendedBlockTrace.Log($"Block # {block.BasicBlock} [{block.BasicBlock.Sequence}] ({block.Start} destination {block.End})");
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

	private String LiveIntervalsToString(List<LiveInterval> liveIntervals, bool operand)
	{
		if (liveIntervals.Count == 0)
			return string.Empty;

		var sb = new StringBuilder();

		foreach (var liveInterval in liveIntervals)
		{
			if (operand && !liveInterval.IsPhysicalRegister)
				sb.Append('[').Append(liveInterval.Start).Append(',').Append(liveInterval.End).Append("]/").Append(liveInterval.AssignedOperand).Append(",");
			else
				sb.Append('[').Append(liveInterval.Start).Append(',').Append(liveInterval.End).Append("],");
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
		return operand.IsCPURegister ? operand.Register.Index : operand.Index + PhysicalRegisterCount - 1;
	}

	private void CreateExtendedBlocks()
	{
		var blockOrder = new LoopAwareBlockOrder(BasicBlocks);

		// The re-ordering is not strictly necessary; however, it reduces "holes" in live ranges.
		// And less "holes" improves the readability of the debug logs.
		//basicBlocks.ReorderBlocks(loopAwareBlockOrder.NewBlockOrder);

		// Allocate and setup extended blocks
		for (var i = 0; i < BasicBlocks.Count; i++)
		{
			ExtendedBlocks.Add(new ExtendedBlock(BasicBlocks[i], RegisterCount, blockOrder.GetLoopDepth(BasicBlocks[i])));
		}
	}

	protected void AddSpilledInterval(LiveInterval liveInterval)
	{
		SpilledIntervals.Add(liveInterval);
	}

	private void CollectInstructionData()
	{
		var paramStoreSet = new HashSet<Operand>();

		SlotsToNodes.Add(null);

		foreach (var block in BasicBlocks)
		{
			for (var node = block.First; ; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				node.Offset = SlotsToNodes.Count << 1;

				SlotsToNodes.Add(node);

				if (Architecture.IsParameterStore(node, out var storeParam))
				{
					paramStoreSet.Add(storeParam);
				}
				else if (Architecture.IsParameterLoad(node, out var loadParam))
				{
					var result = node.Result;

					// FUTURE: check can be improved to allow multiple defines, as long as the load is exactly the same
					if (result.IsDefinedOnce)
					{
						var register = VirtualRegisters[GetIndex(result)];

						register.IsParamLoad = true;
						register.ParamLoadNode = node;
						register.ParamOperand = loadParam;
					}
				}

				if (node.IsBlockEndInstruction)
					break;
			}

			Debug.Assert(block.Last.Offset != 0);
		}

		// Mark if parameter is writable (vs. read-only)
		foreach (var register in VirtualRegisters)
		{
			if (register.ParamOperand != null && paramStoreSet.Contains(register.ParamOperand))
			{
				register.IsParamStore = true;
			}
		}
	}

	private void CollectBlockInformation()
	{
		foreach (var block in BasicBlocks)
		{
			ExtendedBlocks[block.Sequence].Start = new SlotIndex(block.First);
			ExtendedBlocks[block.Sequence].End = new SlotIndex(block.Last);
		}
	}

	private void TraceNumberInstructions()
	{
		var numberTrace = CreateTrace("InstructionNumber", 9);
		var outputTrace = CreateTrace("InstructionOutput", 9);

		if (numberTrace == null)
			return;

		var sb = new StringBuilder();

		foreach (var block in BasicBlocks)
		{
			for (var node = block.First; ; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				sb.Clear();

				var log = $"{node.Offset} = {node}";

				if (node.IsBlockStartInstruction)
				{
					log = log + " # " + block;
				}

				numberTrace.Log(log);

				if (node.OperandCount != 0 || node.ResultCount != 0)
				{
					sb.Append($"{node.Label:X5}/{node.Offset}: ");

					foreach (var operand in node.Results)
					{
						if (operand.IsCPURegister)
							sb.Append($"{operand.Register} ");
						else if (operand.IsVirtualRegister)
							sb.Append($"v{operand.Index} ");
					}

					sb.Append("<= ");

					foreach (var operand in node.Operands)
					{
						if (operand.IsCPURegister)
							sb.Append($"{operand.Register} ");
						else if (operand.IsVirtualRegister)
							sb.Append($"v{operand.Index} ");
					}

					outputTrace.Log(sb);
				}

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

				var sb = new StringBuilder();

				var def = new List<Operand>();
				var use = new List<Operand>();

				foreach (var operand in node.Operands)
				{
					if (operand.IsVirtualRegister || operand.IsCPURegister)
					{
						def.AddIfNew(operand);
					}
				}

				foreach (var operand in node.Results)
				{
					if (operand.IsVirtualRegister || operand.IsCPURegister)
					{
						use.AddIfNew(operand);
					}
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
						sb.Append(op.ToString());
						sb.Append(' ');
					}
				}

				if (use.Count > 0)
				{
					sb.Append("USE: ");
					foreach (var op in use)
					{
						sb.Append(op);
						sb.Append(' ');
					}
				}

				locationTrace.Log(sb);

				if (node.IsBlockEndInstruction)
					break;
			}
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
				for (var s = 0; s < PhysicalRegisterCount; s++)
				{
					liveKill.Set(s, true);
				}

				liveSetTrace?.Log("KILL ALL PHYSICAL");
			}

			for (var node = block.BasicBlock.First; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				liveSetTrace?.Log(node.ToString());

				foreach (var op in node.Operands)
				{
					if (!(op.IsVirtualRegister || (op.IsCPURegister && !op.Register.IsSpecial)))
						continue;

					liveSetTrace?.Log($"INPUT:  {op}");

					var index = GetIndex(op);
					if (!liveKill.Get(index))
					{
						liveGen.Set(index, true);

						liveSetTrace?.Log($"GEN:  {index} {op}");
					}
				}

				if (node.Instruction.IsCall || node.Instruction == IRInstruction.KillAll)
				{
					for (var reg = 0; reg < PhysicalRegisterCount; reg++)
					{
						liveKill.Set(reg, true);
					}

					liveSetTrace?.Log("KILL ALL PHYSICAL");
				}
				else if (node.Instruction == IRInstruction.KillAllExcept)
				{
					var except = node.Operand1.Register.Index;

					for (var reg = 0; reg < PhysicalRegisterCount; reg++)
					{
						if (reg != except)
						{
							liveKill.Set(reg, true);
						}
					}

					liveSetTrace?.Log($"KILL EXCEPT PHYSICAL: {node.Operand1}");
				}

				foreach (var op in node.Results)
				{
					if (!(op.IsVirtualRegister || (op.IsCPURegister && !op.Register.IsSpecial)))
						continue;

					liveSetTrace?.Log($"OUTPUT: {op}");

					var index = GetIndex(op);
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
		var changed = true;
		while (changed)
		{
			changed = false;

			foreach (var block in ExtendedBlocks)
			{
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

		var endSlots = new SlotIndex[RegisterCount];

		for (var b = BasicBlocks.Count - 1; b >= 0; b--)
		{
			var block = ExtendedBlocks[b];

			intervalTrace?.Log($"Block # {block.BasicBlock.Sequence}");

			for (var r = 0; r < RegisterCount; r++)
			{
				if (!block.LiveOut.Get(r))
				{
					endSlots[r] = SlotIndex.Null;
					continue;
				}

				endSlots[r] = b + 1 != BasicBlocks.Count && ExtendedBlocks[b + 1].LiveIn.Get(r)
					? ExtendedBlocks[b + 1].Start
					: block.End;

				intervalTrace?.Log($"EndSlot: {VirtualRegisters[r]} = {endSlots[r]} [LiveOut]");
			}

			for (var node = block.BasicBlock.Last; !node.IsBlockStartInstruction; node = node.Previous)
			{
				if (node.IsEmptyOrNop)
					continue;

				var slot = new SlotIndex(node);
				var slotNext = slot.Next;

				if (node.Instruction.IsCall || node.Instruction == IRInstruction.KillAll || node.Instruction == IRInstruction.KillAllExcept)
				{
					for (var r = 0; r < PhysicalRegisterCount; r++)
					{
						var register = VirtualRegisters[r];

						if (register.IsReserved)
							continue;

						if (node.Instruction == IRInstruction.KillAllExcept && r == node.Operand1.Register.Index)
							continue;

						if (endSlots[r].IsNotNull)
						{
							register.AddLiveInterval(endSlots[r], slotNext);

							endSlots[r] = SlotIndex.Null;

							intervalTrace?.Log($"Range:   {node.Label:X5}/{node.Offset} : {register} = {endSlots[r]} to {slotNext} [Add]");
							intervalTrace?.Log($"EndSlot: {node.Label:X5}/{node.Offset} : {register} = {endSlots[r]} [KillSite]");
						}
						else
						{
							register.AddLiveInterval(slotNext, slotNext);

							intervalTrace?.Log($"Range:   {node.Label:X5}/{node.Offset} : {register} = {slot} to {slotNext} [Add]");
						}
					}

					KillSite.Add(slot);
				}

				foreach (var result in node.Results)
				{
					if (!(result.IsVirtualRegister || (result.IsCPURegister && !result.Register.IsSpecial)))
						continue;

					var r = GetIndex(result);

					var register = VirtualRegisters[r];

					if (register.IsReserved)
						continue;

					var dual = node.Operands.Contains(result);

					var slotStart = dual ? slot : slotNext;

					intervalTrace?.Log($"Dual:    {dual}");

					if (endSlots[r].IsNotNull)
					{
						register.AddLiveInterval(slotStart, endSlots[r]);

						intervalTrace?.Log($"Range:   {node.Label:X5}/{node.Offset} : {register} = {slotStart} to {endSlots[r]} [Add]");

						endSlots[r] = SlotIndex.Null;

						intervalTrace?.Log($"EndSlot: {node.Label:X5}/{node.Offset} : {register} = {endSlots[r]} [Add]");
					}
					else
					{
						register.AddLiveInterval(slotStart, slotNext);

						intervalTrace?.Log($"Range:   {node.Label:X5}/{node.Offset} : {register} = {slotStart} to {slotNext} [Add - Output]");
					}
				}

				foreach (var operand in node.Operands)
				{
					if (!(operand.IsVirtualRegister || (operand.IsCPURegister && !operand.Register.IsSpecial)))
						continue;

					var r = GetIndex(operand);

					var register = VirtualRegisters[r];

					if (register.IsReserved)
						continue;

					if (endSlots[r].IsNotNull)
						continue;

					endSlots[r] = slot;

					intervalTrace?.Log($"EndSlot: {node.Label:X5}/{node.Offset} : {register} = {endSlots[r]} [Add - Input]");
				}
			}

			for (var r = 0; r < RegisterCount; r++)
			{
				if (endSlots[r].IsNotNull)
				{
					var register = VirtualRegisters[r];

					if (register.IsReserved)
						continue;

					register.AddLiveInterval(block.Start, endSlots[r]);

					intervalTrace?.Log($"Range:   {register} = {block.Start} to {endSlots[r]} [Block Start]");
				}
			}
		}

		KillSite.Sort();
	}

	protected SlotIndex FindKillAllSite(LiveInterval liveInterval)
	{
		// FUTURE: Optimization - KillAll is sorted!
		foreach (var slot in KillSite)
		{
			if (liveInterval.Contains(slot))
			{
				return slot;
			}
		}
		return SlotIndex.Null;
	}

	private void SplitIntervalsAtCallSites()
	{
		foreach (var virtualRegister in VirtualRegisters)
		{
			if (virtualRegister.IsPhysicalRegister)
				continue;

			for (var i = 0; i < virtualRegister.LiveIntervals.Count; i++)
			{
				var liveInterval = virtualRegister.LiveIntervals[i];

				if (liveInterval.ForceSpill)
					continue;

				if (liveInterval.IsEmpty)
					continue;

				var callSite = FindKillAllSite(liveInterval);

				if (callSite.IsNull)
					continue;

				if (liveInterval.End == callSite)
					continue;

				var low = FindSplit_LowerBoundary(liveInterval.LiveRange, callSite.Next);
				var high = FindSplit_UpperBoundary(liveInterval.LiveRange, callSite.Next);

				var newInternals = high == liveInterval.End
					? liveInterval.SplitAt(low)
					: liveInterval.SplitAt(low, high);

				UpdateLiveIntervals(liveInterval, newInternals, false);

				i = 0; // reset loop - list was modified
			}
		}
	}

	protected virtual void CollectMoveHints()
	{
		return;
	}

	private void TraceMoveHints()
	{
		var moveHintTrace = CreateTrace("MoveHints", 9);

		if (moveHintTrace == null)
			return;

		foreach (var moveHint in MoveHints)
		{
			moveHintTrace.Log(moveHint.Value.ToString());
		}
	}

	private ExtendedBlock GetContainingBlock(SlotIndex slot)
	{
		var node = GetNode(slot);
		var block = ExtendedBlocks[node.Block.Sequence];
		return block;
	}

	protected int GetLoopDepth(SlotIndex slot) => GetContainingBlock(slot).LoopDepth + 1;

	protected SlotIndex GetBlockEnd(SlotIndex slot) => GetContainingBlock(slot).End;

	protected SlotIndex GetBlockStart(SlotIndex slot) => GetContainingBlock(slot).Start;

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
		Debug.Assert(liveInterval.LiveIntervalTrack == null);

		// priority is based on allocation stage (primary, lower first) and interval size (secondary, higher first)
		var value = CalculatePriorityValue(liveInterval);

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
					Tracks[liveInterval.VirtualRegister.PhysicalRegister.Index].Add(liveInterval);

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

	protected bool TryPlaceLiveIntervalOnTrack(LiveInterval liveInterval, RegisterTrack track)
	{
		if (track.IsReserved)
		{
			//Trace?.Log($"  No - Reserved");
			return false;
		}

		if (track.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
		{
			//Trace?.Log($"  No - Type Mismatch");
			return false;
		}

		if (track.Intersects(liveInterval))
		{
			Trace?.Log($"  No - Intersected; track: {track}");
			return false;
		}

		Trace?.Log($"  Assigned live interval destination: {track}");

		track.Add(liveInterval);

		return true;
	}

	protected bool PlaceLiveIntervalOnAnyAvailableTrack(LiveInterval liveInterval)
	{
		foreach (var track in Tracks)
		{
			if (TryPlaceLiveIntervalOnTrack(liveInterval, track))
			{
				return true;
			}
		}

		return false;
	}

	protected bool PlaceLiveIntervalOnTrackAllowEvictions(LiveInterval liveInterval)
	{
		// find live interval(s) to evict based on spill costs
		foreach (var track in Tracks)
		{
			if (track.IsReserved)
				continue;

			if (track.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
				continue;

			var evict = true;
			var intersections = track.GetIntersections(liveInterval);

			foreach (var intersection in intersections)
			{
				if (intersection.SpillCost >= liveInterval.SpillCost
					|| intersection.SpillCost == int.MaxValue
					|| intersection.VirtualRegister.IsPhysicalRegister
					|| intersection.IsPhysicalRegister)
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
		//Debug.Assert(!liveInterval.IsSplit);

		Trace?.Log();
		Trace?.Log($"Processing Interval: {liveInterval} / Length: {liveInterval.Length} / Spill Cost: {liveInterval.SpillCost} / Stage: {liveInterval.Stage}");
		Trace?.Log($"  Defs ({liveInterval.LiveRange.DefCount}): {SlotsToString(liveInterval.DefPositions)}");
		Trace?.Log($"  Uses ({liveInterval.LiveRange.UseCount}): {SlotsToString(liveInterval.UsePositions)}");

		if (liveInterval.LiveIntervalTrack != null)
			Trace?.Log("ERROR!");

		Debug.Assert(liveInterval.LiveIntervalTrack == null);

		if (PlaceLiveInterval(liveInterval))
		{
			return;
		}

		// No live intervals to evict!
		Trace?.Log("  No live intervals to evict");

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

	#region Split Options

	protected SlotIndex FindAnySplitPoint(LiveRange liveRange)
	{
		var at = FindSplit_TrimFrontDef(liveRange);

		if (at.IsNotNull)
			return at;

		at = FindSplit_TrimBackUse(liveRange);

		if (at.IsNotNull)
			return at;

		at = FindSplit_AfterFirstUse(liveRange);

		if (at.IsNotNull)
			return at;

		at = FindSplit_AfterFirstDef(liveRange);

		if (at.IsNotNull)
			return at;

		at = FindSplit_TrimFrontUse(liveRange);

		if (at.IsNotNull)
			return at;

		// what remains is the smallest interval

		return SlotIndex.Null;
	}

	protected SlotIndex FindSplit_TrimFrontDef(LiveRange liveRange)
	{
		if (liveRange.IsEmpty
			|| liveRange.DefCount == 0
			|| liveRange.FirstDef == liveRange.Start)
			return SlotIndex.Null;

		var at = liveRange.FirstDef.Previous;

		while (liveRange.ContainsUseAt(at) || liveRange.ContainsDefAt(at))
		{
			at = at.Previous;

			if (at < liveRange.Start)
				return SlotIndex.Null;
		}

		return at;
	}

	protected SlotIndex FindSplit_TrimBackUse(LiveRange liveRange)
	{
		if (liveRange.IsEmpty
			|| liveRange.UseCount == 0
			|| liveRange.LastUse == liveRange.End)
			return SlotIndex.Null;

		var at = liveRange.LastUse.Next;

		while (liveRange.ContainsUseAt(at) || liveRange.ContainsDefAt(at))
		{
			at = at.Next;

			if (at > liveRange.End)
				return SlotIndex.Null;
		}

		return at;
	}

	protected SlotIndex FindSplit_AfterFirstUse(LiveRange liveRange)
	{
		if (liveRange.IsEmpty
			|| liveRange.UseCount == 0
			|| liveRange.FirstUse == liveRange.End)
			return SlotIndex.Null;

		var at = liveRange.FirstUse.Next;

		while (liveRange.ContainsUseAt(at) || liveRange.ContainsDefAt(at))
		{
			at = at.Next;

			if (at > liveRange.End)
				return SlotIndex.Null;
		}

		return at;
	}

	protected SlotIndex FindSplit_AfterFirstDef(LiveRange liveRange)
	{
		if (liveRange.IsEmpty
			|| liveRange.DefCount == 0
			|| liveRange.FirstDef == liveRange.End)
			return SlotIndex.Null;

		var at = liveRange.FirstDef.Next;

		while (liveRange.ContainsUseAt(at) || liveRange.ContainsDefAt(at))
		{
			at = at.Next;

			if (at > liveRange.End)
				return SlotIndex.Null;
		}

		return at;
	}

	protected SlotIndex FindSplit_TrimFrontUse(LiveRange liveRange)
	{
		if (liveRange.IsEmpty
			|| liveRange.UseCount == 0
			|| liveRange.FirstUse == liveRange.Start)
			return SlotIndex.Null;

		var at = liveRange.LastUse.Previous;

		while (liveRange.ContainsUseAt(at) || liveRange.ContainsDefAt(at))
		{
			at = at.Previous;

			if (at < liveRange.Start)
				return SlotIndex.Null;
		}

		return at;
	}

	protected SlotIndex FindSplit_LowerBoundary(LiveRange liveRange, SlotIndex at)
	{
		var block = GetBlockStart(at).GetClamp(liveRange.Start, liveRange.End);

		var use = liveRange.GetPreviousUse(at).GetNext();
		var def = liveRange.GetPreviousDef(at).GetNext();

		var max = SlotIndex.Max(block, use, def);

		return max;
	}

	protected SlotIndex FindSplit_UpperBoundary(LiveRange liveRange, SlotIndex at)
	{
		var block = GetBlockEnd(at).GetClamp(liveRange.Start, liveRange.End);
		var use = liveRange.GetNextUse(at).GetPrevious();
		var def = liveRange.GetNextDef(at).GetPrevious();

		var min = SlotIndex.Min(block, use, def);

		return min;
	}

	#endregion Split Options

	private void SplitLastResort(LiveInterval liveInterval)
	{
		// This is the last option when all other split options fail.
		// Split interval up into very small pieces that can always be placed.

		if (liveInterval.IsEmpty)
		{
			SpilledIntervals.Add(liveInterval);
			return;
		}

		Trace?.Log(" Splitting around first use/def");

		var splitAt = FindAnySplitPoint(liveInterval.LiveRange);

		if (!liveInterval.LiveRange.CanSplitAt(splitAt))
		{
			return;
		}

		if (splitAt.IsNull)
		{
			// can not be split any further
			return;
		}

		var intervals = liveInterval.SplitAt(splitAt);

		UpdateLiveIntervals(liveInterval, intervals, true);
	}

	protected void UpdateLiveIntervals(LiveInterval replacedInterval, List<LiveInterval> newIntervals, bool addToQueue)
	{
		CalculateSpillCosts(newIntervals);

		replacedInterval.VirtualRegister.ReplaceWithSplit(replacedInterval, newIntervals);

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
		for (var i = 0; i < array.Count; i++)
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

	protected void CreateSpillSlotOperands()
	{
		foreach (var register in VirtualRegisters)
		{
			if (!register.IsSpilled)
				continue;

			//if (!register.IsParamLoad || register.IsParamStore)
			//	continue;

			Debug.Assert(register.IsVirtualRegister);

			register.SpillSlotOperand = LocalStack.Allocate(register.VirtualRegisterOperand);
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

				liveInterval.AssignedPhysicalOperand = Operand.CreateCPURegister(liveInterval.VirtualRegister.VirtualRegisterOperand, liveInterval.AssignedPhysicalRegister);
			}
		}
	}

	protected void InsertSpillMoves()
	{
		foreach (var register in VirtualRegisters)
		{
			if (!register.IsUsed || register.IsPhysicalRegister || !register.IsSpilled)
				continue;

			//if (register.IsParamLoad && !register.IsParamStore)
			//	continue; // No store required

			foreach (var liveInterval in register.LiveIntervals)
			{
				foreach (var def in liveInterval.DefPositions)
				{
					// FUTURE: Spills after every definition; this can be improved

					var context = new Context(GetNode(def));

					Architecture.InsertStoreInstruction(context, StackFrame, register.SpillSlotOperand, liveInterval.AssignedPhysicalOperand);

					SpillMoves++;

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

	protected static void AssignPhysicalRegistersToInstructions(InstructionNode node, Operand old, Operand replacement)
	{
		for (var i = 0; i < node.OperandCount; i++)
		{
			var operand = node.GetOperand(i);

			if (operand == old)
			{
				node.SetOperand(i, replacement);
			}
		}

		for (var i = 0; i < node.ResultCount; i++)
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
				var fromAnchorFlag = from.BasicBlock.NextBlocks.Count == 1;

				var anchor = fromAnchorFlag ? from : to;
				var anchorIndex = fromAnchorFlag ? 0 : 1;

				var moveResolver = moveResolvers[anchorIndex, anchor.Sequence];

				if (moveResolver == null)
				{
					moveResolver = new MoveResolver(anchor.BasicBlock, from.BasicBlock, to.BasicBlock);
					moveResolvers[anchorIndex, anchor.Sequence] = moveResolver;
				}

				foreach (var virtualRegister in GetVirtualRegisters(to.LiveIn))
				{
					//if (virtualRegister.IsPhysicalRegister)
					//continue;

					var fromLiveInterval = virtualRegister.GetIntervalAt(from.End);
					var toLiveInterval = virtualRegister.GetIntervalAt(to.Start);

					Debug.Assert(fromLiveInterval != null);
					Debug.Assert(toLiveInterval != null);

					if (fromLiveInterval.AssignedPhysicalRegister != toLiveInterval.AssignedPhysicalRegister)
					{
						resolverTrace?.Log($"REGISTER: {fromLiveInterval.VirtualRegister}");
						resolverTrace?.Log($"    FROM: {from,-7} {fromLiveInterval.AssignedOperand}");
						resolverTrace?.Log($"      TO: {to,-7} {toLiveInterval.AssignedOperand}");

						resolverTrace?.Log($"  INSERT: {(fromAnchorFlag ? "FROM (bottom)" : "TO (Before)")}{(toLiveInterval.AssignedPhysicalOperand == null ? "  ****SKIPPED***" : string.Empty)}");
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

		for (var b = 0; b < BasicBlocks.Count; b++)
		{
			for (var fromTag = 0; fromTag < 2; fromTag++)
			{
				var moveResolver = moveResolvers[fromTag, b];

				if (moveResolver == null)
					continue;

				DataFlowMoves += moveResolver.InsertResolvingMoves(Architecture, StackFrame);
			}
		}
	}

	protected class MoveKeyedList : KeyedList<SlotIndex, OperandMove>
	{
		public void Add(SlotIndex at, Operand source, Operand destination)
		{
			Add(at, new OperandMove(source, destination));
		}
	}

	protected void InsertRegisterMoves()
	{
		var insertTrace = CreateTrace("InsertRegisterMoves", 9);

		var moves = GetRegisterMoves();

		foreach (var key in moves.Keys)
		{
			var moveResolver = new MoveResolver(GetNode(key), !key.IsAfterSlot, moves[key]);

			ResolvingMoves += moveResolver.InsertResolvingMoves(Architecture, StackFrame);

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
		var blockEdges = new HashSet<SlotIndex>();

		foreach (var block in ExtendedBlocks)
		{
			blockEdges.Add(block.Start);
			blockEdges.Add(block.End);
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
				if (blockEdges.Contains(currentInterval.End))
					continue;

				var currentInternalEndNext = currentInterval.End.Next;

				// List is not sorted, so scan thru each one
				foreach (var nextInterval in virtualRegister.LiveIntervals)
				{
					if (currentInternalEndNext != nextInterval.Start)
						continue;

					// same interval
					if (currentInterval == nextInterval)
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
					if (nextInterval.LiveRange.DefCount != 0
						&& (nextInterval.LiveRange.UseCount == 0 || nextInterval.LiveRange.FirstDef < nextInterval.LiveRange.FirstUse))
						continue;

					keyedList.Add(nextInterval.Start, currentInterval.AssignedOperand, nextInterval.AssignedOperand);

					break;
				}
			}
		}

		return keyedList;
	}

	protected int GetSpillCost(SlotIndex use, int factor) => factor * GetLoopDepth(use) * 100;
}
