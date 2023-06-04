// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class StubMethods
{
	[StubMethod("System.Runtime.CompilerServices.Unsafe::SizeOf")]
	public static void Unsafe_SizeOf(Context context, TransformContext transformContext)
	{
		var type = transformContext.Method.GenericArguments[0];
		var opReturn = transformContext.VirtualRegisters.Allocate32();

		var size = transformContext.MethodCompiler.GetElementSize(type);

		context.AppendInstruction(IRInstruction.Move32, opReturn, Operand.CreateConstant32(size));
		context.AppendInstruction(IRInstruction.SetReturn32, null, opReturn);
		context.AppendInstruction(IRInstruction.Jmp, transformContext.BasicBlocks.EpilogueBlock);
	}

	[StubMethod("System.Runtime.CompilerServices.Unsafe::AsPointer")]
	[StubMethod("System.Runtime.CompilerServices.Unsafe::As")]
	public static void Unsafe_As(Context context, TransformContext transformContext)
	{
		CILDecoderStage.CreateParameters(transformContext.MethodCompiler);

		var source = transformContext.MethodCompiler.Parameters[0];

		var opReturn = source.IsObject
			? transformContext.VirtualRegisters.AllocateNativeInteger()
			: transformContext.VirtualRegisters.AllocateManagedPointer();

		var loadSource = source.IsObject
			? IRInstruction.LoadParamObject
			: transformContext.Is32BitPlatform ? IRInstruction.LoadParam32 : IRInstruction.LoadParam64;

		var setReturn = source.IsObject
			? IRInstruction.SetReturnObject
			: transformContext.Is32BitPlatform ? IRInstruction.SetReturn32 : IRInstruction.SetReturn64;

		// Load source into return operand
		context.AppendInstruction(loadSource, opReturn, source);

		// Set return
		context.AppendInstruction(setReturn, null, opReturn);

		context.AppendInstruction(IRInstruction.Jmp, transformContext.BasicBlocks.EpilogueBlock);
	}

	[StubMethod("System.Runtime.CompilerServices.Unsafe::AreSame")]
	public static void Unsafe_AreSame(Context context, TransformContext transformContext)
	{
		CILDecoderStage.CreateParameters(transformContext.MethodCompiler);

		var left = transformContext.MethodCompiler.Parameters[0];
		var right = transformContext.MethodCompiler.Parameters[1];
		var opLeft = transformContext.VirtualRegisters.AllocateObject();
		var opRight = transformContext.VirtualRegisters.AllocateObject();
		var opReturn = transformContext.VirtualRegisters.AllocateNativeInteger();

		// Load left parameter
		context.AppendInstruction(IRInstruction.LoadParamObject, opLeft, left);

		// Load right parameter
		context.AppendInstruction(IRInstruction.LoadParamObject, opRight, right);

		// Compare and store into result operand
		context.AppendInstruction(IRInstruction.CompareObject, ConditionCode.Equal, opReturn, opLeft, opRight);

		// Set return
		var setReturn = transformContext.Is32BitPlatform ? IRInstruction.SetReturn32 : IRInstruction.SetReturn64;
		context.AppendInstruction(setReturn, null, opReturn);

		context.AppendInstruction(IRInstruction.Jmp, transformContext.BasicBlocks.EpilogueBlock);
	}

	[StubMethod("System.Runtime.CompilerServices.Unsafe::AddByteOffset")]
	public static void Unsafe_AddByteOffset(Context context, TransformContext transformContext)
	{
		CILDecoderStage.CreateParameters(transformContext.MethodCompiler);

		var source = transformContext.MethodCompiler.Parameters[0];
		var byteOffset = transformContext.MethodCompiler.Parameters[1];
		var opSource = transformContext.VirtualRegisters.AllocateManagedPointer();
		var opByteOffset = transformContext.VirtualRegisters.AllocateNativeInteger();
		var opReturn = transformContext.VirtualRegisters.AllocateNativeInteger();

		// Load left parameter
		var loadSource = transformContext.Is32BitPlatform ? IRInstruction.LoadParam32 : IRInstruction.LoadParam64;
		context.AppendInstruction(loadSource, opSource, source);

		// Load right parameter
		var loadByteOffset = transformContext.Is32BitPlatform ? IRInstruction.LoadParam32 : IRInstruction.LoadParam64;
		context.AppendInstruction(loadByteOffset, opByteOffset, byteOffset);

		// Compare and store into result operand
		var add = transformContext.Is32BitPlatform ? IRInstruction.Add32 : IRInstruction.Add64;
		context.AppendInstruction(add, opReturn, opSource, opByteOffset);

		// Return comparison result
		var setReturn = transformContext.Is32BitPlatform ? IRInstruction.SetReturn32 : IRInstruction.SetReturn64;
		context.AppendInstruction(setReturn, null, opReturn);

		context.AppendInstruction(IRInstruction.Jmp, transformContext.BasicBlocks.EpilogueBlock);
	}
}
