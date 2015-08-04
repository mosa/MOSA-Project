// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal sealed class Memclr256 : IIntrinsicPlatformMethod
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

			// The size will be 128 bits
			var v0 = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.Void, SSE2Register.XMM0);
			var memDest1 = Operand.CreateMemoryAddress(methodCompiler.TypeSystem.BuiltIn.Void, dest, 0);
			var memDest2 = Operand.CreateMemoryAddress(methodCompiler.TypeSystem.BuiltIn.Void, dest, 16);

			context.SetInstruction(X86.PXor, InstructionSize.Size128, v0, v0, v0);
			context.AppendInstruction(X86.MovUPS, InstructionSize.Size128, memDest1, v0);
			context.AppendInstruction(X86.MovUPS, InstructionSize.Size128, memDest2, v0);
		}

		#endregion Methods
	}
}
