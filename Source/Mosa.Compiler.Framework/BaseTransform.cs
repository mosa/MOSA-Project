// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.Framework;

[Flags]
public enum TransformType
{ Transform, Auto, Manual, Optimization, Window, Search }

public abstract class BaseTransform : IComparable<BaseTransform>
{
	#region Properties

	public BaseInstruction Instruction { get; private set; }

	public bool Log { get; private set; }

	public string Name { get; }

	public int Priority { get; } = 0;

	public virtual bool IsAuto { get; protected set; }

	public bool IsManual => !IsAuto;

	public virtual bool IsOptimization { get; protected set; }

	public bool IsTranformation => !IsOptimization;

	#endregion Properties

	#region Constructors

	public BaseTransform(BaseInstruction instruction, TransformType type, int priority = 0, bool log = false)
	{
		Instruction = instruction;
		Priority = priority;
		Log = log;

		IsAuto = type.HasFlag(TransformType.Auto);
		IsOptimization = type.HasFlag(TransformType.Optimization);

		Name = GetType().FullName.Replace("Mosa.Compiler.", string.Empty).Replace("Mosa.Compiler.Framework.Transforms", "IR").Replace("Transforms.", string.Empty);
	}

	public BaseTransform(BaseInstruction instruction, TransformType type, bool log)
		: this(instruction, type, 0, log)
	{
	}

	public BaseTransform(TransformType type, bool log)
		: this(null, type, 0, log)
	{
	}

	public BaseTransform(TransformType type, int priority = 0, bool log = false)
		: this(null, type, priority, log)
	{
	}

	#endregion Constructors

	#region Internals

	int IComparable<BaseTransform>.CompareTo(BaseTransform other)
	{
		return Priority.CompareTo(other.Priority);
	}

	#endregion Internals

	#region Abstract Methods

	public abstract bool Match(Context context, Transform transform);

	public abstract void Transform(Context context, Transform transform);

	#endregion Abstract Methods

	#region Filter Methods

	protected static bool AreSame(Operand operand1, Operand operand2)
	{
		if (operand1 == operand2)
			return true;

		if (operand1.IsPhysicalRegister && operand2.IsPhysicalRegister && operand1.Register == operand2.Register)
			return true;

		if (operand1.IsResolvedConstant && operand2.IsResolvedConstant)
		{
			if (operand1.IsInteger && operand1.ConstantUnsigned64 == operand2.ConstantUnsigned64)
				return true;

			if (operand1.IsR4 && operand1.IsR4 && operand1.ConstantDouble == operand2.ConstantDouble)
				return true;

			if (operand1.IsR8 && operand1.IsR8 && operand2.ConstantFloat == operand2.ConstantFloat)
				return true;
		}

		return false;
	}

	protected static bool IsConstant(Operand operand)
	{
		return operand.IsConstant;
	}

	protected static bool IsCPURegister(Operand operand)
	{
		return operand.IsPhysicalRegister;
	}

	protected static bool IsCPURegister(Operand operand, PhysicalRegister register)
	{
		return operand.IsPhysicalRegister && operand.Register == register;
	}

	protected static bool IsVirtualRegister(Operand operand)
	{
		return operand.IsVirtualRegister;
	}

	protected static bool IsEqual(ulong a, ulong b)
	{
		return a == b;
	}

	protected static bool Contains(Operand operand, long a)
	{
		return operand.IsResolvedConstant && operand.ConstantSigned32 == a;
	}

	protected static bool Contains(Operand operand, long a, long b)
	{
		return operand.IsResolvedConstant && (operand.ConstantSigned32 == a || operand.ConstantSigned32 == b);
	}

	protected static bool Contains(Operand operand, long a, long b, long c)
	{
		return operand.IsResolvedConstant && (operand.ConstantSigned32 == a || operand.ConstantSigned32 == b || operand.ConstantSigned32 == c);
	}

	protected static bool Contains(Operand operand, ulong a)
	{
		return operand.IsResolvedConstant && operand.ConstantUnsigned64 == a;
	}

	protected static bool Contains(Operand operand, ulong a, ulong b)
	{
		return operand.IsResolvedConstant && (operand.ConstantUnsigned64 == a || operand.ConstantUnsigned64 == b);
	}

	protected static bool Contains(Operand operand1, ulong a, ulong b, ulong c)
	{
		return IsResolvedConstant(operand1) && (operand1.ConstantUnsigned64 == a || operand1.ConstantUnsigned64 == b && operand1.ConstantUnsigned64 == c);
	}

	protected static bool IsEvenInteger(Operand operand)
	{
		return operand.IsResolvedConstant && operand.IsInteger && (operand.ConstantUnsigned64 & 1) == 0;
	}

	protected static bool IsFloatingPoint(Operand operand)
	{
		return operand.IsFloatingPoint;
	}

	protected static bool IsGreater(ulong a, ulong b)
	{
		return a > b;
	}

