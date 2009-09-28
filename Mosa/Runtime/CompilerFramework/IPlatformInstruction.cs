/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Interface to a platform instruction
	/// </summary>
	public interface IPlatformInstruction : IInstruction
	{
		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="codeStream">The code stream.</param>
		void Emit(Context context, System.IO.Stream codeStream);
	}
}
