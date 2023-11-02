// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

/// <summary>
/// LowerTo32 Transforms
/// </summary>
public static class LowerTo32Transforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new Add64(),
		new And64(),
		new Branch64Extends(),
		new Compare64x32EqualOrNotEqual(),
		new Compare64x64EqualOrNotEqual(),
		//Compare64x32UnsignedGreater(),
		new ArithShiftRight64By32(),
		new ArithShiftRight64By63(),
		new ShiftRight64ByConstant32(),
		new ShiftRight64ByConstant32Plus(),
		new ShiftLeft64ByConstant32Plus(),
		new ShiftLeft64ByConstant32(),
		new Load64(),
		new LoadParam64(),
		new LoadParamSignExtend16x64(),
		new LoadParamSignExtend32x64(),
		new LoadParamSignExtend8x64(),
		new LoadParamZeroExtend16x64(),
		new LoadParamZeroExtend32x64(),
		new LoadParamZeroExtend8x64(),
		new LoadZeroExtend8x64(),
		new LoadZeroExtend16x64(),
		new LoadZeroExtend32x64(),
		new LoadSignExtend8x64(),
		new LoadSignExtend16x64(),
		new LoadSignExtend32x64(),
		new Not64(),
		new Or64(),
		new SignExtend16x64(),
		new SignExtend32x64(),
		new SignExtend8x64(),
		new Store64(),
		new StoreParam64(),
		new Sub64(),
		new Truncate64x32(),
		new Xor64(),
		new ZeroExtend8x64(),
		new ZeroExtend16x64(),
		new ZeroExtend32x64(),
		new Move64(),

		new Compare64x32Rest(),
		new Compare64x32RestInSSA(),
		new Compare64x64Rest(),
		new Compare64x64RestInSSA(),
		new Branch64(),

		new Phi64(),
		new MulSigned64(),
		new MulUnsigned64(),
		new Compare32x64(),
	};
}
