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
	public static class Instruction
	{
		private static AddressOfInstruction _addressOfInstruction = new AddressOfInstruction();
		private static ArithmeticShiftRightInstruction _arithmeticShiftRightInstruction = new ArithmeticShiftRightInstruction();
		private static BranchInstruction _branchInstruction = new BranchInstruction();
		private static EpilogueInstruction _epilogueInstruction = new EpilogueInstruction();
		private static FloatingPointCompareInstruction _floatingPointCompareInstruction = new FloatingPointCompareInstruction();

		private static PrologueInstruction _prologueInstruction = new PrologueInstruction();

		/// <summary>
		/// Gets the address of instruction.
		/// </summary>
		/// <value>The address of instruction.</value>
		public static AddressOfInstruction AddressOfInstruction { get { return _addressOfInstruction; } }
		/// <summary>
		/// Gets the arithmetic shift right instruction.
		/// </summary>
		/// <value>The arithmetic shift right instruction.</value>
		public static ArithmeticShiftRightInstruction ArithmeticShiftRightInstruction { get { return _arithmeticShiftRightInstruction; } }
		/// <summary>
		/// Gets the branch instruction.
		/// </summary>
		/// <value>The branch instruction.</value>
		public static BranchInstruction BranchInstruction { get { return _branchInstruction; } }
		/// <summary>
		/// Gets the epilogue instruction.
		/// </summary>
		/// <value>The epilogue instruction.</value>
		public static EpilogueInstruction EpilogueInstruction { get { return _epilogueInstruction; } }
		/// <summary>
		/// Gets the floating point compare instruction.
		/// </summary>
		/// <value>The floating point compare instruction.</value>
		public static FloatingPointCompareInstruction FloatingPointCompareInstruction { get { return _floatingPointCompareInstruction; } }
		/// <summary>
		/// Gets the prologue instruction.
		/// </summary>
		/// <value>The prologue instruction.</value>
		public static PrologueInstruction PrologueInstruction { get { return _prologueInstruction; } }

	}
}
