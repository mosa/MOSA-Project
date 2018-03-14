// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// InternalsForArray
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Intrinsics.BaseInternals" />
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicInternalMethod" />
	[ReplacementTarget("System.Array::Copy")]
	[ReplacementTarget("System.Array::GetLength")]
	[ReplacementTarget("System.Array::GetLowerBound")]
	public sealed class InternalsForArray : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			IntrinsicsHelper.MapToRunTime(context, methodCompiler, context.InvokeMethod.Name, "InternalsForArray");
		}
	}
}
