// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("System.String::InternalAllocateString")]
	public sealed class InternalAllocateString : IIntrinsicInternalMethod
	{
		/// <summary>
		/// The string class type definition symbol name
		/// </summary>
		private const string StringClassTypeDefinitionSymbolName = "System.String" + Metadata.TypeDefinition;

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var method = methodCompiler.Compiler.InternalRuntimeType.FindMethodByName("AllocateString");
			var symbol = Operand.CreateSymbolFromMethod(method, methodCompiler.TypeSystem);

			var typeDef = Operand.CreateUnmanagedSymbolPointer(StringClassTypeDefinitionSymbolName, methodCompiler.TypeSystem);
			var length = context.Operand1;
			var result = context.Result;

			context.SetInstruction(IRInstruction.CallStatic, result, symbol, typeDef, length);
		}
	}
}
