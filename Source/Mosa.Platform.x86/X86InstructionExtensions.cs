/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 adc instruction.
	/// </summary>
	public static class X86InstructionExtensions
	{
		// this is just play for now...

		public static void Adc(this InsertContext insert, Operand destination, Operand source)
		{
			insert.Context.AppendInstruction(X86.Add, source, destination);
		}

		public static void Adc(this SetContext set, Operand destination, Operand source)
		{
			set.Context.SetInstruction(X86.Add, source, destination);
		}

		public static void Add(this InsertContext insert, Operand destination, Operand source)
		{
			insert.Context.AppendInstruction(X86.Add, source, destination);
		}

		public static void Add(this SetContext set, Operand destination, Operand source)
		{
			set.Context.AppendInstruction(X86.Add, source, destination);
		}

	}
}
