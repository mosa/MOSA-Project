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
		/// <summary>
		/// 
		/// </summary>
		public static AddressOfInstruction AddressOfInstruction = new AddressOfInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static ArithmeticShiftRightInstruction ArithmeticShiftRightInstruction = new ArithmeticShiftRightInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static BranchInstruction BranchInstruction = new BranchInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static EpilogueInstruction EpilogueInstruction = new EpilogueInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static FloatingPointCompareInstruction FloatingPointCompareInstruction = new FloatingPointCompareInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static FloatingPointToIntegerConversionInstruction FloatingPointToIntegerConversionInstruction = new FloatingPointToIntegerConversionInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static IntegerCompareInstruction IntegerCompareInstruction = new IntegerCompareInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static IntegerToFloatingPointConversionInstruction IntegerToFloatingPointConversionInstruction = new IntegerToFloatingPointConversionInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static JmpInstruction JmpInstruction = new JmpInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static LoadInstruction LoadInstruction = new LoadInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static LogicalAndInstruction LogicalAndInstruction = new LogicalAndInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static LogicalNotInstruction LogicalNotInstruction = new LogicalNotInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static LogicalOrInstruction LogicalOrInstruction = new LogicalOrInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static LogicalXorInstruction LogicalXorInstruction = new LogicalXorInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static MoveInstruction MoveInstruction = new MoveInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static PhiInstruction PhiInstruction = new PhiInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static PopInstruction PopInstruction = new PopInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static PrologueInstruction PrologueInstruction = new PrologueInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static ReturnInstruction ReturnInstruction = new ReturnInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static ShiftLeftInstruction ShiftLeftInstruction = new ShiftLeftInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static ShiftRightInstruction ShiftRightInstruction = new ShiftRightInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static SignExtendedMoveInstruction SignExtendedMoveInstruction = new SignExtendedMoveInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static StoreInstruction StoreInstruction = new StoreInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static UDivInstruction UDivInstruction = new UDivInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static URemInstruction URemInstruction = new URemInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static ZeroExtendedMoveInstruction ZeroExtendedMoveInstruction = new ZeroExtendedMoveInstruction();
	}
}
