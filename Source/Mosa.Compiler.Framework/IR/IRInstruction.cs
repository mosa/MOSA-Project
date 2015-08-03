// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
		public static readonly BaseIRInstruction AddFloat = new AddFloat();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction AddSigned = new AddSigned();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction AddUnsigned = new AddUnsigned();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction AddressOf = new AddressOf();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction ArithmeticShiftRight = new ArithmeticShiftRight();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Break = new Break();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction DivFloat = new DivFloat();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction DivSigned = new DivSigned();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction DivUnsigned = new DivUnsigned();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Epilogue = new Epilogue();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction FloatCompare = new FloatCompare();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction FloatToIntegerConversion = new FloatToIntegerConversion();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction IntegerCompareBranch = new IntegerCompareBranch();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction IntegerCompare = new IntegerCompare();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction IntegerToFloatConversion = new IntegerToFloatConversion();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Jmp = new Jmp();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Load = new Load();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction CompoundLoad = new CompoundLoad();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction LoadZeroExtended = new LoadZeroExtended();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction LoadSignExtended = new LoadSignExtended();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction LogicalAnd = new LogicalAnd();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction LogicalNot = new LogicalNot();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction LogicalOr = new LogicalOr();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction LogicalXor = new LogicalXor();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Move = new Move();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction CompoundMove = new CompoundMove();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction MulFloat = new MulFloat();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction MulSigned = new MulSigned();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction MulUnsigned = new MulUnsigned();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Phi = new Phi();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Prologue = new Prologue();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction RemFloat = new RemFloat();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction RemSigned = new RemSigned();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction RemUnsigned = new RemUnsigned();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Return = new Return();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction InternalCall = new InternalCall();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction InternalReturn = new InternalReturn();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction ShiftRight = new ShiftRight();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction ShiftLeft = new ShiftLeft();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction SignExtendedMove = new SignExtendedMove();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Store = new Store();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction CompoundStore = new CompoundStore();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction SubFloat = new SubFloat();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction SubSigned = new SubSigned();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction SubUnsigned = new SubUnsigned();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction ZeroExtendedMove = new ZeroExtendedMove();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Call = new Call();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Nop = new Nop();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Switch = new Switch();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Throw = new Throw();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction TryStart = new TryStart();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction ExceptionStart = new ExceptionStart();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction FinallyStart = new FinallyStart();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction TryEnd = new TryEnd();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction ExceptionEnd = new ExceptionEnd();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction FinallyEnd = new FinallyEnd();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction FinallyReturn = new FinallyReturn();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction IntrinsicMethodCall = new IntrinsicMethodCall();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction BlockEnd = new BlockEnd();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction BlockStart = new BlockStart();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Gen = new Gen();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Kill = new Kill();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction KillAll = new KillAll();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction KillAllExcept = new KillAllExcept();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction CallFinally = new CallFinally();

		/// <summary>
		///
		/// </summary>
		public static readonly BaseIRInstruction Flow = new Flow();
	}
}
