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
			var result = context.Result;
			var address = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.I4);
			context.SetInstruction(IRInstruction.Move, address, Operand.CreateUnmanagedSymbolPointer(methodCompiler.TypeSystem, Multiboot0695Stage.MultibootEAX));
			context.AppendInstruction(IRInstruction.Move, result, Operand.CreateMemoryAddress(methodCompiler.TypeSystem.BuiltIn.I4, address, 0));
		}

		#endregion Methods
	}
}
