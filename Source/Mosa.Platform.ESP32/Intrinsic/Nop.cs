// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ESP32.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.ESP32.Intrinsic:Nop")]
		private static void Nop(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(ESP32.Nop);
		}
	}
}
