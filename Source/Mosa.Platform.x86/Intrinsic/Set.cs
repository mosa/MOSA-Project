// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal sealed class Set : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand zero = Operand.CreateConstant(methodCompiler.TypeSystem, 0);
			var size = BaseMethodCompilerStage.GetInstructionSize(context.Size, context.InvokeMethod.Signature.Parameters[1].ParameterType);
			context.SetInstruction(X86.MovStore, size, null, context.Operand1, zero, context.Operand2);
		}

		#endregion Methods
	}
}
