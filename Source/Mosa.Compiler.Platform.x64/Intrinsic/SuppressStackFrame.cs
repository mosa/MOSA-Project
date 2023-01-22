// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x64.Intrinsic:SuppressStackFrame")]
		private static void SurpressStackFrame(Context context, MethodCompiler methodCompiler)
		{
			methodCompiler.IsStackFrameRequired = false;
			context.Empty();
		}
	}
}
