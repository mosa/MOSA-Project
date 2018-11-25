// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;
using System.Diagnostics;

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
