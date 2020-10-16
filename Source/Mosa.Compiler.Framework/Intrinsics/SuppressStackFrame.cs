// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Runtime.Intrinsic::SuppressStackFrame")]
		private static void SurpressStackFrame(Context context, MethodCompiler methodCompiler)
		{
			methodCompiler.IsStackFrameRequired = false;
			context.Empty();
		}
	}
}
