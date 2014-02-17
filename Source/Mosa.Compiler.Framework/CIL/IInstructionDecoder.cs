/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using dnlib.DotNet.Emit;
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
		/// Gets the Instruction being decoded.
		/// </summary>
		Instruction Instruction { get; }

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>
		/// The type system.
		/// </value>
		TypeSystem TypeSystem { get; }
	}
}