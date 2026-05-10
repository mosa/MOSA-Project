// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual;

/// <summary>
/// Transformations
/// </summary>
public static class ManualTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		ConstantMove.Compare32x32.Instance,
		ConstantMove.Compare32x64.Instance,
		ConstantMove.Compare64x32.Instance,
		ConstantMove.Compare64x64.Instance,
		ConstantMove.Branch32.Instance,
		ConstantMove.Branch64.Instance,
		ConstantMove.BranchObject.Instance,
		ConstantMove.BranchManagedPointer.Instance,
		ConstantMove.AddCarryOut32.Instance,
		ConstantMove.AddCarryOut64.Instance,
		ConstantMove.AddOverflowOut32.Instance,
		ConstantMove.AddOverflowOut64.Instance,
		ConstantMove.MulCarryOut32.Instance,
		ConstantMove.MulCarryOut64.Instance,
		ConstantMove.MulOverflowOut32.Instance,
		ConstantMove.MulOverflowOut64.Instance,

		ConstantFolding.AddCarryOut32.Instance,
		ConstantFolding.AddCarryOut64.Instance,
		ConstantFolding.AddOverflowOut32.Instance,
		ConstantFolding.AddOverflowOut64.Instance,
		ConstantFolding.SubCarryOut32.Instance,
		ConstantFolding.SubCarryOut64.Instance,
		ConstantFolding.SubOverflowOut32.Instance,
		ConstantFolding.SubOverflowOut64.Instance,
		ConstantFolding.MulCarryOut32.Instance,
		ConstantFolding.MulCarryOut64.Instance,
		ConstantFolding.MulOverflowOut32.Instance,
		ConstantFolding.MulOverflowOut64.Instance,
		ConstantFolding.Compare32x32.Instance,
		ConstantFolding.Compare32x64.Instance,
		ConstantFolding.Compare64x32.Instance,
		ConstantFolding.Compare64x64.Instance,
		ConstantFolding.Branch32.Instance,
		ConstantFolding.Branch64.Instance,
		ConstantFolding.BranchObject.Instance,
		ConstantFolding.BranchManagedPointer.Instance,
		ConstantFolding.Switch.Instance,

		Special.Deadcode.Instance,
		Special.GetLow32From64.Instance,
		Special.GetLow32CPURegister.Instance,
		Special.Store32AddressOf.Instance,
		Special.Store64AddressOf.Instance,
		Special.Load32AddressOf.Instance,
		Special.Load64AddressOf.Instance,
		Special.StoreLoadObject.Instance,
		Special.StoreLoadManagedPointer.Instance,
		Special.StoreLoad32.Instance,
		Special.StoreLoad64.Instance,

		Propagate.Move32Propagate.Instance,
		Propagate.Move32PropagateConstant.Instance,
		Propagate.Move64Propagate.Instance,
		Propagate.Move64PropagateConstant.Instance,
		Propagate.MoveR4Propagate.Instance,
		Propagate.MoveR8Propagate.Instance,

		Propagate.MoveCompoundPropagate.Instance,
		Propagate.MoveObjectPropagate.Instance,
		Propagate.MoveManagedPointerPropagate.Instance,
		Propagate.MoveObjectPropagateConstant.Instance,
		Propagate.MoveManagedPointerPropagateConstant.Instance,

		Propagate.Phi32Propagate.Instance,
		Propagate.Phi64Propagate.Instance,
		Propagate.PhiObjectPropagate.Instance,
		Propagate.PhiManagedPointerPropagate.Instance,
		Propagate.PhiR4Propagate.Instance,
		Propagate.PhiR8Propagate.Instance,

		Phi.Phi32Dead.Instance,
		Phi.Phi64Dead.Instance,
		Phi.PhiR4Dead.Instance,
		Phi.PhiR8Dead.Instance,

		Phi.Phi32Update.Instance,
		Phi.Phi64Update.Instance,
		Phi.PhiR4Update.Instance,
		Phi.PhiR8Update.Instance,

		Simplification.Branch32OnlyOneExit.Instance,
		Simplification.Branch64OnlyOneExit.Instance,
		Simplification.BranchObjectOnlyOneExit.Instance,
		Simplification.BranchManagedPointerOnlyOneExit.Instance,

		Rewrite.Branch32.Instance,
		Rewrite.Branch64.Instance,

		Rewrite.Branch32Object.Instance,
		Rewrite.Branch32ManagedPointer.Instance,
		Rewrite.Branch64Object.Instance,
		Rewrite.Branch64ManagedPointer.Instance,
		Rewrite.Branch32From64.Instance,
		Rewrite.Branch64From32.Instance,

		Rewrite.Compare32x32Combine32x32.Instance,
		Rewrite.Compare32x32Combine64x32.Instance,
		Rewrite.Compare32x32Combine32x64.Instance,
		Rewrite.Compare64x64Combine32x32.Instance,
		Rewrite.Compare64x64Combine64x32.Instance,
		Rewrite.Compare64x64Combine64x64.Instance,

		Rewrite.Branch32Combine32x32.Instance,
		Rewrite.Branch32Combine32x64.Instance,
		Rewrite.Branch32Combine64x32.Instance,
		Rewrite.Branch32Combine64x64.Instance,
		Rewrite.Branch64Combine32x32.Instance,
		Rewrite.Branch64Combine32x64.Instance,
		Rewrite.Branch64Combine64x32.Instance,
		Rewrite.Branch64Combine64x64.Instance,

		Rewrite.Compare64x32ZeroExtend.Instance,
		Rewrite.Compare64x32SignExtended.Instance,
		Rewrite.Compare64x32SignZeroExtend.Instance,
		Rewrite.Compare64x32ZeroSignExtend.Instance,

		Simplification.Compare64x32SameHigh.Instance,
		Simplification.Compare64x32SameLow.Instance,

		Memory.StoreLoadParam32.Instance,
		Memory.StoreLoadParam64.Instance,
		Memory.StoreLoadParamR4.Instance,
		Memory.StoreLoadParamR8.Instance,
		Memory.StoreLoadParamObject.Instance, // Dup for MP

		Memory.LoadStoreParam32.Instance,
		Memory.LoadStoreParam64.Instance,
		Memory.LoadStoreParamR4.Instance,
		Memory.LoadStoreParamR8.Instance,
		Memory.LoadStoreParamObject.Instance, // Dup for MP

		Memory.DoubleStoreParam32.Instance,
		Memory.DoubleStoreParam64.Instance,
		Memory.DoubleStoreParamR4.Instance,
		Memory.DoubleStoreParamR8.Instance,
		Memory.DoubleStoreParamObject.Instance, // Dup for MP

		Memory.LoadStore32.Instance,
		Memory.LoadStore64.Instance,
		Memory.LoadStoreR4.Instance,
		Memory.LoadStoreR8.Instance,
		Memory.LoadStoreObject.Instance, // Dup for MP

		Memory.StoreLoad32.Instance,
		Memory.StoreLoad64.Instance,
		Memory.StoreLoadR4.Instance,
		Memory.StoreLoadR8.Instance,
		Memory.StoreLoadObject.Instance, // Dup for MP

		Memory.DoubleStore32.Instance,
		Memory.DoubleStore64.Instance,
		Memory.DoubleStoreR4.Instance,
		Memory.DoubleStoreR8.Instance,
		Memory.DoubleStoreObject.Instance, // Dup for MP

		Memory.LoadZeroExtend16x32Store16.Instance,
		Memory.LoadZeroExtend16x64Store16.Instance,
		Memory.LoadZeroExtend8x32Store8.Instance,
		Memory.LoadZeroExtend8x64Store8.Instance,
		Memory.LoadZeroExtend32x64Store32.Instance,

		Memory.LoadSignExtend32x64Store32.Instance,
		Memory.LoadSignExtend16x32Store16.Instance,
		Memory.LoadSignExtend16x64Store16.Instance,
		Memory.LoadSignExtend8x32Store8.Instance,
		Memory.LoadSignExtend8x64Store8.Instance,

		Memory.LoadParamSignExtend16x32Store16.Instance,
		Memory.LoadParamSignExtend16x64Store16.Instance,
		Memory.LoadParamSignExtend8x32Store8.Instance,
		Memory.LoadParamSignExtend8x64Store8.Instance,
		Memory.LoadParamSignExtend32x64Store32.Instance,
		Memory.LoadParamZeroExtend16x32Store16.Instance,
		Memory.LoadParamZeroExtend16x64Store16.Instance,
		Memory.LoadParamZeroExtend8x32Store8.Instance,
		Memory.LoadParamZeroExtend8x64Store8.Instance,
		Memory.LoadParamZeroExtend32x64Store32.Instance,

		Memory.DoubleLoad32.Instance,
		Memory.DoubleLoad64.Instance,
		Memory.DoubleLoadR4.Instance,
		Memory.DoubleLoadR8.Instance,
		Memory.DoubleLoadParamObject.Instance, // Dup for MP

		Memory.DoubleLoadParam32.Instance,
		Memory.DoubleLoadParam64.Instance,
		Memory.DoubleLoadParamR4.Instance,
		Memory.DoubleLoadParamR8.Instance,
		Memory.DoubleLoadParamObject.Instance, // Dup for MP

		Rewrite.Branch32GreaterOrEqualThanZero.Instance,
		Rewrite.Branch32LessThanZero.Instance,
		Rewrite.Branch32GreaterThanZero.Instance,
		Rewrite.Branch32LessOrEqualThanZero.Instance,

		Rewrite.Branch64GreaterOrEqualThanZero.Instance,
		Rewrite.Branch64LessThanZero.Instance,
		Rewrite.Branch64GreaterThanZero.Instance,
		Rewrite.Branch64LessOrEqualThanZero.Instance,

		Phi.Phi32Add32.Instance,

		Checked.CheckThrowDivideByZero.Instance,
		Checked.CheckThrowIndexOutOfRange.Instance,
		Checked.CheckThrowOverflow.Instance,

		CodeMotion.Load32.Instance,
		CodeMotion.Load64.Instance,
		CodeMotion.LoadR4.Instance,
		CodeMotion.LoadR8.Instance,
		CodeMotion.LoadObject.Instance, // Dup for MP
		CodeMotion.LoadCompound.Instance,

		CodeMotion.LoadSignExtend16x32.Instance,
		CodeMotion.LoadSignExtend8x32.Instance,
		CodeMotion.LoadZeroExtend16x32.Instance,
		CodeMotion.LoadZeroExtend8x32.Instance,

		CodeMotion.LoadParam32.Instance,
		CodeMotion.LoadParam64.Instance,
		CodeMotion.LoadParamR4.Instance,
		CodeMotion.LoadParamR8.Instance,
		CodeMotion.LoadParamObject.Instance,
		CodeMotion.LoadParamManagedPointer.Instance,
		CodeMotion.LoadParamCompound.Instance,
		CodeMotion.LoadParamSignExtend16x32.Instance,
		CodeMotion.LoadParamSignExtend8x32.Instance,
		CodeMotion.LoadParamZeroExtend16x32.Instance,
		CodeMotion.LoadParamZeroExtend8x32.Instance,

		Overwrite.Move32Overwrite.Instance,
		Overwrite.Move64Overwrite.Instance,

		StaticLoad.Load32.Instance,
		StaticLoad.Load64.Instance,

		Useless.LoadParamZeroExtend8x32Double.Instance,
		Useless.LoadParamZeroExtend16x32Double.Instance,
		Useless.LoadParamZeroExtend8x64Double.Instance,
		Useless.LoadParamZeroExtend16x64Double.Instance,
		Useless.LoadParamZeroExtend32x64Double.Instance,
		Useless.LoadParamSignExtend8x32Double.Instance,
		Useless.LoadParamSignExtend16x32Double.Instance,
		Useless.LoadParamSignExtend8x64Double.Instance,
		Useless.LoadParamSignExtend16x64Double.Instance,
		Useless.LoadParamSignExtend32x64Double.Instance,

		Useless.ZeroExtend8x32Compare32x32.Instance,
		Useless.ZeroExtend8x64Compare32x64.Instance,
		Useless.ZeroExtend8x64Compare64x64.Instance,

		Useless.Store8ZeroExtend8x32.Instance,
		Useless.Store16ZeroExtend16x32.Instance,

		ConstantMove.BranchManagedPointer.Instance,
		ConstantMove.BranchObject.Instance,

		Phi.Phi32BranchHoisting.Instance,
		Phi.Phi64BranchHoisting.Instance,
		Phi.PhiObjectBranchHoisting.Instance,
		Phi.PhiManagedPointerBranchHoisting.Instance,

		Phi.PhiObjectDead.Instance,
		Phi.PhiObjectUpdate.Instance,
		Phi.PhiManagedPointerDead.Instance,
		Phi.PhiManagedPointerUpdate.Instance,

		BitValue.Compare32x32BitValue.Instance,
		BitValue.Compare32x64BitValue.Instance,
		BitValue.Compare64x32BitValue.Instance,
		BitValue.Compare64x64BitValue.Instance,

		BitValue.Branch32.Instance,
		BitValue.Branch64.Instance,
		BitValue.BranchManagedPointer.Instance,
		BitValue.BranchObject.Instance,

		StrengthReduction.DivUnsignedMagicNumber32.Instance,
		StrengthReduction.DivUnsignedMagicNumber64.Instance,

		NonSSA.Move32.Instance,
		NonSSA.Move32Constant.Instance,
		NonSSA.Move32NotUsed.Instance,
	};
}
