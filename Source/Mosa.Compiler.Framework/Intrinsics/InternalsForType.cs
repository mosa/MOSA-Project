// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("System.Type::GetTypeImpl")]
	[ReplacementTarget("System.Type::GetTypeFromHandleImpl")]
	public sealed class InternalsForType : InternalsBase, IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Internal(context, methodCompiler, context.InvokeMethod.Name, "InternalsForType");
		}
	}
}