	protected static bool IsGreaterOrEqual(ulong a, ulong b)
	{
		return a >= b;
	}

	protected static bool IsInteger(Operand operand)
	{
		return operand.IsInteger;
	}

	protected static bool IsUnsignedBetween32(Operand operand, uint a, uint b)
	{
		return operand.IsResolvedConstant && operand.IsInteger && operand.ConstantUnsigned32 >= a && operand.ConstantUnsigned32 <= b;
	}

	protected static bool IsUnsignedBetween64(Operand operand, uint a, uint b)
	{
		return operand.IsResolvedConstant && operand.IsInteger && operand.ConstantUnsigned64 >= a && operand.ConstantUnsigned64 <= b;
	}

	protected static bool IsSignedBetween32(Operand operand, uint a, uint b)
	{
		return operand.IsResolvedConstant && operand.IsInteger && operand.ConstantSigned32 >= a && operand.ConstantSigned32 <= b;
	}

	protected static bool IsSignedBetween64(Operand operand, uint a, uint b)
	{
		return operand.IsResolvedConstant && operand.IsInteger && operand.ConstantSigned64 >= a && operand.ConstantSigned64 <= b;
	}

	protected static bool IsIntegerBetween0And32(Operand operand)
	{
		return operand.IsResolvedConstant && operand.IsInteger && operand.ConstantSigned64 >= 0 && operand.ConstantSigned64 <= 32;
	}

	protected static bool IsIntegerBetween0And64(Operand operand)
	{
		return operand.IsResolvedConstant && operand.IsInteger && operand.ConstantSigned64 >= 0 && operand.ConstantSigned64 <= 64;
	}

	protected static bool IsLessOrEqual(ulong a, ulong b)
	{
		return a <= b;
	}

	protected static bool IsLessThan(ulong a, ulong b)
	{
		return a < b;
	}

	protected static bool IsLessThanZero(ulong a)
	{
		return (long)a < 0;
	}

	protected static bool IsNaturalSquareRoot32(Operand operand)
	{
		if (!operand.IsResolvedConstant)
			return false;

		var value = operand.ConstantUnsigned32;

		var sqrt = Sqrt32(value);
		var s2 = sqrt * sqrt;

		return value == s2;
	}

	protected static bool IsNaturalSquareRoot64(Operand operand)
	{
		if (!operand.IsResolvedConstant)
			return false;

		var value = operand.ConstantUnsigned64;

		var sqrt = Sqrt64(value);
		var s2 = sqrt * sqrt;

		return value == s2;
	}

	protected static bool IsOddInteger(Operand operand)
	{
		return operand.IsResolvedConstant && operand.IsInteger && (operand.ConstantUnsigned64 & 1) == 1;
	}

	protected static bool IsOne(Operand operand)
	{
		return operand.IsConstantOne;
	}

	protected static bool IsParameter(Operand operand)
	{
		return operand.IsParameter;
	}

	protected static bool IsPowerOfTwo32(Operand operand)
	{
		return operand.IsResolvedConstant && BitTwiddling.IsPowerOfTwo(operand.ConstantUnsigned32);
	}

	protected static bool IsPowerOfTwo64(Operand operand)
	{
		return operand.IsResolvedConstant && BitTwiddling.IsPowerOfTwo(operand.ConstantUnsigned64);
	}

	protected static bool IsResolvedConstant(Operand operand)
	{
		return operand.IsResolvedConstant;
	}

	protected static bool IsSignedIntegerPositive(Operand operand)
	{
		return operand.IsResolvedConstant && operand.IsResolvedConstant && operand.IsInteger && operand.ConstantSigned64 >= 0;
	}

	protected static bool IsUnsignedIntegerPositive(Operand operand)
	{
		return operand.IsResolvedConstant && operand.IsResolvedConstant && operand.IsInteger && operand.ConstantUnsigned64 >= 0;
	}

	protected static bool IsZero(Operand operand)
	{
		return operand.IsConstantZero;
	}

	protected static bool IsZero(ulong value)
	{
		return value == 0;
	}

	public static bool AreAnyStatusFlagsUsed(Context context, int window)
	{
		return AreStatusFlagsUsed(context.Instruction, context.Node, window) != TriState.No;
	}

	public static bool IsCarryFlagUsed(Context context, int window)
	{
		return IsCarryFlagUsed(context.Node, window) != TriState.No;
	}

	protected static bool IsResultAndOperand1Same(Context context)
	{
		return context.Result == context.Operand1;
	}

	protected static bool IsResult1Used(Context context)
	{
		return context.Result.IsUsed;
	}

	protected static bool IsResult2Used(Context context)
	{
		return context.Result2.IsUsed;
	}

	protected static bool IsSignedMax32(uint a)
	{
		return a == uint.MaxValue;
	}

	protected static bool IsUnsignedMax32(uint a)
	{
		return (int)a == int.MaxValue;
	}

