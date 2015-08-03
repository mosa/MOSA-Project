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
			Operand result = context.Result;
			Operand v1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.Pointer);
			Operand operand = Operand.CreateMemoryAddress(methodCompiler.TypeSystem.BuiltIn.U2, v1, 0);

			Debug.Assert(result.IsI4 | result.IsU4);

			context.SetInstruction(X86.Mov, v1, context.Operand1);
			context.AppendInstruction(X86.Movzx, InstructionSize.Size16, result, operand);
		}

		#endregion Methods
	}
}
