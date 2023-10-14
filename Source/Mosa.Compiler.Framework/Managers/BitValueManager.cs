// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Managers;

public class BitValueManager : BaseTransformManager
{
	private readonly Dictionary<Operand, (BitValue BitValue, BitValueStage Stage)> Values = new();

	private readonly BitValue Any;
	private readonly BitValue Zero;

	public BitValueManager(bool is32BitPlatform)
	{
		Any = is32BitPlatform ? BitValue.Any32 : BitValue.Any64;
		Zero = is32BitPlatform ? BitValue.Zero32 : BitValue.Zero64;
	}

	public BitValue GetBitValue(Operand operand)
	{
		if (Values.TryGetValue(operand, out (BitValue BitValue, BitValueStage Stage) value))
			return value.BitValue;

		if (operand.IsResolvedConstant && operand.IsInteger)
		{
			value.BitValue = BitValue.CreateValue(operand.ConstantUnsigned64, operand.IsInt32);
			value.Stage = BitValueStage.Partial;

			Values.Add(operand, value);

			return value.BitValue;
		}
		else if (operand.IsUnresolvedConstant && operand.IsInteger)
		{
			return Any;
		}
		else if (operand.IsNull && operand.IsObject)
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

	public (BitValue BitValue, BitValueStage Stage) GetBitValueV2(Operand operand)
	{
		if (Values.TryGetValue(operand, out (BitValue BitValue, BitValueStage Stage) value))
			return value;

		if (operand.IsResolvedConstant && operand.IsInteger)
		{
			value.BitValue = BitValue.CreateValue(operand.ConstantUnsigned64, operand.IsInt32);
			value.Stage = BitValueStage.Stable;

			Values.Add(operand, value);

			return value;
		}
		else if (operand.IsUnresolvedConstant && operand.IsInteger)
		{
			return (Any, BitValueStage.Stable);
		}
		else if (operand.IsNull && operand.IsObject)
		{
			return (Zero, BitValueStage.Stable);
		}
		else if (operand.IsR4)
		{
			return (BitValue.Any32, BitValueStage.Stable);
		}
		else if (operand.IsR8)
		{
			return (BitValue.Any64, BitValueStage.Stable);
		}

		if (operand.IsObject || operand.IsManagedPointer)
		{
			return (Any, BitValueStage.Stable);
		}

		if (operand.IsInt64)
		{
			return (BitValue.Any64, BitValueStage.Stable);
		}
		else
		{
			return (BitValue.Any32, BitValueStage.Stable);
		}
	}

	//public BitValue GetBitValueDefaultAny(Operand operand)
	//{
	//	var value = GetBitValue(operand);

	//	if (value != null)
	//		return value;

	//	return Any;
	//}

	public void UpdateBitValue(Operand operand, BitValue value, BitValueStage stage = BitValueStage.Partial)
	{
		Values.Remove(operand);
		Values.Add(operand, new(value, stage));
	}
}
