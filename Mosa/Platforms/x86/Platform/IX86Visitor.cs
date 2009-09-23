/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.Platform
{
	/// <summary>
	/// 
	/// </summary>
	public interface IX86Visitor
	{
		/// <summary>
		/// Nops the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Nop(Context ctx);
	}
}
