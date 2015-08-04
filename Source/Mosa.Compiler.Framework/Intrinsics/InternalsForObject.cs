// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("System.Object::GetType")]
	[ReplacementTarget("System.Object::MemberwiseClone")]
	public sealed class InternalsForObject : InternalsBase, IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			this.Internal(context, methodCompiler, context.InvokeMethod.Name, "InternalsForObject");
		}
	}
}
