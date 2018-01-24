// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Text;

namespace Mosa.Compiler.Framework.Expression
{
	public class Value
	{
		public ValueType ValueType { get; protected set; }

		public ulong UnsignedInteger { get; protected set; }
		public long SignedInteger { get { return (long)UnsignedInteger; } set { UnsignedInteger = (ulong)value; } }
		public double Double { get; protected set; }

		public Value(long i)
		{
			ValueType = ValueType.SignedInteger;
			SignedInteger = i;
		}

		public Value(ulong i)
		{
			ValueType = ValueType.UnsignedInteger;
			UnsignedInteger = i;
		}

		public Value(bool b)
		{
			ValueType = ValueType.SignedInteger;
			SignedInteger = b ? 1 : 0;
		}

		public Value(double d)
		{
			ValueType = ValueType.Double; Double = d;
		}

		public bool IsSignInteger { get { return ValueType == ValueType.SignedInteger; } }
		public bool IsUnsignedInteger { get { return ValueType == ValueType.UnsignedInteger; } }
		public bool IsInteger { get { return ValueType == ValueType.UnsignedInteger || ValueType == ValueType.UnsignedInteger; } }
		public bool IsDouble { get { return ValueType == ValueType.Double; } }

		public bool IsZero { get { return IsInteger && UnsignedInteger == 0; } }
		public bool IsTrue { get { return IsInteger && UnsignedInteger != 0; } }
		public bool IsFalse { get { return IsInteger && UnsignedInteger == 0; } }

		public override string ToString()
		{
			var sb = new StringBuilder();

			if (IsSignInteger)
				sb.Append(SignedInteger.ToString());
			else if (IsUnsignedInteger)
				sb.Append(UnsignedInteger.ToString());
			else if (IsDouble)
				sb.Append(Double.ToString());

			sb.Append(" [");
			sb.Append(ValueType.ToString());
			sb.Append("]");

			return sb.ToString();
		}
	}
}
