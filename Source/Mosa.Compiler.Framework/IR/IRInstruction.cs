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
	public static class IRInstruction
	{
		/// <summary>
		/// 
		/// </summary>
		public static readonly AddF AddF = new AddF();
		/// <summary>
		/// 
		/// </summary>
		public static readonly AddS AddS = new AddS();
		/// <summary>
		/// 
		/// </summary>
		public static readonly AddU AddU = new AddU();
		/// <summary>
		/// 
		/// </summary>
		public static AddressOf AddressOf = new AddressOf();
		/// <summary>
		/// 
		/// </summary>
		public static ArithmeticShiftRight ArithmeticShiftRight = new ArithmeticShiftRight();
		/// <summary>
		/// 
		/// </summary>
		public static Break Break = new Break();
		/// <summary>
		/// 
		/// </summary>
		public static readonly DivF DivF = new DivF();
		/// <summary>
		/// 
		/// </summary>
		public static readonly DivS DivS = new DivS();
		/// <summary>
		/// 
		/// </summary>
		public static readonly DivU DivU = new DivU();
		/// <summary>
		/// 
		/// </summary>
		public static Epilogue Epilogue = new Epilogue();
		/// <summary>
		/// 
		/// </summary>
		public static FloatingPointCompare FloatingPointCompare = new FloatingPointCompare();
		/// <summary>
		/// 
		/// </summary>
		public static FloatingPointToIntegerConversion FloatingPointToIntegerConversion = new FloatingPointToIntegerConversion();
		/// <summary>
		/// 
		/// </summary>
		public static readonly IntegerCompareBranch IntegerCompareBranch = new IntegerCompareBranch();
		/// <summary>
		/// 
		/// </summary>
		public static IntegerCompare IntegerCompare = new IntegerCompare();
		/// <summary>
		/// 
		/// </summary>
		public static IntegerToFloatingPointConversion IntegerToFloatingPointConversion = new IntegerToFloatingPointConversion();
		/// <summary>
		/// 
		/// </summary>
		public static Jmp Jmp = new Jmp();
		/// <summary>
		/// 
		/// </summary>
		public static Load Load = new Load();
		/// <summary>
		/// 
		/// </summary>
		public static LogicalAnd LogicalAnd = new LogicalAnd();
		/// <summary>
		/// 
		/// </summary>
		public static LogicalNot LogicalNot = new LogicalNot();
		/// <summary>
		/// 
		/// </summary>
		public static LogicalOr LogicalOr = new LogicalOr();
		/// <summary>
		/// 
		/// </summary>
		public static LogicalXor LogicalXor = new LogicalXor();
		/// <summary>
		/// 
		/// </summary>
		public static Move Move = new Move();
		/// <summary>
		/// 
		/// </summary>
		public static MulF MulF = new MulF();
		/// <summary>
		/// 
		/// </summary>
		public static MulS MulS = new MulS();
		/// <summary>
		/// 
		/// </summary>
		public static MulU MulU = new MulU();
		/// <summary>
		/// 
		/// </summary>
		public static Phi Phi = new Phi();
		/// <summary>
		/// 
		/// </summary>
		public static Prologue Prologue = new Prologue();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RemF RemF = new RemF();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RemS RemS = new RemS();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RemU RemU = new RemU();
		/// <summary>
		/// 
		/// </summary>
		public static Return Return = new Return();
		/// <summary>
		/// 
		/// </summary>
		public static ShiftLeft ShiftLeft = new ShiftLeft();
		/// <summary>
		/// 
		/// </summary>
		public static ShiftRight ShiftRight = new ShiftRight();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SignExtendedMove SignExtendedMove = new SignExtendedMove();
		/// <summary>
		/// 
		/// </summary>
		public static Store Store = new Store();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SubF SubF = new SubF();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SubS SubS = new SubS();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SubU SubU = new SubU();
		/// <summary>
		/// 
		/// </summary>
		public static ZeroExtendedMove ZeroExtendedMove = new ZeroExtendedMove();
		/// <summary>
		/// 
		/// </summary>
		public static Call Call = new Call();
		/// <summary>
		/// 
		/// </summary>
		public static Nop Nop = new Nop();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Switch Switch = new Switch();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Throw Throw = new Throw();
		/// <summary>
		/// 
		/// </summary>
		public static readonly ExceptionPrologue ExceptionPrologue = new ExceptionPrologue();
	}
}
