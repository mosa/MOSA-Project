// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Linker;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Emits the SafePoint Table for each compiled method.
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

		if (MethodData.SafePointEntries.Count == 0)
			return;

		EmitSafePointTable();
		EmitGCData();
	}

	#region SafePoint Collection

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

	#endregion SafePoint Collection

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

		// 2. Pointer to Method GC Stack Data (null — not emitted)
		writer.WriteZeroBytes(TypeLayout.NativePointerSize);
	}

	#endregion Emission
}
