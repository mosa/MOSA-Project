// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

/// <summary>
/// LowerTo32 Transforms
/// </summary>
public static class LowerTo32Transforms
{
	public static readonly List<BaseTransform> List = new()
	{
		Add64.Instance,
		And64.Instance,
		Branch64Extends.Instance,
		Compare64x32EqualOrNotEqual.Instance,
		Compare64x64EqualOrNotEqual.Instance,
		//Compare64x32UnsignedGreater(),
		ArithShiftRight64By32.Instance,
		ArithShiftRight64By63.Instance,
		ShiftRight64ByConstant32.Instance,
		ShiftRight64ByConstant32Plus.Instance,
		ShiftLeft64ByConstant32Plus.Instance,
		ShiftLeft64ByConstant32.Instance,
		Load64.Instance,
		LoadParam64.Instance,
		LoadParamSignExtend16x64.Instance,
		LoadParamSignExtend32x64.Instance,
		LoadParamSignExtend8x64.Instance,
		LoadParamZeroExtend16x64.Instance,
		LoadParamZeroExtend32x64.Instance,
		LoadParamZeroExtend8x64.Instance,
		LoadZeroExtend8x64.Instance,
		LoadZeroExtend16x64.Instance,
		LoadZeroExtend32x64.Instance,
		LoadSignExtend8x64.Instance,
		LoadSignExtend16x64.Instance,
		LoadSignExtend32x64.Instance,
		Neg64.Instance,
		Not64.Instance,
		Or64.Instance,
		SignExtend16x64.Instance,
		SignExtend32x64.Instance,
		SignExtend8x64.Instance,
		Store64.Instance,
		StoreParam64.Instance,
		Sub64.Instance,
		Truncate64x32.Instance,
		Xor64.Instance,
		ZeroExtend8x64.Instance,
		ZeroExtend16x64.Instance,
		ZeroExtend32x64.Instance,
		Move64.Instance,

		Compare64x32Rest.Instance,
		Compare64x32RestInSSA.Instance,
		Compare64x64Rest.Instance,
		Compare64x64RestInSSA.Instance,
		Branch64.Instance,

		Phi64.Instance,
		MulSigned64.Instance,
		MulUnsigned64.Instance,
		Compare32x64.Instance,
	};
}
