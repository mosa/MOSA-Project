// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// GetMethodLookupTable
	/// </summary>
	internal class GetMethodLookupTable : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(IRInstruction.MoveInteger32, context.Result, Operand.CreateUnmanagedSymbolPointer(Metadata.MethodLookupTable, methodCompiler.TypeSystem));
		}
	}
}
