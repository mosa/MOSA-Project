// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transform.Manual
{
	/// <summary>
	/// Transformations
	/// </summary>
	public static class ManualTransforms
	{
		public static readonly List<BaseTransformation> List = new List<BaseTransformation>
		{
			ManualInstance.IR_ConstantMove_Compare32x32,
			ManualInstance.IR_ConstantMove_Compare32x64,
			ManualInstance.IR_ConstantMove_Compare64x32,
			ManualInstance.IR_ConstantMove_Compare64x64,

			ManualInstance.IR_ConstantMove_BranchCompare32,
			ManualInstance.IR_ConstantMove_BranchCompare64,

			ManualInstance.IR_Rewrite_Compare32x32,
			ManualInstance.IR_Rewrite_Compare32x64,
			ManualInstance.IR_Rewrite_Compare64x32,
			ManualInstance.IR_Rewrite_Compare64x64,

			ManualInstance.IR_ConstantFolding_Compare32x32,
			ManualInstance.IR_ConstantFolding_Compare32x64,
			ManualInstance.IR_ConstantFolding_Compare64x32,
			ManualInstance.IR_ConstantFolding_Compare64x64,

			ManualInstance.IR_ConstantFolding_BranchCompare32,
			ManualInstance.IR_ConstantFolding_BranchCompare64,

			ManualInstance.IR_Special_CodeInDeadBlock,
			ManualInstance.IR_Special_Deadcode,
			ManualInstance.IR_Special_GetLow64From32,

			ManualInstance.IR_Simplification_AddCarryOut32CarryNotUsed,
			ManualInstance.IR_Simplification_AddCarryOut64CarryNotUsed,
			ManualInstance.IR_Simplification_SubCarryOut32CarryNotUsed,
			ManualInstance.IR_Simplification_SubCarryOut64CarryNotUsed,

			ManualInstance.IR_Simplification_Compare32x32Same,
			ManualInstance.IR_Simplification_Compare32x64Same,
			ManualInstance.IR_Simplification_Compare64x32Same,
			ManualInstance.IR_Simplification_Compare64x64Same,

			ManualInstance.IR_Simplification_Compare32x32NotSame,
			ManualInstance.IR_Simplification_Compare32x64NotSame,
			ManualInstance.IR_Simplification_Compare64x32NotSame,
			ManualInstance.IR_Simplification_Compare64x64NotSame,

			ManualInstance.IR_Special_Move32Propagate,
			ManualInstance.IR_Special_Move64Propagate,
			ManualInstance.IR_Special_MoveR4Propagate,
			ManualInstance.IR_Special_MoveR8Propagate,

			ManualInstance.IR_Special_Phi32Propagate,
			ManualInstance.IR_Special_Phi64Propagate,
			ManualInstance.IR_Special_PhiR4Propagate,
			ManualInstance.IR_Special_PhiR8Propagate,

			ManualInstance.IR_Special_Phi32Dead,
			ManualInstance.IR_Special_Phi64Dead,
			ManualInstance.IR_Special_PhiR4Dead,
			ManualInstance.IR_Special_PhiR8Dead,

			ManualInstance.IR_Special_Phi32Update,
			ManualInstance.IR_Special_Phi64Update,
			ManualInstance.IR_Special_PhiR4Update,
			ManualInstance.IR_Special_PhiR8Update,

			ManualInstance.IR_Simplification_BranchCompare32OnlyOneExit,
			ManualInstance.IR_Simplification_BranchCompare64OnlyOneExit,

			ManualInstance.IR_Rewrite_BranchCompare32,
			ManualInstance.IR_Rewrite_BranchCompare64,

			ManualInstance.IR_Special_MoveCompoundPropagate,

			ManualInstance.IR_Rewrite_BranchCompare32From64,
			ManualInstance.IR_Rewrite_BranchCompare64From32,

			ManualInstance.IR_Special_Move32PropagateConstant,
			ManualInstance.IR_Special_Move64PropagateConstant,

			ManualInstance.IR_Rewrite_Compare32x32Combine32x32,
			ManualInstance.IR_Rewrite_Compare32x32Combine64x64,
			ManualInstance.IR_Rewrite_Compare32x32Combine64x32,
			ManualInstance.IR_Rewrite_Compare64x64Combine64x32,
			ManualInstance.IR_Rewrite_Compare64x64Combine64x64,
			ManualInstance.IR_Rewrite_Compare64x64Combine32x32,

			ManualInstance.IR_Rewrite_BranchCompare32Combine32x32,
			ManualInstance.IR_Rewrite_BranchCompare32Combine32x64,
			ManualInstance.IR_Rewrite_BranchCompare32Combine64x32,
			ManualInstance.IR_Rewrite_BranchCompare32Combine64x64,
			ManualInstance.IR_Rewrite_BranchCompare64Combine32x32,
			ManualInstance.IR_Rewrite_BranchCompare64Combine32x64,
			ManualInstance.IR_Rewrite_BranchCompare64Combine64x32,
			ManualInstance.IR_Rewrite_BranchCompare64Combine64x64,

			ManualInstance.IR_Rewrite_Compare64x32SameHigh,
			ManualInstance.IR_Rewrite_Compare64x32SameLow,

			// LowerTo32
			ManualInstance.IR_LowerTo32_Add64,
			ManualInstance.IR_LowerTo32_And64,
			ManualInstance.IR_LowerTo32_BranchCompare64,
			ManualInstance.IR_LowerTo32_Compare64x32EqualOrNotEqual,
			ManualInstance.IR_LowerTo32_Compare64x32Rest,
			ManualInstance.IR_LowerTo32_Compare64x32RestInSSA,
			ManualInstance.IR_LowerTo32_Compare64x64EqualOrNotEqual,
			ManualInstance.IR_LowerTo32_Compare64x64Rest,
			ManualInstance.IR_LowerTo32_Compare64x64RestInSSA,

			//ManualInstance.IR_LowerTo32_Compare64x32UnsignedGreater,
			ManualInstance.IR_LowerTo32_Load64,
			ManualInstance.IR_LowerTo32_LoadParam64,
			ManualInstance.IR_LowerTo32_LoadParamSignExtend16x64,
			ManualInstance.IR_LowerTo32_LoadParamSignExtend32x64,
			ManualInstance.IR_LowerTo32_LoadParamSignExtend8x64,
			ManualInstance.IR_LowerTo32_LoadParamZeroExtend16x64,
			ManualInstance.IR_LowerTo32_LoadParamZeroExtend32x64,
			ManualInstance.IR_LowerTo32_LoadParamZeroExtend8x64,
			ManualInstance.IR_LowerTo32_Not64,
			ManualInstance.IR_LowerTo32_Or64,
			ManualInstance.IR_LowerTo32_SignExtend16x64,
			ManualInstance.IR_LowerTo32_SignExtend32x64,
			ManualInstance.IR_LowerTo32_SignExtend8x64,
			ManualInstance.IR_LowerTo32_Store64,
			ManualInstance.IR_LowerTo32_StoreParam64,
			ManualInstance.IR_LowerTo32_Sub64,
			ManualInstance.IR_LowerTo32_Truncate64x32,
			ManualInstance.IR_LowerTo32_Xor64,
			ManualInstance.IR_LowerTo32_ZeroExtend16x64,
			ManualInstance.IR_LowerTo32_ZeroExtend32x64,

			ManualInstance.IR_LowerTo32_Move64
		};
	}
}