	protected static bool IsSignedMax64(ulong a)
	{
		return (long)a == long.MaxValue;
	}

	protected static bool IsUnsignedMax64(ulong a)
	{
		return a == ulong.MaxValue;
	}

	#endregion Filter Methods

	#region Expression Methods

	protected static uint Add32(uint a, uint b)
	{
		return a + b;
	}

	protected static ulong Add64(ulong a, ulong b)
	{
		return a + b;
	}

	protected static float AddR4(float a, float b)
	{
		return a + b;
	}

	protected static double AddR8(double a, double b)
	{
		return a + b;
	}

	protected static uint And32(uint a, uint b)
	{
		return a & b;
	}

	protected static ulong And64(ulong a, ulong b)
	{
		return a & b;
	}

	protected static uint BoolTo32(bool b)
	{
		return b ? (uint)1 : 0;
	}

	protected static uint BoolTo32(uint a)
	{
		return a == 0 ? (uint)0 : 1;
	}

	protected static long BoolTo64(bool b)
	{
		return b ? 1 : 0;
	}

	protected static ulong BoolTo64(ulong a)
	{
		return a == 0 ? (ulong)0 : 1;
	}

	protected static float DivR4(float a, float b)
	{
		return a / b;
	}

	protected static double DivR8(double a, double b)
	{
		return a / b;
	}

	protected static long DivSigned32(long a, long b)
	{
		return a / b;
	}

	protected static long DivSigned64(long a, long b)
	{
		return a / b;
	}

	protected static uint DivUnsigned32(uint a, uint b)
	{
		return a / b;
	}

	protected static ulong DivUnsigned64(ulong a, ulong b)
	{
		return a / b;
	}

	protected static uint GetHigh32(ulong a)
	{
		return (uint)(a >> 32);
	}

	protected static uint GetHighestSetBitPosition(ulong value)
	{
		return (uint)BitTwiddling.GetHighestSetBitPosition(value);
	}

	protected static uint CountTrailingZeros(ulong value)
	{
		return (uint)BitTwiddling.CountTrailingZeros(value);
	}

	protected static uint GetPowerOfTwo(ulong value)
	{
		return BitTwiddling.GetPowerOfTwo(value);
	}

	protected static uint GetPowerOfTwo(Operand operand)
	{
		return GetPowerOfTwo(operand.ConstantUnsigned64);
	}

	protected static uint Max32(uint a, uint b)
	{
		return Math.Max(a, b);
	}

	protected static ulong Max64(ulong a, ulong b)
	{
		return Math.Max(a, b);
	}

	protected static uint Min32(uint a, uint b)
	{
		return Math.Min(a, b);
	}

	protected static ulong Min64(ulong a, ulong b)
	{
		return Math.Min(a, b);
	}

	protected static long ModSigned32(long a, long b)
	{
		return a % b;
	}

	protected static long ModSigned64(long a, long b)
	{
		return a % b;
	}

	protected static uint ModUnsigned32(uint a, uint b)
	{
		return a % b;
	}

	protected static ulong ModUnsigned64(ulong a, ulong b)
	{
		return a % b;
	}

	protected static float MulR4(float a, float b)
	{
		return a * b;
	}

	protected static double MulR8(double a, double b)
	{
		return a * b;
	}

	protected static int MulSigned32(int a, int b)
	{
		return a * b;
	}

	protected static long MulSigned64(long a, long b)
	{
		return a * b;
	}

	protected static uint MulUnsigned32(uint a, uint b)
	{
		return a * b;
	}

	protected static ulong MulUnsigned64(ulong a, ulong b)
	{
		return a * b;
	}

	protected static uint NaxU32()
	{
		return uint.MaxValue;
	}

	protected static int NaxI32()
	{
		return int.MaxValue;
	}

	protected static ulong NaxU64()
	{
		return ulong.MaxValue;
	}

	protected static long NaxI64()
	{
		return long.MaxValue;
	}

	protected static int Neg32(int a)
	{
		return -a;
	}

	protected static int Neg32(uint a)
	{
		return -((int)a);
	}

	protected static long Neg64(long a)
	{
		return -a;
	}

	protected static long Neg64(ulong a)
	{
		return -((long)a);
	}

	protected static uint Not32(uint a)
	{
		return ~a;
	}

	protected static ulong Not64(ulong a)
	{
		return ~a;
	}

	protected static uint Or32(uint a, uint b)
	{
		return a | b;
	}

	protected static ulong Or64(ulong a, ulong b)
	{
		return a | b;
	}

	protected static float RemR4(float a, float b)
	{
		return a % b;
	}

	protected static double RemR8(double a, double b)
	{
		return a % b;
	}

	protected static long RemSigned32(long a, long b)
	{
		return a % b;
	}

	protected static long RemSigned64(long a, long b)
	{
		return a % b;
	}

	protected static uint RemUnsigned32(uint a, uint b)
	{
		return a % b;
	}

