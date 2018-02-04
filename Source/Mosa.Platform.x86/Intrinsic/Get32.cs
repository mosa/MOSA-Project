// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Get32
	/// </summary>
	internal sealed class Get32 : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Debug.Assert(context.Result.IsI4 | context.Result.IsU4);
			var zero = Operand.CreateConstant(0, methodCompiler.TypeSystem);

			context.SetInstruction(X86.MovLoad32, InstructionSize.Size32, context.Result, context.Operand1, zero);
		}

		#endregion Methods
	}
}
