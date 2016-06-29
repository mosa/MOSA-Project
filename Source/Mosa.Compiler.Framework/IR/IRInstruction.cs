// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	public static class IRInstruction
	{
		public static readonly AddFloat AddFloat = new AddFloat();

		public static readonly AddSigned AddSigned = new AddSigned();

		public static readonly AddUnsigned AddUnsigned = new AddUnsigned();

		public static readonly AddressOf AddressOf = new AddressOf();

		public static readonly ArithmeticShiftRight ArithmeticShiftRight = new ArithmeticShiftRight();

		public static readonly Break Break = new Break();

		public static readonly DivFloat DivFloat = new DivFloat();

		public static readonly DivSigned DivSigned = new DivSigned();

		public static readonly DivUnsigned DivUnsigned = new DivUnsigned();

		public static readonly Epilogue Epilogue = new Epilogue();

		public static readonly FloatCompare FloatCompare = new FloatCompare();

		public static readonly FloatToIntegerConversion FloatToIntegerConversion = new FloatToIntegerConversion();

		public static readonly IntegerCompareBranch IntegerCompareBranch = new IntegerCompareBranch();

		public static readonly IntegerCompare IntegerCompare = new IntegerCompare();

		public static readonly IntegerToFloatConversion IntegerToFloatConversion = new IntegerToFloatConversion();

		public static readonly Jmp Jmp = new Jmp();

		public static readonly Load Load = new Load();

		public static readonly Load2 Load2 = new Load2();

		public static readonly CompoundLoad CompoundLoad = new CompoundLoad();

		public static readonly LoadZeroExtended LoadZeroExtended = new LoadZeroExtended();

		public static readonly LoadSignExtended LoadSignExtended = new LoadSignExtended();

		public static readonly LogicalAnd LogicalAnd = new LogicalAnd();

		public static readonly LogicalNot LogicalNot = new LogicalNot();

		public static readonly LogicalOr LogicalOr = new LogicalOr();

		public static readonly LogicalXor LogicalXor = new LogicalXor();

		public static readonly Move Move = new Move();

		public static readonly CompoundMove CompoundMove = new CompoundMove();

		public static readonly MulFloat MulFloat = new MulFloat();

		public static readonly MulSigned MulSigned = new MulSigned();

		public static readonly MulUnsigned MulUnsigned = new MulUnsigned();

		public static readonly Phi Phi = new Phi();

		public static readonly Prologue Prologue = new Prologue();

		public static readonly RemFloat RemFloat = new RemFloat();

		public static readonly RemSigned RemSigned = new RemSigned();

		public static readonly RemUnsigned RemUnsigned = new RemUnsigned();

		public static readonly Return Return = new Return();

		public static readonly InternalCall InternalCall = new InternalCall();

		public static readonly InternalReturn InternalReturn = new InternalReturn();

		public static readonly ShiftRight ShiftRight = new ShiftRight();

		public static readonly ShiftLeft ShiftLeft = new ShiftLeft();

		public static readonly SignExtendedMove SignExtendedMove = new SignExtendedMove();

		public static readonly Store Store = new Store();

		public static readonly CompoundStore CompoundStore = new CompoundStore();

		public static readonly SubFloat SubFloat = new SubFloat();

		public static readonly SubSigned SubSigned = new SubSigned();

		public static readonly SubUnsigned SubUnsigned = new SubUnsigned();

		public static readonly ZeroExtendedMove ZeroExtendedMove = new ZeroExtendedMove();

		public static readonly Call Call = new Call();

		public static readonly Nop Nop = new Nop();

		public static readonly Switch Switch = new Switch();

		public static readonly Throw Throw = new Throw();

		public static readonly TryStart TryStart = new TryStart();

		public static readonly ExceptionStart ExceptionStart = new ExceptionStart();

		public static readonly FilterStart FilterStart = new FilterStart();

		public static readonly FinallyStart FinallyStart = new FinallyStart();

		public static readonly TryEnd TryEnd = new TryEnd();

		public static readonly ExceptionEnd ExceptionEnd = new ExceptionEnd();

		public static readonly FinallyEnd FinallyEnd = new FinallyEnd();

		public static readonly FilterEnd FilterEnd = new FilterEnd();

		public static readonly GotoLeaveTarget GotoLeaveTarget = new GotoLeaveTarget();

		public static readonly SetLeaveTarget SetLeaveTarget = new SetLeaveTarget();

		public static readonly IntrinsicMethodCall IntrinsicMethodCall = new IntrinsicMethodCall();

		public static readonly BlockEnd BlockEnd = new BlockEnd();

		public static readonly BlockStart BlockStart = new BlockStart();

		public static readonly Gen Gen = new Gen();

		public static readonly Kill Kill = new Kill();

		public static readonly KillAll KillAll = new KillAll();

		public static readonly KillAllExcept KillAllExcept = new KillAllExcept();

		public static readonly Flow Flow = new Flow();
	}
}
