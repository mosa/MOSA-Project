// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("Mosa.Internal.Intrinsic::GetAssemblyListTable")]
	internal class GetAssemblyListTable : IIntrinsicInternalMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			context.SetInstruction(IRInstruction.Move, context.Result, Operand.CreateUnmanagedSymbolPointer(methodCompiler.TypeSystem, Metadata.AssembliesTable));
		}

		#endregion Methods
	}
}
