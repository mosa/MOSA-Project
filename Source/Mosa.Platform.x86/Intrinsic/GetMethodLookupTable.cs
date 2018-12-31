// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic:GetMethodLookupTable")]
		private static void GetMethodLookupTable(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(IRInstruction.MoveInt32, context.Result, Operand.CreateUnmanagedSymbolPointer(Metadata.MethodLookupTable, methodCompiler.TypeSystem));
		}
	}
}
