// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	[ReplacementTarget("Mosa.Internal.Intrinsic::GetAssemblyListTable")]
	internal class CreateInstanceSimple : IIntrinsicInternalMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var ctor = context.Operand1;
			var thisObject = context.Operand2;
			var result = context.Result;

			context.SetInstruction(IRInstruction.Call, result, ctor, thisObject);
		}

		#endregion Methods
	}
}
