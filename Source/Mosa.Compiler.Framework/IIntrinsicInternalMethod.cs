// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Interface to an intrinsic instruction
	/// </summary>
	public interface IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler);
	}
}