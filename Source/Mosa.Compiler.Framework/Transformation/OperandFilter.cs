// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transformation
{
	public sealed class OperandFilter
	{
		#region PreBuilt

		public readonly static OperandFilter Any = new OperandFilter(Type.Any);
		public readonly static OperandFilter VirtualRegister = new OperandFilter(Type.VirtualRegister);
		public readonly static OperandFilter Constant = new OperandFilter(Type.Constant);
		public readonly static OperandFilter ResolvedConstant = new OperandFilter(Type.ResolvedConstant);
		public readonly static OperandFilter UnresolvedConstant = new OperandFilter(Type.UnresolvedConstant);
		public readonly static OperandFilter CPURegister = new OperandFilter(Type.CPURegister);
		public readonly static OperandFilter Register = new OperandFilter(Type.Register);

		public readonly static OperandFilter Constant_0 = new OperandFilter((ulong)0);
		public readonly static OperandFilter Constant_1 = new OperandFilter((ulong)1);
		public readonly static OperandFilter ConstantR4_0 = new OperandFilter((float)0);
		public readonly static OperandFilter ConstantR8_0 = new OperandFilter((double)0);

		#endregion PreBuilt

		public enum Type { Any, VirtualRegister, Constant, ResolvedConstant, UnresolvedConstant, CPURegister, Register }

		public bool IsVirtualRegister { get; private set; }
		public bool IsCPURegister { get; private set; }
		public bool IsConstant { get; private set; }
		public bool IsResolvedConstant { get; private set; }
		public bool IsUnresolvedConstant { get; private set; }
		public bool HasSpecificConstant { get; private set; }

		public ulong IntegerConstant;

		public float FloatConstant;

		public double DoubleConstant;

		private OperandFilter()
		{
			IsVirtualRegister = false;
			IsCPURegister = false;
			IsConstant = false;
			IsResolvedConstant = false;
			IsUnresolvedConstant = false;
			HasSpecificConstant = false;
		}

		public OperandFilter(Type type)
			: base()
		{
			switch (type)
			{
				case Type.Any: { IsVirtualRegister = true; IsCPURegister = true; IsConstant = true; IsResolvedConstant = true; IsUnresolvedConstant = true; break; }
				case Type.VirtualRegister: { IsVirtualRegister = true; break; }
				case Type.CPURegister: { IsCPURegister = true; break; }
				case Type.Register: { IsVirtualRegister = true; IsCPURegister = true; break; }
				case Type.Constant: { IsConstant = true; IsCPURegister = true; break; }
				case Type.ResolvedConstant: { IsResolvedConstant = true; break; }
				case Type.UnresolvedConstant: { IsUnresolvedConstant = true; break; }
				default: break;
			}
		}

		public OperandFilter(ulong constant)
			: base()
		{
			HasSpecificConstant = true;
			IsResolvedConstant = true;
			IntegerConstant = constant;
		}

		public OperandFilter(double constant)
			: base()
		{
			HasSpecificConstant = true;
			IsResolvedConstant = true;
			DoubleConstant = constant;
		}

		public OperandFilter(float constant)
			: base()
		{
			HasSpecificConstant = true;
			IsResolvedConstant = true;
			FloatConstant = constant;
		}

		public bool Compare(Context context, int index)
		{
			return Compare(context.GetOperand(index));
		}

		public bool Compare(Operand operand)
		{
			if (operand == null)
				return false;

			if (operand.IsVirtualRegister == IsVirtualRegister)
				return true;

			if (operand.IsCPURegister == IsCPURegister)
				return true;

			if (operand.IsConstant == IsConstant && !HasSpecificConstant)
				return true;

			if (operand.IsResolvedConstant == IsResolvedConstant && !HasSpecificConstant)
				return true;

			if (operand.IsUnresolvedConstant == IsUnresolvedConstant && !HasSpecificConstant)
				return true;

			if (operand.IsResolvedConstant == IsResolvedConstant && HasSpecificConstant)
			{
				if (operand.IsInteger && operand.ConstantUnsignedLongInteger == IntegerConstant)
					return true;

				if (operand.IsR8 && operand.ConstantDoubleFloatingPoint == DoubleConstant)
					return true;

				if (operand.IsR4 && operand.ConstantSingleFloatingPoint == FloatConstant)
					return true;
			}

			return false;
		}
	}
}
