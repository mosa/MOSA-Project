/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using Mosa.Compiler.Framework.Operands;


namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class PhiData
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
