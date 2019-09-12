// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		private const string StringClassTypeDefinitionSymbolName = Metadata.TypeDefinition + "System.String";

		[IntrinsicMethod("Mosa.Runtime.Intrinsic:GetStringType")]
		private static void GetStringType(Context context, MethodCompiler methodCompiler)
		{
			var typeDef = Operand.CreateUnmanagedSymbolPointer(StringClassTypeDefinitionSymbolName, methodCompiler.TypeSystem);

			var move = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.Move32 : IRInstruction.Move64;

			context.SetInstruction(move, context.Result, typeDef);
		}
	}
}
