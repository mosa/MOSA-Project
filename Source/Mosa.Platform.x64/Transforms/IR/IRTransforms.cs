// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// IR Transformation List
/// </summary>
public static class IRTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new AddR4(),
		new AddR8(),
		new AddressOf(),
		new Add32(),
		new AddCarryOut32(),
		new AddCarryIn32(),
		new ArithShiftRight32(),
		new CallDirect(),
		new CompareR4(),
		new CompareR8(),
		new Compare32x32(),
		new Branch32(),
		new Branch64(),
		new BranchObject(),
		new CompareObject(),
		new IfThenElse32(),
		new ConvertR4ToR8(),
		new ConvertR8ToR4(),
		new ConvertR4ToI32(),
		new ConvertR8ToI32(),
		new ConvertI32ToR4(),
		new ConvertI32ToR8(),
		new DivR4(),
		new DivR8(),
		new DivSigned32(),
		new DivUnsigned32(),
		new Jmp(),
		new LoadR4(),
		new LoadR8(),
		new LoadObject(),
		new Load32(),
		new LoadParamObject(),
		new LoadSignExtend8x32(),
		new LoadSignExtend16x32(),
		new LoadZeroExtend8x32(),
		new LoadZeroExtend16x32(),
		new LoadParamR4(),
		new LoadParamR8(),
		new LoadParam32(),
		new LoadParamSignExtend8x32(),
		new LoadParamSignExtend16x32(),
		new LoadParamZeroExtend8x32(),
		new LoadParamZeroExtend16x32(),
		new And32(),
		new Not32(),
		new Or32(),
		new Xor32(),
		new MoveR4(),
		new MoveR8(),
		new Move32(),
		new Move64(),
		new MoveObject(),
		new SignExtend8x32(),
		new SignExtend16x32(),
		new ZeroExtend8x32(),
		new ZeroExtend16x32(),
		new MulR4(),
		new MulR8(),
		new MulSigned32(),
		new MulUnsigned32(),
		new Nop(),
		new RemSigned32(),
		new RemUnsigned32(),
		new ShiftLeft32(),
		new ShiftRight32(),
		new StoreR4(),
		new StoreR8(),
		new StoreObject(),
		new Store8(),
		new Store16(),
		new Store32(),
		new StoreParamR4(),
		new StoreParamR8(),
		new StoreParam8(),
		new StoreParam16(),
		new StoreParam32(),
		new StoreParamObject(),
		new SubR4(),
		new SubR8(),
		new Sub32(),
		new SubCarryOut32(),
		new SubCarryIn32(),
		new Switch(),
		new Add64(),
		new ArithShiftRight64(),
		new Compare32x64(),
		new Compare64x32(),
		new Compare64x64(),
		new ConvertR4ToI64(),
		new ConvertR8ToI64(),
		new ConvertI64ToR4(),
		new ConvertI64ToR8(),
		new IfThenElse64(),
		new Load64(),
		new LoadSignExtend32x64(),
		new LoadParam64(),
		new LoadParamSignExtend16x64(),
		new LoadParamSignExtend32x64(),
		new LoadParamSignExtend8x64(),
		new LoadParamZeroExtend16x64(),
		new LoadParamZeroExtend32x64(),
		new LoadParamZeroExtend8x64(),
		new And64(),
		new Not64(),
		new Or64(),
		new Xor64(),
		new MulSigned64(),
		new MulUnsigned64(),
		new ShiftLeft64(),
		new ShiftRight64(),
		new SignExtend16x64(),
		new SignExtend32x64(),
		new SignExtend8x64(),
		new Store64(),
		new StoreParam64(),
		new Sub64(),
		new Truncate64x32(),
		new ZeroExtend16x64(),
		new ZeroExtend32x64(),
		new ZeroExtend8x64(),
		new DivSigned64(),
		new DivUnsigned64(),
		new LoadSignExtend16x64(),
		new LoadSignExtend8x64(),
		new LoadZeroExtend16x64(),
		new LoadZeroExtend32x64(),
		new LoadZeroExtend8x64(),
		new RemSigned64(),
		new RemUnsigned64(),
	};
}
