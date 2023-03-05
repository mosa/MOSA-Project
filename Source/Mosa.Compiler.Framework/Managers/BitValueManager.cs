// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Managers;

public class BitValueManager : BaseTransformManager
{
	private readonly Dictionary<Operand, BitValue> Values = new Dictionary<Operand, BitValue>();

	private readonly BitValue Any;
	private readonly BitValue Zero;

	public BitValueManager(bool is32BitPlatform)
	{
		Any = is32BitPlatform ? BitValue.Any32 : BitValue.Any64;
		Zero = is32BitPlatform ? BitValue.Zero32 : BitValue.Zero64;
	}

	public BitValue GetBitValue(Operand operand)
	{
		BitValue value;

		if (Values.TryGetValue(operand, out value))
			return value;

		if (operand.IsResolvedConstant && operand.IsInteger)
		{
			value = BitValue.CreateValue(operand.ConstantUnsigned64, operand.IsInteger32);

			Values.Add(operand, value);

			return value;
		}
		else if (operand.IsUnresolvedConstant && operand.IsInteger)
		{
			return Any;
		}
		else if (operand.IsNull && operand.IsReferenceType)
		{
			return Zero;
		}
		else if (operand.IsR4)
		{
			return BitValue.Any32;
		}
		else if (operand.IsR8)
		{
			return BitValue.Any64;
		}

		return null;
	}

	public BitValue GetBitValueDefaultAny(Operand operand)
	{
		var value = GetBitValue(operand);

		if (value != null)
			return value;

		return Any;
	}

	public void UpdateBitValue(Operand operand, BitValue value)
	{
		Values.Remove(operand);
		Values.Add(operand, value);
	}
}
