// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::Memclr256")]
		private static void Memclr256(Context context, MethodCompiler methodCompiler)
		{
			var dest = context.Operand1;

			var v0 = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.Void, SSE2Register.XMM0);
			var offset16 = methodCompiler.CreateConstant(16);

			context.SetInstruction(X86.PXor, v0, v0, v0);
			context.AppendInstruction(X86.MovupsStore, dest, methodCompiler.ConstantZero32, v0);
			context.AppendInstruction(X86.MovupsStore, dest, offset16, v0);
		}
	}
}
