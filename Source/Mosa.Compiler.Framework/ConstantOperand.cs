// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework
{
	internal static class ConstantOperand
	{
		#region Constant Helper Methods

		public static Operand Create(MosaType type, long value)
		{
			return Operand.CreateConstant(type, (ulong)value);
		}

		public static Operand Create(MosaType type, ulong value)
		{
			return Operand.CreateConstant(type, value);
		}

		public static Operand Create(MosaType type, int value)
		{
			return Operand.CreateConstant(type, (long)value);
		}

		public static Operand Create(MosaType type, uint value)
		{
			return Operand.CreateConstant(type, value);
		}

		public static Operand Create(MosaType type, float value)
		{
			return Operand.CreateConstant(type, value);
		}

		public static Operand Create(MosaType type, double value)
		{
			return Operand.CreateConstant(type, value);
		}

		#endregion Constant Helper Methods
	}
}
