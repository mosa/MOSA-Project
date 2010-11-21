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
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platform.X86.CPUx86
{
	/// <summary>
	/// Interface to a X86 instruction
	/// </summary>
	public interface IX86Instruction : IInstruction, IPlatformInstruction
	{

	}
}
