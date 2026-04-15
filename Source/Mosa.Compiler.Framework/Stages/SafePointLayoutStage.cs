// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Linker;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Emits GC metadata for each compiled method:
/// <list type="bullet">
///   <item>SafePoint Table — per-safepoint register/type bitmaps for the breakpoint-based GC pause mechanism.</item>
///   <item>GC Stack Data — per-slot live ranges covering the entire method so the GC can scan a thread paused at any instruction.</item>
///   <item>GC Data — root record that links the two tables and is referenced from the Method Definition.</item>
/// </list>
/// Must run after <see cref="CodeGenerationStage"/> so that instruction code offsets are available.
/// </summary>
public sealed class SafePointLayoutStage : BaseMethodCompilerStage
{
	private PatchType NativePatchType;

	protected override void Initialize()
	{
		NativePatchType = TypeLayout.NativePointerSize == 4 ? PatchType.I32 : PatchType.I64;
	}

	protected override void Run()
	{
		if (!HasCode)
			return;

		CollectSafePoints();
		BuildGCStackMap();

		var hasSafePoints = MethodData.SafePointEntries.Count > 0;
		var hasStackData = MethodData.GCStackEntries.Count > 0;

		if (!hasSafePoints && !hasStackData)
			return;

		if (hasSafePoints)
			EmitSafePointTable();

		if (hasStackData)
			EmitGCStackData();

		EmitGCData();
	}

	#region SafePoint collection

	private void CollectSafePoints()
	{
		MethodData.SafePointEntries.Clear();

		foreach (var block in BasicBlocks)
		{
			for (var node = block.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (node.Instruction != IR.SafePoint)
					continue;

				MethodData.SafePointEntries.Add(new SafePointEntry
				{
					Offset = GetNextCodeOffset(node),
					RegisterBitmap = node.Operand1 == null ? 0u : node.Operand1.ConstantUnsigned32,
					TypeBitmap = node.Operand2 == null ? 0u : node.Operand2.ConstantUnsigned32,
				});

				break;
			}
		}
	}

	/// <summary>
	/// Returns the method-relative code offset of the first real instruction following <paramref name="node"/>.
	/// Because <see cref="IR.SafePoint"/> has <c>IgnoreDuringCodeGeneration = true</c> it emits no bytes; the
	/// effective safepoint address is therefore the address of the next emitted instruction.
	/// </summary>
	private static int GetNextCodeOffset(Node node)
	{
		for (var n = node.Next; n != null; n = n.Next)
		{
			if (n.IsBlockEndInstruction)
				return n.Offset;

			if (!n.IsEmptyOrNop && n.Instruction != null && !n.Instruction.IgnoreDuringCodeGeneration)
				return n.Offset;
		}

		return 0;
	}

	#endregion

	#region GC stack map

	/// <summary>
	/// Populates <see cref="MethodData.GCStackEntries"/> with live-range information for every
	/// GC-root stack slot so the GC can scan a thread paused at any instruction.
	/// <para>
	/// Parameters are conservatively marked live for the whole method (the caller holds the only
	/// reference). Locals use block-level backward liveness seeded from the nodes in
	/// <see cref="Operand.Uses"/> and bounded by per-block definitions/KILL information from
	/// <see cref="Operand.Definitions"/>, so ranges are tight while remaining safe.
	/// </para>
	/// </summary>
	private void BuildGCStackMap()
	{
		MethodData.GCStackEntries.Clear();

		var methodEnd = GetMethodEndOffset();

		// Parameters: conservatively live for the entire method.
		foreach (var param in MethodCompiler.Parameters)
		{
			if (!param.IsObject && !param.IsManagedPointer)
				continue;

			MethodData.GCStackEntries.Add(new GCStackEntry
			{
				StackOffset = (int)param.Offset,
				IsObject = param.IsObject,
				Start = 0,
				Length = methodEnd,
			});
		}

		// Locals: compute block-level live ranges from the IR use-def chains.
		foreach (var local in MethodCompiler.LocalStack)
		{
			if (!local.IsResolved || !local.IsUsed)
				continue;

			if (!local.IsObject && !local.IsManagedPointer)
				continue;

			var ranges = ComputeLocalLiveRanges(local);

			if (ranges.Count == 0)
				continue;

			foreach (var (start, length) in ranges)
			{
				MethodData.GCStackEntries.Add(new GCStackEntry
				{
					StackOffset = (int)local.Offset,
					IsObject = local.IsObject,
					Start = start,
					Length = length,
				});
			}
		}
	}

