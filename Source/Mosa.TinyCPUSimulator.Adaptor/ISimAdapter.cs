/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.TypeSystem;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	/// <summary>
	///
	/// </summary>
	public interface ISimAdapter
	{
		/// <summary>
		/// Gets or sets the sim cpu.
		/// </summary>
		/// <value>
		/// The sim cpu.
		/// </value>
		SimCPU SimCPU { get; }

		/// <summary>
		/// Converts the specified method.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="method">The method.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="opcodeSize">Size of the opcode.</param>
		/// <returns></returns>
		SimInstruction Convert(Context context, RuntimeMethod method, BasicBlocks basicBlocks, byte opcodeSize);
	}
}