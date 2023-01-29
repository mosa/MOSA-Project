// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Remainder")]
	private static void Remainder(Context context, MethodCompiler methodCompiler)
	{
		var result = context.Result;
		var dividend = context.Operand1;
		var divisor = context.Operand2;

		if (result.IsR8)
		{
			var xmm1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.R8);
			var xmm2 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.R8);
			var xmm3 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.R8);

			context.SetInstruction(X64.Divsd, xmm1, dividend, divisor);
			context.AppendInstruction(X64.Roundsd, xmm2, xmm1, methodCompiler.CreateConstant((byte)0x3));
			context.AppendInstruction(X64.Mulsd, xmm3, divisor, xmm2);
			context.AppendInstruction(X64.Subsd, result, dividend, xmm3);
		}
		else
		{
			var xmm1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.R4);
			var xmm2 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.R4);
			var xmm3 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.R4);

			context.SetInstruction(X64.Divss, xmm1, dividend, divisor);
			context.AppendInstruction(X64.Roundss, xmm2, xmm1, methodCompiler.CreateConstant((byte)0x3));
			context.AppendInstruction(X64.Mulss, xmm3, divisor, xmm2);
			context.AppendInstruction(X64.Subss, result, dividend, xmm3);
		}
	}
}
