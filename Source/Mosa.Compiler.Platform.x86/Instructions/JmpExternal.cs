// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x86.Instructions
{
	/// <summary>
	/// JmpExternal
	/// </summary>
	/// <seealso cref="X86Instruction" />
	public sealed class JmpExternal : X86Instruction
	{
		internal JmpExternal()
			: base(0, 1)
		{
		}

		public override FlowControl FlowControl { get { return FlowControl.UnconditionalBranch; } }

		public override bool IsZeroFlagUnchanged { get { return true; } }

		public override bool IsZeroFlagUndefined { get { return true; } }

		public override bool IsCarryFlagUnchanged { get { return true; } }

		public override bool IsCarryFlagUndefined { get { return true; } }

		public override bool IsSignFlagUnchanged { get { return true; } }

		public override bool IsSignFlagUndefined { get { return true; } }

		public override bool IsOverflowFlagUnchanged { get { return true; } }

		public override bool IsOverflowFlagUndefined { get { return true; } }

		public override bool IsParityFlagUnchanged { get { return true; } }

		public override bool IsParityFlagUndefined { get { return true; } }

		public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 0);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			if (node.Operand1.IsCPURegister)
			{
				opcodeEncoder.Append8Bits(0xFF);
				opcodeEncoder.Append2Bits(0b11);
				opcodeEncoder.Append3Bits(0b100);
				opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
				return;
			}

			if (node.Operand1.IsConstant)
			{
				opcodeEncoder.Append8Bits(0xE9);
				opcodeEncoder.EmitRelative32(node.Operand1);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
