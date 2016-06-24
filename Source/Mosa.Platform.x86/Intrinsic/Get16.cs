// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal sealed class Get16 : IIntrinsicPlatformMethod
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
			Operand zero = Operand.CreateConstant(methodCompiler.TypeSystem, 0);

			context.SetInstruction(X86.MovzxLoad, InstructionSize.Size16, context.Result, context.Operand1, zero);
		}

		#endregion Methods
	}
}