	protected static ulong RemUnsigned64(ulong a, ulong b)
	{
		return a % b;
	}

	protected static uint ShiftLeft32(uint a, long b)
	{
		return a << (int)b;
	}

	protected static ulong ShiftLeft64(ulong a, long b)
	{
		return a << (int)b;
	}

	protected static ulong ShiftLeft64(ulong a, ulong b)
	{
		return a << (int)b;
	}

	protected static uint ShiftRight32(uint a, long b)
	{
		return a >> (int)b;
	}

	protected static ulong ShiftRight64(ulong a, long b)
	{
		return a >> (int)b;
	}

	protected static uint ArithmeticShiftRight32(uint a, int b)
	{
		return (uint)((int)a >> b);
	}

	protected static ulong ArithmeticShiftRight64(ulong a, long b)
	{
		return (ulong)((long)a >> (int)b);
	}

	protected static ulong Sqrt32(uint num)
	{
		if (0 == num)
			return 0;

		var n = num / 2 + 1;
		var n1 = (n + num / n) / 2;

		while (n1 < n)
		{
			n = n1;
			n1 = (n + num / n) / 2;
		}

		return n;
	}

	protected static ulong Sqrt64(ulong num)
	{
		if (0 == num)
			return 0;

		var n = num / 2 + 1;
		var n1 = (n + num / n) / 2;

		while (n1 < n)
		{
			n = n1;
			n1 = (n + num / n) / 2;
		}

		return n;
	}

	protected static long Sqrt64(long num)
	{
		if (0 == num)
			return 0;

		var n = num / 2 + 1;
		var n1 = (n + num / n) / 2;

		while (n1 < n)
		{
			n = n1;
			n1 = (n + num / n) / 2;
		}

		return n;
	}

	protected static ulong Square32(uint n)
	{
		return n * n;
	}

	protected static ulong Square64(ulong n)
	{
		return n * n;
	}

	protected static uint Sub32(uint a, uint b)
	{
		return a - b;
	}

	protected static ulong Sub64(ulong a, ulong b)
	{
		return a - b;
	}

	protected static float SubR4(float a, float b)
	{
		return a - b;
	}

	protected static double SubR8(double a, double b)
	{
		return a - b;
	}

	protected static uint To32(Operand operand)
	{
		return operand.ConstantUnsigned32;
	}

	protected static uint To32(ulong value)
	{
		return (uint)value;
	}

	protected static ulong To64(uint low, uint high)
	{
		return ((ulong)high << 32) | low;
	}

	protected static ulong To64(Operand operand)
	{
		return operand.ConstantUnsigned64;
	}

	protected static ulong To64(ulong value)
	{
		return value;
	}

	protected static byte ToByte(ulong value)
	{
		return (byte)value;
	}

	protected static byte ToByte(Operand operand)
	{
		return (byte)operand.ConstantUnsigned32;
	}

	protected static float ToR4(Operand operand)
	{
		return operand.ConstantFloat;
	}

	protected static float ToR4(float value)
	{
		return value;
	}

	protected static float ToR4(int a)
	{
		return a;
	}

	protected static float ToR4(long a)
	{
		return a;
	}

	protected static double ToR8(Operand operand)
	{
		return operand.ConstantDouble;
	}

	protected static double ToR8(double value)
	{
		return value;
	}

	protected static double ToR8(int a)
	{
		return a;
	}

	protected static double ToR8(long a)
	{
		return a;
	}

	protected static ushort ToShort(ulong value)
	{
		return (ushort)value;
	}

	protected static ushort ToShort(Operand operand)
	{
		return (ushort)operand.ConstantUnsigned32;
	}

	protected static int ToSigned32(Operand operand)
	{
		return operand.ConstantSigned32;
	}

	protected static long ToSigned64(Operand operand)
	{
		return operand.ConstantSigned64;
	}

	protected static uint Xor32(uint a, uint b)
	{
		return a ^ b;
	}

	protected static ulong Xor64(ulong a, ulong b)
	{
		return a ^ b;
	}

	protected static uint UseCount(Operand operand)
	{
		return (uint)operand.Uses.Count;
	}

	protected static bool IsAddSignedOverflow32(int a, int b)
	{
		return IntegerTwiddling.IsAddSignedOverflow(a, b);
	}

	protected static bool IsAddSignedOverflow64(long a, long b)
	{
		return IntegerTwiddling.IsAddSignedOverflow(a, b);
	}

	protected static bool IsAddUnsignedOverflow32(uint a, uint b)
	{
		return IntegerTwiddling.IsAddUnsignedCarry(a, b);
	}

	protected static bool IsAddUnsignedOverflow64(ulong a, ulong b)
	{
		return IntegerTwiddling.IsAddUnsignedCarry(a, b);
	}

	protected static bool IsAddOverflow32(ulong a, ulong b)
	{
		return IsAddSignedOverflow32((int)a, (int)b) || IsAddUnsignedOverflow32((uint)a, (uint)b);
	}

