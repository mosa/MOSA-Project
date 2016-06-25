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

			var v0 = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.Void, SSE2Register.XMM0);
			var zero = Operand.CreateConstant(methodCompiler.TypeSystem, 0);
			var offset16 = Operand.CreateConstant(methodCompiler.TypeSystem, 16);

			context.SetInstruction(X86.PXor, InstructionSize.Size128, v0, v0, v0);
			context.AppendInstruction(X86.MovupsStore, InstructionSize.Size128, dest, zero, v0);
			context.AppendInstruction(X86.MovupsStore, InstructionSize.Size128, dest, offset16, v0);
		}

		#endregion Methods
	}
}
