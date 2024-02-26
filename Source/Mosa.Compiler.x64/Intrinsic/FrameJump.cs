// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::FrameJump")]
	private static void FrameJump(Context context, Transform transform)
	{
		var v0 = context.Operand1;
		var v1 = context.Operand2;
		var v2 = context.Operand3;
		var v3 = context.Operand4;

		var rsp = transform.PhysicalRegisters.Allocate64(CPURegister.RSP);
		var rbp = transform.PhysicalRegisters.Allocate64(CPURegister.RBP);

		var rax = transform.PhysicalRegisters.Allocate64(CPURegister.RAX);
		var rbx = transform.PhysicalRegisters.Allocate64(CPURegister.RBX);
		var rcx = transform.PhysicalRegisters.Allocate64(CPURegister.RCX);

		// Move all virtual registers into physical registers - necessary since stack frame pointer will change
		context.SetInstruction(X64.Mov64, rax, v0);
		context.AppendInstruction(X64.Mov64, rbx, v1);
		context.AppendInstruction(X64.Mov64, rcx, v2);
		context.AppendInstruction(X64.Mov64, transform.ExceptionRegister, v3);

		// Update the frame and stack registers
		context.AppendInstruction(X64.Mov64, rbp, rcx);
		context.AppendInstruction(X64.Mov64, rsp, rbx);
		context.AppendInstruction(X64.JmpExternal, null, rax);

		// future - common code (refactor opportunity)
		context.GotoNext();

		// Remove all remaining instructions in block and clear next block list
		while (!context.IsBlockEndInstruction)
		{
			if (!context.IsEmpty)
			{
				context.Empty();
			}
			context.GotoNext();
		}

		var nextBlocks = context.Block.NextBlocks;

		foreach (var next in nextBlocks)
		{
			next.PreviousBlocks.Remove(context.Block);
		}

		nextBlocks.Clear();
	}
}
