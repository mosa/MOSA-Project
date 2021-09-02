// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Hlt")]
		private static void Hlt(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X64.Hlt);
		}
	}
}
