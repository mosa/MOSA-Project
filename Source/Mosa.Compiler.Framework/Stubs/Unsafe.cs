// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class StubMethods
	{
		[StubMethod("System.Runtime.CompilerServices.Unsafe::SizeOf")]
		public static void Unsafe_SizeOf(Context context, MethodCompiler methodCompiler)
		{
			var type = methodCompiler.Method.GenericArguments[0];
			var size = methodCompiler.TypeLayout.GetTypeSize(type);
			var opReturn = methodCompiler.AllocateVirtualRegisterOrStackSlot(methodCompiler.Method.Signature.ReturnType);

			// Move constant into return operand
			var move = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.Move32 : IRInstruction.Move64;
			context.AppendInstruction(move, opReturn, methodCompiler.CreateConstant(size));

			// Set return
			var setReturn = BaseMethodCompilerStage.GetSetReturnInstruction(opReturn.Type, methodCompiler.Architecture.Is32BitPlatform);
			context.AppendInstruction(setReturn, null, opReturn);

			context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		[StubMethod("System.Runtime.CompilerServices.Unsafe::AsPointer")]
		[StubMethod("System.Runtime.CompilerServices.Unsafe::As")]
		public static void Unsafe_As(Context context, MethodCompiler methodCompiler)
		{
			var source = methodCompiler.Parameters[0];
			var opReturn = methodCompiler.AllocateVirtualRegisterOrStackSlot(methodCompiler.Method.Signature.ReturnType);

			// Load source into return operand
			var loadSource = BaseMethodCompilerStage.GetLoadParameterInstruction(source.Type, methodCompiler.Architecture.Is32BitPlatform);
			context.AppendInstruction(loadSource, opReturn, source);

			// Set return
			var setReturn = BaseMethodCompilerStage.GetSetReturnInstruction(opReturn.Type, methodCompiler.Architecture.Is32BitPlatform);
			context.AppendInstruction(setReturn, null, opReturn);

			context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		[StubMethod("System.Runtime.CompilerServices.Unsafe::AreSame")]
		public static void Unsafe_AreSame(Context context, MethodCompiler methodCompiler)
		{
			var left = methodCompiler.Parameters[0];
			var right = methodCompiler.Parameters[1];
			var opLeft = methodCompiler.AllocateVirtualRegisterOrStackSlot(left.Type);
			var opRight = methodCompiler.AllocateVirtualRegisterOrStackSlot(right.Type);
			var opReturn = methodCompiler.AllocateVirtualRegisterOrStackSlot(methodCompiler.Method.Signature.ReturnType);

			// Load left parameter
			var loadLeft = BaseMethodCompilerStage.GetLoadParameterInstruction(left.Type, methodCompiler.Architecture.Is32BitPlatform);
			context.AppendInstruction(loadLeft, opLeft, left);

			// Load right parameter
			var loadRight = BaseMethodCompilerStage.GetLoadParameterInstruction(right.Type, methodCompiler.Architecture.Is32BitPlatform);
			context.AppendInstruction(loadRight, opRight, right);

			// Compare and store into result operand
			context.AppendInstruction(IRInstruction.CompareObject, ConditionCode.Equal, opReturn, opLeft, opRight);

			// Set return
			var setReturn = BaseMethodCompilerStage.GetSetReturnInstruction(opReturn.Type, methodCompiler.Architecture.Is32BitPlatform);
			context.AppendInstruction(setReturn, null, opReturn);

			context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		[StubMethod("System.Runtime.CompilerServices.Unsafe::AddByteOffset")]
		public static void Unsafe_AddByteOffset(Context context, MethodCompiler methodCompiler)
		{
			var source = methodCompiler.Parameters[0];
			var byteOffset = methodCompiler.Parameters[1];
			var opSource = methodCompiler.AllocateVirtualRegisterOrStackSlot(source.Type);
			var opByteOffset = methodCompiler.AllocateVirtualRegisterOrStackSlot(byteOffset.Type);
			var opReturn = methodCompiler.AllocateVirtualRegisterOrStackSlot(methodCompiler.Method.Signature.ReturnType);

			// Load left parameter
			var loadSource = BaseMethodCompilerStage.GetLoadParameterInstruction(source.Type, methodCompiler.Architecture.Is32BitPlatform);
			context.AppendInstruction(loadSource, opSource, source);

			// Load right parameter
			var loadByteOffset = BaseMethodCompilerStage.GetLoadParameterInstruction(byteOffset.Type, methodCompiler.Architecture.Is32BitPlatform);
			context.AppendInstruction(loadByteOffset, opByteOffset, byteOffset);

			// Compare and store into result operand
			var add = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.Add32 : IRInstruction.Add64;
			context.AppendInstruction(add, opReturn, opSource, opByteOffset);

			// Return comparison result
			var setReturn = BaseMethodCompilerStage.GetSetReturnInstruction(opReturn.Type, methodCompiler.Architecture.Is32BitPlatform);
			context.AppendInstruction(setReturn, null, opReturn);

			context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}
	}
}
