// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System;
using System.Collections.Generic;

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

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var typeDef = Operand.CreateUnmanagedSymbolPointer(StringClassTypeDefinitionSymbolName, methodCompiler.TypeSystem);

			var move = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.MoveInteger32 : IRInstruction.MoveInteger64;

			context.SetInstruction(move, context.Result, typeDef);
		}
	}
}
