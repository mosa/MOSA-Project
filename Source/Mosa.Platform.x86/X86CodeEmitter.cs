// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using System;
using System.Diagnostics;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// An x86 machine code emitter.
	/// </summary>
	public sealed class X86CodeEmitter : BaseCodeEmitter
	{
		/// <summary>
		/// Emits a far jump to next instruction.
		/// </summary>
		public void EmitFarJumpToNextInstruction()
		{
			CodeStream.WriteByte(0xEA);

			Linker.Link(LinkType.AbsoluteAddress, PatchType.I4, SectionKind.Text, MethodName, (int)CodeStream.Position, SectionKind.Text, MethodName, (int)CodeStream.Position + 6);

			CodeStream.WriteZeroBytes(4);
			CodeStream.WriteByte(0x08);
			CodeStream.WriteByte(0x00);
		}
	}
}
