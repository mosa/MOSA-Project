// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// InternalsForType
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Intrinsics.BaseInternals" />
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicInternalMethod" />
	[ReplacementTarget("System.Type::GetTypeImpl")]
	[ReplacementTarget("System.Type::GetTypeFromHandleImpl")]
	public sealed class InternalsForType : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			IntrinsicsHelper.MapToRunTime(context, methodCompiler, context.InvokeMethod.Name, "InternalsForType");
		}
	}
}
