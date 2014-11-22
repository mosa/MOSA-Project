/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("Mosa.Internal.Native::GetObjectAddress")]
	public sealed class GetObjectAddress : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			context.SetInstruction(IRInstruction.AddressOf, context.Result, context.Operand1);
		}
	}
}