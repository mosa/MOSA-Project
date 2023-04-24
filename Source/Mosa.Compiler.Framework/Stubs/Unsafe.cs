// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class StubMethods
{
	[StubMethod("System.Runtime.CompilerServices.Unsafe::SizeOf")]
	public static void Unsafe_SizeOf(Context context, MethodCompiler methodCompiler)
	{
		var type = methodCompiler.Method.GenericArguments[0];
		var size = methodCompiler.GetSize(type);
		var opReturn = methodCompiler.VirtualRegisters.Allocate32();

		context.AppendInstruction(IRInstruction.Move32, opReturn, Operand.CreateConstant(size));
		context.AppendInstruction(IRInstruction.SetReturn32, null, opReturn);
		context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
	}

	[StubMethod("System.Runtime.CompilerServices.Unsafe::AsPointer")]
	[StubMethod("System.Runtime.CompilerServices.Unsafe::As")]
	public static void Unsafe_As(Context context, MethodCompiler methodCompiler)
	{
		CILDecoderStage.CreateParameters(methodCompiler);

		var source = methodCompiler.Parameters[0];

		var opReturn = source.IsObject
			? methodCompiler.VirtualRegisters.AllocateNativeInteger()
			: methodCompiler.VirtualRegisters.AllocateManagedPointer();

		var loadSource = source.IsObject
			? IRInstruction.LoadParamObject
			: methodCompiler.Is32BitPlatform ? IRInstruction.LoadParam32 : IRInstruction.LoadParam64;

		var setReturn = source.IsObject
			? IRInstruction.SetReturnObject
			: methodCompiler.Is32BitPlatform ? IRInstruction.SetReturn32 : IRInstruction.SetReturn64;

		// Load source into return operand
		context.AppendInstruction(loadSource, opReturn, source);

		// Set return
		context.AppendInstruction(setReturn, null, opReturn);

		context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
	}

	[StubMethod("System.Runtime.CompilerServices.Unsafe::AreSame")]
	public static void Unsafe_AreSame(Context context, MethodCompiler methodCompiler)
	{
		CILDecoderStage.CreateParameters(methodCompiler);

		var left = methodCompiler.Parameters[0];
		var right = methodCompiler.Parameters[1];
		var opLeft = methodCompiler.VirtualRegisters.AllocateObject();
		var opRight = methodCompiler.VirtualRegisters.AllocateObject();
		var opReturn = methodCompiler.VirtualRegisters.AllocateNativeInteger();

		// Load left parameter
		context.AppendInstruction(IRInstruction.LoadParamObject, opLeft, left);

		// Load right parameter
		context.AppendInstruction(IRInstruction.LoadParamObject, opRight, right);

		// Compare and store into result operand
		context.AppendInstruction(IRInstruction.CompareObject, ConditionCode.Equal, opReturn, opLeft, opRight);

		// Set return
		var setReturn = methodCompiler.Is32BitPlatform ? IRInstruction.SetReturn32 : IRInstruction.SetReturn64;
		context.AppendInstruction(setReturn, null, opReturn);

		context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
	}

	[StubMethod("System.Runtime.CompilerServices.Unsafe::AddByteOffset")]
	public static void Unsafe_AddByteOffset(Context context, MethodCompiler methodCompiler)
	{
		CILDecoderStage.CreateParameters(methodCompiler);

		var source = methodCompiler.Parameters[0];
		var byteOffset = methodCompiler.Parameters[1];
		var opSource = methodCompiler.VirtualRegisters.AllocateManagedPointer();
		var opByteOffset = methodCompiler.VirtualRegisters.AllocateNativeInteger();
		var opReturn = methodCompiler.VirtualRegisters.AllocateNativeInteger();

		// Load left parameter
		var loadSource = methodCompiler.Is32BitPlatform ? IRInstruction.LoadParam32 : IRInstruction.LoadParam64;
		context.AppendInstruction(loadSource, opSource, source);

		// Load right parameter
		var loadByteOffset = methodCompiler.Is32BitPlatform ? IRInstruction.LoadParam32 : IRInstruction.LoadParam64;
		context.AppendInstruction(loadByteOffset, opByteOffset, byteOffset);

		// Compare and store into result operand
		var add = methodCompiler.Is32BitPlatform ? IRInstruction.Add32 : IRInstruction.Add64;
		context.AppendInstruction(add, opReturn, opSource, opByteOffset);

		// Return comparison result
		var setReturn = methodCompiler.Is32BitPlatform ? IRInstruction.SetReturn32 : IRInstruction.SetReturn64;
		context.AppendInstruction(setReturn, null, opReturn);

		context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
	}
}
