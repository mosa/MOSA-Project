// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Platform.x86.CompilerStages;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic:GetMultibootEBX")]
		private static void GetMultibootEBX(Context context, MethodCompiler methodCompiler)
		{
			var MultibootEBX = Operand.CreateUnmanagedSymbolPointer(MultibootV1Stage.MultibootEBX, methodCompiler.TypeSystem);

			context.SetInstruction(IRInstruction.Load32, context.Result, MultibootEBX, methodCompiler.ConstantZero32);
		}
	}
}
