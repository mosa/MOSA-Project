/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray")]
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::GetHashCode")]
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::Equals")]
	public sealed class InternalsForRuntimeHelpers : InternalsBase, IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			this.Internal(context, methodCompiler, context.MosaMethod.Name, "InternalsForRuntimeHelpers");
		}
	}
}