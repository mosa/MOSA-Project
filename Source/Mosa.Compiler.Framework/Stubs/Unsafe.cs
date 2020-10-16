// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class StubMethods
	{
		[IntrinsicMethod("System.Runtime.CompilerServices.Unsafe::SizeOf")]
		private static void SizeOf(Context context, MethodCompiler methodCompiler)
		{
			//var size = type.IsPointer ? methodCompiler.Architecture.NativePointerSize : methodCompiler.TypeLayout.GetTypeSize(type);

			var type = context.InvokeMethod.GenericArguments[0];
			var size = methodCompiler.TypeLayout.GetTypeSize(type);
			var move = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.Move32 : IRInstruction.Move64;

			context.SetInstruction(move, context.Result, methodCompiler.CreateConstant(size));
		}
	}
}
