// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.x86.Stages;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// LockSet
	/// </summary>
	internal sealed class LockSet : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var zero = Operand.CreateConstant(0, methodCompiler.TypeSystem);

			context.SetInstruction(X86.MovStore32, null, context.Operand1, zero, context.Operand2);
		}

		#endregion Methods
	}
}
