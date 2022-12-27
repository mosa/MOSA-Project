// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::GetMultibootRAX")]
		private static void GetMultibootRAX(Context context, MethodCompiler methodCompiler)
		{
			var MultibootEAX = Operand.CreateUnmanagedSymbolPointer(Intel.CompilerStages.MultibootV1Stage.MultibootEAX, methodCompiler.TypeSystem);

			context.SetInstruction(IRInstruction.Load64, context.Result, MultibootEAX, methodCompiler.ConstantZero32);
		}
	}
}
