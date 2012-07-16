/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */


using System.Collections.Generic;
using Mosa.Compiler.Framework;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 out instruction.
	/// </summary>
	public sealed class Out : IIntrinsicPlatformMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand edx = Operand.CreateCPURegister(operand1.Type, GeneralPurposeRegister.EDX);
			Operand eax = Operand.CreateCPURegister(operand2.Type, GeneralPurposeRegister.EAX);

			context.SetInstruction(X86.Mov, edx, operand1);
			context.AppendInstruction(X86.Mov, eax, operand2);
			context.AppendInstruction(X86.Out, null, edx, eax);
		}

		#endregion // Methods

	}
}