	protected static bool IsAddOverflow64(ulong a, ulong b)
	{
		return IsAddSignedOverflow64((long)a, (long)b) || IsAddUnsignedOverflow64(a, b);
	}

	#endregion Expression Methods

	#region SignExtend Helpers

	protected static uint SignExtend16x32(ushort value)
	{
		return (value & 0x8000) == 0 ? value : value | 0xFFFF0000;
	}

	protected static ulong SignExtend16x64(ushort value)
	{
		return (value & 0x8000) == 0 ? value : value | 0xFFFFFFFFFFFF0000ul;
	}

	protected static ulong SignExtend32x64(uint value)
	{
		return (value & 0x80000000) == 0 ? value : value | 0xFFFFFFFF00000000ul;
	}

	protected static uint SignExtend8x32(byte value)
	{
		return (value & 0x80) == 0 ? value : value | 0xFFFFFF00;
	}

	protected static ulong SignExtend8x64(byte value)
	{
		return (value & 0x80) == 0 ? value : value | 0xFFFFFFFFFFFFFF00ul;
	}

	#endregion SignExtend Helpers

	#region Status Helpers

	public enum TriState
	{ Yes, No, Unknown };

	public static TriState AreAnyStatusFlagsUsed(Node node, int window)
	{
		return AreStatusFlagsUsed(node.Next, true, true, true, true, true, window);
	}

	public static TriState IsCarryFlagUsed(Node node, int window)
	{
		return AreStatusFlagsUsed(node.Next, false, true, false, false, false, window);
	}

	public static TriState AreStatusFlagsUsed(BaseInstruction instruction, Node node, int window)
	{
		return AreStatusFlagsUsed(node.Next,
			instruction.IsZeroFlagModified,
			instruction.IsCarryFlagModified,
			instruction.IsSignFlagModified,
			instruction.IsOverflowFlagModified,
			instruction.IsParityFlagModified,
			window);
	}

	public static TriState AreStatusFlagsUsed(Node node, bool checkZero, bool checkCarry, bool checkSign, bool checkOverlow, bool checkParity, int window)
	{
		// if none are being checked, then for sure it's a no
		if (!checkZero && !checkCarry && !checkSign && !checkOverlow && !checkParity)
			return TriState.No;

		for (var at = node; ; at = at.Next)
		{
			if (window <= 0)
				return TriState.Unknown;

			if (at.IsEmptyOrNop)
				continue;

			window--;

			if (at.IsBlockEndInstruction)
				return TriState.Unknown;

			if (at.Instruction == IR.Kill
				|| at.Instruction == IR.KillAll
				|| at.Instruction == IR.KillAllExcept
				|| at.Instruction == IR.Gen)
				continue;

			if (at.Instruction.IsReturn)
				return TriState.No;

			if (at.Instruction == IR.Epilogue)
				return TriState.No;

			if (at.Instruction.IsUnconditionalBranch && at.Block.NextBlocks.Count == 1)
			{
				at = at.BranchTargets[0].First;
				continue;
			}

			if (!at.Instruction.IsFlowNext)
				return TriState.Unknown; // Flow direction changed

			var instruction = at.Instruction;

			if (!instruction.IsPlatformInstruction)
				return TriState.Unknown; // Unknown IR instruction

			if ((checkZero && instruction.IsZeroFlagUsed)
				|| (checkCarry && instruction.IsCarryFlagUsed)
				|| (checkSign && instruction.IsSignFlagUsed)
				|| (checkOverlow && instruction.IsOverflowFlagUsed)
				|| (checkParity && instruction.IsParityFlagUsed))
				return TriState.Yes;

			if (checkZero && (instruction.IsZeroFlagCleared || instruction.IsZeroFlagSet || instruction.IsZeroFlagUndefined || instruction.IsZeroFlagModified))
				checkZero = false;

			if (checkCarry && (instruction.IsCarryFlagCleared || instruction.IsCarryFlagSet || instruction.IsCarryFlagUndefined || instruction.IsCarryFlagModified))
				checkCarry = false;

			if (checkSign && (instruction.IsSignFlagCleared || instruction.IsSignFlagSet || instruction.IsSignFlagUndefined || instruction.IsSignFlagModified))
				checkSign = false;

			if (checkOverlow && (instruction.IsOverflowFlagCleared || instruction.IsOverflowFlagSet || instruction.IsOverflowFlagUndefined || instruction.IsOverflowFlagModified))
				checkOverlow = false;

			if (checkParity && (instruction.IsParityFlagCleared || instruction.IsParityFlagSet || instruction.IsParityFlagUndefined || instruction.IsParityFlagModified))
				checkParity = false;

			if (!checkZero && !checkCarry && !checkSign && !checkOverlow && !checkParity)
				return TriState.No;
		}
	}

	#endregion Status Helpers

	#region BitTracker Helpers

