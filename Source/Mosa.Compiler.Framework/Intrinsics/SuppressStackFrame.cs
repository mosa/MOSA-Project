// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::SuppressStackFrame")]
	private static void SurpressStackFrame(Context context, Transform transform)
	{
		transform.MethodCompiler.IsStackFrameRequired = false;
		context.Empty();
	}
}
