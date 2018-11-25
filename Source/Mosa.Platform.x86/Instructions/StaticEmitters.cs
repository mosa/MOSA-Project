// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	public static class StaticEmitters
	{
		internal static void EmitJmpFar(InstructionNode node, BaseCodeEmitter emitter)
		{
			(emitter as X86CodeEmitter).EmitFarJumpToNextInstruction();
		}
	}
}
