// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class StubMethods
{
	[StubMethod("System.Runtime.CompilerServices.RuntimeHelpers::IsReferenceOrContainsReferences")]
	public static void RuntimeHelpers_IsReferenceOrContainsReferences(Context context, MethodCompiler methodCompiler)
	{
		// FIXME: we're only checking if the current type is a reference type, we aren't checking if it contains references
		var type = methodCompiler.Method.GenericArguments[0];
		var isReferenceOrContainsReferences = type.IsReferenceType;

		var result = methodCompiler.VirtualRegisters.AllocateNativeInteger();

		// Move constant into return operand
		var move = methodCompiler.Is32BitPlatform ? IRInstruction.Move32 : IRInstruction.Move64;
		context.AppendInstruction(move, result, methodCompiler.CreateConstant(isReferenceOrContainsReferences ? 1 : 0));

		// Set return
		var setReturn = methodCompiler.Is32BitPlatform ? (BaseIRInstruction)IRInstruction.SetReturn32 : IRInstruction.SetReturn64;
		context.AppendInstruction(setReturn, null, result);

		context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
	}
}
