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
using System.Diagnostics;


namespace Mosa.Runtime.CompilerFramework.IR2
{
	/// <summary>
	/// 
	/// </summary>
	public class PhiData
	{
		/// <summary>
		/// 
		/// </summary>
		public List<BasicBlock> Blocks = new List<BasicBlock>();
		/// <summary>
		/// 
		/// </summary>
		public List<Operand> Operands = new List<Operand>();
	}
}
