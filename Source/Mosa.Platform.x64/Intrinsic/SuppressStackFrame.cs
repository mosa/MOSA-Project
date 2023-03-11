// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic:SuppressStackFrame")]
	private static void SurpressStackFrame(Context context, MethodCompiler methodCompiler)
	{
		methodCompiler.IsStackFrameRequired = false;
		context.Empty();
	}
}
