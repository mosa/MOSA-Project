// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	[ReplacementTarget("Mosa.Runtime.Intrinsic::CreateInstanceSimple")]
	internal class CreateInstanceSimple : IIntrinsicInternalMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var ctor = context.Operand1;
			var thisObject = context.Operand2;
			var result = context.Result;
			var method = context.InvokeMethod;

			context.SetInstruction(IRInstruction.Call, null, ctor, thisObject);

			//context.InvokeMethod = method;
			context.AppendInstruction(IRInstruction.MoveInteger, result, thisObject);
		}

		#endregion Methods
	}
}
