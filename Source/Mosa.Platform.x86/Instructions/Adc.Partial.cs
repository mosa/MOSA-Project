// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 adc instruction.
	/// </summary>
	public sealed partial class Adc
	{
		#region Data members

		private static readonly LegacyOpCode RM_C = new LegacyOpCode(new byte[] { 0x81 }, 2);
		private static readonly LegacyOpCode R_RM = new LegacyOpCode(new byte[] { 0x13 });
		private static readonly LegacyOpCode M_R = new LegacyOpCode(new byte[] { 0x11 });

		#endregion Data members

		#region Methods

		internal static LegacyOpCode GetOpcode(Operand destination, Operand operand2)
		{
			if (destination.IsCPURegister && operand2.IsConstant) return RM_C;
			if (destination.IsCPURegister && operand2.IsCPURegister) return R_RM;

			throw new ArgumentException("No opcode for operand type.");
		}

		internal static void EmitOpcode(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);

			var opCode = GetOpcode(node.Result, node.Operand2);

			(emitter as X86CodeEmitter).Emit(opCode, node.Result, node.Operand2);
		}

		#endregion Methods
	}
}
