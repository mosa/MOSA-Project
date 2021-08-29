// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class StubMethods
	{
		[StubMethod("System.ByReference`1::.ctor")]
		public static void ByReference__ctor(Context context, MethodCompiler methodCompiler)
		{
			var instance = methodCompiler.Parameters[0];
			var value = methodCompiler.Parameters[1];
			var opInstance = methodCompiler.AllocateVirtualRegisterOrStackSlot(instance.Type);
			var opValue = methodCompiler.AllocateVirtualRegisterOrStackSlot(value.Type);

			// Load instance parameter
			var loadInstance = BaseMethodCompilerStage.GetLoadParameterInstruction(instance.Type, methodCompiler.Is32BitPlatform);
			context.AppendInstruction(loadInstance, opInstance, instance);

			// Load value parameter
			var loadValue = BaseMethodCompilerStage.GetLoadParameterInstruction(value.Type, methodCompiler.Is32BitPlatform);
			context.AppendInstruction(loadValue, opValue, value);

			// Store value inside instance
			var store = methodCompiler.Is32BitPlatform ? (BaseInstruction)IRInstruction.Store32 : IRInstruction.Store64;
			context.AppendInstruction(store, null, opInstance, methodCompiler.ConstantZero, opValue);
			context.MosaType = methodCompiler.TypeSystem.BuiltIn.I;

			context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		[StubMethod("System.ByReference`1::get_Value")]
		public static void ByReference_get_Value(Context context, MethodCompiler methodCompiler)
		{
			var instance = methodCompiler.Parameters[0];
			var opInstance = methodCompiler.AllocateVirtualRegisterOrStackSlot(instance.Type);
			var opReturn = methodCompiler.AllocateVirtualRegisterOrStackSlot(methodCompiler.Method.Signature.ReturnType);

			// Load instance parameter
			var loadInstance = BaseMethodCompilerStage.GetLoadParameterInstruction(instance.Type, methodCompiler.Is32BitPlatform);
			context.AppendInstruction(loadInstance, opInstance, instance);

			// Load value from instance into return operand
			var loadValue = methodCompiler.Is32BitPlatform ? (BaseInstruction)IRInstruction.Load32 : IRInstruction.Load64;
			context.AppendInstruction(loadValue, opReturn, opInstance, methodCompiler.ConstantZero);
			context.MosaType = methodCompiler.TypeSystem.BuiltIn.I;

			// Set return
			var setReturn = BaseMethodCompilerStage.GetSetReturnInstruction(opReturn.Type, methodCompiler.Is32BitPlatform);
			context.AppendInstruction(setReturn, null, opReturn);

			context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}
	}
}
