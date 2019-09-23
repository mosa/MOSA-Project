// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual
{
	/// <summary>
	/// Transformations
	/// </summary>
	public static class ManualInstance
	{
		public static readonly BaseTransformation IR_ConstantFolding_Compare32x32 = new IR.ConstantFolding.Compare32x32();
		public static readonly BaseTransformation IR_ConstantFolding_Compare32x64 = new IR.ConstantFolding.Compare32x64();
		public static readonly BaseTransformation IR_ConstantFolding_Compare64x32 = new IR.ConstantFolding.Compare64x32();
		public static readonly BaseTransformation IR_ConstantFolding_Compare64x64 = new IR.ConstantFolding.Compare64x64();

		public static readonly BaseTransformation IR_ConstantFolding_CompareBranch32 = new IR.ConstantFolding.CompareBranch32();
		public static readonly BaseTransformation IR_ConstantFolding_CompareBranch64 = new IR.ConstantFolding.CompareBranch64();

		public static readonly BaseTransformation IR_ConstantMove_Compare32x32 = new IR.ConstantMove.Compare32x32();
		public static readonly BaseTransformation IR_ConstantMove_Compare32x64 = new IR.ConstantMove.Compare32x64();
		public static readonly BaseTransformation IR_ConstantMove_Compare64x32 = new IR.ConstantMove.Compare64x32();
		public static readonly BaseTransformation IR_ConstantMove_Compare64x64 = new IR.ConstantMove.Compare64x64();

		public static readonly BaseTransformation IR_Rewrite_Compare32x32 = new IR.Rewrite.Compare32x32GreaterThanZero();
		public static readonly BaseTransformation IR_Rewrite_Compare32x64 = new IR.Rewrite.Compare32x64GreaterThanZero();
		public static readonly BaseTransformation IR_Rewrite_Compare64x32 = new IR.Rewrite.Compare64x32GreaterThanZero();
		public static readonly BaseTransformation IR_Rewrite_Compare64x64 = new IR.Rewrite.Compare64x64GreaterThanZero();

		public static readonly BaseTransformation IR_LowerTo32_Add64 = new IR.LowerTo32.Add64();
		public static readonly BaseTransformation IR_Special_CodeInDeadBlock = new Transform.IR.Special.CodeInDeadBlock();
		public static readonly BaseTransformation IR_Special_Deadcode = new Transform.IR.Special.Deadcode();

		public static readonly BaseTransformation IR_Simplification_AddCarryOut32CarryNotUsed = new IR.Simplification.AddCarryOut32CarryNotUsed();
		public static readonly BaseTransformation IR_Simplification_AddCarryOut64CarryNotUsed = new IR.Simplification.AddCarryOut64CarryNotUsed();
		public static readonly BaseTransformation IR_Simplification_SubCarryOut32CarryNotUsed = new IR.Simplification.SubCarryOut32CarryNotUsed();
		public static readonly BaseTransformation IR_Simplification_SubCarryOut64CarryNotUsed = new IR.Simplification.SubCarryOut64CarryNotUsed();

		public static readonly BaseTransformation IR_Simplification_Compare32x32Same = new IR.Simplification.Compare32x32Same();
		public static readonly BaseTransformation IR_Simplification_Compare32x64Same = new IR.Simplification.Compare32x64Same();
		public static readonly BaseTransformation IR_Simplification_Compare64x32Same = new IR.Simplification.Compare64x32Same();
		public static readonly BaseTransformation IR_Simplification_Compare64x64Same = new IR.Simplification.Compare64x64Same();

		public static readonly BaseTransformation IR_Simplification_Compare32x32NotSame = new IR.Simplification.Compare32x32NotSame();
		public static readonly BaseTransformation IR_Simplification_Compare32x64NotSame = new IR.Simplification.Compare32x64NotSame();
		public static readonly BaseTransformation IR_Simplification_Compare64x32NotSame = new IR.Simplification.Compare64x32NotSame();
		public static readonly BaseTransformation IR_Simplification_Compare64x64NotSame = new IR.Simplification.Compare64x64NotSame();

		public static readonly BaseTransformation IR_Special_Move32Propagate = new Transform.IR.Special.Move32Propagate();
		public static readonly BaseTransformation IR_Special_Move64Propagate = new Transform.IR.Special.Move64Propagate();
		public static readonly BaseTransformation IR_Special_MoveR4Propagate = new Transform.IR.Special.MoveR4Propagate();
		public static readonly BaseTransformation IR_Special_MoveR8Propagate = new Transform.IR.Special.MoveR8Propagate();

		public static readonly BaseTransformation IR_Special_Phi32Propagate = new IR.Special.Phi32Propagate();
		public static readonly BaseTransformation IR_Special_Phi64Propagate = new IR.Special.Phi64Propagate();
		public static readonly BaseTransformation IR_Special_PhiR4Propagate = new IR.Special.PhiR4Propagate();
		public static readonly BaseTransformation IR_Special_PhiR8Propagate = new IR.Special.PhiR8Propagate();

		public static readonly BaseTransformation IR_Special_Phi32Dead = new IR.Special.Phi32Dead();
		public static readonly BaseTransformation IR_Special_Phi64Dead = new IR.Special.Phi64Dead();
		public static readonly BaseTransformation IR_Special_PhiR4Dead = new IR.Special.PhiR4Dead();
		public static readonly BaseTransformation IR_Special_PhiR8Dead = new IR.Special.PhiR8Dead();

		public static readonly BaseTransformation IR_Special_Phi32Update = new IR.Special.Phi32Update();
		public static readonly BaseTransformation IR_Special_Phi64Update = new IR.Special.Phi64Update();
		public static readonly BaseTransformation IR_Special_PhiR4Update = new IR.Special.PhiR4Update();
		public static readonly BaseTransformation IR_Special_PhiR8Update = new IR.Special.PhiR8Update();

		public static readonly BaseTransformation IR_Special_Phi32Invalid = new IR.Special.Phi32Invalid();
		public static readonly BaseTransformation IR_Special_Phi64Invalid = new IR.Special.Phi64Invalid();
		public static readonly BaseTransformation IR_Special_PhiR4Invalid = new IR.Special.PhiR4Invalid();
		public static readonly BaseTransformation IR_Special_PhiR8Invalid = new IR.Special.PhiR8Invalid();

		public static readonly BaseTransformation IR_Simplification_CompareBranch32OnlyOneExit = new IR.Simplification.CompareBranch32OnlyOneExit();
		public static readonly BaseTransformation IR_Simplification_CompareBranch64OnlyOneExit = new IR.Simplification.CompareBranch64OnlyOneExit();

		public static readonly BaseTransformation IR_ConstantMove_CompareBranch32 = new IR.ConstantMove.CompareBranch32();
		public static readonly BaseTransformation IR_ConstantMove_CompareBranch64 = new IR.ConstantMove.CompareBranch64();

		public static readonly BaseTransformation IR_Rewrite_CompareBranch32 = new IR.Rewrite.CompareBranch32();
		public static readonly BaseTransformation IR_Rewrite_CompareBranch64 = new IR.Rewrite.CompareBranch64();

		public static readonly BaseTransformation IR_Rewrite_CompareBranch32From64 = new IR.Rewrite.CompareBranch32From64();
		public static readonly BaseTransformation IR_Rewrite_CompareBranch64From32 = new IR.Rewrite.CompareBranch64From32();

		public static readonly BaseTransformation IR_Special_MoveCompoundPropagate = new IR.Special.MoveCompoundPropagate();

		public static readonly BaseTransformation IR_Special_Move32PropagateConstant = new Transform.IR.Special.Move32PropagateConstant();
		public static readonly BaseTransformation IR_Special_Move64PropagateConstant = new Transform.IR.Special.Move64PropagateConstant();

		public static readonly BaseTransformation IR_Rewrite_Compare32x32Combine32x32 = new IR.Rewrite.Compare32x32Combine32x32();
		public static readonly BaseTransformation IR_Rewrite_Compare32x32Combine64x32 = new IR.Rewrite.Compare32x32Combine64x32();
		public static readonly BaseTransformation IR_Rewrite_Compare32x32Combine64x64 = new IR.Rewrite.Compare32x32Combine32x64();
		public static readonly BaseTransformation IR_Rewrite_Compare64x64Combine32x32 = new IR.Rewrite.Compare64x64Combine32x32();
		public static readonly BaseTransformation IR_Rewrite_Compare64x64Combine64x32 = new IR.Rewrite.Compare64x64Combine64x32();
		public static readonly BaseTransformation IR_Rewrite_Compare64x64Combine64x64 = new IR.Rewrite.Compare64x64Combine64x64();

		public static readonly BaseTransformation IR_Rewrite_CompareBranch32Combine32x32 = new IR.Rewrite.CompareBranch32Combine32x32();
		public static readonly BaseTransformation IR_Rewrite_CompareBranch32Combine32x64 = new IR.Rewrite.CompareBranch32Combine32x64();
		public static readonly BaseTransformation IR_Rewrite_CompareBranch32Combine64x32 = new IR.Rewrite.CompareBranch32Combine64x32();
		public static readonly BaseTransformation IR_Rewrite_CompareBranch32Combine64x64 = new IR.Rewrite.CompareBranch32Combine64x64();
		public static readonly BaseTransformation IR_Rewrite_CompareBranch64Combine32x32 = new IR.Rewrite.CompareBranch64Combine32x32();
		public static readonly BaseTransformation IR_Rewrite_CompareBranch64Combine32x64 = new IR.Rewrite.CompareBranch64Combine32x64();
		public static readonly BaseTransformation IR_Rewrite_CompareBranch64Combine64x32 = new IR.Rewrite.CompareBranch64Combine64x32();
		public static readonly BaseTransformation IR_Rewrite_CompareBranch64Combine64x64 = new IR.Rewrite.CompareBranch64Combine64x64();

		public static readonly BaseTransformation IR_Rewrite_Compare64x32SameHigh = new IR.Simplification.Compare64x32SameHigh();
		public static readonly BaseTransformation IR_Rewrite_Compare64x32SameLow = new IR.Simplification.Compare64x32SameLow();
	}
}
