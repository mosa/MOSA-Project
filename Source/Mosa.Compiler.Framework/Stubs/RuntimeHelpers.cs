// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class StubMethods
{
	[StubMethod("System.Runtime.CompilerServices.RuntimeHelpers::IsReferenceOrContainsReferences")]
	public static void RuntimeHelpers_IsReferenceOrContainsReferences(Context context, Transform transform)
	{
		// FIXME: we're only checking if the current type is a reference type, we aren't checking if it contains references
		var type = transform.Method.GenericArguments[0];
		var isReferenceOrContainsReferences = type.IsReferenceType;

		var result = transform.VirtualRegisters.AllocateNativeInteger();

		// Move constant into return operand
		var move = transform.Is32BitPlatform ? IR.Move32 : IR.Move64;
		context.AppendInstruction(move, result, isReferenceOrContainsReferences ? Operand.Constant32_1 : Operand.Constant32_0);

		// Set return
		var setReturn = transform.Is32BitPlatform ? IR.SetReturn32 : IR.SetReturn64;
		context.AppendInstruction(setReturn, null, result);

		context.AppendInstruction(IR.Jmp, transform.BasicBlocks.EpilogueBlock);
	}
}
