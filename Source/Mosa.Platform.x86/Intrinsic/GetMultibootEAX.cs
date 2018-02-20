// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Platform.x86.CompilerStages;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// GetMultibootEAX
	/// </summary>
	internal class GetMultibootEAX : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var MultibootEAX = Operand.CreateUnmanagedSymbolPointer(Multiboot0695Stage.MultibootEAX, methodCompiler.TypeSystem);

			context.SetInstruction(IRInstruction.LoadInteger32, context.Result, MultibootEAX, methodCompiler.ConstantZero);
		}

		#endregion Methods
	}
}
