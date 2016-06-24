// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal sealed class Memcpy256 : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var dest = context.Operand1;
			var src = context.Operand2;

			// The size will be 128 bits
			var v1 = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.Void, SSE2Register.XMM1);
			var v2 = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.Void, SSE2Register.XMM2);
			var memDest1 = Operand.CreateMemoryAddress(methodCompiler.TypeSystem.BuiltIn.Void, dest, 0);
			var memDest2 = Operand.CreateMemoryAddress(methodCompiler.TypeSystem.BuiltIn.Void, dest, 16);
			var memSrc1 = Operand.CreateMemoryAddress(methodCompiler.TypeSystem.BuiltIn.Void, src, 0);
			var memSrc2 = Operand.CreateMemoryAddress(methodCompiler.TypeSystem.BuiltIn.Void, src, 16);

			context.SetInstruction(X86.Movups, InstructionSize.Size128, v1, memSrc1);
			context.AppendInstruction(X86.Movups, InstructionSize.Size128, v2, memSrc2);
			context.AppendInstruction(X86.Movups, InstructionSize.Size128, memDest1, v1);
			context.AppendInstruction(X86.Movups, InstructionSize.Size128, memDest2, v2);
		}

		#endregion Methods
	}
}
