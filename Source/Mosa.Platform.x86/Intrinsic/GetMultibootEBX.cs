// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Platform.x86.CompilerStages;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// GetMultibootEBX
	/// </summary>
	internal class GetMultibootEBX : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var MultibootEBX = Operand.CreateUnmanagedSymbolPointer(Multiboot0695Stage.MultibootEBX, methodCompiler.TypeSystem);

			context.SetInstruction(IRInstruction.LoadInt32, context.Result, MultibootEBX, methodCompiler.ConstantZero);
		}
	}
}
