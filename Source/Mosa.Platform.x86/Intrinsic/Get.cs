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
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class Get : IIntrinsicPlatformMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			Operand result = context.Result;

			Operand tmp = Operand.CreateCPURegister(BuiltInSigType.Ptr, GeneralPurposeRegister.EDX);
			Operand operand = Operand.CreateMemoryAddress(context.Operand1.Type, GeneralPurposeRegister.EDX, new System.IntPtr(0));

			context.SetInstruction(X86.Mov, tmp, context.Operand1);
			context.AppendInstruction(X86.Mov, result, operand);
		}

		#endregion // Methods

	}
}
