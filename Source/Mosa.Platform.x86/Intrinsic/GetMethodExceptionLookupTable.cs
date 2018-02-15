// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// GetMethodExceptionLookupTable
	/// </summary>
	internal class GetMethodExceptionLookupTable : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			context.SetInstruction(IRInstruction.MoveInteger32, context.Result, Operand.CreateUnmanagedSymbolPointer(Metadata.MethodExceptionLookupTable, methodCompiler.TypeSystem));
		}

		#endregion Methods
	}
}
