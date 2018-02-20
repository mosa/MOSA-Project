// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Get16
	/// </summary>
	internal sealed class Get16 : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			Debug.Assert(context.Result.IsI4 | context.Result.IsU4);
			Operand zero = Operand.CreateConstant(0, methodCompiler.TypeSystem);

			context.SetInstruction(X86.MovzxLoad16, context.Result, context.Operand1, zero);
		}

		#endregion Methods
	}
}
