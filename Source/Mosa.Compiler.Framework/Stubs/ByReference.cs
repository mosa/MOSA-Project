// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class StubMethods
{
	[StubMethod("System.ByReference`1::.ctor")]
	public static void ByReference__ctor(Context context, MethodCompiler methodCompiler)
	{
		CILDecoderStage.CreateParameters(methodCompiler);

		var instance = methodCompiler.Parameters[0];
		var value = methodCompiler.Parameters[1];
		var opInstance = methodCompiler.VirtualRegisters.AllocateManagedPointer();
		var opValue = methodCompiler.VirtualRegisters.AllocateManagedPointer();

		// Load instance parameter
		var loadInstance = methodCompiler.GetLoadParamInstruction(instance.Element);
		context.AppendInstruction(loadInstance, opInstance, instance);

		// Load value parameter
		var loadValue = methodCompiler.GetLoadParamInstruction(value.Element);
		context.AppendInstruction(loadValue, opValue, value);

		// Store value inside instance
		var store = methodCompiler.Is32BitPlatform ? IRInstruction.Store32 : IRInstruction.Store64;
		context.AppendInstruction(store, null, opInstance, Operand.Constant64_0, opValue);

		context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
	}

	[StubMethod("System.ByReference`1::get_Value")]
	public static void ByReference_get_Value(Context context, MethodCompiler methodCompiler)
	{
		CILDecoderStage.CreateParameters(methodCompiler);

		var instance = methodCompiler.Parameters[0];
		var opInstance = methodCompiler.VirtualRegisters.AllocateManagedPointer();
		var opReturn = methodCompiler.VirtualRegisters.AllocateManagedPointer();

		// Load instance parameter
		var loadInstance = methodCompiler.GetLoadParamInstruction(instance.Element);
		context.AppendInstruction(loadInstance, opInstance, instance);

		// Load value from instance into return operand
		var loadValue = methodCompiler.Is32BitPlatform ? IRInstruction.Load32 : IRInstruction.Load64;
		context.AppendInstruction(loadValue, opReturn, opInstance, Operand.Constant64_0);

		// Set return
		var setReturn = methodCompiler.GetReturnInstruction(opReturn.Primitive);
		context.AppendInstruction(setReturn, null, opReturn);

		context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
	}
}
