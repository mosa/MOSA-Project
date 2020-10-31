// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;

namespace Mosa.Compiler.Framework.Transform
{
	public abstract class BaseTransformation
	{
		#region Properties

		public BaseInstruction Instruction { get; private set; }

		public bool Log { get; private set; } = false;
		public string Name { get; }

		#endregion Properties

		#region Constructors

		public BaseTransformation(BaseInstruction instruction)
			: this()
		{
			Instruction = instruction;
		}

		public BaseTransformation(BaseInstruction instruction, bool log = false)
			: this()
		{
			Instruction = instruction;
			Log = log;
		}

		protected BaseTransformation()
		{
			Name = ExtractName();
			TransformationDirectory.Add(this);
		}

		#endregion Constructors

		#region Internals

		private string ExtractName()
		{
			string name = GetType().FullName;

			int offset1 = name.IndexOf('.');
			int offset2 = name.IndexOf('.', offset1 + 1);
			int offset3 = name.IndexOf('.', offset2 + 1);
			int offset4 = name.IndexOf('.', offset3 + 1);

			return name.Substring(offset4 + 1);
		}

		#endregion Internals

		public abstract bool Match(Context context, TransformContext transformContext);

		public abstract void Transform(Context context, TransformContext transformContext);

		#region Filter Methods

		protected static bool AreSame(Operand operand1, Operand operand2)
		{
			if (operand1 == operand2)
				return true;

			if (operand1.IsVirtualRegister && operand1.IsVirtualRegister && operand1 == operand2)
				return true;

			if (operand1.IsCPURegister && operand1.IsCPURegister && operand1 == operand2)
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
			return operand.IsCPURegister;
		}

		protected static bool IsVirtualRegister(Operand operand)
		{
			return operand.IsVirtualRegister;
		}

		protected static bool IsEqual(ulong a, ulong b)
		{
			return a == b;
		}

