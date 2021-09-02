// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::GetMultibootEBX")]
		private static void GetMultibootEBX(Context context, MethodCompiler methodCompiler)
		{
			var MultibootEBX = Operand.CreateUnmanagedSymbolPointer(Intel.CompilerStages.MultibootV1Stage.MultibootEBX, methodCompiler.TypeSystem);

			context.SetInstruction(IRInstruction.Load64, context.Result, MultibootEBX, methodCompiler.ConstantZero32);
		}
	}
}
