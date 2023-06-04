// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class StubMethods
{
	[StubMethod("System.ByReference`1::.ctor")]
	public static void ByReference__ctor(Context context, TransformContext transformContext)
	{
		CILDecoderStage.CreateParameters(transformContext.MethodCompiler);

		var instance = transformContext.MethodCompiler.Parameters[0];
		var value = transformContext.MethodCompiler.Parameters[1];
		var opInstance = transformContext.VirtualRegisters.AllocateManagedPointer();
		var opValue = transformContext.VirtualRegisters.AllocateManagedPointer();

		// Load instance parameter
		var loadInstance = transformContext.MethodCompiler.GetLoadParamInstruction(instance.Element);
		context.AppendInstruction(loadInstance, opInstance, instance);

		// Load value parameter
		var loadValue = transformContext.MethodCompiler.GetLoadParamInstruction(value.Element);
		context.AppendInstruction(loadValue, opValue, value);

		// Store value inside instance
		var store = transformContext.Is32BitPlatform ? IRInstruction.Store32 : IRInstruction.Store64;
		context.AppendInstruction(store, null, opInstance, transformContext.ConstantZero, opValue);

		context.AppendInstruction(IRInstruction.Jmp, transformContext.BasicBlocks.EpilogueBlock);
	}

	[StubMethod("System.ByReference`1::get_Value")]
	public static void ByReference_get_Value(Context context, TransformContext transformContext)
	{
		CILDecoderStage.CreateParameters(transformContext.MethodCompiler);

		var instance = transformContext.MethodCompiler.Parameters[0];
		var opInstance = transformContext.VirtualRegisters.AllocateManagedPointer();
		var opReturn = transformContext.VirtualRegisters.AllocateManagedPointer();

		// Load instance parameter
		var loadInstance = transformContext.MethodCompiler.GetLoadParamInstruction(instance.Element);
		context.AppendInstruction(loadInstance, opInstance, instance);

		// Load value from instance into return operand
		var loadValue = transformContext.Is32BitPlatform ? IRInstruction.Load32 : IRInstruction.Load64;
		context.AppendInstruction(loadValue, opReturn, opInstance, transformContext.ConstantZero);

		// Set return
		var setReturn = transformContext.MethodCompiler.GetReturnInstruction(opReturn.Primitive);
		context.AppendInstruction(setReturn, null, opReturn);

		context.AppendInstruction(IRInstruction.Jmp, transformContext.BasicBlocks.EpilogueBlock);
	}
}
