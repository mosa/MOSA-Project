// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis;

public class BitValueManager
{
	private Dictionary<Operand, BitValue> values = new Dictionary<Operand, BitValue>();

	private bool Is32BitPlatform;

	public BitValueManager(bool is32BitPlatform)
	{
		Is32BitPlatform = is32BitPlatform;
	}

	public void Clear()
	{
		values.Clear();
	}

	public BitValue GetBitValue(Operand operand)
	{
		BitValue value;

		if (values.TryGetValue(operand, out value))
			return value;

		if (operand.IsResolvedConstant && operand.IsInteger)
		{
			value = BitValue.CreateValue(operand.ConstantUnsigned64, operand.IsInteger32);

			values.Add(operand, value);

			return value;
		}
		else if (operand.IsUnresolvedConstant && operand.IsInteger)
		{
			return Is32BitPlatform ? BitValue.Any32 : BitValue.Any64;
		}
		else if (operand.IsNull && operand.IsReferenceType)
		{
			return Is32BitPlatform ? BitValue.Zero32 : BitValue.Zero64;
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

	public BitValue GetBitValueWithDefault(Operand operand)
	{
		var value = GetBitValue(operand);

		if (value != null)
			return value;

		return Is32BitPlatform ? BitValue.Any32 : BitValue.Any64;
	}

	public void UpdateBitValue(Operand operand, BitValue value)
	{
		values.Remove(operand);
		values.Add(operand, value);
	}
}
