/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class SwitchTask : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem)
		{
			SigType I4 = new SigType(CilElementType.I4);
			RegisterOperand esp = new RegisterOperand(I4, GeneralPurposeRegister.ESP);

			context.SetInstruction(CPUx86.Instruction.MovInstruction, esp, context.Operand1);
			context.AppendInstruction(CPUx86.Instruction.PopadInstruction);
			context.AppendInstruction(CPUx86.Instruction.AddInstruction, esp, new ConstantOperand(I4, 0x08));
			context.AppendInstruction(CPUx86.Instruction.StiInstruction);
			context.AppendInstruction(CPUx86.Instruction.IRetdInstruction);
		}

		#endregion // Methods

	}
}
