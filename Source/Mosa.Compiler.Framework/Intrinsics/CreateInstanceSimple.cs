// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicInternalMethod" />
	[ReplacementTarget("Mosa.Runtime.Intrinsic::CreateInstanceSimple")]
	internal class CreateInstanceSimple : IIntrinsicInternalMethod
	{
		private const string InternalMethodName = "CreateInstanceSimple";

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

			context.SetInstruction(IRInstruction.CallDynamic, null, ctor, thisObject);
			context.AppendInstruction(IRInstruction.MoveInteger32, result, thisObject);
		}

		#endregion Methods
	}
}
