// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// GetObjectAddress
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicInternalMethod" />
	[ReplacementTarget("Mosa.Runtime.Intrinsic::GetDelegateMethodAddress")]
	public sealed class GetDelegateMethodAddress : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var load = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.LoadInt32 : IRInstruction.LoadInt64;

			context.SetInstruction(load, context.Result, context.Operand1, methodCompiler.CreateConstant(2 * methodCompiler.Architecture.NativePointerSize));

		}
	}
}
