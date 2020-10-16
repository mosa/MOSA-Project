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
		MethodCompiler MethodCompiler { get; }

		/// <summary>
		/// Gets the MosaMethod being compiled.
		/// </summary>
		MosaMethod Method { get; }

		/// <summary>
		/// Gets the type system.
		/// </summary>
		TypeSystem TypeSystem { get; }

		/// <summary>
		/// Gets the type system.
		/// </summary>
		MethodScanner MethodScanner { get; }

		/// <summary>
		/// Gets the instruction being decoded.
		/// </summary>
		MosaInstruction Instruction { get; }

		/// <summary>
		/// Gets the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		BasicBlock GetBlock(int label);

		/// <summary>
		/// Converts the virtual register to stack local.
		/// </summary>
		/// <param name="virtualRegister">The virtual register.</param>
		/// <returns></returns>
		Operand ConvertVirtualRegisterToStackLocal(Operand virtualRegister);
	}
}
