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
	[ReplacementTarget("Mosa.Runtime.Intrinsic::GetDelegateTargetAddress")]
	public sealed class GetDelegateTargetAddress : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var load = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.LoadInteger32 : IRInstruction.LoadInteger64;

			context.SetInstruction(load, context.Result, context.Operand1, methodCompiler.CreateConstant(3 * methodCompiler.Architecture.NativePointerSize));

		}
	}
}
