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
		/// Executes this simulation/emulation.
		/// </summary>
		void Execute();

		/// <summary>
		/// Resets this simulation/emulation.
		/// </summary>
		void Reset();

		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <returns></returns>
		SimState GetState();

		/// <summary>
		/// Gets the sim monitor.
		/// </summary>
		/// <value>
		/// The sim monitor.
		/// </value>
		SimMonitor Monitor { get; }

		/// <summary>
		/// Converts the specified method.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="context">The context.</param>
		/// <param name="opcodeSize">Size of the opcode.</param>
		/// <returns></returns>
		SimInstruction Convert(RuntimeMethod method, Context context, byte opcodeSize);

		/// <summary>
		/// Adds the instruction.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="instruction">The instruction.</param>
		void AddInstruction(ulong address, SimInstruction instruction);

		/// <summary>
		/// Sets the label.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="label">The label.</param>
		void SetLabel(string label, ulong address);
	}
}