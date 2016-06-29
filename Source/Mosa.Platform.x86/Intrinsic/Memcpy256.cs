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

			var v0 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.Void);
			var v1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.Void);
			var zero = Operand.CreateConstant(methodCompiler.TypeSystem, 0);
			var offset16 = Operand.CreateConstant(methodCompiler.TypeSystem, 16);

			context.AppendInstruction(X86.MovupsLoad, InstructionSize.Size128, v0, dest, zero);
			context.AppendInstruction(X86.MovupsLoad, InstructionSize.Size128, v1, dest, offset16);
			context.AppendInstruction(X86.MovupsStore, InstructionSize.Size128, null, dest, zero, v0);
			context.AppendInstruction(X86.MovupsStore, InstructionSize.Size128, null, dest, offset16, v1);
		}

		#endregion Methods
	}
}
