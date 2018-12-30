// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// GetObjectAddress
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicInternalMethod" />
	[ReplacementTarget("Mosa.Runtime.Intrinsic::GetStringType")]
	public sealed class GetStringType : IIntrinsicInternalMethod
	{
		private const string StringClassTypeDefinitionSymbolName = "System.String" + Metadata.TypeDefinition;

		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var typeDef = Operand.CreateUnmanagedSymbolPointer(StringClassTypeDefinitionSymbolName, methodCompiler.TypeSystem);

			var move = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.MoveInt32 : IRInstruction.MoveInt64;

			context.SetInstruction(move, context.Result, typeDef);
		}
	}
}
