// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transform.Auto
{
	/// <summary>
	/// Transformations
	/// </summary>
	public static class AutoTransforms
	{
		public static readonly List<BaseTransformation> List = new List<BaseTransformation> {
			 new IR.ConstantFolding.Add32(),
			 new IR.ConstantFolding.Add64(),
			 new IR.ConstantFolding.AddR4(),
			 new IR.ConstantFolding.AddR8(),
			 new IR.ConstantFolding.AddCarryIn32(),
			 new IR.ConstantFolding.AddCarryIn64(),
			 new IR.ConstantFolding.ShiftRight32(),
			 new IR.ConstantFolding.ShiftRight64(),
			 new IR.ConstantFolding.ShiftLeft32(),
			 new IR.ConstantFolding.ShiftLeft64(),
			 new IR.ConstantFolding.DivUnsigned32(),
			 new IR.ConstantFolding.DivUnsigned64(),
			 new IR.ConstantFolding.DivSigned32(),
			 new IR.ConstantFolding.DivSigned64(),
			 new IR.ConstantFolding.DivR4(),
			 new IR.ConstantFolding.DivR8(),
			 new IR.ConstantFolding.And32(),
			 new IR.ConstantFolding.And64(),
			 new IR.ConstantFolding.Or32(),
			 new IR.ConstantFolding.Or64(),
			 new IR.ConstantFolding.Xor32(),
			 new IR.ConstantFolding.Xor64(),
			 new IR.ConstantFolding.Not32(),
			 new IR.ConstantFolding.Not64(),
			 new IR.ConstantFolding.MulUnsigned32(),
			 new IR.ConstantFolding.MulUnsigned64(),
			 new IR.ConstantFolding.MulSigned32(),
			 new IR.ConstantFolding.MulSigned64(),
			 new IR.ConstantFolding.MulR4(),
			 new IR.ConstantFolding.MulR8(),
			 new IR.ConstantFolding.RemUnsigned32(),
			 new IR.ConstantFolding.RemUnsigned64(),
			 new IR.ConstantFolding.RemSigned32(),
			 new IR.ConstantFolding.RemSigned64(),
			 new IR.ConstantFolding.RemR4(),
			 new IR.ConstantFolding.RemR8(),
			 new IR.ConstantFolding.Sub32(),
			 new IR.ConstantFolding.Sub64(),
			 new IR.ConstantFolding.SubR4(),
			 new IR.ConstantFolding.SubR8(),
			 new IR.ConstantFolding.SubCarryIn32(),
			 new IR.ConstantFolding.SubCarryIn64(),
			 new IR.ConstantFolding.IfThenElse32AlwaysTrue(),
			 new IR.ConstantFolding.IfThenElse64AlwaysTrue(),
			 new IR.ConstantFolding.IfThenElse32AlwaysFalse(),
			 new IR.ConstantFolding.IfThenElse64AlwaysFalse(),
			 new IR.ConstantFolding.SignExtend16x32(),
			 new IR.ConstantFolding.SignExtend16x64(),
			 new IR.ConstantFolding.SignExtend32x64(),
			 new IR.ConstantFolding.SignExtend8x32(),
			 new IR.ConstantFolding.SignExtend8x64(),
			 new IR.ConstantFolding.ZeroExtend16x32(),
			 new IR.ConstantFolding.ZeroExtend16x64(),
			 new IR.ConstantFolding.ZeroExtend32x64(),
			 new IR.ConstantFolding.ZeroExtend8x32(),
			 new IR.ConstantFolding.ZeroExtend8x64(),
			 new IR.ConstantFolding.Convert32ToR4(),
			 new IR.ConstantFolding.Convert32ToR8(),
			 new IR.ConstantFolding.Convert64ToR4(),
			 new IR.ConstantFolding.Convert64ToR8(),
			 new IR.ConstantFolding.GetHigh32(),
			 new IR.ConstantFolding.GetLow32(),
			 new IR.ConstantFolding.GetLow32FromTo64(),
			 new IR.ConstantFolding.GetHigh32FromTo64(),
			 new IR.ConstantMove.Add32(),
			 new IR.ConstantMove.Add64(),
			 new IR.ConstantMove.AddR4(),
			 new IR.ConstantMove.AddR8(),
			 new IR.ConstantMove.MulSigned32(),
			 new IR.ConstantMove.MulSigned64(),
			 new IR.ConstantMove.MulUnsigned32(),
			 new IR.ConstantMove.MulUnsigned64(),
			 new IR.ConstantMove.MulR4(),
			 new IR.ConstantMove.MulR8(),
			 new IR.ConstantMove.And32(),
			 new IR.ConstantMove.And64(),
			 new IR.ConstantMove.Or32(),
			 new IR.ConstantMove.Or64(),
			 new IR.ConstantMove.Xor32(),
			 new IR.ConstantMove.Xor64(),
			 new IR.Simplification.Move32Propagation(),
			 new IR.Simplification.Move64Propagation(),
			 new IR.Simplification.MoveObjectPropagation(),
			 new IR.Simplification.Not32Twice(),
			 new IR.Simplification.Not64Twice(),
			 new IR.Simplification.GetLow32FromTo64(),
			 new IR.Simplification.GetHigh32FromTo64(),
			 new IR.Simplification.GetHigh32To64(),
			 new IR.Simplification.GetLow32To64(),
			 new IR.Simplification.GetLow32FromShiftedRight32(),
			 new IR.Simplification.GetLow32FromRightShiftAndTo64(),
			 new IR.Simplification.GetHigh32FromRightLeftAndTo64(),
			 new IR.Simplification.GetHigh32FromShiftedMore32(),
			 new IR.Simplification.GetLow32FromShiftedMore32(),
			 new IR.Simplification.Truncate64x32Add64FromZeroExtended32x64(),
			 new IR.Simplification.Truncate64x32Sub64FromZeroExtended32x64(),
			 new IR.Simplification.Truncate64x32MulUnsigned64FromZeroExtended32x64(),
			 new IR.Simplification.Truncate64x32And64FromZeroExtended32x64(),
			 new IR.Simplification.Truncate64x32Or64FromZeroExtended32x64(),
			 new IR.Simplification.Truncate64x32Xor64FromZeroExtended32x64(),
			 new IR.Simplification.Add32MultipleWithCommon(),
			 new IR.Simplification.Add32MultipleWithCommon_v1(),
			 new IR.Simplification.Add32MultipleWithCommon_v2(),
			 new IR.Simplification.Add32MultipleWithCommon_v3(),
			 new IR.Simplification.Add32MultipleWithCommon_v4(),
			 new IR.Simplification.Add32MultipleWithCommon_v5(),
			 new IR.Simplification.Add32MultipleWithCommon_v6(),
			 new IR.Simplification.Add32MultipleWithCommon_v7(),
			 new IR.Simplification.Add64MultipleWithCommon(),
			 new IR.Simplification.Add64MultipleWithCommon_v1(),
			 new IR.Simplification.Add64MultipleWithCommon_v2(),
			 new IR.Simplification.Add64MultipleWithCommon_v3(),
			 new IR.Simplification.Add64MultipleWithCommon_v4(),
			 new IR.Simplification.Add64MultipleWithCommon_v5(),
			 new IR.Simplification.Add64MultipleWithCommon_v6(),
			 new IR.Simplification.Add64MultipleWithCommon_v7(),
			 new IR.Simplification.Sub32MultipleWithCommon(),
			 new IR.Simplification.Sub32MultipleWithCommon_v1(),
			 new IR.Simplification.Sub32MultipleWithCommon_v2(),
			 new IR.Simplification.Sub32MultipleWithCommon_v3(),
			 new IR.Simplification.Sub64MultipleWithCommon(),
			 new IR.Simplification.Sub64MultipleWithCommon_v1(),
			 new IR.Simplification.Sub64MultipleWithCommon_v2(),
			 new IR.Simplification.Sub64MultipleWithCommon_v3(),
			 new IR.Simplification.And32Not32Not32(),
			 new IR.Simplification.And64Not64Not64(),
			 new IR.Simplification.Or32Not32Not32(),
			 new IR.Simplification.Or64Not64Not64(),
			 new IR.Simplification.MulSigned32ByNegative1(),
			 new IR.Simplification.MulSigned32ByNegative1_v1(),
			 new IR.Simplification.MulSigned64ByNegative1(),
			 new IR.Simplification.MulSigned64ByNegative1_v1(),
			 new IR.Simplification.And32Same(),
			 new IR.Simplification.And64Same(),
			 new IR.Simplification.Or32Same(),
			 new IR.Simplification.Or64Same(),
			 new IR.Simplification.And32Double(),
			 new IR.Simplification.And32Double_v1(),
			 new IR.Simplification.And32Double_v2(),
			 new IR.Simplification.And32Double_v3(),
			 new IR.Simplification.And64Double(),
			 new IR.Simplification.And64Double_v1(),
			 new IR.Simplification.And64Double_v2(),
			 new IR.Simplification.And64Double_v3(),
			 new IR.Simplification.Or32Double(),
			 new IR.Simplification.Or32Double_v1(),
			 new IR.Simplification.Or32Double_v2(),
			 new IR.Simplification.Or32Double_v3(),
			 new IR.Simplification.Or64Double(),
			 new IR.Simplification.Or64Double_v1(),
			 new IR.Simplification.Or64Double_v2(),
			 new IR.Simplification.Or64Double_v3(),
			 new IR.Simplification.Xor32Double(),
			 new IR.Simplification.Xor32Double_v1(),
			 new IR.Simplification.Xor32Double_v2(),
			 new IR.Simplification.Xor32Double_v3(),
			 new IR.Simplification.Xor64Double(),
			 new IR.Simplification.Xor64Double_v1(),
			 new IR.Simplification.Xor64Double_v2(),
			 new IR.Simplification.Xor64Double_v3(),
			 new IR.StrengthReduction.Add32Zero(),
			 new IR.StrengthReduction.Add32Zero_v1(),
			 new IR.StrengthReduction.Add64Zero(),
			 new IR.StrengthReduction.Add64Zero_v1(),
			 new IR.StrengthReduction.And32Zero(),
			 new IR.StrengthReduction.And32Zero_v1(),
			 new IR.StrengthReduction.And64Zero(),
			 new IR.StrengthReduction.And64Zero_v1(),
			 new IR.StrengthReduction.And32Max(),
			 new IR.StrengthReduction.And32Max_v1(),
			 new IR.StrengthReduction.And64Max(),
			 new IR.StrengthReduction.And64Max_v1(),
			 new IR.StrengthReduction.Or32Max(),
			 new IR.StrengthReduction.Or32Max_v1(),
			 new IR.StrengthReduction.Or64Max(),
			 new IR.StrengthReduction.Or64Max_v1(),
			 new IR.StrengthReduction.Or32Zero(),
			 new IR.StrengthReduction.Or32Zero_v1(),
			 new IR.StrengthReduction.Or64Zero(),
			 new IR.StrengthReduction.Or64Zero_v1(),
			 new IR.StrengthReduction.Xor32Zero(),
			 new IR.StrengthReduction.Xor32Zero_v1(),
			 new IR.StrengthReduction.Xor64Zero(),
			 new IR.StrengthReduction.Xor64Zero_v1(),
			 new IR.StrengthReduction.Xor32Max(),
			 new IR.StrengthReduction.Xor32Max_v1(),
			 new IR.StrengthReduction.Xor64Max(),
			 new IR.StrengthReduction.Xor64Max_v1(),
			 new IR.StrengthReduction.ShiftRight32ZeroValue(),
			 new IR.StrengthReduction.ShiftRight64ZeroValue(),
			 new IR.StrengthReduction.ShiftRight32ByZero(),
			 new IR.StrengthReduction.ShiftRight64ByZero(),
			 new IR.StrengthReduction.ShiftLeft32ByZero(),
			 new IR.StrengthReduction.ShiftLeft64ByZero(),
			 new IR.StrengthReduction.Sub32ByZero(),
			 new IR.StrengthReduction.Sub64ByZero(),
			 new IR.StrengthReduction.Sub32Same(),
			 new IR.StrengthReduction.Sub64Same(),
			 new IR.StrengthReduction.Xor32Same(),
			 new IR.StrengthReduction.Xor64Same(),
			 new IR.StrengthReduction.MulSigned32ByZero(),
			 new IR.StrengthReduction.MulSigned32ByZero_v1(),
			 new IR.StrengthReduction.MulSigned64ByZero(),
			 new IR.StrengthReduction.MulSigned64ByZero_v1(),
			 new IR.StrengthReduction.MulUnsigned32ByZero(),
			 new IR.StrengthReduction.MulUnsigned32ByZero_v1(),
			 new IR.StrengthReduction.MulUnsigned64ByZero(),
			 new IR.StrengthReduction.MulUnsigned64ByZero_v1(),
			 new IR.StrengthReduction.MulSigned32ByOne(),
			 new IR.StrengthReduction.MulSigned32ByOne_v1(),
			 new IR.StrengthReduction.MulSigned64ByOne(),
			 new IR.StrengthReduction.MulSigned64ByOne_v1(),
			 new IR.StrengthReduction.MulUnsigned32ByOne(),
			 new IR.StrengthReduction.MulUnsigned32ByOne_v1(),
			 new IR.StrengthReduction.MulUnsigned64ByOne(),
			 new IR.StrengthReduction.MulUnsigned64ByOne_v1(),
			 new IR.StrengthReduction.MulUnsigned32ByPowerOfTwo(),
			 new IR.StrengthReduction.MulUnsigned32ByPowerOfTwo_v1(),
			 new IR.StrengthReduction.MulUnsigned64ByPowerOfTwo(),
			 new IR.StrengthReduction.MulUnsigned64ByPowerOfTwo_v1(),
			 new IR.StrengthReduction.MulSigned32ByPowerOfTwo(),
			 new IR.StrengthReduction.MulSigned32ByPowerOfTwo_v1(),
			 new IR.StrengthReduction.MulSigned64ByPowerOfTwo(),
			 new IR.StrengthReduction.MulSigned64ByPowerOfTwo_v1(),
			 new IR.StrengthReduction.DivSigned32ByZero(),
			 new IR.StrengthReduction.DivSigned64ByZero(),
			 new IR.StrengthReduction.DivSigned32ByOne(),
			 new IR.StrengthReduction.DivSigned64ByOne(),
			 new IR.StrengthReduction.DivUnsigned32ByOne(),
			 new IR.StrengthReduction.DivUnsigned64ByOne(),
			 new IR.StrengthReduction.DivUnsigned32ByPowerOfTwo(),
			 new IR.StrengthReduction.DivUnsigned64ByPowerOfTwo(),
			 new IR.StrengthReduction.DivSigned32ByPowerOfTwo(),
			 new IR.StrengthReduction.DivSigned64ByPowerOfTwo(),
			 new IR.StrengthReduction.RemUnsigned32ByPowerOfTwo(),
			 new IR.StrengthReduction.RemUnsigned64ByPowerOfTwo(),
			 new IR.StrengthReduction.Or32And32ClearAndSet(),
			 new IR.StrengthReduction.Or32And32ClearAndSet_v1(),
			 new IR.StrengthReduction.Or32And32ClearAndSet_v2(),
			 new IR.StrengthReduction.Or32And32ClearAndSet_v3(),
			 new IR.StrengthReduction.Or64And64ClearAndSet(),
			 new IR.StrengthReduction.Or64And64ClearAndSet_v1(),
			 new IR.StrengthReduction.Or64And64ClearAndSet_v2(),
			 new IR.StrengthReduction.Or64And64ClearAndSet_v3(),
			 new IR.StrengthReduction.Compare32x32RemUnsigned32Sign(),
			 new IR.StrengthReduction.Compare32x32RemUnsigned32Sign_v1(),
			 new IR.StrengthReduction.Compare32x64RemUnsigned64Sign(),
			 new IR.StrengthReduction.Compare32x64RemUnsigned64Sign_v1(),
			 new IR.StrengthReduction.Compare64x32RemUnsigned32Sign(),
			 new IR.StrengthReduction.Compare64x32RemUnsigned32Sign_v1(),
			 new IR.StrengthReduction.Compare64x64RemUnsigned64Sign(),
			 new IR.StrengthReduction.Compare64x64RemUnsigned64Sign_v1(),
			 new IR.StrengthReduction.Compare32x32Add32UnsignedRange(),
			 new IR.StrengthReduction.Compare32x32Add32UnsignedRange_v1(),
			 new IR.StrengthReduction.Compare32x64Add64UnsignedRange(),
			 new IR.StrengthReduction.Compare32x64Add64UnsignedRange_v1(),
			 new IR.StrengthReduction.Compare64x32Add32UnsignedRange(),
			 new IR.StrengthReduction.Compare64x32Add32UnsignedRange_v1(),
			 new IR.StrengthReduction.Compare64x64Add64UnsignedRange(),
			 new IR.StrengthReduction.Compare64x64Add64UnsignedRange_v1(),
			 new IR.StrengthReduction.UselessAnd32ShiftRight32(),
			 new IR.StrengthReduction.UselessAnd32ShiftRight32_v1(),
			 new IR.StrengthReduction.UselessAnd64ShiftRight64(),
			 new IR.StrengthReduction.UselessAnd64ShiftRight64_v1(),
			 new IR.StrengthReduction.UselessOr32ShiftRight32(),
			 new IR.StrengthReduction.UselessOr32ShiftRight32_v1(),
			 new IR.StrengthReduction.UselessOr64ShiftRight64(),
			 new IR.StrengthReduction.UselessOr64ShiftRight64_v1(),
			 new IR.StrengthReduction.UselessXor32ShiftRight32(),
			 new IR.StrengthReduction.UselessXor32ShiftRight32_v1(),
			 new IR.StrengthReduction.UselessXor64ShiftRight64(),
			 new IR.StrengthReduction.UselessXor64ShiftRight64_v1(),
			 new IR.StrengthReduction.UselessAnd32ShiftLeft32(),
			 new IR.StrengthReduction.UselessAnd32ShiftLeft32_v1(),
			 new IR.StrengthReduction.UselessAnd64ShiftLeft64(),
			 new IR.StrengthReduction.UselessAnd64ShiftLeft64_v1(),
			 new IR.StrengthReduction.UselessOr32ShiftLeft32(),
			 new IR.StrengthReduction.UselessOr32ShiftLeft32_v1(),
			 new IR.StrengthReduction.UselessOr64ShiftLeft64(),
			 new IR.StrengthReduction.UselessOr64ShiftLeft64_v1(),
			 new IR.StrengthReduction.UselessXor32ShiftLeft32(),
			 new IR.StrengthReduction.UselessXor32ShiftLeft32_v1(),
			 new IR.StrengthReduction.UselessXor64ShiftLeft64(),
			 new IR.StrengthReduction.UselessXor64ShiftLeft64_v1(),
			 new IR.Reorder.MulUnsigned32WithShiftLeft32(),
			 new IR.Reorder.MulUnsigned32WithShiftLeft32_v1(),
			 new IR.Reorder.MulUnsigned64WithShiftLeft64(),
			 new IR.Reorder.MulUnsigned64WithShiftLeft64_v1(),
			 new IR.Reorder.MulSigned32WithShiftLeft32(),
			 new IR.Reorder.MulSigned32WithShiftLeft32_v1(),
			 new IR.Reorder.MulSigned64WithShiftLeft64(),
			 new IR.Reorder.MulSigned64WithShiftLeft64_v1(),
			 new IR.Reorder.SubToAdd32(),
			 new IR.Reorder.SubToAdd64(),
			 new IR.ConstantMove.Add32Expression(),
			 new IR.ConstantMove.Add32Expression_v1(),
			 new IR.ConstantMove.Add32Expression_v2(),
			 new IR.ConstantMove.Add32Expression_v3(),
			 new IR.ConstantMove.Add64Expression(),
			 new IR.ConstantMove.Add64Expression_v1(),
			 new IR.ConstantMove.Add64Expression_v2(),
			 new IR.ConstantMove.Add64Expression_v3(),
			 new IR.ConstantMove.AddR4Expression(),
			 new IR.ConstantMove.AddR4Expression_v1(),
			 new IR.ConstantMove.AddR4Expression_v2(),
			 new IR.ConstantMove.AddR4Expression_v3(),
			 new IR.ConstantMove.AddR8Expression(),
			 new IR.ConstantMove.AddR8Expression_v1(),
			 new IR.ConstantMove.AddR8Expression_v2(),
			 new IR.ConstantMove.AddR8Expression_v3(),
			 new IR.ConstantMove.And32Expression(),
			 new IR.ConstantMove.And32Expression_v1(),
			 new IR.ConstantMove.And32Expression_v2(),
			 new IR.ConstantMove.And32Expression_v3(),
			 new IR.ConstantMove.And64Expression(),
			 new IR.ConstantMove.And64Expression_v1(),
			 new IR.ConstantMove.And64Expression_v2(),
			 new IR.ConstantMove.And64Expression_v3(),
			 new IR.ConstantMove.Or32Expression(),
			 new IR.ConstantMove.Or32Expression_v1(),
			 new IR.ConstantMove.Or32Expression_v2(),
			 new IR.ConstantMove.Or32Expression_v3(),
			 new IR.ConstantMove.Or64Expression(),
			 new IR.ConstantMove.Or64Expression_v1(),
			 new IR.ConstantMove.Or64Expression_v2(),
			 new IR.ConstantMove.Or64Expression_v3(),
			 new IR.ConstantMove.Xor32Expression(),
			 new IR.ConstantMove.Xor32Expression_v1(),
			 new IR.ConstantMove.Xor32Expression_v2(),
			 new IR.ConstantMove.Xor32Expression_v3(),
			 new IR.ConstantMove.Xor64Expression(),
			 new IR.ConstantMove.Xor64Expression_v1(),
			 new IR.ConstantMove.Xor64Expression_v2(),
			 new IR.ConstantMove.Xor64Expression_v3(),
			 new IR.ConstantMove.MulUnsigned32Expression(),
			 new IR.ConstantMove.MulUnsigned32Expression_v1(),
			 new IR.ConstantMove.MulUnsigned32Expression_v2(),
			 new IR.ConstantMove.MulUnsigned32Expression_v3(),
			 new IR.ConstantMove.MulUnsigned64Expression(),
			 new IR.ConstantMove.MulUnsigned64Expression_v1(),
			 new IR.ConstantMove.MulUnsigned64Expression_v2(),
			 new IR.ConstantMove.MulUnsigned64Expression_v3(),
			 new IR.ConstantMove.MulSigned32Expression(),
			 new IR.ConstantMove.MulSigned32Expression_v1(),
			 new IR.ConstantMove.MulSigned32Expression_v2(),
			 new IR.ConstantMove.MulSigned32Expression_v3(),
			 new IR.ConstantMove.MulSigned64Expression(),
			 new IR.ConstantMove.MulSigned64Expression_v1(),
			 new IR.ConstantMove.MulSigned64Expression_v2(),
			 new IR.ConstantMove.MulSigned64Expression_v3(),
			 new IR.ConstantMove.MulR4Expression(),
			 new IR.ConstantMove.MulR4Expression_v1(),
			 new IR.ConstantMove.MulR4Expression_v2(),
			 new IR.ConstantMove.MulR4Expression_v3(),
			 new IR.ConstantMove.MulR8Expression(),
			 new IR.ConstantMove.MulR8Expression_v1(),
			 new IR.ConstantMove.MulR8Expression_v2(),
			 new IR.ConstantMove.MulR8Expression_v3(),
			 new IR.ConstantFolding.MulSignedShiftLeft32(),
			 new IR.ConstantFolding.MulSignedShiftLeft32_v1(),
			 new IR.ConstantFolding.MulSignedShiftLeft64(),
			 new IR.ConstantFolding.MulSignedShiftLeft64_v1(),
			 new IR.ConstantFolding.MulUnsignedShiftLeft32(),
			 new IR.ConstantFolding.MulUnsignedShiftLeft32_v1(),
			 new IR.ConstantFolding.MulUnsignedShiftLeft64(),
			 new IR.ConstantFolding.MulUnsignedShiftLeft64_v1(),
			 new IR.ConstantFolding.AddCarryIn32Inside(),
			 new IR.ConstantFolding.AddCarryIn64Inside(),
			 new IR.ConstantFolding.AddCarryIn32Outside1(),
			 new IR.ConstantFolding.AddCarryIn64Outside1(),
			 new IR.ConstantFolding.AddCarryIn32Outside2(),
			 new IR.ConstantFolding.AddCarryIn64Outside2(),
			 new IR.ConstantFolding.AddCarryIn32NoCarry(),
			 new IR.ConstantFolding.AddCarryIn64NoCarry(),
			 new IR.ConstantFolding.SubCarryIn32Inside(),
			 new IR.ConstantFolding.SubCarryIn64Inside(),
			 new IR.ConstantFolding.SubCarryIn32Outside1(),
			 new IR.ConstantFolding.SubCarryIn64Outside1(),
			 new IR.ConstantFolding.SubCarryIn32Outside2(),
			 new IR.ConstantFolding.SubCarryIn64Outside2(),
			 new IR.ConstantFolding.SubCarryIn32NoCarry(),
			 new IR.ConstantFolding.SubCarryIn64NoCarry(),
			 new IR.ConstantFolding.Add32x2(),
			 new IR.ConstantFolding.Add32x2_v1(),
			 new IR.ConstantFolding.Add32x2_v2(),
			 new IR.ConstantFolding.Add32x2_v3(),
			 new IR.ConstantFolding.Add64x2(),
			 new IR.ConstantFolding.Add64x2_v1(),
			 new IR.ConstantFolding.Add64x2_v2(),
			 new IR.ConstantFolding.Add64x2_v3(),
			 new IR.ConstantFolding.AddR4x2(),
			 new IR.ConstantFolding.AddR4x2_v1(),
			 new IR.ConstantFolding.AddR4x2_v2(),
			 new IR.ConstantFolding.AddR4x2_v3(),
			 new IR.ConstantFolding.AddR8x2(),
			 new IR.ConstantFolding.AddR8x2_v1(),
			 new IR.ConstantFolding.AddR8x2_v2(),
			 new IR.ConstantFolding.AddR8x2_v3(),
			 new IR.ConstantFolding.Sub32x2(),
			 new IR.ConstantFolding.Sub64x2(),
			 new IR.ConstantFolding.SubR4x2(),
			 new IR.ConstantFolding.SubR8x2(),
			 new IR.ConstantFolding.MulSigned32x2(),
			 new IR.ConstantFolding.MulSigned32x2_v1(),
			 new IR.ConstantFolding.MulSigned32x2_v2(),
			 new IR.ConstantFolding.MulSigned32x2_v3(),
			 new IR.ConstantFolding.MulSigned64x2(),
			 new IR.ConstantFolding.MulSigned64x2_v1(),
			 new IR.ConstantFolding.MulSigned64x2_v2(),
			 new IR.ConstantFolding.MulSigned64x2_v3(),
			 new IR.ConstantFolding.MulR4x2(),
			 new IR.ConstantFolding.MulR4x2_v1(),
			 new IR.ConstantFolding.MulR4x2_v2(),
			 new IR.ConstantFolding.MulR4x2_v3(),
			 new IR.ConstantFolding.MulR8x2(),
			 new IR.ConstantFolding.MulR8x2_v1(),
			 new IR.ConstantFolding.MulR8x2_v2(),
			 new IR.ConstantFolding.MulR8x2_v3(),
			 new IR.ConstantFolding.MulUnsigned32x2(),
			 new IR.ConstantFolding.MulUnsigned32x2_v1(),
			 new IR.ConstantFolding.MulUnsigned32x2_v2(),
			 new IR.ConstantFolding.MulUnsigned32x2_v3(),
			 new IR.ConstantFolding.MulUnsigned64x2(),
			 new IR.ConstantFolding.MulUnsigned64x2_v1(),
			 new IR.ConstantFolding.MulUnsigned64x2_v2(),
			 new IR.ConstantFolding.MulUnsigned64x2_v3(),
			 new IR.ConstantFolding.Or32x2(),
			 new IR.ConstantFolding.Or32x2_v1(),
			 new IR.ConstantFolding.Or32x2_v2(),
			 new IR.ConstantFolding.Or32x2_v3(),
			 new IR.ConstantFolding.Or64x2(),
			 new IR.ConstantFolding.Or64x2_v1(),
			 new IR.ConstantFolding.Or64x2_v2(),
			 new IR.ConstantFolding.Or64x2_v3(),
			 new IR.ConstantFolding.And32x2(),
			 new IR.ConstantFolding.And32x2_v1(),
			 new IR.ConstantFolding.And32x2_v2(),
			 new IR.ConstantFolding.And32x2_v3(),
			 new IR.ConstantFolding.And64x2(),
			 new IR.ConstantFolding.And64x2_v1(),
			 new IR.ConstantFolding.And64x2_v2(),
			 new IR.ConstantFolding.And64x2_v3(),
			 new IR.ConstantFolding.Xor32x2(),
			 new IR.ConstantFolding.Xor32x2_v1(),
			 new IR.ConstantFolding.Xor32x2_v2(),
			 new IR.ConstantFolding.Xor32x2_v3(),
			 new IR.ConstantFolding.Xor64x2(),
			 new IR.ConstantFolding.Xor64x2_v1(),
			 new IR.ConstantFolding.Xor64x2_v2(),
			 new IR.ConstantFolding.Xor64x2_v3(),
			 new IR.ConstantFolding.AddSub32(),
			 new IR.ConstantFolding.AddSub32_v1(),
			 new IR.ConstantFolding.AddSub64(),
			 new IR.ConstantFolding.AddSub64_v1(),
			 new IR.ConstantFolding.AddSubR4(),
			 new IR.ConstantFolding.AddSubR4_v1(),
			 new IR.ConstantFolding.AddSubR8(),
			 new IR.ConstantFolding.AddSubR8_v1(),
			 new IR.ConstantFolding.SubAdd32(),
			 new IR.ConstantFolding.SubAdd32_v1(),
			 new IR.ConstantFolding.SubAdd64(),
			 new IR.ConstantFolding.SubAdd64_v1(),
			 new IR.ConstantFolding.SubAddR4(),
			 new IR.ConstantFolding.SubAddR4_v1(),
			 new IR.ConstantFolding.SubAddR8(),
			 new IR.ConstantFolding.SubAddR8_v1(),
			 new IR.ConstantFolding.ShiftLeft32x2(),
			 new IR.ConstantFolding.ShiftLeft64x2(),
			 new IR.ConstantFolding.ShiftRight32x2(),
			 new IR.ConstantFolding.ShiftRight64x2(),
			 new IR.ConstantFolding.Load32FoldAdd32(),
			 new IR.ConstantFolding.Load64FoldAdd32(),
			 new IR.ConstantFolding.LoadR4FoldAdd32(),
			 new IR.ConstantFolding.LoadR8FoldAdd32(),
			 new IR.ConstantFolding.LoadSignExtend8x32FoldAdd32(),
			 new IR.ConstantFolding.LoadSignExtend16x32FoldAdd32(),
			 new IR.ConstantFolding.LoadSignExtend8x64FoldAdd32(),
			 new IR.ConstantFolding.LoadSignExtend16x64FoldAdd32(),
			 new IR.ConstantFolding.LoadSignExtend32x64FoldAdd32(),
			 new IR.ConstantFolding.LoadZeroExtend8x32FoldAdd32(),
			 new IR.ConstantFolding.LoadZeroExtend16x32FoldAdd32(),
			 new IR.ConstantFolding.LoadZeroExtend8x64FoldAdd32(),
			 new IR.ConstantFolding.LoadZeroExtend16x64FoldAdd32(),
			 new IR.ConstantFolding.LoadZeroExtend32x64FoldAdd32(),
			 new IR.ConstantFolding.Load32FoldAdd64(),
			 new IR.ConstantFolding.Load64FoldAdd64(),
			 new IR.ConstantFolding.LoadR4FoldAdd64(),
			 new IR.ConstantFolding.LoadR8FoldAdd64(),
			 new IR.ConstantFolding.LoadSignExtend8x32FoldAdd64(),
			 new IR.ConstantFolding.LoadSignExtend16x32FoldAdd64(),
			 new IR.ConstantFolding.LoadSignExtend8x64FoldAdd64(),
			 new IR.ConstantFolding.LoadSignExtend16x64FoldAdd64(),
			 new IR.ConstantFolding.LoadSignExtend32x64FoldAdd64(),
			 new IR.ConstantFolding.LoadZeroExtend8x32FoldAdd64(),
			 new IR.ConstantFolding.LoadZeroExtend16x32FoldAdd64(),
			 new IR.ConstantFolding.LoadZeroExtend8x64FoldAdd64(),
			 new IR.ConstantFolding.LoadZeroExtend16x64FoldAdd64(),
			 new IR.ConstantFolding.LoadZeroExtend32x64FoldAdd64(),
			 new IR.ConstantFolding.Load32FoldSub32(),
			 new IR.ConstantFolding.Load64FoldSub32(),
			 new IR.ConstantFolding.LoadR4FoldSub32(),
			 new IR.ConstantFolding.LoadR8FoldSub32(),
			 new IR.ConstantFolding.LoadSignExtend8x32FoldSub32(),
			 new IR.ConstantFolding.LoadSignExtend16x32FoldSub32(),
			 new IR.ConstantFolding.LoadSignExtend8x64FoldSub32(),
			 new IR.ConstantFolding.LoadSignExtend16x64FoldSub32(),
			 new IR.ConstantFolding.LoadSignExtend32x64FoldSub32(),
			 new IR.ConstantFolding.LoadZeroExtend8x32FoldSub32(),
			 new IR.ConstantFolding.LoadZeroExtend16x32FoldSub32(),
			 new IR.ConstantFolding.LoadZeroExtend8x64FoldSub32(),
			 new IR.ConstantFolding.LoadZeroExtend16x64FoldSub32(),
			 new IR.ConstantFolding.LoadZeroExtend32x64FoldSub32(),
			 new IR.ConstantFolding.Load32FoldSub64(),
			 new IR.ConstantFolding.Load64FoldSub64(),
			 new IR.ConstantFolding.LoadR4FoldSub64(),
			 new IR.ConstantFolding.LoadR8FoldSub64(),
			 new IR.ConstantFolding.LoadSignExtend8x32FoldSub64(),
			 new IR.ConstantFolding.LoadSignExtend16x32FoldSub64(),
			 new IR.ConstantFolding.LoadSignExtend8x64FoldSub64(),
			 new IR.ConstantFolding.LoadSignExtend16x64FoldSub64(),
			 new IR.ConstantFolding.LoadSignExtend32x64FoldSub64(),
			 new IR.ConstantFolding.LoadZeroExtend8x32FoldSub64(),
			 new IR.ConstantFolding.LoadZeroExtend16x32FoldSub64(),
			 new IR.ConstantFolding.LoadZeroExtend8x64FoldSub64(),
			 new IR.ConstantFolding.LoadZeroExtend16x64FoldSub64(),
			 new IR.ConstantFolding.LoadZeroExtend32x64FoldSub64(),
			 new IR.ConstantFolding.Store8FoldAdd32(),
			 new IR.ConstantFolding.Store8FoldAdd64(),
			 new IR.ConstantFolding.Store8FoldSub32(),
			 new IR.ConstantFolding.Store8FoldSub64(),
			 new IR.ConstantFolding.Store16FoldAdd32(),
			 new IR.ConstantFolding.Store16FoldAdd64(),
			 new IR.ConstantFolding.Store16FoldSub32(),
			 new IR.ConstantFolding.Store16FoldSub64(),
			 new IR.ConstantFolding.Store32FoldAdd32(),
			 new IR.ConstantFolding.Store32FoldAdd64(),
			 new IR.ConstantFolding.Store32FoldSub32(),
			 new IR.ConstantFolding.Store32FoldSub64(),
			 new IR.ConstantFolding.Store64FoldAdd32(),
			 new IR.ConstantFolding.Store64FoldAdd64(),
			 new IR.ConstantFolding.Store64FoldSub32(),
			 new IR.ConstantFolding.Store64FoldSub64(),
			 new IR.ConstantFolding.StoreR4FoldAdd32(),
			 new IR.ConstantFolding.StoreR4FoldAdd64(),
			 new IR.ConstantFolding.StoreR4FoldSub32(),
			 new IR.ConstantFolding.StoreR4FoldSub64(),
			 new IR.ConstantFolding.StoreR8FoldAdd32(),
			 new IR.ConstantFolding.StoreR8FoldAdd64(),
			 new IR.ConstantFolding.StoreR8FoldSub32(),
			 new IR.ConstantFolding.StoreR8FoldSub64(),
			 new IR.ConstantFolding.Load32AddressFold(),
			 new IR.ConstantFolding.Load64AddressFold(),
			 new IR.ConstantFolding.LoadR4AddressFold(),
			 new IR.ConstantFolding.LoadR8AddressFold(),
			 new IR.ConstantFolding.LoadSignExtend8x32AddressFold(),
			 new IR.ConstantFolding.LoadSignExtend16x32AddressFold(),
			 new IR.ConstantFolding.LoadSignExtend8x64AddressFold(),
			 new IR.ConstantFolding.LoadSignExtend16x64AddressFold(),
			 new IR.ConstantFolding.LoadSignExtend32x64AddressFold(),
			 new IR.ConstantFolding.LoadZeroExtend8x32AddressFold(),
			 new IR.ConstantFolding.LoadZeroExtend16x32AddressFold(),
			 new IR.ConstantFolding.LoadZeroExtend8x64AddressFold(),
			 new IR.ConstantFolding.LoadZeroExtend16x64AddressFold(),
			 new IR.ConstantFolding.LoadZeroExtend32x64AddressFold(),
			 new IR.Rewrite.CompareObjectGreaterThanZero(),
			 new IR.Rewrite.CompareObjectGreaterThanZero_v1(),
			 new IR.Rewrite.Compare32x32GreaterThanZero(),
			 new IR.Rewrite.Compare32x32GreaterThanZero_v1(),
			 new IR.Rewrite.Compare32x64GreaterThanZero(),
			 new IR.Rewrite.Compare32x64GreaterThanZero_v1(),
			 new IR.Rewrite.Compare64x32GreaterThanZero(),
			 new IR.Rewrite.Compare64x32GreaterThanZero_v1(),
			 new IR.Rewrite.Compare64x64GreaterThanZero(),
			 new IR.Rewrite.Compare64x64GreaterThanZero_v1(),
			 new IR.Algebraic.Signed32AAPlusBBPlus2AB(),
			 new IR.Algebraic.Signed32AAPlusBBPlus2AB_v1(),
			 new IR.Algebraic.Signed32AAPlusBBPlus2AB_v2(),
			 new IR.Algebraic.Signed32AAPlusBBPlus2AB_v3(),
			 new IR.Algebraic.Signed32AAPlusBBPlus2AB_v4(),
			 new IR.Algebraic.Signed32AAPlusBBPlus2AB_v5(),
			 new IR.Algebraic.Signed32AAPlusBBPlus2AB_v6(),
			 new IR.Algebraic.Signed32AAPlusBBPlus2AB_v7(),
			 new IR.Algebraic.Signed64AAPlusBBPlus2AB(),
			 new IR.Algebraic.Signed64AAPlusBBPlus2AB_v1(),
			 new IR.Algebraic.Signed64AAPlusBBPlus2AB_v2(),
			 new IR.Algebraic.Signed64AAPlusBBPlus2AB_v3(),
			 new IR.Algebraic.Signed64AAPlusBBPlus2AB_v4(),
			 new IR.Algebraic.Signed64AAPlusBBPlus2AB_v5(),
			 new IR.Algebraic.Signed64AAPlusBBPlus2AB_v6(),
			 new IR.Algebraic.Signed64AAPlusBBPlus2AB_v7(),
			 new IR.Algebraic.Unsigned32AAPlusBBPlus2AB(),
			 new IR.Algebraic.Unsigned32AAPlusBBPlus2AB_v1(),
			 new IR.Algebraic.Unsigned32AAPlusBBPlus2AB_v2(),
			 new IR.Algebraic.Unsigned32AAPlusBBPlus2AB_v3(),
			 new IR.Algebraic.Unsigned32AAPlusBBPlus2AB_v4(),
			 new IR.Algebraic.Unsigned32AAPlusBBPlus2AB_v5(),
			 new IR.Algebraic.Unsigned32AAPlusBBPlus2AB_v6(),
			 new IR.Algebraic.Unsigned32AAPlusBBPlus2AB_v7(),
			 new IR.Algebraic.Unsigned64AAPlusBBPlus2AB(),
			 new IR.Algebraic.Unsigned64AAPlusBBPlus2AB_v1(),
			 new IR.Algebraic.Unsigned64AAPlusBBPlus2AB_v2(),
			 new IR.Algebraic.Unsigned64AAPlusBBPlus2AB_v3(),
			 new IR.Algebraic.Unsigned64AAPlusBBPlus2AB_v4(),
			 new IR.Algebraic.Unsigned64AAPlusBBPlus2AB_v5(),
			 new IR.Algebraic.Unsigned64AAPlusBBPlus2AB_v6(),
			 new IR.Algebraic.Unsigned64AAPlusBBPlus2AB_v7(),
			 new IR.Algebraic.Signed32AAPlusBBMinus2AB(),
			 new IR.Algebraic.Signed32AAPlusBBMinus2AB_v1(),
			 new IR.Algebraic.Signed32AAPlusBBMinus2AB_v2(),
			 new IR.Algebraic.Signed32AAPlusBBMinus2AB_v3(),
			 new IR.Algebraic.Signed64AAPlusBBMinus2AB(),
			 new IR.Algebraic.Signed64AAPlusBBMinus2AB_v1(),
			 new IR.Algebraic.Signed64AAPlusBBMinus2AB_v2(),
			 new IR.Algebraic.Signed64AAPlusBBMinus2AB_v3(),
			 new IR.Algebraic.Unsigned32AAPlusBBMinus2AB(),
			 new IR.Algebraic.Unsigned32AAPlusBBMinus2AB_v1(),
			 new IR.Algebraic.Unsigned32AAPlusBBMinus2AB_v2(),
			 new IR.Algebraic.Unsigned32AAPlusBBMinus2AB_v3(),
			 new IR.Algebraic.Unsigned64AAPlusBBMinus2AB(),
			 new IR.Algebraic.Unsigned64AAPlusBBMinus2AB_v1(),
			 new IR.Algebraic.Unsigned64AAPlusBBMinus2AB_v2(),
			 new IR.Algebraic.Unsigned64AAPlusBBMinus2AB_v3(),
			 new IR.Algebraic.Signed32AAMinusBB(),
			 new IR.Algebraic.Signed64AAMinusBB(),
			 new IR.Algebraic.Unsigned32AAMinusBB(),
			 new IR.Algebraic.Unsigned64AAMinusBB(),
			 new IR.Algebraic.Unsigned32PerfectSquareFormula(),
			 new IR.Algebraic.Unsigned32PerfectSquareFormula_v1(),
			 new IR.Algebraic.Unsigned32PerfectSquareFormula_v2(),
			 new IR.Algebraic.Unsigned32PerfectSquareFormula_v3(),
			 new IR.Algebraic.Unsigned32PerfectSquareFormula_v4(),
			 new IR.Algebraic.Unsigned32PerfectSquareFormula_v5(),
			 new IR.Algebraic.Unsigned32PerfectSquareFormula_v6(),
			 new IR.Algebraic.Unsigned32PerfectSquareFormula_v7(),
			 new IR.Algebraic.Unsigned64PerfectSquareFormula(),
			 new IR.Algebraic.Unsigned64PerfectSquareFormula_v1(),
			 new IR.Algebraic.Unsigned64PerfectSquareFormula_v2(),
			 new IR.Algebraic.Unsigned64PerfectSquareFormula_v3(),
			 new IR.Algebraic.Unsigned64PerfectSquareFormula_v4(),
			 new IR.Algebraic.Unsigned64PerfectSquareFormula_v5(),
			 new IR.Algebraic.Unsigned64PerfectSquareFormula_v6(),
			 new IR.Algebraic.Unsigned64PerfectSquareFormula_v7(),
			 new IR.Algebraic.Signed32PerfectSquareFormula(),
			 new IR.Algebraic.Signed32PerfectSquareFormula_v1(),
			 new IR.Algebraic.Signed32PerfectSquareFormula_v2(),
			 new IR.Algebraic.Signed32PerfectSquareFormula_v3(),
			 new IR.Algebraic.Signed32PerfectSquareFormula_v4(),
			 new IR.Algebraic.Signed32PerfectSquareFormula_v5(),
			 new IR.Algebraic.Signed32PerfectSquareFormula_v6(),
			 new IR.Algebraic.Signed32PerfectSquareFormula_v7(),
			 new IR.Algebraic.Signed64PerfectSquareFormula(),
			 new IR.Algebraic.Signed64PerfectSquareFormula_v1(),
			 new IR.Algebraic.Signed64PerfectSquareFormula_v2(),
			 new IR.Algebraic.Signed64PerfectSquareFormula_v3(),
			 new IR.Algebraic.Signed64PerfectSquareFormula_v4(),
			 new IR.Algebraic.Signed64PerfectSquareFormula_v5(),
			 new IR.Algebraic.Signed64PerfectSquareFormula_v6(),
			 new IR.Algebraic.Signed64PerfectSquareFormula_v7(),
		};
	}
}
