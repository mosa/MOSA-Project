// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("System.Array::GetLength")]
	[ReplacementTarget("System.Array::GetLowerBound")]
	public sealed class InternalsForArray : InternalsBase, IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Internal(context, methodCompiler, context.InvokeMethod.Name, "InternalsForArray");
		}
	}
}