	/// <summary>
	/// Computes the live ranges for a single GC-root local stack operand using block-level backward
	/// liveness with GEN and KILL sets derived from the IR use-def chains.
	/// <para>
	/// GEN[B]  = true when B contains an upward-exposed use (a read of the slot before any write in B).<br/>
	/// KILL[B] = true when B contains a definition (store) of the slot, preventing liveness from
	///           propagating backward past the definition into predecessor blocks so that blocks
	///           before the first store are not incorrectly reported as live.
	/// </para>
	/// </summary>
	private List<(int Start, int Length)> ComputeLocalLiveRanges(Operand operand)
	{
		var numBlocks = BasicBlocks.Count;
		var gen = new bool[numBlocks];
		var kill = new bool[numBlocks];

		// Build per-block use/kill sets from the IR use-def chains.
		var hasUse = new bool[numBlocks];

		foreach (var node in operand.Uses)
		{
			var block = node.Block;

			if (block == null || block.Sequence < 0 || block.Sequence >= numBlocks)
				continue;

			hasUse[block.Sequence] = true;
		}

		foreach (var node in operand.Definitions)
		{
			var block = node.Block;

			if (block == null || block.Sequence < 0 || block.Sequence >= numBlocks)
				continue;

			kill[block.Sequence] = true;
		}

		// gen[B] = upward-exposed use:
		//   - block has only uses (no def): gen = true
		//   - block has both: scan forward to detect whether any use precedes the first def
		for (var i = 0; i < numBlocks; i++)
		{
			if (!hasUse[i])
				continue;

			gen[i] = !kill[i] || HasUseBeforeFirstDefInBlock(operand, BasicBlocks[i]);
		}

		// Backward dataflow fixpoint:
		//   liveIn[B]  = gen[B] | (liveOut[B] & ~kill[B])
		//   liveOut[B] = OR of liveIn[S] for all successors S
		var liveIn = new bool[numBlocks];
		var liveOut = new bool[numBlocks];

		bool changed;
		do
		{
			changed = false;

			for (var i = numBlocks - 1; i >= 0; i--)
			{
				var block = BasicBlocks[i];

				var newOut = false;
				foreach (var succ in block.NextBlocks)
				{
					if (liveIn[succ.Sequence]) { newOut = true; break; }
				}

				var newIn = gen[i] || (newOut && !kill[i]);

				if (newOut == liveOut[i] && newIn == liveIn[i])
					continue;

				liveOut[i] = newOut;
				liveIn[i] = newIn;
				changed = true;
			}
		} while (changed);

		// Collect code ranges for each live block.
		var codeRanges = new List<(int Start, int End)>(numBlocks);

		for (var i = 0; i < numBlocks; i++)
		{
			if (!liveIn[i] && !liveOut[i])
				continue;

			var block = BasicBlocks[i];

			if (!MethodCompiler.Labels.TryGetValue(block.Label, out var start))
				continue;

			if (!MethodCompiler.Labels.TryGetValue(block.EndLabel, out var end))
				continue;

			if (end > start)
				codeRanges.Add((start, end));
		}

		return MergeRanges(codeRanges);
	}

	/// <summary>
	/// Returns <c>true</c> if <paramref name="operand"/> is read before its first write in
	/// <paramref name="block"/>, scanning instructions in forward order.
	/// </summary>
	private static bool HasUseBeforeFirstDefInBlock(Operand operand, BasicBlock block)
	{
		for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmptyOrNop)
				continue;

			foreach (var use in node.Operands)
			{
				if (use == operand)
					return true; // use seen before any def in this block
			}

