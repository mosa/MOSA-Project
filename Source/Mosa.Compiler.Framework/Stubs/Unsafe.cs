// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class StubMethods
{
	[StubMethod("System.Runtime.CompilerServices.Unsafe::SizeOf")]
	public static void Unsafe_SizeOf(Context context, Transform transform)
	{
		var type = transform.Method.GenericArguments[0];
		var opReturn = transform.VirtualRegisters.Allocate32();

		var size = transform.MethodCompiler.GetElementSize(type);

		context.AppendInstruction(IR.Move32, opReturn, Operand.CreateConstant32(size));
		context.AppendInstruction(IR.SetReturn32, null, opReturn);
		context.AppendInstruction(IR.Jmp, transform.BasicBlocks.EpilogueBlock);
	}

	[StubMethod("System.Runtime.CompilerServices.Unsafe::AsPointer")]
	[StubMethod("System.Runtime.CompilerServices.Unsafe::As")]
	public static void Unsafe_As(Context context, Transform transform)
	{
		CILDecoderStage.CreateParameters(transform.MethodCompiler);

		var source = transform.MethodCompiler.Parameters[0];

		var opReturn = source.IsObject
			? transform.VirtualRegisters.AllocateNativeInteger()
			: transform.VirtualRegisters.AllocateManagedPointer();

		var loadSource = source.IsObject
			? IR.LoadParamObject
			: transform.Is32BitPlatform ? IR.LoadParam32 : IR.LoadParam64;

		var setReturn = source.IsObject
			? IR.SetReturnObject
			: transform.Is32BitPlatform ? IR.SetReturn32 : IR.SetReturn64;

		// Load source into return operand
		context.AppendInstruction(loadSource, opReturn, source);

		// Set return
		context.AppendInstruction(setReturn, null, opReturn);

		context.AppendInstruction(IR.Jmp, transform.BasicBlocks.EpilogueBlock);
	}

	[StubMethod("System.Runtime.CompilerServices.Unsafe::AreSame")]
	public static void Unsafe_AreSame(Context context, Transform transform)
	{
		CILDecoderStage.CreateParameters(transform.MethodCompiler);

		var left = transform.MethodCompiler.Parameters[0];
		var right = transform.MethodCompiler.Parameters[1];
		var opLeft = transform.VirtualRegisters.AllocateObject();
		var opRight = transform.VirtualRegisters.AllocateObject();
		var opReturn = transform.VirtualRegisters.AllocateNativeInteger();

		// Load left parameter
		context.AppendInstruction(IR.LoadParamObject, opLeft, left);

		// Load right parameter
		context.AppendInstruction(IR.LoadParamObject, opRight, right);

		// Compare and store into result operand
		context.AppendInstruction(IR.CompareObject, ConditionCode.Equal, opReturn, opLeft, opRight);

		// Set return
		var setReturn = transform.Is32BitPlatform ? IR.SetReturn32 : IR.SetReturn64;
		context.AppendInstruction(setReturn, null, opReturn);

		context.AppendInstruction(IR.Jmp, transform.BasicBlocks.EpilogueBlock);
	}

	[StubMethod("System.Runtime.CompilerServices.Unsafe::AddByteOffset")]
	public static void Unsafe_AddByteOffset(Context context, Transform transform)
	{
		CILDecoderStage.CreateParameters(transform.MethodCompiler);

		var source = transform.MethodCompiler.Parameters[0];
		var byteOffset = transform.MethodCompiler.Parameters[1];
		var opSource = transform.VirtualRegisters.AllocateManagedPointer();
		var opByteOffset = transform.VirtualRegisters.AllocateNativeInteger();
		var opReturn = transform.VirtualRegisters.AllocateNativeInteger();

		// Load left parameter
		var loadSource = transform.Is32BitPlatform ? IR.LoadParam32 : IR.LoadParam64;
		context.AppendInstruction(loadSource, opSource, source);

		// Load right parameter
		var loadByteOffset = transform.Is32BitPlatform ? IR.LoadParam32 : IR.LoadParam64;
		context.AppendInstruction(loadByteOffset, opByteOffset, byteOffset);

		// Compare and store into result operand
		var add = transform.Is32BitPlatform ? IR.Add32 : IR.Add64;
		context.AppendInstruction(add, opReturn, opSource, opByteOffset);

		// Return comparison result
		var setReturn = transform.Is32BitPlatform ? IR.SetReturn32 : IR.SetReturn64;
		context.AppendInstruction(setReturn, null, opReturn);

		context.AppendInstruction(IR.Jmp, transform.BasicBlocks.EpilogueBlock);
	}
}
