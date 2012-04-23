/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// 
	/// </summary>
	public static class Instruction
	{
		/// <summary>
		/// 
		/// </summary>
		public static readonly AddFInstruction AddFInstruction = new AddFInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly AddSInstruction AddSInstruction = new AddSInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly AddUInstruction AddUInstruction = new AddUInstruction();
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
		public static BreakInstruction BreakInstruction = new BreakInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly DivFInstruction DivFInstruction = new DivFInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly DivSInstruction DivSInstruction = new DivSInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly DivUInstruction DivUInstruction = new DivUInstruction();
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
		public static readonly IntegerCompareBranchInstruction IntegerCompareBranchInstruction = new IntegerCompareBranchInstruction();
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
		public static MulFInstruction MulFInstruction = new MulFInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static MulSInstruction MulSInstruction = new MulSInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static MulUInstruction MulUInstruction = new MulUInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static PhiInstruction PhiInstruction = new PhiInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static PrologueInstruction PrologueInstruction = new PrologueInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RemFInstruction RemFInstruction = new RemFInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RemSInstruction RemSInstruction = new RemSInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RemUInstruction RemUInstruction = new RemUInstruction();
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
		public static readonly SignExtendedMoveInstruction SignExtendedMoveInstruction = new SignExtendedMoveInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static StoreInstruction StoreInstruction = new StoreInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SubFInstruction SubFInstruction = new SubFInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SubSInstruction SubSInstruction = new SubSInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SubUInstruction SubUInstruction = new SubUInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static ZeroExtendedMoveInstruction ZeroExtendedMoveInstruction = new ZeroExtendedMoveInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static CallInstruction CallInstruction = new CallInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static NopInstruction NopInstruction = new NopInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SwitchInstruction SwitchInstruction = new SwitchInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly ThrowInstruction ThrowInstruction = new ThrowInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly ExceptionPrologueInstruction ExceptionPrologueInstruction = new ExceptionPrologueInstruction();
	}
}
