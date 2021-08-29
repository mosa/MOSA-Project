// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class StubMethods
	{
		[StubMethod("System.Runtime.CompilerServices.RuntimeHelpers::IsReferenceOrContainsReferences")]
		public static void RuntimeHelpers_IsReferenceOrContainsReferences(Context context, MethodCompiler methodCompiler)
		{
			var type = methodCompiler.Method.GenericArguments[0];
			var opReturn = methodCompiler.AllocateVirtualRegisterOrStackSlot(methodCompiler.Method.Signature.ReturnType);

			// FIXME: we're only checking if the current type is a reference type, we aren't checking if it contains references
			var isReferenceOrContainsReferences = type.IsReferenceType;

			// Move constant into return operand
			var move = methodCompiler.Is32BitPlatform ? (BaseInstruction)IRInstruction.Move32 : IRInstruction.Move64;
			context.AppendInstruction(move, opReturn, methodCompiler.CreateConstant(isReferenceOrContainsReferences ? 1 : 0));

			// Set return
			var setReturn = BaseMethodCompilerStage.GetSetReturnInstruction(opReturn.Type, methodCompiler.Is32BitPlatform);
			context.AppendInstruction(setReturn, null, opReturn);

			context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}
	}
}