	protected static uint BitValueMax32(Operand operand)
	{
		return (uint)operand.BitValue.MaxValue;
	}

	protected static ulong BitValueMax64(Operand operand)
	{
		return (ulong)operand.BitValue.MaxValue;
	}

	protected static uint BitValueMin32(Operand operand)
	{
		return (uint)operand.BitValue.MinValue;
	}

	protected static ulong BitValueMin64(Operand operand)
	{
		return (ulong)operand.BitValue.MinValue;
	}

	protected static bool IsBitValueSignBitCleared32(Operand operand)
	{
		return operand.BitValue.IsSignBitClear32;
	}

	protected static bool IsBitValueSignBitCleared64(Operand operand)
	{
		return operand.BitValue.IsSignBitClear64;
	}

	protected static bool? EvaluateCompare(Operand operand1, Operand operand2, ConditionCode condition)
	{
		return EvaluateCompare(operand1.BitValue, operand2.BitValue, condition);
	}

	protected static bool IsBitValueZeroOrOne(Operand operand)
	{
		return operand.BitValue.IsZeroOrOne;
	}

	public static bool? EvaluateCompare(BitValue value1, BitValue value2, ConditionCode condition)
	{
		switch (condition)
		{
			case ConditionCode.Equal:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					return value1.BitsSet == value2.BitsSet;
				}
				else if (value1.MaxValue == value1.MinValue && value1.MaxValue == value2.MaxValue && value1.MinValue == value2.MinValue)
				{
					return true;
				}
				else if (((value1.BitsSet & value2.BitsSet) != value1.BitsSet || (value1.BitsClear & value2.BitsClear) != value1.BitsClear) && !value1.AreAnyBitsKnown && !value2.AreAnyBitsKnown)
				{
					return false;
				}
				else if ((value1.BitsSet & value2.BitsClear) != 0 || (value2.BitsSet & value1.BitsClear) != 0)
				{
					return false;
				}
				else if (value1.MaxValue < value2.MinValue)
				{
					return false;
				}
				else if (value1.MinValue > value2.MaxValue)
				{
					return false;
				}
				break;

			case ConditionCode.NotEqual:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					return value1.BitsSet != value2.BitsSet;
				}
				else if (value1.MaxValue == value1.MinValue && value1.MaxValue == value2.MaxValue && value1.MinValue == value2.MinValue)
				{
					return false;
				}
				else if (value1.AreAll64BitsKnown && value1.MaxValue == 0 && value2.BitsSet != 0)
				{
					return true;
				}
				else if (value2.AreAll64BitsKnown && value2.MaxValue == 0 && value1.BitsSet != 0)
				{
					return true;
				}
				else if ((value1.BitsSet & value2.BitsClear) != 0 || (value2.BitsSet & value1.BitsClear) != 0)
				{
					return true;
				}
				else if (value1.MaxValue < value2.MinValue)
				{
					return true;
				}
				else if (value1.MinValue > value2.MaxValue)
				{
					return true;
				}
				break;