		protected static bool IsEvenInteger(Operand operand)
		{
			return operand.IsInteger && ((operand.ConstantUnsigned64 & 1) == 0);
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

		protected static bool IsIntegerBetween0And32(Operand operand)
		{
			return operand.IsInteger && operand.ConstantSigned64 >= 0 && operand.ConstantSigned64 <= 32;
		}

		protected static bool IsIntegerBetween0And64(Operand operand)
		{
			return operand.IsInteger && operand.ConstantSigned64 >= 0 && operand.ConstantSigned64 <= 64;
		}

		protected static bool IsLessOrEqual(ulong a, ulong b)
		{
			return a <= b;
		}

		protected static bool IsLessThan(ulong a, ulong b)
		{
			return a < b;
		}

		protected static bool IsNaturalSquareRoot32(Operand operand)
		{
			var value = operand.ConstantUnsigned32;

			var sqrt = Sqrt32(value);
			var s2 = sqrt * sqrt;

			return value == s2;
		}

		protected static bool IsNaturalSquareRoot64(Operand operand)
		{
			var value = operand.ConstantUnsigned64;

			var sqrt = Sqrt64(value);
			var s2 = sqrt * sqrt;

			return value == s2;
		}

		protected static bool IsOddInteger(Operand operand)
		{
			return operand.IsInteger && ((operand.ConstantUnsigned64 & 1) == 1);
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
			return BitTwiddling.IsPowerOfTwo(operand.ConstantUnsigned32);
		}

		protected static bool IsPowerOfTwo64(Operand operand)
		{
			return BitTwiddling.IsPowerOfTwo(operand.ConstantUnsigned64);
		}

		protected static bool IsResolvedConstant(Operand operand)
		{
			return operand.IsResolvedConstant;
		}

		protected static bool IsSignedIntegerPositive(Operand operand)
		{
			return operand.IsResolvedConstant && operand.IsInteger && operand.ConstantSigned64 >= 0;
		}

		protected static bool IsUnsignedIntegerPositive(Operand operand)
		{
			return operand.IsResolvedConstant && operand.IsInteger && operand.ConstantUnsigned64 >= 0;
		}

		protected static bool IsZero(Operand operand)
		{
			return operand.IsConstantZero;
		}

		protected static bool IsZero(ulong value)
		{
			return value == 0;
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

		protected static ulong GetHighestSetBit(ulong value)
		{
			return (ulong)BitTwiddling.GetHighestSetBit(value);
		}

		protected static ulong GetLowestSetBit(ulong value)
		{
			return (ulong)BitTwiddling.GetLowestSetBit(value);
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

		protected static int Neg32(int a)
		{
			return -a;
		}

		protected static long Neg64(long a)
		{
			return -a;
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

		protected static ulong Sqrt32(uint num)
		{
			if (0 == num)
				return 0;

			uint n = (num / 2) + 1;
			uint n1 = (n + (num / n)) / 2;

			while (n1 < n)
			{
				n = n1;
				n1 = (n + (num / n)) / 2;
			}

			return n;
		}

		protected static ulong Sqrt64(ulong num)
		{
			if (0 == num)
				return 0;

			ulong n = (num / 2) + 1;
			ulong n1 = (n + (num / n)) / 2;

			while (n1 < n)
			{
				n = n1;
				n1 = (n + (num / n)) / 2;
			}

			return n;
		}

		protected static long Sqrt64(long num)
		{
			if (0 == num)
				return 0;

			long n = (num / 2) + 1;
			long n1 = (n + (num / n)) / 2;

			while (n1 < n)
			{
				n = n1;
				n1 = (n + (num / n)) / 2;
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

		#endregion Expression Methods

		#region SignExtend Helpers

		protected static uint SignExtend16x32(ushort value)
		{
			return ((value & 0x8000) == 0) ? value : (value | 0xFFFF0000);
		}

		protected static ulong SignExtend16x64(ushort value)
		{
			return ((value & 0x8000) == 0) ? value : (value | 0xFFFFFFFFFFFF0000ul);
		}

		protected static ulong SignExtend32x64(uint value)
		{
			return ((value & 0x80000000) == 0) ? value : (value | 0xFFFFFFFF00000000ul);
		}

		protected static uint SignExtend8x32(byte value)
		{
			return ((value & 0x80) == 0) ? value : (value | 0xFFFFFF00);
		}

		protected static ulong SignExtend8x64(byte value)
		{
			return ((value & 0x80) == 0) ? value : (value | 0xFFFFFFFFFFFFFF00ul);
		}

		#endregion SignExtend Helpers

		#region Status Helpers

		public enum TriState { Yes, No, Unknown };

		public static TriState AreStatusFlagsUsed(InstructionNode start)
		{
			var first = start.Instruction;

			var zeroModified = first.IsZeroFlagModified && !first.IsZeroFlagUndefined;
			var carryModified = first.IsCarryFlagModified && !first.IsCarryFlagUndefined;
			var signModified = first.IsSignFlagModified && !first.IsSignFlagUndefined;
			var overflowModified = first.IsOverflowFlagSet && !first.IsOverflowFlagUndefined;
			var parityModified = first.IsParityFlagModified && !first.IsParityFlagUndefined;

			return AreStatusFlagsUsed(start.Next, zeroModified, carryModified, signModified, overflowModified, parityModified);
		}

		public static TriState AreStatusFlagsUsed(InstructionNode start, bool zeroModified, bool carryModified, bool signModified, bool overflowModified, bool parityModified)
		{
			// if none are modified (or not undefined), then they can't be used later
			if (!zeroModified && !carryModified && !signModified && !overflowModified && !parityModified)
				return TriState.No;

			for (var at = start; ; at = at.Next)
			{
				if (at.IsEmptyOrNop)
					continue;

				if (at.IsBlockEndInstruction)
					return TriState.Unknown;

				if (at.Instruction == IRInstruction.StableObjectTracking
					|| at.Instruction == IRInstruction.UnstableObjectTracking
					|| at.Instruction == IRInstruction.Kill
					|| at.Instruction == IRInstruction.KillAll
					|| at.Instruction == IRInstruction.KillAllExcept
					|| at.Instruction == IRInstruction.Gen)
					continue;

				if (at.Instruction.FlowControl == FlowControl.Return)
					return TriState.No;

				//if (at.Instruction.FlowControl == FlowControl.Throw)
				//	return TriState.No;

				if (at.Instruction.FlowControl == FlowControl.UnconditionalBranch && at.Block.NextBlocks.Count == 1)
				{
					at = at.BranchTargets[0].First;
					continue;
				}

				if (at.Instruction.FlowControl != FlowControl.Next)
					return TriState.Unknown; // Flow direction changed

				var instruction = at.Instruction;

				if (!instruction.IsPlatformInstruction)
					return TriState.Unknown; // Unknown IR instruction

				if ((zeroModified && instruction.IsZeroFlagUsed)
					|| (carryModified && instruction.IsCarryFlagUsed)
					|| (signModified && instruction.IsSignFlagUsed)
					|| (overflowModified && instruction.IsOverflowFlagUsed)
					|| (parityModified && instruction.IsParityFlagUsed))
					return TriState.Yes;

				if (zeroModified && (instruction.IsZeroFlagCleared || instruction.IsZeroFlagSet || instruction.IsZeroFlagUndefined || instruction.IsZeroFlagModified))
					zeroModified = false;

				if (carryModified && (instruction.IsCarryFlagCleared || instruction.IsCarryFlagSet || instruction.IsCarryFlagUndefined || instruction.IsCarryFlagModified))
					carryModified = false;

				if (signModified && (instruction.IsSignFlagCleared || instruction.IsSignFlagSet || instruction.IsSignFlagUndefined || instruction.IsSignFlagModified))
					signModified = false;

				if (overflowModified && (instruction.IsOverflowFlagCleared || instruction.IsOverflowFlagSet || instruction.IsOverflowFlagUndefined || instruction.IsOverflowFlagModified))
					overflowModified = false;

				if (parityModified && (instruction.IsParityFlagCleared || instruction.IsParityFlagSet || instruction.IsParityFlagUndefined || instruction.IsParityFlagModified))
					parityModified = false;

				if (!zeroModified && !carryModified && !signModified && !overflowModified && !parityModified)
					return TriState.No;
			}
		}

		#endregion Status Helpers

		protected static bool IsSSAForm(Operand operand)
		{
			return operand.Definitions.Count == 1;
		}
	}
}
