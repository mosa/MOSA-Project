// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// BtrConst64
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class BtrConst64 : X64Instruction
	{
		public override int ID { get { return 417; } }

		internal BtrConst64()
			: base(1, 2)
		{
		}

		public static readonly LegacyOpCode LegacyOpcode = new LegacyOpCode(new byte[] { 0x0F, 0xB3 }, 0x06);

		public override bool ThreeTwoAddressConversion { get { return true; } }

		public override bool IsCarryFlagModified { get { return true; } }

		public override bool IsSignFlagUnchanged { get { return true; } }

		public override bool IsSignFlagUndefined { get { return true; } }

		public override bool IsOverflowFlagUnchanged { get { return true; } }

		public override bool IsOverflowFlagUndefined { get { return true; } }

		public override bool IsParityFlagUnchanged { get { return true; } }

		public override bool IsParityFlagUndefined { get { return true; } }

		internal override void EmitLegacy(InstructionNode node, X64CodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);
			System.Diagnostics.Debug.Assert(node.Result.IsCPURegister);
			System.Diagnostics.Debug.Assert(node.Operand1.IsCPURegister);
			System.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);

			emitter.Emit(LegacyOpcode, node.Result, node.Operand2);
		}
	}
}
