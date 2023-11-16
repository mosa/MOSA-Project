// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class StubMethods
{
	[StubMethod("System.ByReference`1::.ctor")]
	public static void ByReference__ctor(Context context, Transform transform)
	{
		CILDecoderStage.CreateParameters(transform.MethodCompiler);

		var instance = transform.MethodCompiler.Parameters[0];
		var value = transform.MethodCompiler.Parameters[1];
		var opInstance = transform.VirtualRegisters.AllocateManagedPointer();
		var opValue = transform.VirtualRegisters.AllocateManagedPointer();

		// Load instance parameter
		var loadInstance = transform.MethodCompiler.GetLoadParamInstruction(instance.Element);
		context.AppendInstruction(loadInstance, opInstance, instance);

		// Load value parameter
		var loadValue = transform.MethodCompiler.GetLoadParamInstruction(value.Element);
		context.AppendInstruction(loadValue, opValue, value);

		// Store value inside instance
		var store = transform.Is32BitPlatform ? IR.Store32 : IR.Store64;
		context.AppendInstruction(store, null, opInstance, transform.ConstantZero, opValue);

		context.AppendInstruction(IR.Jmp, transform.BasicBlocks.EpilogueBlock);
	}

	[StubMethod("System.ByReference`1::get_Value")]
	public static void ByReference_get_Value(Context context, Transform transform)
	{
		CILDecoderStage.CreateParameters(transform.MethodCompiler);

		var instance = transform.MethodCompiler.Parameters[0];
		var opInstance = transform.VirtualRegisters.AllocateManagedPointer();
		var opReturn = transform.VirtualRegisters.AllocateManagedPointer();

		// Load instance parameter
		var loadInstance = transform.MethodCompiler.GetLoadParamInstruction(instance.Element);
		context.AppendInstruction(loadInstance, opInstance, instance);

		// Load value from instance into return operand
		var loadValue = transform.Is32BitPlatform ? IR.Load32 : IR.Load64;
		context.AppendInstruction(loadValue, opReturn, opInstance, transform.ConstantZero);

		// Set return
		var setReturn = transform.MethodCompiler.GetReturnInstruction(opReturn.Primitive);
		context.AppendInstruction(setReturn, null, opReturn);

		context.AppendInstruction(IR.Jmp, transform.BasicBlocks.EpilogueBlock);
	}
}
