// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Frame Call
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicPlatformMethod" />
	internal class FrameCall : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand methodAddress = context.Operand1;
			Operand newESP = context.Operand2;

			context.SetInstruction(X86.Call, null, methodAddress);
		}

		#endregion Methods
	}
}
