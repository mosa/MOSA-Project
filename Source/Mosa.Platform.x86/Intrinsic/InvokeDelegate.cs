/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 cli instruction.
	/// </summary>
	public sealed class InvokeDelegate : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			var result = context.Result;
			var op1 = context.Operand1;
			var op2 = context.Operand2;

			var eax = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX);
			var edx = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EDX);
			var esp = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.ESP);
			var ebp = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EBP);
			context.SetInstruction(CPUx86.Instruction.SubInstruction, esp, new ConstantOperand(new SigType(CilElementType.I), parameters.Count * 4));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, edx, esp);

			var size = parameters.Count * 4;
			foreach (var parameter in parameters)
			{
				context.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(new SigType(CilElementType.I), edx.Register, new IntPtr(size - 4)), new MemoryOperand(new SigType(CilElementType.I), ebp.Register, new IntPtr(size + 8)));
				size -= 4;
			}

			context.AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op2);
			context.AppendInstruction(CPUx86.Instruction.CallPointerInstruction, null, new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX));
			context.AppendInstruction(CPUx86.Instruction.AddInstruction, esp, new ConstantOperand(new SigType(CilElementType.I), parameters.Count * 4));
		}

		#endregion // Methods

	}
}
