/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework;

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
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand result = context.Result;
			Operand edx = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.Pointer);
			Operand operand = Operand.CreateMemoryAddress(context.Operand1.Type, edx, 0);

			context.SetInstruction(X86.Mov, edx, context.Operand1);
			context.AppendInstruction(X86.Mov, result, operand);
		}

		#endregion Methods
	}
}