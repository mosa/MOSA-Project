// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;

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
		/// <param name="node">The node.</param>
		/// <param name="method">The method.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="opcodeSize">Size of the opcode.</param>
		/// <returns></returns>
		SimInstruction Convert(InstructionNode node, MosaMethod method, BasicBlocks basicBlocks, byte opcodeSize);
	}
}
