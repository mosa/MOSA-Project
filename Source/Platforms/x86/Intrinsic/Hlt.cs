/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 hlt instruction.
	/// </summary>
	public class Hlt : IIntrinsicMethod
	{
	
		#region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		public void ReplaceIntrinsicCall(Context context)
		{
			context.SetInstruction(CPUx86.Instruction.HltInstruction);
		}

		#endregion // Methods

	}
}