			case ConditionCode.UnsignedGreater:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					return value1.BitsSet > value2.BitsSet;
				}
				else if (value2.AreAll64BitsKnown && value2.MaxValue == 0 && value1.BitsSet != 0)
				{
					return true;
				}
				else if (value1.MinValue > value2.MaxValue)
				{
					return true;
				}
				else if (value1.MaxValue <= value2.MinValue)
				{
					return false;
				}
				break;

			case ConditionCode.UnsignedLess:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					return value1.MaxValue < value2.MaxValue;
				}
				else if (value1.AreAll64BitsKnown && value1.MaxValue == 0 && value2.BitsSet != 0)
				{
					return true;
				}
				else if (value2.MinValue > value1.MaxValue)
				{
					return true;
				}
				else if (value2.MaxValue <= value1.MinValue)
				{
					return false;
				}

				break;

			case ConditionCode.UnsignedGreaterOrEqual:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					return value1.BitsSet <= value2.BitsSet;
				}
				else if (value1.AreAll64BitsKnown && value1.MaxValue == 0 && value2.BitsSet != 0)
				{
					return true;
				}
				else if (value1.MinValue >= value2.MaxValue)
				{
					return true;
				}
				else if (value1.MaxValue < value2.MinValue)
				{
					return false;
				}

				break;

			case ConditionCode.UnsignedLessOrEqual:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					return value1.BitsSet <= value2.BitsSet;
				}
				else if (value1.AreAll64BitsKnown && value1.MaxValue == 0 && value2.BitsSet != 0)
				{
					return true;
				}
				else if (value2.MinValue >= value1.MaxValue)
				{
					return true;
				}
				else if (value2.MaxValue < value1.MinValue)
				{
					return false;
				}

				break;

			case ConditionCode.Greater:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is64Bit && value2.Is64Bit)
				{
					return (long)value1.BitsSet > (long)value2.BitsSet;
				}
				else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is32Bit && value2.Is32Bit)
				{
					return (int)value1.BitsSet > (int)value2.BitsSet;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitKnown32 && value2.IsSignBitKnown32 && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MinValue > value2.MaxValue)
				{
					return true;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitKnown32 && value2.IsSignBitKnown32 && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MaxValue < value2.MinValue)
				{
					return false;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitKnown64 && value2.IsSignBitKnown64 && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MinValue > value2.MaxValue)
				{
					return true;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitKnown64 && value2.IsSignBitKnown64 && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MaxValue < value2.MinValue)
				{
					return false;
				}

				break;

			case ConditionCode.Less:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is64Bit && value2.Is64Bit)
				{
					return (long)value1.BitsSet < (long)value2.BitsSet;
				}
				else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is32Bit && value2.Is32Bit)
				{
					return (int)value1.BitsSet < (int)value2.BitsSet;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitKnown32 && value2.IsSignBitKnown32 && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MaxValue < value2.MinValue)
				{
					return true;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitKnown32 && value2.IsSignBitKnown32 && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MinValue > value2.MaxValue)
				{
					return false;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitKnown64 && value2.IsSignBitKnown64 && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MaxValue < value2.MinValue)
				{
					return true;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitKnown64 && value2.IsSignBitKnown64 && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MinValue > value2.MaxValue)
				{
					return false;
				}

				break;

			case ConditionCode.GreaterOrEqual:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is64Bit && value2.Is64Bit)
				{
					return (long)value1.BitsSet >= (long)value2.BitsSet;
				}
				else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is32Bit && value2.Is32Bit)
				{
					return (int)value1.BitsSet >= (int)value2.BitsSet;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitKnown32 && value2.IsSignBitKnown32 && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MinValue >= value2.MaxValue)
				{
					return true;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitKnown32 && value2.IsSignBitKnown32 && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MaxValue <= value2.MinValue)
				{
					return false;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitKnown64 && value2.IsSignBitKnown64 && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MinValue >= value2.MaxValue)
				{
					return true;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitKnown64 && value2.IsSignBitKnown64 && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MaxValue <= value2.MinValue)
				{
					return false;
				}

				break;

			case ConditionCode.LessOrEqual:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is64Bit && value2.Is64Bit)
				{
					return (long)value1.BitsSet <= (long)value2.BitsSet;
				}
				else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is32Bit && value2.Is32Bit)
				{
					return (int)value1.BitsSet <= (int)value2.BitsSet;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitKnown32 && value2.IsSignBitKnown32 && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MaxValue <= value2.MinValue)
				{
					return true;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitKnown32 && value2.IsSignBitKnown32 && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MinValue >= value2.MaxValue)
				{
					return false;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitKnown64 && value2.IsSignBitKnown64 && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MaxValue <= value2.MinValue)
				{
					return true;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitKnown64 && value2.IsSignBitKnown64 && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MinValue >= value2.MaxValue)
				{
					return false;
				}

				break;

			default:
				return null;
		}

		return null;
	}

	#endregion BitTracker Helpers

	#region Navigation

	protected static Node GetPreviousNodeUntil(Context context, BaseInstruction untilInstruction, int window, Operand operand1 = null, Operand operand2 = null)
	{
		return GetPreviousNodeUntil(context, untilInstruction, window, out _, operand1, operand2);
	}

	protected static Node GetPreviousNodeUntil(Context context, BaseInstruction untilInstruction, int window, out bool immediate, Operand operand1 = null, Operand operand2 = null)
	{
		immediate = false;

		if (window != 0)
			return null;

		var previous = context.Node.Previous;
		var count = 0;

		while (count < window)
		{
			if (previous.IsEmptyOrNop)
			{
				previous = previous.Previous;
				continue;
			}

			if (previous.Instruction == untilInstruction)
			{
				immediate = count == 0;
				return previous;
			}

			if (previous.IsBlockStartInstruction
				|| previous.Instruction.IsMemoryRead
				|| previous.Instruction.IsMemoryWrite
				|| previous.Instruction.IsIOOperation
				|| previous.Instruction.HasUnspecifiedSideEffect
				|| !previous.Instruction.IsFlowNext)
				return null;

			if (operand1 != null)
			{
				if ((previous.ResultCount >= 1 && previous.Result == operand1)
					|| (previous.ResultCount >= 2 && previous.Result2 == operand1))

					return null;
			}

			if (operand2 != null)
			{
				if ((previous.ResultCount >= 1 && previous.Result == operand2)
					|| (previous.ResultCount >= 2 && previous.Result2 == operand2))

					return null;
			}

			previous = previous.Previous;
			count++;
		}

		return null;
	}

	protected static Node GetNextNodeUntil(Context context, BaseInstruction untilInstruction, int window, Operand operand = null)
	{
		return GetNextNodeUntil(context, untilInstruction, window, out _, operand);
	}

	protected static Node GetNextNodeUntil(Context context, BaseInstruction untilInstruction, int window, out bool immediate, Operand operand = null)
	{
		immediate = false;

		if (window != 0)
			return null;

		var next = context.Node.Next;
		var count = 0;

		while (count < window)
		{
			if (next.IsEmptyOrNop)
			{
				next = next.Next;
				continue;
			}

			if (next.Instruction == untilInstruction)
			{
				immediate = count == 0;
				return next;
			}

			if (next.IsBlockEndInstruction
				|| next.Instruction.IsMemoryRead
				|| next.Instruction.IsMemoryWrite
				|| next.Instruction.IsIOOperation
				|| next.Instruction.HasUnspecifiedSideEffect
				|| !next.Instruction.IsFlowNext)
				return null;

			if (operand != null)
			{
				if ((next.ResultCount >= 1 && next.Result == operand)
					|| (next.ResultCount >= 2 && next.Result2 == operand))

					return null;
			}

			next = next.Next;
			count++;
		}

		return null;
	}

	#endregion Navigation

	#region Helpers

	protected static bool Compare32(ConditionCode conditionCode, Operand a, Operand b)
	{
		return conditionCode switch
		{
			ConditionCode.Equal => a.ConstantSigned32 == b.ConstantSigned32,
			ConditionCode.NotEqual => a.ConstantSigned32 != b.ConstantSigned32,
			ConditionCode.GreaterOrEqual => a.ConstantSigned32 >= b.ConstantSigned32,
			ConditionCode.Greater => a.ConstantSigned32 > b.ConstantSigned32,
			ConditionCode.LessOrEqual => a.ConstantSigned32 <= b.ConstantSigned32,
			ConditionCode.Less => a.ConstantSigned32 < b.ConstantSigned32,
			ConditionCode.UnsignedGreater => a.ConstantUnsigned32 > b.ConstantUnsigned32,
			ConditionCode.UnsignedGreaterOrEqual => a.ConstantUnsigned32 >= b.ConstantUnsigned32,
			ConditionCode.UnsignedLess => a.ConstantUnsigned32 < b.ConstantUnsigned32,
			ConditionCode.UnsignedLessOrEqual => a.ConstantUnsigned32 <= b.ConstantUnsigned32,
			_ => throw new InvalidOperationCompilerException()
		};
	}

	protected static bool Compare64(ConditionCode conditionCode, Operand a, Operand b)
	{
		return conditionCode switch
		{
			ConditionCode.Equal => a.ConstantSigned64 == b.ConstantSigned64,
			ConditionCode.NotEqual => a.ConstantSigned64 != b.ConstantSigned64,
			ConditionCode.GreaterOrEqual => a.ConstantSigned64 >= b.ConstantSigned64,
			ConditionCode.Greater => a.ConstantSigned64 > b.ConstantSigned64,
			ConditionCode.LessOrEqual => a.ConstantSigned64 <= b.ConstantSigned64,
			ConditionCode.Less => a.ConstantSigned64 < b.ConstantSigned64,
			ConditionCode.UnsignedGreater => a.ConstantUnsigned64 > b.ConstantUnsigned64,
			ConditionCode.UnsignedGreaterOrEqual => a.ConstantUnsigned64 >= b.ConstantUnsigned64,
			ConditionCode.UnsignedLess => a.ConstantUnsigned64 < b.ConstantUnsigned64,
			ConditionCode.UnsignedLessOrEqual => a.ConstantUnsigned64 <= b.ConstantUnsigned64,
			_ => throw new InvalidOperationCompilerException()
		};
	}

	protected static bool IsNormal(ConditionCode conditionCode)
	{
		return conditionCode switch
		{
			ConditionCode.Equal => true,
			ConditionCode.NotEqual => true,
			ConditionCode.GreaterOrEqual => true,
			ConditionCode.Greater => true,
			ConditionCode.LessOrEqual => true,
			ConditionCode.Less => true,
			ConditionCode.UnsignedGreater => true,
			ConditionCode.UnsignedGreaterOrEqual => true,
			ConditionCode.UnsignedLess => true,
			ConditionCode.UnsignedLessOrEqual => true,
			_ => false
		};
	}

	#endregion Helpers

	#region Block Helpers

	public static void RemoveRemainingInstructionInBlock(Context context)
	{
		var node = context.Node.Next;

		while (!node.IsBlockEndInstruction)
		{
			if (!node.IsEmptyOrNop)
			{
				node.SetNop();
			}
			node = node.Next;
		}
	}

	public static BasicBlock GetOtherBranchTarget(BasicBlock block, BasicBlock target)
	{
		return block.NextBlocks[0] == target ? block.NextBlocks[1] : block.NextBlocks[0];
	}

	#endregion Block Helpers

	#region Transform Helpers

	public static void SwapOperands1And2(Context context)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		context.Operand1 = operand2;
		context.Operand2 = operand1;
	}

	#endregion Transform Helpers
}
