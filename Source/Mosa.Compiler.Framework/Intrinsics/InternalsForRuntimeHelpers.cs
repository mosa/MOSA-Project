// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray")]
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::GetHashCode")]
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::Equals")]
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::UnsafeCast")]
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::GetAssemblies")]
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::CreateInstance")]
	public sealed class InternalsForRuntimeHelpers : InternalsBase, IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			this.Internal(context, methodCompiler, context.InvokeMethod.Name, "InternalsForRuntimeHelpers");
		}
	}
}