			foreach (var result in node.Results)
			{
				if (result == operand)
					return false; // def seen before any use in this block
			}
		}

		return false;
	}

	private int GetMethodEndOffset()
	{
		var max = 0;

		foreach (var block in BasicBlocks)
		{
			if (MethodCompiler.Labels.TryGetValue(block.EndLabel, out var end) && end > max)
				max = end;
		}

		return max;
	}

	/// <summary>
	/// Merges overlapping or adjacent <c>(Start, End)</c> intervals and converts them to
	/// <c>(Start, Length)</c> tuples.
	/// </summary>
	private static List<(int Start, int Length)> MergeRanges(List<(int Start, int End)> ranges)
	{
		if (ranges.Count == 0)
			return [];

		ranges.Sort(static (a, b) => a.Start.CompareTo(b.Start));

		var merged = new List<(int Start, int Length)>(ranges.Count);
		var (curStart, curEnd) = ranges[0];

		for (var i = 1; i < ranges.Count; i++)
		{
			var (s, e) = ranges[i];

			if (s <= curEnd)
				curEnd = Math.Max(curEnd, e);
			else
			{
				merged.Add((curStart, curEnd - curStart));
				(curStart, curEnd) = (s, e);
			}
		}

		merged.Add((curStart, curEnd - curStart));
		return merged;
	}

	#endregion

	#region Emission

	private void EmitSafePointTable()
	{
		var trace = CreateTraceLog("SafePoints", 5);

		var tableSymbol = Linker.DefineSymbol(
			Metadata.SafePointTable + Method.FullName,
			SectionKind.ROData,
			Architecture.NativeAlignment,
			0);

		var writer = new BinaryWriter(tableSymbol.Stream);

		// 1. Number of SafePoints
		writer.Write((uint)MethodData.SafePointEntries.Count, TypeLayout.NativePointerSize);

		trace?.Log($"SafePoint Table: {Method.FullName} ({MethodData.SafePointEntries.Count} entries)");

		foreach (var entry in MethodData.SafePointEntries)
		{
			// 2. Address Offset (method-relative, native pointer width)
			writer.Write((uint)entry.Offset, TypeLayout.NativePointerSize);

			// 3. Address Range (0 = single-address safepoint)
			writer.Write(0u, TypeLayout.NativePointerSize);

			// 4. CPU Registers Bitmap (32-bit)
			writer.Write(entry.RegisterBitmap);

			// 5. Type Bitmap (32-bit)
			writer.Write(entry.TypeBitmap);

			// 6. Breakpoint Indicator (0 = no breakpoint currently installed)
			writer.Write(0u);

			trace?.Log($"  Offset=0x{entry.Offset:X4}  RegBitmap=0b{Convert.ToString(entry.RegisterBitmap, 2).PadLeft(32, '0')}  TypeBitmap=0b{Convert.ToString(entry.TypeBitmap, 2).PadLeft(32, '0')}");
		}
	}

	private void EmitGCStackData()
	{
		var trace = CreateTraceLog("GCStack", 5);

		var stackDataSymbol = Linker.DefineSymbol(
			Metadata.GCStackData + Method.FullName,
			SectionKind.ROData,
			Architecture.NativeAlignment,
			0);

		var stackDataWriter = new BinaryWriter(stackDataSymbol.Stream);

		// 1. Number of GC Stack Entries (back-filled below)
		stackDataWriter.Write((uint)0);

		trace?.Log($"GC Stack Data: {Method.FullName} ({MethodData.GCStackEntries.Count} entries)");

		var count = 0;

		foreach (var entry in MethodData.GCStackEntries)
		{
			count++;

			var entrySymbol = CreateGCStackEntrySymbol(count, entry, trace);

			// 2. Pointer to GC Stack Entry
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, stackDataSymbol, stackDataWriter.GetPosition(), entrySymbol, 0);
			stackDataWriter.WriteZeroBytes(TypeLayout.NativePointerSize);
		}

		// Back-fill count
		stackDataWriter.SetPosition(0);
		stackDataWriter.Write((uint)count);
	}

	private LinkerSymbol CreateGCStackEntrySymbol(int index, GCStackEntry entry, TraceLog trace)
	{
		var name = $"{Metadata.GCStackEntry}{Method.FullName}${index}";
		var symbol = Linker.DefineSymbol(name, SectionKind.ROData, Architecture.NativeAlignment, 0);
		var writer = new BinaryWriter(symbol.Stream);

		// 1. Stack Offset (frame-relative, native pointer width for alignment)
		writer.Write(entry.StackOffset, TypeLayout.NativePointerSize);

		// 2. Type: 0 = managed pointer, 1 = object reference
		writer.Write(entry.IsObject ? 1u : 0u, TypeLayout.NativePointerSize);

		// 3. Number of Live Ranges
		writer.Write(1u);

		trace?.Log($"  Entry #{index}: StackOffset={entry.StackOffset} Type={(entry.IsObject ? "Object" : "ManagedPointer")} Ranges=1");

		// 4a. Address Offset (method-relative)
		writer.Write((uint)entry.Start, TypeLayout.NativePointerSize);

		// 4b. Address Range (length in bytes)
		writer.Write((uint)entry.Length, TypeLayout.NativePointerSize);

		trace?.Log($"    Range: 0x{entry.Start:X4} len=0x{entry.Length:X4}");

		return symbol;
	}

	private void EmitGCData()
	{
		var hasSafePoints = MethodData.SafePointEntries.Count > 0;
		var hasStackData = MethodData.GCStackEntries.Count > 0;

		var gcDataSymbol = Linker.DefineSymbol(
			Metadata.GCData + Method.FullName,
			SectionKind.ROData,
			Architecture.NativeAlignment,
			0);

		var writer = new BinaryWriter(gcDataSymbol.Stream);

		// 1. Pointer to SafePoint Table (null if no safepoints)
		if (hasSafePoints)
		{
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, gcDataSymbol, writer.GetPosition(),
				Metadata.SafePointTable + Method.FullName, 0);
		}
		writer.WriteZeroBytes(TypeLayout.NativePointerSize);

		// 2. Pointer to Method GC Stack Data (null if no stack GC roots)
		if (hasStackData)
		{
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, gcDataSymbol, writer.GetPosition(),
				Metadata.GCStackData + Method.FullName, 0);
		}
		writer.WriteZeroBytes(TypeLayout.NativePointerSize);
	}

	#endregion
}
