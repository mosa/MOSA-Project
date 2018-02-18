// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	///
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicInternalMethod" />
	[ReplacementTarget("Mosa.Runtime.Intrinsic::GetAssemblyListTable")]
	internal class GetAssemblyListTable : IIntrinsicInternalMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var move = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.MoveInteger32 : IRInstruction.MoveInteger64;

			context.SetInstruction(move, context.Result, Operand.CreateUnmanagedSymbolPointer(Metadata.AssembliesTable, methodCompiler.TypeSystem));
		}

		#endregion Methods
	}
}
