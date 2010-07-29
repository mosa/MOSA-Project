/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public class Nop : IIntrinsicMethod
	{
	
		#region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		public void ReplaceIntrinsicCall(Context context, RuntimeBase runtime)
		{
			context.SetInstruction(CPUx86.Instruction.NopInstruction);
		}

		#endregion // Methods

	}
}
