// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Propagate;
using Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Rewrite;
using Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual;

/// <summary>
/// Transformations
/// </summary>
public static class ManualTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new ConstantMove.Compare32x32(),
		new ConstantMove.Compare32x64(),
		new ConstantMove.Compare64x32(),
		new ConstantMove.Compare64x64(),
		new ConstantMove.Branch32(),
		new ConstantMove.Branch64(),
		new ConstantMove.AddCarryOut32(),
		new ConstantMove.AddCarryOut64(),
		new ConstantMove.AddOverflowOut32(),
		new ConstantMove.AddOverflowOut64(),
		new ConstantMove.MulCarryOut32(),
		new ConstantMove.MulCarryOut64(),
		new ConstantMove.MulOverflowOut32(),
		new ConstantMove.MulOverflowOut64(),

		new ConstantFolding.AddCarryOut32(),
		new ConstantFolding.AddCarryOut64(),
		new ConstantFolding.AddOverflowOut32(),
		new ConstantFolding.AddOverflowOut64(),
		new ConstantFolding.SubCarryOut32(),
		new ConstantFolding.SubCarryOut64(),
		new ConstantFolding.SubOverflowOut32(),
		new ConstantFolding.SubOverflowOut64(),
		new ConstantFolding.MulCarryOut32(),
		new ConstantFolding.MulCarryOut64(),
		new ConstantFolding.MulOverflowOut32(),
		new ConstantFolding.MulOverflowOut64(),
		new ConstantFolding.Compare32x32(),
		new ConstantFolding.Compare32x64(),
		new ConstantFolding.Compare64x32(),
		new ConstantFolding.Compare64x64(),
		new ConstantFolding.Branch32(),
		new ConstantFolding.Branch64(),
		new ConstantFolding.Switch(),

		new StrengthReduction.AddCarryOut32ByZero(),
		new StrengthReduction.AddCarryOut64ByZero(),
		new StrengthReduction.AddOverflowOut32ByZero(),
		new StrengthReduction.AddOverflowOut64ByZero(),
		new StrengthReduction.SubCarryOut32ByZero(),
		new StrengthReduction.SubCarryOut64ByZero(),
		new StrengthReduction.SubOverflowOut32ByZero(),
		new StrengthReduction.SubOverflowOut64ByZero(),

		new StrengthReduction.AddCarryOut32ByZero2(),
		new StrengthReduction.AddCarryOut64ByZero2(),
		new StrengthReduction.AddOverflowOut32ByZero2(),
		new StrengthReduction.AddOverflowOut64ByZero2(),
		new StrengthReduction.SubOverflowOut32ByZero2(),
		new StrengthReduction.SubOverflowOut64ByZero2(),

		new StrengthReduction.MulCarryOut32ByOne(),
		new StrengthReduction.MulCarryOut32ByZero(),
		new StrengthReduction.MulCarryOut64ByOne(),
		new StrengthReduction.MulCarryOut64ByZero(),
		new StrengthReduction.MulOverflowOut32ByOne(),
		new StrengthReduction.MulOverflowOut32ByZero(),
		new StrengthReduction.MulOverflowOut64ByOne(),
		new StrengthReduction.MulOverflowOut64ByZero(),

		new Special.Deadcode(),
		new Special.GetLow32From64(),
		new Special.PromoteAddressOf32(),
		new Special.PromoteAddressOf64(),
		new Special.GetLow32CPURegister(),

		new Simplification.AddCarryOut32CarryNotUsed(),
		new Simplification.AddCarryOut64CarryNotUsed(),
		new Simplification.SubCarryOut32CarryNotUsed(),
		new Simplification.SubCarryOut64CarryNotUsed(),

		new Propagate.Move32Propagate(),
		new Propagate.Move32PropagateConstant(),
		new Propagate.Move64Propagate(),
		new Propagate.Move64PropagateConstant(),
		new Propagate.MoveR4Propagate(),
		new Propagate.MoveR8Propagate(),

		new Propagate.MoveCompoundPropagate(),
		new Propagate.MoveObjectPropagate(),
		new Propagate.MoveManagedPointerPropagate(),
		new Propagate.MoveObjectPropagateConstant(),
		// new Propagate.MoveManagedPointerPropagateConstant(),

		new Propagate.Phi32Propagate(),
		new Propagate.Phi64Propagate(),
		new Propagate.PhiObjectPropagate(),
		new Propagate.PhiManagedPointerPropagate(),
		new Propagate.PhiR4Propagate(),
		new Propagate.PhiR8Propagate(),

		new Phi.Phi32Dead(),
		new Phi.Phi64Dead(),
		new Phi.PhiR4Dead(),
		new Phi.PhiR8Dead(),

		new Phi.Phi32Update(),
		new Phi.Phi64Update(),
		new Phi.PhiR4Update(),
		new Phi.PhiR8Update(),

		new Simplification.Branch32OnlyOneExit(),
		new Simplification.Branch64OnlyOneExit(),
		new Simplification.BranchObjectOnlyOneExit(),
		new Simplification.BranchManagedPointerOnlyOneExit(),

		new Rewrite.Branch32(),
		new Rewrite.Branch64(),

		new Rewrite.Branch32Object(),
		new Rewrite.Branch32ManagedPointer(),
		new Rewrite.Branch64Object(),
		new Rewrite.Branch64ManagedPointer(),
		new Rewrite.Branch32From64(),
		new Rewrite.Branch64From32(),

		new Rewrite.Compare32x32Combine32x32(),
		new Rewrite.Compare32x32Combine64x32(),
		new Rewrite.Compare32x32Combine32x64(),
		new Rewrite.Compare64x64Combine32x32(),
		new Rewrite.Compare64x64Combine64x32(),
		new Rewrite.Compare64x64Combine64x64(),

		new Rewrite.Branch32Combine32x32(),
		new Rewrite.Branch32Combine32x64(),
		new Rewrite.Branch32Combine64x32(),
		new Rewrite.Branch32Combine64x64(),
		new Rewrite.Branch64Combine32x32(),
		new Rewrite.Branch64Combine32x64(),
		new Rewrite.Branch64Combine64x32(),
		new Rewrite.Branch64Combine64x64(),

		new Rewrite.Compare64x32ZeroExtend(),
		new Rewrite.Compare64x32SignExtended(),
		new Rewrite.Compare64x32SignZeroExtend(),
		new Rewrite.Compare64x32ZeroSignExtend(),

		new Simplification.Compare64x32SameHigh(),
		new Simplification.Compare64x32SameLow(),

		new Memory.StoreLoadParam32(),
		new Memory.StoreLoadParam64(),
		new Memory.StoreLoadParamR4(),
		new Memory.StoreLoadParamR8(),
		new Memory.StoreLoadParamObject(), // Dup for MP

		new Memory.LoadStoreParam32(),
		new Memory.LoadStoreParam64(),
		new Memory.LoadStoreParamR4(),
		new Memory.LoadStoreParamR8(),
		new Memory.LoadStoreParamObject(), // Dup for MP

		new Memory.DoubleStoreParam32(),
		new Memory.DoubleStoreParam64(),
		new Memory.DoubleStoreParamR4(),
		new Memory.DoubleStoreParamR8(),
		new Memory.DoubleStoreParamObject(), // Dup for MP

		new Memory.LoadStore32(),
		new Memory.LoadStore64(),
		new Memory.LoadStoreR4(),
		new Memory.LoadStoreR8(),
		new Memory.LoadStoreObject(), // Dup for MP

		new Memory.StoreLoad32(),
		new Memory.StoreLoad64(),
		new Memory.StoreLoadR4(),
		new Memory.StoreLoadR8(),
		new Memory.StoreLoadObject(), // Dup for MP

		new Memory.DoubleStore32(),
		new Memory.DoubleStore64(),
		new Memory.DoubleStoreR4(),
		new Memory.DoubleStoreR8(),
		new Memory.DoubleStoreObject(), // Dup for MP

		new Memory.LoadZeroExtend16x32Store16(),
		new Memory.LoadZeroExtend16x64Store16(),
		new Memory.LoadZeroExtend8x32Store8(),
		new Memory.LoadZeroExtend8x64Store8(),
		new Memory.LoadZeroExtend32x64Store32(),

		new Memory.LoadSignExtend32x64Store32(),
		new Memory.LoadSignExtend16x32Store16(),
		new Memory.LoadSignExtend16x64Store16(),
		new Memory.LoadSignExtend8x32Store8(),
		new Memory.LoadSignExtend8x64Store8(),

		new Memory.LoadParamSignExtend16x32Store16(),
		new Memory.LoadParamSignExtend16x64Store16(),
		new Memory.LoadParamSignExtend8x32Store8(),
		new Memory.LoadParamSignExtend8x64Store8(),
		new Memory.LoadParamSignExtend32x64Store32(),
		new Memory.LoadParamZeroExtend16x32Store16(),
		new Memory.LoadParamZeroExtend16x64Store16(),
		new Memory.LoadParamZeroExtend8x32Store8(),
		new Memory.LoadParamZeroExtend8x64Store8(),
		new Memory.LoadParamZeroExtend32x64Store32(),

		new Memory.DoubleLoad32(),
		new Memory.DoubleLoad64(),
		new Memory.DoubleLoadR4(),
		new Memory.DoubleLoadR8(),
		new Memory.DoubleLoadParamObject(), // Dup for MP

		new Memory.DoubleLoadParam32(),
		new Memory.DoubleLoadParam64(),
		new Memory.DoubleLoadParamR4(),
		new Memory.DoubleLoadParamR8(),
		new Memory.DoubleLoadParamObject(), // Dup for MP

		//new Special.Phi32Conditional(),

		new Rewrite.Branch32GreaterOrEqualThanZero(),
		new Rewrite.Branch32LessThanZero(),
		new Rewrite.Branch32GreaterThanZero(),
		new Rewrite.Branch32LessOrEqualThanZero(),

		new Rewrite.Branch64GreaterOrEqualThanZero(),
		new Rewrite.Branch64LessThanZero(),
		new Rewrite.Branch64GreaterThanZero(),
		new Rewrite.Branch64LessOrEqualThanZero(),

		new Phi.Phi32Add32(),

		new Checked.CheckThrowDivideByZero(),
		new Checked.CheckThrowIndexOutOfRange(),
		new Checked.CheckThrowOverflow(),

		new CodeMotion.Load32(),
		new CodeMotion.Load64(),
		new CodeMotion.LoadR4(),
		new CodeMotion.LoadR8(),
		new CodeMotion.LoadObject(), // Dup for MP
		new CodeMotion.LoadCompound(),

		new CodeMotion.LoadSignExtend16x32(),
		new CodeMotion.LoadSignExtend8x32(),
		new CodeMotion.LoadZeroExtend16x32(),
		new CodeMotion.LoadZeroExtend8x32(),

		new CodeMotion.LoadParam32(),
		new CodeMotion.LoadParam64(),
		new CodeMotion.LoadParamR4(),
		new CodeMotion.LoadParamR8(),
		new CodeMotion.LoadParamObject(),
		new CodeMotion.LoadParamManagedPointer(),
		new CodeMotion.LoadParamCompound(),
		new CodeMotion.LoadParamSignExtend16x32(),
		new CodeMotion.LoadParamSignExtend8x32(),
		new CodeMotion.LoadParamZeroExtend16x32(),
		new CodeMotion.LoadParamZeroExtend8x32(),
	};
}
