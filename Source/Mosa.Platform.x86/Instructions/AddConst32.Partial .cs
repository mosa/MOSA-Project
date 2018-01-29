// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	public sealed partial class AddConst32
	{
		internal static void EmitOpcode(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand2.IsConstant);

			(emitter as X86CodeEmitter).Emit(legacyOpcode, node.Result, node.Operand2);
		}
	}
}
