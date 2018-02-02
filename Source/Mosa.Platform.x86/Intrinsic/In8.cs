// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 in instruction.
	/// </summary>
	internal sealed class In8 : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand v1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U4);

			var result = context.Result;

			context.SetInstruction(X86.In8, InstructionSize.Size8, v1, context.Operand1);
			context.AppendInstruction(X86.Movzx8To32, InstructionSize.Size16, result, v1);

			//context.SetInstruction(X86.In8, InstructionSize.Size8, context.Result, context.Operand1);
		}

		#endregion Methods
	}
}
