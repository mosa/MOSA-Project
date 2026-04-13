// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Linker;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Emits the GC safepoint metadata table for each compiled method.
/// Runs after <see cref="CodeGenerationStage"/> so that instruction code offsets are available.
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

		if (MethodData.SafePointEntries.Count == 0)
			return;

		EmitSafePointTable();
		EmitGCData();
	}

	private void CollectSafePoints()
	{
		MethodData.SafePointEntries.Clear();

		foreach (var block in BasicBlocks)
		{
			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (node.Instruction != IR.SafePoint)
					continue;

				var offset = GetNextCodeOffset(node);
				var (registerBitmap, typeBitmap) = BuildBitmaps(node);

				MethodData.SafePointEntries.Add(new SafePointEntry
				{
					Offset = offset,
					RegisterBitmap = registerBitmap,
					TypeBitmap = typeBitmap,
				});
			}
		}
	}

	/// <summary>
	/// Returns the method-relative code offset of the first real instruction following <paramref name="node"/>.
	/// Since <see cref="IR.SafePoint"/> has <c>IgnoreDuringCodeGeneration = true</c> it emits no bytes; the
	/// "safepoint address" is therefore the address of the next emitted instruction.
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

	/// <summary>
	/// Builds the register and type bitmaps from the live GC-root operands attached to the SafePoint node
	/// by <see cref="SafePointStage.AnnotateSafePoints"/>.
	/// Bit N in each bitmap corresponds to the register whose <see cref="PhysicalRegister.Index"/> is N.
	/// In the type bitmap: 0 = object reference, 1 = managed pointer.
	/// </summary>
	private static (uint registerBitmap, uint typeBitmap) BuildBitmaps(Node node)
	{
		uint registerBitmap = 0;
		uint typeBitmap = 0;

		foreach (var operand in node.Operands)
		{
			if (!operand.IsPhysicalRegister)
				continue;

			var bit = operand.Register.Index;

			if (bit >= 32)
				continue;

			registerBitmap |= 1u << bit;

			if (operand.IsManagedPointer)
				typeBitmap |= 1u << bit;
		}

		return (registerBitmap, typeBitmap);
	}

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
		writer.Write((uint)MethodData.SafePointEntries.Count);

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

	private void EmitGCData()
	{
		var gcDataSymbol = Linker.DefineSymbol(
			Metadata.GCData + Method.FullName,
			SectionKind.ROData,
			Architecture.NativeAlignment,
			0);

		var writer = new BinaryWriter(gcDataSymbol.Stream);

		// 1. Pointer to SafePoint Table
		Linker.Link(LinkType.AbsoluteAddress, NativePatchType, gcDataSymbol, writer.GetPosition(),
			Metadata.SafePointTable + Method.FullName, 0);
		writer.WriteZeroBytes(TypeLayout.NativePointerSize);

		// 2. Pointer to Method GC Stack Data (not yet implemented)
		writer.WriteZeroBytes(TypeLayout.NativePointerSize);
	}
}
