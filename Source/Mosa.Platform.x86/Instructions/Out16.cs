// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Out16
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class Out16 : X86Instruction
	{
		public override int ID { get { return 274; } }

		internal Out16()
			: base(0, 2)
		{
		}

		public override bool IsIOOperation { get { return true; } }

		public override bool HasUnspecifiedSideEffect { get { return true; } }

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 0);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);

			if (node.Operand1.IsCPURegister)
			{
				emitter.OpcodeEncoder.AppendByte(0xEF);
				return;
			}

			if (node.Operand1.IsConstant)
			{
				emitter.OpcodeEncoder.AppendByte(0xE7);
				emitter.OpcodeEncoder.Append8BitImmediate(node.Operand1);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
