// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Platform.x86.Stages;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal class GetMultibootEAX : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var zero = Operand.CreateConstant(methodCompiler.TypeSystem.BuiltIn.I4, 0);
			var MultibootEAX = Operand.CreateUnmanagedSymbolPointer(methodCompiler.TypeSystem, Multiboot0695Stage.MultibootEAX);

			context.SetInstruction(IRInstruction.Load2, context.Result, MultibootEAX, zero);
		}

		#endregion Methods
	}
}
