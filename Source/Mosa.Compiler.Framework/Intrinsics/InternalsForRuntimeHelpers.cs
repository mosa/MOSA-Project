// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// InternalsForRuntimeHelpers
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicInternalMethod" />
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray")]
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::GetHashCode")]
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::Equals")]
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::UnsafeCast")]
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::GetAssemblies")]
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::CreateInstance")]
	public sealed class InternalsForRuntimeHelpers : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			IntrinsicsHelper.MapToRunTime(context, methodCompiler, context.InvokeMethod.Name, "InternalsForRuntimeHelpers");
		}
	}
}
