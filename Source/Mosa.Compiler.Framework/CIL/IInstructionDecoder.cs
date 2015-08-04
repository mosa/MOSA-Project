// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Interface of instruction decoders.
	/// </summary>
	public interface IInstructionDecoder
	{
		/// <summary>
		/// Gets the method compiler that is currently executing.
		/// </summary>
		BaseMethodCompiler Compiler { get; }

		/// <summary>
		/// Gets the MosaMethod being compiled.
		/// </summary>
		MosaMethod Method { get; }

		/// <summary>
		/// Gets the instruction being decoded.
		/// </summary>
		MosaInstruction Instruction { get; }

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>
		/// The type system.
		/// </value>
		TypeSystem TypeSystem { get; }

		/// <summary>
		/// Gets the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		BasicBlock GetBlock(int label);
	}
}
