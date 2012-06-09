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
		public static readonly AddFloat AddF = new AddFloat();
		/// <summary>
		/// 
		/// </summary>
		public static readonly AddSigned AddS = new AddSigned();
		/// <summary>
		/// 
		/// </summary>
		public static readonly AddUnsigned AddU = new AddUnsigned();
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
		public static readonly DivFloat DivF = new DivFloat();
		/// <summary>
		/// 
		/// </summary>
		public static readonly DivSigned DivS = new DivSigned();
		/// <summary>
		/// 
		/// </summary>
		public static readonly DivUnsigned DivU = new DivUnsigned();
		/// <summary>
		/// 
		/// </summary>
		public static Epilogue Epilogue = new Epilogue();
		/// <summary>
		/// 
		/// </summary>
		public static FloatCompare FloatingPointCompare = new FloatCompare();
		/// <summary>
		/// 
		/// </summary>
		public static FloatToIntegerConversion FloatingPointToIntegerConversion = new FloatToIntegerConversion();
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
		public static IntegerToFloatConversion IntegerToFloatConversion = new IntegerToFloatConversion();
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
		public static MulFloat MulF = new MulFloat();
		/// <summary>
		/// 
		/// </summary>
		public static MulSigned MulS = new MulSigned();
		/// <summary>
		/// 
		/// </summary>
		public static MulUnsigned MulU = new MulUnsigned();
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
		public static readonly RemFloat RemF = new RemFloat();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RemSigned RemS = new RemSigned();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RemUnsigned RemU = new RemUnsigned();
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
		public static readonly SubFloat SubF = new SubFloat();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SubSigned SubS = new SubSigned();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SubUnsigned SubU = new SubUnsigned();
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
