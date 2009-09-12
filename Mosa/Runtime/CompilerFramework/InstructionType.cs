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
	/// 
	/// </summary>
	public abstract class _InstructionType
	{
		/// <summary>
		/// Emits the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="stream">The stream.</param>
		public abstract void Emit(ref InstructionData instruction, System.IO.Stream stream);
	}
}
