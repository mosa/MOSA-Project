// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.Transform;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transform.Auto
{
	/// <summary>
	/// Transformations
	/// </summary>
	public static class AutoTransforms
	{
		public static readonly List<BaseTransformation> List = new List<BaseTransformation> {
			 new ConstantFolding.Add32(),
			 new ConstantFolding.Add64(),
			 new ConstantFolding.AddR4(),
			 new ConstantFolding.AddR8(),
			 new ConstantFolding.AddCarryIn32(),
			 new ConstantFolding.AddCarryIn64(),
			 new ConstantFolding.ShiftRight32(),
			 new ConstantFolding.ShiftRight64(),
			 new ConstantFolding.ShiftLeft32(),
			 new ConstantFolding.ShiftLeft64(),
			 new ConstantFolding.DivUnsigned32(),
			 new ConstantFolding.DivUnsigned64(),
			 new ConstantFolding.DivSigned32(),
			 new ConstantFolding.DivSigned64(),
			 new ConstantFolding.DivR4(),
			 new ConstantFolding.DivR8(),
			 new ConstantFolding.And32(),
			 new ConstantFolding.And64(),
			 new ConstantFolding.Or32(),
			 new ConstantFolding.Or64(),
			 new ConstantFolding.Xor32(),
			 new ConstantFolding.Xor64(),
			 new ConstantFolding.Not32(),
			 new ConstantFolding.Not64(),
			 new ConstantFolding.MulUnsigned32(),
			 new ConstantFolding.MulUnsigned64(),
			 new ConstantFolding.MulSigned32(),
			 new ConstantFolding.MulSigned64(),
			 new ConstantFolding.MulR4(),
			 new ConstantFolding.MulR8(),
			 new ConstantFolding.RemUnsigned32(),
			 new ConstantFolding.RemUnsigned64(),
			 new ConstantFolding.RemSigned32(),
			 new ConstantFolding.RemSigned64(),
			 new ConstantFolding.RemR4(),
			 new ConstantFolding.RemR8(),
			 new ConstantFolding.Sub32(),
			 new ConstantFolding.Sub64(),
			 new ConstantFolding.SubR4(),
			 new ConstantFolding.SubR8(),
			 new ConstantFolding.SubCarryIn32(),
			 new ConstantFolding.SubCarryIn64(),
			 new ConstantFolding.IfThenElse32AlwaysTrue(),
			 new ConstantFolding.IfThenElse64AlwaysTrue(),
			 new ConstantFolding.IfThenElse32AlwaysFalse(),
			 new ConstantFolding.IfThenElse64AlwaysFalse(),
			 new ConstantFolding.SignExtend16x32(),
			 new ConstantFolding.SignExtend16x64(),
			 new ConstantFolding.SignExtend32x64(),
			 new ConstantFolding.SignExtend8x32(),
			 new ConstantFolding.SignExtend8x64(),
			 new ConstantFolding.ZeroExtend16x32(),
			 new ConstantFolding.ZeroExtend16x64(),
			 new ConstantFolding.ZeroExtend32x64(),
			 new ConstantFolding.ZeroExtend8x32(),
			 new ConstantFolding.ZeroExtend8x64(),
			 new ConstantFolding.ConvertI32ToR4(),
			 new ConstantFolding.ConvertI32ToR8(),
			 new ConstantFolding.ConvertI64ToR4(),
			 new ConstantFolding.ConvertI64ToR8(),
			 new ConstantFolding.ConvertU32ToR4(),
			 new ConstantFolding.ConvertU32ToR8(),
			 new ConstantFolding.ConvertU64ToR4(),
			 new ConstantFolding.ConvertU64ToR8(),
			 new ConstantFolding.GetHigh32(),
			 new ConstantFolding.GetLow32(),
			 new ConstantFolding.GetLow32FromTo64(),
			 new ConstantFolding.GetHigh32FromTo64(),
			 new ConstantFolding.ArithShiftRight32(),
			 new ConstantFolding.ArithShiftRight64(),
			 new ConstantFolding.Compare32x32GreaterOrEqualThanZero(),
			 new ConstantFolding.Compare32x64GreaterOrEqualThanZero(),
			 new ConstantFolding.Compare64x32GreaterOrEqualThanZero(),
			 new ConstantFolding.Compare64x64GreaterOrEqualThanZero(),
			 new ConstantFolding.Compare32x32LessThanZero(),
			 new ConstantFolding.Compare32x64LessThanZero(),
			 new ConstantFolding.Compare64x32LessThanZero(),
			 new ConstantFolding.Compare64x64LessThanZero(),
			 new ConstantFolding.Compare32x32LessThanZero(),
			 new ConstantFolding.Compare32x64LessThanZero(),
			 new ConstantFolding.Compare64x32LessThanZero(),
			 new ConstantFolding.Compare64x64LessThanZero(),
			 new ConstantFolding.Compare32x32LessOrEqualThanZero(),
			 new ConstantFolding.Compare32x64LessOrEqualThanZero(),
			 new ConstantFolding.Compare64x32LessOrEqualThanZero(),
			 new ConstantFolding.Compare64x64LessOrEqualThanZero(),
			 new ConstantFolding.Compare32x32GreaterOrEqualThanMax(),
			 new ConstantFolding.Compare32x64GreaterOrEqualThanMax(),
			 new ConstantFolding.Compare64x32GreaterOrEqualThanMax(),
			 new ConstantFolding.Compare64x64GreaterOrEqualThanMax(),
			 new ConstantFolding.Compare32x32LessThanMax(),
			 new ConstantFolding.Compare32x64LessThanMax(),
			 new ConstantFolding.Compare64x32LessThanMax(),
			 new ConstantFolding.Compare64x64LessThanMax(),
			 new ConstantFolding.Compare32x32LessThanMax(),
			 new ConstantFolding.Compare32x64LessThanMax(),
			 new ConstantFolding.Compare64x32LessThanMax(),
			 new ConstantFolding.Compare64x64LessThanMax(),
			 new ConstantFolding.Compare32x32LessThanMax(),
			 new ConstantFolding.Compare32x64LessThanMax(),
			 new ConstantFolding.Compare64x32LessThanMax(),
			 new ConstantFolding.Compare64x64LessThanMax(),
			 new ConstantMove.Add32(),
			 new ConstantMove.Add64(),
			 new ConstantMove.AddR4(),
			 new ConstantMove.AddR8(),
			 new ConstantMove.MulSigned32(),
			 new ConstantMove.MulSigned64(),
			 new ConstantMove.MulUnsigned32(),
			 new ConstantMove.MulUnsigned64(),
			 new ConstantMove.MulR4(),
			 new ConstantMove.MulR8(),
			 new ConstantMove.And32(),
			 new ConstantMove.And64(),
			 new ConstantMove.Or32(),
			 new ConstantMove.Or64(),
			 new ConstantMove.Xor32(),
			 new ConstantMove.Xor64(),
			 new Simplification.Move32Propagation(),
			 new Simplification.Move64Propagation(),
			 new Simplification.MoveObjectPropagation(),
			 new Simplification.Not32Twice(),
			 new Simplification.Not64Twice(),
			 new Simplification.GetLow32FromTo64(),
			 new Simplification.GetHigh32FromTo64(),
			 new Simplification.GetHigh32To64(),
			 new Simplification.GetLow32To64(),
			 new Simplification.GetLow32FromShiftedRight32(),
			 new Simplification.GetLow32FromRightShiftAndTo64(),
			 new Simplification.GetHigh32FromRightLeftAndTo64(),
			 new Simplification.GetHigh32FromShiftedMore32(),
			 new Simplification.GetLow32FromShiftedMore32(),
			 new Simplification.Truncate64x32Add64FromZeroExtended32x64(),
			 new Simplification.Truncate64x32Sub64FromZeroExtended32x64(),
			 new Simplification.Truncate64x32MulUnsigned64FromZeroExtended32x64(),
			 new Simplification.Truncate64x32And64FromZeroExtended32x64(),
			 new Simplification.Truncate64x32Or64FromZeroExtended32x64(),
			 new Simplification.Truncate64x32Xor64FromZeroExtended32x64(),
			 new Simplification.Add32MultipleWithCommon(),
			 new Simplification.Add32MultipleWithCommon_v1(),
			 new Simplification.Add32MultipleWithCommon_v2(),
			 new Simplification.Add32MultipleWithCommon_v3(),
			 new Simplification.Add32MultipleWithCommon_v4(),
			 new Simplification.Add32MultipleWithCommon_v5(),
			 new Simplification.Add32MultipleWithCommon_v6(),
			 new Simplification.Add32MultipleWithCommon_v7(),
			 new Simplification.Add64MultipleWithCommon(),
			 new Simplification.Add64MultipleWithCommon_v1(),
			 new Simplification.Add64MultipleWithCommon_v2(),
			 new Simplification.Add64MultipleWithCommon_v3(),
			 new Simplification.Add64MultipleWithCommon_v4(),
			 new Simplification.Add64MultipleWithCommon_v5(),
			 new Simplification.Add64MultipleWithCommon_v6(),
			 new Simplification.Add64MultipleWithCommon_v7(),
			 new Simplification.Sub32MultipleWithCommon(),
			 new Simplification.Sub32MultipleWithCommon_v1(),
			 new Simplification.Sub32MultipleWithCommon_v2(),
			 new Simplification.Sub32MultipleWithCommon_v3(),
			 new Simplification.Sub64MultipleWithCommon(),
			 new Simplification.Sub64MultipleWithCommon_v1(),
			 new Simplification.Sub64MultipleWithCommon_v2(),
			 new Simplification.Sub64MultipleWithCommon_v3(),
			 new Simplification.And32Not32Not32(),
			 new Simplification.And64Not64Not64(),
			 new Simplification.Or32Not32Not32(),
			 new Simplification.Or64Not64Not64(),
			 new Simplification.MulSigned32ByNegative1(),
			 new Simplification.MulSigned32ByNegative1_v1(),
			 new Simplification.MulSigned64ByNegative1(),
			 new Simplification.MulSigned64ByNegative1_v1(),
			 new Simplification.And32Same(),
			 new Simplification.And64Same(),
			 new Simplification.Or32Same(),
			 new Simplification.Or64Same(),
			 new Simplification.And32Double(),
			 new Simplification.And32Double_v1(),
			 new Simplification.And32Double_v2(),
			 new Simplification.And32Double_v3(),
			 new Simplification.And64Double(),
			 new Simplification.And64Double_v1(),
			 new Simplification.And64Double_v2(),
			 new Simplification.And64Double_v3(),
			 new Simplification.Or32Double(),
			 new Simplification.Or32Double_v1(),
			 new Simplification.Or32Double_v2(),
			 new Simplification.Or32Double_v3(),
			 new Simplification.Or64Double(),
			 new Simplification.Or64Double_v1(),
			 new Simplification.Or64Double_v2(),
			 new Simplification.Or64Double_v3(),
			 new Simplification.Xor32Double(),
			 new Simplification.Xor32Double_v1(),
			 new Simplification.Xor32Double_v2(),
			 new Simplification.Xor32Double_v3(),
			 new Simplification.Xor64Double(),
			 new Simplification.Xor64Double_v1(),
			 new Simplification.Xor64Double_v2(),
			 new Simplification.Xor64Double_v3(),
			 new Simplification.CompareObjectSameAndEqual(),
			 new Simplification.CompareObjectSameAndNotEqual(),
			 new Simplification.Compare32x32SameAndEqual(),
			 new Simplification.Compare32x64SameAndEqual(),
			 new Simplification.Compare64x32SameAndEqual(),
			 new Simplification.Compare64x64SameAndEqual(),
			 new Simplification.Compare32x32SameAndNotEqual(),
			 new Simplification.Compare32x64SameAndNotEqual(),
			 new Simplification.Compare64x32SameAndNotEqual(),
			 new Simplification.Compare64x64SameAndNotEqual(),
			 new Simplification.Add32v1(),
			 new Simplification.Add32v1_v1(),
			 new Simplification.Add64v1(),
			 new Simplification.Add64v1_v1(),
			 new Simplification.Sub32v1(),
			 new Simplification.Sub32v1_v1(),
			 new Simplification.Sub64v1(),
			 new Simplification.Sub64v1_v1(),
			 new Simplification.Compare32x32SwapToZero(),
			 new Simplification.Compare32x32SwapToZero_v1(),
			 new Simplification.Compare32x64SwapToZero(),
			 new Simplification.Compare32x64SwapToZero_v1(),
			 new Simplification.Compare64x32SwapToZero(),
			 new Simplification.Compare64x32SwapToZero_v1(),
			 new Simplification.Compare64x64SwapToZero(),
			 new Simplification.Compare64x64SwapToZero_v1(),
			 new Simplification.Compare32x32PassThru(),
			 new Simplification.Compare32x32PassThru_v1(),
			 new Simplification.Compare32x64PassThru(),
			 new Simplification.Compare32x64PassThru_v1(),
			 new Simplification.Compare64x32PassThru(),
			 new Simplification.Compare64x32PassThru_v1(),
			 new Simplification.Compare64x64PassThru(),
			 new Simplification.Compare64x64PassThru_v1(),
			 new Simplification.Compare32x32NotPassThru(),
			 new Simplification.Compare32x32NotPassThru_v1(),
			 new Simplification.Compare32x64NotPassThru(),
			 new Simplification.Compare32x64NotPassThru_v1(),
			 new Simplification.Compare64x32NotPassThru(),
			 new Simplification.Compare64x32NotPassThru_v1(),
			 new Simplification.Compare64x64NotPassThru(),
			 new Simplification.Compare64x64NotPassThru_v1(),
			 new Simplification.Compare32x32PassThru2(),
			 new Simplification.Compare32x32PassThru2_v1(),
			 new Simplification.Compare32x64PassThru2(),
			 new Simplification.Compare32x64PassThru2_v1(),
			 new Simplification.Compare64x32PassThru2(),
			 new Simplification.Compare64x32PassThru2_v1(),
			 new Simplification.Compare64x64PassThru2(),
			 new Simplification.Compare64x64PassThru2_v1(),
			 new StrengthReduction.Add32Zero(),
			 new StrengthReduction.Add32Zero_v1(),
			 new StrengthReduction.Add64Zero(),
			 new StrengthReduction.Add64Zero_v1(),
			 new StrengthReduction.And32Zero(),
			 new StrengthReduction.And32Zero_v1(),
			 new StrengthReduction.And64Zero(),
			 new StrengthReduction.And64Zero_v1(),
			 new StrengthReduction.And32Max(),
			 new StrengthReduction.And32Max_v1(),
			 new StrengthReduction.And64Max(),
			 new StrengthReduction.And64Max_v1(),
			 new StrengthReduction.Or32Max(),
			 new StrengthReduction.Or32Max_v1(),
			 new StrengthReduction.Or64Max(),
			 new StrengthReduction.Or64Max_v1(),
			 new StrengthReduction.Or32Zero(),
			 new StrengthReduction.Or32Zero_v1(),
			 new StrengthReduction.Or64Zero(),
			 new StrengthReduction.Or64Zero_v1(),
			 new StrengthReduction.Xor32Zero(),
			 new StrengthReduction.Xor32Zero_v1(),
			 new StrengthReduction.Xor64Zero(),
			 new StrengthReduction.Xor64Zero_v1(),
			 new StrengthReduction.Xor32Max(),
			 new StrengthReduction.Xor32Max_v1(),
			 new StrengthReduction.Xor64Max(),
			 new StrengthReduction.Xor64Max_v1(),
			 new StrengthReduction.ShiftRight32ZeroValue(),
			 new StrengthReduction.ShiftRight64ZeroValue(),
			 new StrengthReduction.ShiftRight32ByZero(),
			 new StrengthReduction.ShiftRight64ByZero(),
			 new StrengthReduction.ShiftLeft32ByZero(),
			 new StrengthReduction.ShiftLeft64ByZero(),
			 new StrengthReduction.Sub32ByZero(),
			 new StrengthReduction.Sub64ByZero(),
			 new StrengthReduction.Sub32Same(),
			 new StrengthReduction.Sub64Same(),
			 new StrengthReduction.Xor32Same(),
			 new StrengthReduction.Xor64Same(),
			 new StrengthReduction.MulSigned32ByZero(),
			 new StrengthReduction.MulSigned32ByZero_v1(),
			 new StrengthReduction.MulSigned64ByZero(),
			 new StrengthReduction.MulSigned64ByZero_v1(),
			 new StrengthReduction.MulUnsigned32ByZero(),
			 new StrengthReduction.MulUnsigned32ByZero_v1(),
			 new StrengthReduction.MulUnsigned64ByZero(),
			 new StrengthReduction.MulUnsigned64ByZero_v1(),
			 new StrengthReduction.MulSigned32ByOne(),
			 new StrengthReduction.MulSigned32ByOne_v1(),
			 new StrengthReduction.MulSigned64ByOne(),
			 new StrengthReduction.MulSigned64ByOne_v1(),
			 new StrengthReduction.MulUnsigned32ByOne(),
			 new StrengthReduction.MulUnsigned32ByOne_v1(),
			 new StrengthReduction.MulUnsigned64ByOne(),
			 new StrengthReduction.MulUnsigned64ByOne_v1(),
			 new StrengthReduction.MulUnsigned32ByPowerOfTwo(),
			 new StrengthReduction.MulUnsigned32ByPowerOfTwo_v1(),
			 new StrengthReduction.MulUnsigned64ByPowerOfTwo(),
			 new StrengthReduction.MulUnsigned64ByPowerOfTwo_v1(),
			 new StrengthReduction.MulSigned32ByPowerOfTwo(),
			 new StrengthReduction.MulSigned32ByPowerOfTwo_v1(),
			 new StrengthReduction.MulSigned64ByPowerOfTwo(),
			 new StrengthReduction.MulSigned64ByPowerOfTwo_v1(),
			 new StrengthReduction.DivSigned32ByZero(),
			 new StrengthReduction.DivSigned64ByZero(),
			 new StrengthReduction.DivSigned32ByOne(),
			 new StrengthReduction.DivSigned64ByOne(),
			 new StrengthReduction.DivUnsigned32ByOne(),
			 new StrengthReduction.DivUnsigned64ByOne(),
			 new StrengthReduction.DivUnsigned32ByPowerOfTwo(),
			 new StrengthReduction.DivUnsigned64ByPowerOfTwo(),
			 new StrengthReduction.DivSigned32ByPowerOfTwo(),
			 new StrengthReduction.DivSigned64ByPowerOfTwo(),
			 new StrengthReduction.RemUnsigned32ByPowerOfTwo(),
			 new StrengthReduction.RemUnsigned64ByPowerOfTwo(),
			 new StrengthReduction.Or32And32ClearAndSet(),
			 new StrengthReduction.Or32And32ClearAndSet_v1(),
			 new StrengthReduction.Or32And32ClearAndSet_v2(),
			 new StrengthReduction.Or32And32ClearAndSet_v3(),
			 new StrengthReduction.Or64And64ClearAndSet(),
			 new StrengthReduction.Or64And64ClearAndSet_v1(),
			 new StrengthReduction.Or64And64ClearAndSet_v2(),
			 new StrengthReduction.Or64And64ClearAndSet_v3(),
			 new StrengthReduction.Compare32x32RemUnsigned(),
			 new StrengthReduction.Compare32x32RemUnsigned_v1(),
			 new StrengthReduction.Compare32x64RemUnsigned(),
			 new StrengthReduction.Compare32x64RemUnsigned_v1(),
			 new StrengthReduction.Compare64x32RemUnsigned(),
			 new StrengthReduction.Compare64x32RemUnsigned_v1(),
			 new StrengthReduction.Compare64x64RemUnsigned(),
			 new StrengthReduction.Compare64x64RemUnsigned_v1(),
			 new StrengthReduction.Compare32x32DivUnsignedRange(),
			 new StrengthReduction.Compare32x32DivUnsignedRange_v1(),
			 new StrengthReduction.Compare32x64DivUnsignedRange(),
			 new StrengthReduction.Compare32x64DivUnsignedRange_v1(),
			 new StrengthReduction.Compare64x32DivUnsignedRange(),
			 new StrengthReduction.Compare64x32DivUnsignedRange_v1(),
			 new StrengthReduction.Compare64x64DivUnsignedRange(),
			 new StrengthReduction.Compare64x64DivUnsignedRange_v1(),
			 new StrengthReduction.UselessAnd32ShiftRight32(),
			 new StrengthReduction.UselessAnd32ShiftRight32_v1(),
			 new StrengthReduction.UselessAnd64ShiftRight64(),
			 new StrengthReduction.UselessAnd64ShiftRight64_v1(),
			 new StrengthReduction.UselessOr32ShiftRight32(),
			 new StrengthReduction.UselessOr32ShiftRight32_v1(),
			 new StrengthReduction.UselessOr64ShiftRight64(),
			 new StrengthReduction.UselessOr64ShiftRight64_v1(),
			 new StrengthReduction.UselessXor32ShiftRight32(),
			 new StrengthReduction.UselessXor32ShiftRight32_v1(),
			 new StrengthReduction.UselessXor64ShiftRight64(),
			 new StrengthReduction.UselessXor64ShiftRight64_v1(),
			 new StrengthReduction.UselessAnd32ShiftLeft32(),
			 new StrengthReduction.UselessAnd32ShiftLeft32_v1(),
			 new StrengthReduction.UselessAnd64ShiftLeft64(),
			 new StrengthReduction.UselessAnd64ShiftLeft64_v1(),
			 new StrengthReduction.UselessOr32ShiftLeft32(),
			 new StrengthReduction.UselessOr32ShiftLeft32_v1(),
			 new StrengthReduction.UselessOr64ShiftLeft64(),
			 new StrengthReduction.UselessOr64ShiftLeft64_v1(),
			 new StrengthReduction.UselessXor32ShiftLeft32(),
			 new StrengthReduction.UselessXor32ShiftLeft32_v1(),
			 new StrengthReduction.UselessXor64ShiftLeft64(),
			 new StrengthReduction.UselessXor64ShiftLeft64_v1(),
			 new Reorder.MulUnsigned32WithShiftLeft32(),
			 new Reorder.MulUnsigned32WithShiftLeft32_v1(),
			 new Reorder.MulUnsigned64WithShiftLeft64(),
			 new Reorder.MulUnsigned64WithShiftLeft64_v1(),
			 new Reorder.MulSigned32WithShiftLeft32(),
			 new Reorder.MulSigned32WithShiftLeft32_v1(),
			 new Reorder.MulSigned64WithShiftLeft64(),
			 new Reorder.MulSigned64WithShiftLeft64_v1(),
			 new Reorder.SubToAdd32(),
			 new Reorder.SubToAdd64(),
			 new ConstantMove.Add32Expression(),
			 new ConstantMove.Add32Expression_v1(),
			 new ConstantMove.Add32Expression_v2(),
			 new ConstantMove.Add32Expression_v3(),
			 new ConstantMove.Add64Expression(),
			 new ConstantMove.Add64Expression_v1(),
			 new ConstantMove.Add64Expression_v2(),
			 new ConstantMove.Add64Expression_v3(),
			 new ConstantMove.AddR4Expression(),
			 new ConstantMove.AddR4Expression_v1(),
			 new ConstantMove.AddR4Expression_v2(),
			 new ConstantMove.AddR4Expression_v3(),
			 new ConstantMove.AddR8Expression(),
			 new ConstantMove.AddR8Expression_v1(),
			 new ConstantMove.AddR8Expression_v2(),
			 new ConstantMove.AddR8Expression_v3(),
			 new ConstantMove.And32Expression(),
			 new ConstantMove.And32Expression_v1(),
			 new ConstantMove.And32Expression_v2(),
			 new ConstantMove.And32Expression_v3(),
			 new ConstantMove.And64Expression(),
			 new ConstantMove.And64Expression_v1(),
			 new ConstantMove.And64Expression_v2(),
			 new ConstantMove.And64Expression_v3(),
			 new ConstantMove.Or32Expression(),
			 new ConstantMove.Or32Expression_v1(),
			 new ConstantMove.Or32Expression_v2(),
			 new ConstantMove.Or32Expression_v3(),
			 new ConstantMove.Or64Expression(),
			 new ConstantMove.Or64Expression_v1(),
			 new ConstantMove.Or64Expression_v2(),
			 new ConstantMove.Or64Expression_v3(),
			 new ConstantMove.Xor32Expression(),
			 new ConstantMove.Xor32Expression_v1(),
			 new ConstantMove.Xor32Expression_v2(),
			 new ConstantMove.Xor32Expression_v3(),
			 new ConstantMove.Xor64Expression(),
			 new ConstantMove.Xor64Expression_v1(),
			 new ConstantMove.Xor64Expression_v2(),
			 new ConstantMove.Xor64Expression_v3(),
			 new ConstantMove.MulUnsigned32Expression(),
			 new ConstantMove.MulUnsigned32Expression_v1(),
			 new ConstantMove.MulUnsigned32Expression_v2(),
			 new ConstantMove.MulUnsigned32Expression_v3(),
			 new ConstantMove.MulUnsigned64Expression(),
			 new ConstantMove.MulUnsigned64Expression_v1(),
			 new ConstantMove.MulUnsigned64Expression_v2(),
			 new ConstantMove.MulUnsigned64Expression_v3(),
			 new ConstantMove.MulSigned32Expression(),
			 new ConstantMove.MulSigned32Expression_v1(),
			 new ConstantMove.MulSigned32Expression_v2(),
			 new ConstantMove.MulSigned32Expression_v3(),
			 new ConstantMove.MulSigned64Expression(),
			 new ConstantMove.MulSigned64Expression_v1(),
			 new ConstantMove.MulSigned64Expression_v2(),
			 new ConstantMove.MulSigned64Expression_v3(),
			 new ConstantMove.MulR4Expression(),
			 new ConstantMove.MulR4Expression_v1(),
			 new ConstantMove.MulR4Expression_v2(),
			 new ConstantMove.MulR4Expression_v3(),
			 new ConstantMove.MulR8Expression(),
			 new ConstantMove.MulR8Expression_v1(),
			 new ConstantMove.MulR8Expression_v2(),
			 new ConstantMove.MulR8Expression_v3(),
			 new ConstantFolding.MulSignedShiftLeft32(),
			 new ConstantFolding.MulSignedShiftLeft32_v1(),
			 new ConstantFolding.MulSignedShiftLeft64(),
			 new ConstantFolding.MulSignedShiftLeft64_v1(),
			 new ConstantFolding.MulUnsignedShiftLeft32(),
			 new ConstantFolding.MulUnsignedShiftLeft32_v1(),
			 new ConstantFolding.MulUnsignedShiftLeft64(),
			 new ConstantFolding.MulUnsignedShiftLeft64_v1(),
			 new ConstantFolding.AddCarryIn32Inside(),
			 new ConstantFolding.AddCarryIn64Inside(),
			 new ConstantFolding.AddCarryIn32Outside1(),
			 new ConstantFolding.AddCarryIn64Outside1(),
			 new ConstantFolding.AddCarryIn32Outside2(),
			 new ConstantFolding.AddCarryIn64Outside2(),
			 new ConstantFolding.AddCarryIn32NoCarry(),
			 new ConstantFolding.AddCarryIn64NoCarry(),
			 new ConstantFolding.AddCarryIn32Zero1(),
			 new ConstantFolding.AddCarryIn64Zero1(),
			 new ConstantFolding.AddCarryIn32Zero2(),
			 new ConstantFolding.AddCarryIn64Zero2(),
			 new ConstantFolding.SubCarryIn32Inside(),
			 new ConstantFolding.SubCarryIn64Inside(),
			 new ConstantFolding.SubCarryIn32Outside1(),
			 new ConstantFolding.SubCarryIn64Outside1(),
			 new ConstantFolding.SubCarryIn32Outside2(),
			 new ConstantFolding.SubCarryIn64Outside2(),
			 new ConstantFolding.SubCarryIn32NoCarry(),
			 new ConstantFolding.SubCarryIn64NoCarry(),
			 new ConstantFolding.Add32x2(),
			 new ConstantFolding.Add32x2_v1(),
			 new ConstantFolding.Add32x2_v2(),
			 new ConstantFolding.Add32x2_v3(),
			 new ConstantFolding.Add64x2(),
			 new ConstantFolding.Add64x2_v1(),
			 new ConstantFolding.Add64x2_v2(),
			 new ConstantFolding.Add64x2_v3(),
			 new ConstantFolding.AddR4x2(),
			 new ConstantFolding.AddR4x2_v1(),
			 new ConstantFolding.AddR4x2_v2(),
			 new ConstantFolding.AddR4x2_v3(),
			 new ConstantFolding.AddR8x2(),
			 new ConstantFolding.AddR8x2_v1(),
			 new ConstantFolding.AddR8x2_v2(),
			 new ConstantFolding.AddR8x2_v3(),
			 new ConstantFolding.Sub32x2(),
			 new ConstantFolding.Sub64x2(),
			 new ConstantFolding.SubR4x2(),
			 new ConstantFolding.SubR8x2(),
			 new ConstantFolding.MulSigned32x2(),
			 new ConstantFolding.MulSigned32x2_v1(),
			 new ConstantFolding.MulSigned32x2_v2(),
			 new ConstantFolding.MulSigned32x2_v3(),
			 new ConstantFolding.MulSigned64x2(),
			 new ConstantFolding.MulSigned64x2_v1(),
			 new ConstantFolding.MulSigned64x2_v2(),
			 new ConstantFolding.MulSigned64x2_v3(),
			 new ConstantFolding.MulR4x2(),
			 new ConstantFolding.MulR4x2_v1(),
			 new ConstantFolding.MulR4x2_v2(),
			 new ConstantFolding.MulR4x2_v3(),
			 new ConstantFolding.MulR8x2(),
			 new ConstantFolding.MulR8x2_v1(),
			 new ConstantFolding.MulR8x2_v2(),
			 new ConstantFolding.MulR8x2_v3(),
			 new ConstantFolding.MulUnsigned32x2(),
			 new ConstantFolding.MulUnsigned32x2_v1(),
			 new ConstantFolding.MulUnsigned32x2_v2(),
			 new ConstantFolding.MulUnsigned32x2_v3(),
			 new ConstantFolding.MulUnsigned64x2(),
			 new ConstantFolding.MulUnsigned64x2_v1(),
			 new ConstantFolding.MulUnsigned64x2_v2(),
			 new ConstantFolding.MulUnsigned64x2_v3(),
			 new ConstantFolding.Or32x2(),
			 new ConstantFolding.Or32x2_v1(),
			 new ConstantFolding.Or32x2_v2(),
			 new ConstantFolding.Or32x2_v3(),
			 new ConstantFolding.Or64x2(),
			 new ConstantFolding.Or64x2_v1(),
			 new ConstantFolding.Or64x2_v2(),
			 new ConstantFolding.Or64x2_v3(),
			 new ConstantFolding.And32x2(),
			 new ConstantFolding.And32x2_v1(),
			 new ConstantFolding.And32x2_v2(),
			 new ConstantFolding.And32x2_v3(),
			 new ConstantFolding.And64x2(),
			 new ConstantFolding.And64x2_v1(),
			 new ConstantFolding.And64x2_v2(),
			 new ConstantFolding.And64x2_v3(),
			 new ConstantFolding.Xor32x2(),
			 new ConstantFolding.Xor32x2_v1(),
			 new ConstantFolding.Xor32x2_v2(),
			 new ConstantFolding.Xor32x2_v3(),
			 new ConstantFolding.Xor64x2(),
			 new ConstantFolding.Xor64x2_v1(),
			 new ConstantFolding.Xor64x2_v2(),
			 new ConstantFolding.Xor64x2_v3(),
			 new ConstantFolding.AddSub32(),
			 new ConstantFolding.AddSub32_v1(),
			 new ConstantFolding.AddSub64(),
			 new ConstantFolding.AddSub64_v1(),
			 new ConstantFolding.AddSubR4(),
			 new ConstantFolding.AddSubR4_v1(),
			 new ConstantFolding.AddSubR8(),
			 new ConstantFolding.AddSubR8_v1(),
			 new ConstantFolding.SubAdd32(),
			 new ConstantFolding.SubAdd32_v1(),
			 new ConstantFolding.SubAdd64(),
			 new ConstantFolding.SubAdd64_v1(),
			 new ConstantFolding.SubAddR4(),
			 new ConstantFolding.SubAddR4_v1(),
			 new ConstantFolding.SubAddR8(),
			 new ConstantFolding.SubAddR8_v1(),
			 new ConstantFolding.ShiftLeft32x2(),
			 new ConstantFolding.ShiftLeft64x2(),
			 new ConstantFolding.ShiftRight32x2(),
			 new ConstantFolding.ShiftRight64x2(),
			 new ConstantFolding.Load32FoldAdd32(),
			 new ConstantFolding.Load64FoldAdd32(),
			 new ConstantFolding.LoadR4FoldAdd32(),
			 new ConstantFolding.LoadR8FoldAdd32(),
			 new ConstantFolding.LoadSignExtend8x32FoldAdd32(),
			 new ConstantFolding.LoadSignExtend16x32FoldAdd32(),
			 new ConstantFolding.LoadSignExtend8x64FoldAdd32(),
			 new ConstantFolding.LoadSignExtend16x64FoldAdd32(),
			 new ConstantFolding.LoadSignExtend32x64FoldAdd32(),
			 new ConstantFolding.LoadZeroExtend8x32FoldAdd32(),
			 new ConstantFolding.LoadZeroExtend16x32FoldAdd32(),
			 new ConstantFolding.LoadZeroExtend8x64FoldAdd32(),
			 new ConstantFolding.LoadZeroExtend16x64FoldAdd32(),
			 new ConstantFolding.LoadZeroExtend32x64FoldAdd32(),
			 new ConstantFolding.Load32FoldAdd64(),
			 new ConstantFolding.Load64FoldAdd64(),
			 new ConstantFolding.LoadR4FoldAdd64(),
			 new ConstantFolding.LoadR8FoldAdd64(),
			 new ConstantFolding.LoadSignExtend8x32FoldAdd64(),
			 new ConstantFolding.LoadSignExtend16x32FoldAdd64(),
			 new ConstantFolding.LoadSignExtend8x64FoldAdd64(),
			 new ConstantFolding.LoadSignExtend16x64FoldAdd64(),
			 new ConstantFolding.LoadSignExtend32x64FoldAdd64(),
			 new ConstantFolding.LoadZeroExtend8x32FoldAdd64(),
			 new ConstantFolding.LoadZeroExtend16x32FoldAdd64(),
			 new ConstantFolding.LoadZeroExtend8x64FoldAdd64(),
			 new ConstantFolding.LoadZeroExtend16x64FoldAdd64(),
			 new ConstantFolding.LoadZeroExtend32x64FoldAdd64(),
			 new ConstantFolding.Load32FoldSub32(),
			 new ConstantFolding.Load64FoldSub32(),
			 new ConstantFolding.LoadR4FoldSub32(),
			 new ConstantFolding.LoadR8FoldSub32(),
			 new ConstantFolding.LoadSignExtend8x32FoldSub32(),
			 new ConstantFolding.LoadSignExtend16x32FoldSub32(),
			 new ConstantFolding.LoadSignExtend8x64FoldSub32(),
			 new ConstantFolding.LoadSignExtend16x64FoldSub32(),
			 new ConstantFolding.LoadSignExtend32x64FoldSub32(),
			 new ConstantFolding.LoadZeroExtend8x32FoldSub32(),
			 new ConstantFolding.LoadZeroExtend16x32FoldSub32(),
			 new ConstantFolding.LoadZeroExtend8x64FoldSub32(),
			 new ConstantFolding.LoadZeroExtend16x64FoldSub32(),
			 new ConstantFolding.LoadZeroExtend32x64FoldSub32(),
			 new ConstantFolding.Load32FoldSub64(),
			 new ConstantFolding.Load64FoldSub64(),
			 new ConstantFolding.LoadR4FoldSub64(),
			 new ConstantFolding.LoadR8FoldSub64(),
			 new ConstantFolding.LoadSignExtend8x32FoldSub64(),
			 new ConstantFolding.LoadSignExtend16x32FoldSub64(),
			 new ConstantFolding.LoadSignExtend8x64FoldSub64(),
			 new ConstantFolding.LoadSignExtend16x64FoldSub64(),
			 new ConstantFolding.LoadSignExtend32x64FoldSub64(),
			 new ConstantFolding.LoadZeroExtend8x32FoldSub64(),
			 new ConstantFolding.LoadZeroExtend16x32FoldSub64(),
			 new ConstantFolding.LoadZeroExtend8x64FoldSub64(),
			 new ConstantFolding.LoadZeroExtend16x64FoldSub64(),
			 new ConstantFolding.LoadZeroExtend32x64FoldSub64(),
			 new ConstantFolding.Store8FoldAdd32(),
			 new ConstantFolding.Store8FoldAdd64(),
			 new ConstantFolding.Store8FoldSub32(),
			 new ConstantFolding.Store8FoldSub64(),
			 new ConstantFolding.Store16FoldAdd32(),
			 new ConstantFolding.Store16FoldAdd64(),
			 new ConstantFolding.Store16FoldSub32(),
			 new ConstantFolding.Store16FoldSub64(),
			 new ConstantFolding.Store32FoldAdd32(),
			 new ConstantFolding.Store32FoldAdd64(),
			 new ConstantFolding.Store32FoldSub32(),
			 new ConstantFolding.Store32FoldSub64(),
			 new ConstantFolding.Store64FoldAdd32(),
			 new ConstantFolding.Store64FoldAdd64(),
			 new ConstantFolding.Store64FoldSub32(),
			 new ConstantFolding.Store64FoldSub64(),
			 new ConstantFolding.StoreR4FoldAdd32(),
			 new ConstantFolding.StoreR4FoldAdd64(),
			 new ConstantFolding.StoreR4FoldSub32(),
			 new ConstantFolding.StoreR4FoldSub64(),
			 new ConstantFolding.StoreR8FoldAdd32(),
			 new ConstantFolding.StoreR8FoldAdd64(),
			 new ConstantFolding.StoreR8FoldSub32(),
			 new ConstantFolding.StoreR8FoldSub64(),
			 new ConstantFolding.Load32AddressFold(),
			 new ConstantFolding.Load64AddressFold(),
			 new ConstantFolding.LoadR4AddressFold(),
			 new ConstantFolding.LoadR8AddressFold(),
			 new ConstantFolding.LoadSignExtend8x32AddressFold(),
			 new ConstantFolding.LoadSignExtend16x32AddressFold(),
			 new ConstantFolding.LoadSignExtend8x64AddressFold(),
			 new ConstantFolding.LoadSignExtend16x64AddressFold(),
			 new ConstantFolding.LoadSignExtend32x64AddressFold(),
			 new ConstantFolding.LoadZeroExtend8x32AddressFold(),
			 new ConstantFolding.LoadZeroExtend16x32AddressFold(),
			 new ConstantFolding.LoadZeroExtend8x64AddressFold(),
			 new ConstantFolding.LoadZeroExtend16x64AddressFold(),
			 new ConstantFolding.LoadZeroExtend32x64AddressFold(),
			 new Rewrite.CompareObjectGreaterThanZero(),
			 new Rewrite.CompareObjectGreaterThanZero_v1(),
			 new Rewrite.Compare32x32GreaterThanZero(),
			 new Rewrite.Compare32x32GreaterThanZero_v1(),
			 new Rewrite.Compare32x64GreaterThanZero(),
			 new Rewrite.Compare32x64GreaterThanZero_v1(),
			 new Rewrite.Compare64x32GreaterThanZero(),
			 new Rewrite.Compare64x32GreaterThanZero_v1(),
			 new Rewrite.Compare64x64GreaterThanZero(),
			 new Rewrite.Compare64x64GreaterThanZero_v1(),
			 new Rewrite.IfThenElse32Compare32v1(),
			 new Rewrite.IfThenElse32Compare32v1_v1(),
			 new Rewrite.IfThenElse64Compare64v1(),
			 new Rewrite.IfThenElse64Compare64v1_v1(),
			 new Rewrite.IfThenElse32Compare32v2(),
			 new Rewrite.IfThenElse32Compare32v2_v1(),
			 new Rewrite.IfThenElse64Compare64v2(),
			 new Rewrite.IfThenElse64Compare64v2_v1(),
			 new Rewrite.IfThenElse32Compare32v3(),
			 new Rewrite.IfThenElse32Compare32v3_v1(),
			 new Rewrite.IfThenElse64Compare64v3(),
			 new Rewrite.IfThenElse64Compare64v3_v1(),
			 new Rewrite.IfThenElse32Compare32v4(),
			 new Rewrite.IfThenElse32Compare32v4_v1(),
			 new Rewrite.IfThenElse64Compare64v4(),
			 new Rewrite.IfThenElse64Compare64v4_v1(),
			 new Simplification.Xor32One(),
			 new Simplification.Xor32One_v1(),
			 new Simplification.Xor64One(),
			 new Simplification.Xor64One_v1(),
			 new Algebraic.Signed32AAPlusBBPlus2AB(),
			 new Algebraic.Signed32AAPlusBBPlus2AB_v1(),
			 new Algebraic.Signed32AAPlusBBPlus2AB_v2(),
			 new Algebraic.Signed32AAPlusBBPlus2AB_v3(),
			 new Algebraic.Signed32AAPlusBBPlus2AB_v4(),
			 new Algebraic.Signed32AAPlusBBPlus2AB_v5(),
			 new Algebraic.Signed32AAPlusBBPlus2AB_v6(),
			 new Algebraic.Signed32AAPlusBBPlus2AB_v7(),
			 new Algebraic.Signed64AAPlusBBPlus2AB(),
			 new Algebraic.Signed64AAPlusBBPlus2AB_v1(),
			 new Algebraic.Signed64AAPlusBBPlus2AB_v2(),
			 new Algebraic.Signed64AAPlusBBPlus2AB_v3(),
			 new Algebraic.Signed64AAPlusBBPlus2AB_v4(),
			 new Algebraic.Signed64AAPlusBBPlus2AB_v5(),
			 new Algebraic.Signed64AAPlusBBPlus2AB_v6(),
			 new Algebraic.Signed64AAPlusBBPlus2AB_v7(),
			 new Algebraic.Unsigned32AAPlusBBPlus2AB(),
			 new Algebraic.Unsigned32AAPlusBBPlus2AB_v1(),
			 new Algebraic.Unsigned32AAPlusBBPlus2AB_v2(),
			 new Algebraic.Unsigned32AAPlusBBPlus2AB_v3(),
			 new Algebraic.Unsigned32AAPlusBBPlus2AB_v4(),
			 new Algebraic.Unsigned32AAPlusBBPlus2AB_v5(),
			 new Algebraic.Unsigned32AAPlusBBPlus2AB_v6(),
			 new Algebraic.Unsigned32AAPlusBBPlus2AB_v7(),
			 new Algebraic.Unsigned64AAPlusBBPlus2AB(),
			 new Algebraic.Unsigned64AAPlusBBPlus2AB_v1(),
			 new Algebraic.Unsigned64AAPlusBBPlus2AB_v2(),
			 new Algebraic.Unsigned64AAPlusBBPlus2AB_v3(),
			 new Algebraic.Unsigned64AAPlusBBPlus2AB_v4(),
			 new Algebraic.Unsigned64AAPlusBBPlus2AB_v5(),
			 new Algebraic.Unsigned64AAPlusBBPlus2AB_v6(),
			 new Algebraic.Unsigned64AAPlusBBPlus2AB_v7(),
			 new Algebraic.Signed32AAPlusBBMinus2AB(),
			 new Algebraic.Signed32AAPlusBBMinus2AB_v1(),
			 new Algebraic.Signed32AAPlusBBMinus2AB_v2(),
			 new Algebraic.Signed32AAPlusBBMinus2AB_v3(),
			 new Algebraic.Signed64AAPlusBBMinus2AB(),
			 new Algebraic.Signed64AAPlusBBMinus2AB_v1(),
			 new Algebraic.Signed64AAPlusBBMinus2AB_v2(),
			 new Algebraic.Signed64AAPlusBBMinus2AB_v3(),
			 new Algebraic.Unsigned32AAPlusBBMinus2AB(),
			 new Algebraic.Unsigned32AAPlusBBMinus2AB_v1(),
			 new Algebraic.Unsigned32AAPlusBBMinus2AB_v2(),
			 new Algebraic.Unsigned32AAPlusBBMinus2AB_v3(),
			 new Algebraic.Unsigned64AAPlusBBMinus2AB(),
			 new Algebraic.Unsigned64AAPlusBBMinus2AB_v1(),
			 new Algebraic.Unsigned64AAPlusBBMinus2AB_v2(),
			 new Algebraic.Unsigned64AAPlusBBMinus2AB_v3(),
			 new Algebraic.Signed32AAMinusBB(),
			 new Algebraic.Signed64AAMinusBB(),
			 new Algebraic.Unsigned32AAMinusBB(),
			 new Algebraic.Unsigned64AAMinusBB(),
			 new Algebraic.Unsigned32PerfectSquareFormula(),
			 new Algebraic.Unsigned32PerfectSquareFormula_v1(),
			 new Algebraic.Unsigned32PerfectSquareFormula_v2(),
			 new Algebraic.Unsigned32PerfectSquareFormula_v3(),
			 new Algebraic.Unsigned32PerfectSquareFormula_v4(),
			 new Algebraic.Unsigned32PerfectSquareFormula_v5(),
			 new Algebraic.Unsigned32PerfectSquareFormula_v6(),
			 new Algebraic.Unsigned32PerfectSquareFormula_v7(),
			 new Algebraic.Unsigned64PerfectSquareFormula(),
			 new Algebraic.Unsigned64PerfectSquareFormula_v1(),
			 new Algebraic.Unsigned64PerfectSquareFormula_v2(),
			 new Algebraic.Unsigned64PerfectSquareFormula_v3(),
			 new Algebraic.Unsigned64PerfectSquareFormula_v4(),
			 new Algebraic.Unsigned64PerfectSquareFormula_v5(),
			 new Algebraic.Unsigned64PerfectSquareFormula_v6(),
			 new Algebraic.Unsigned64PerfectSquareFormula_v7(),
			 new Algebraic.Signed32PerfectSquareFormula(),
			 new Algebraic.Signed32PerfectSquareFormula_v1(),
			 new Algebraic.Signed32PerfectSquareFormula_v2(),
			 new Algebraic.Signed32PerfectSquareFormula_v3(),
			 new Algebraic.Signed32PerfectSquareFormula_v4(),
			 new Algebraic.Signed32PerfectSquareFormula_v5(),
			 new Algebraic.Signed32PerfectSquareFormula_v6(),
			 new Algebraic.Signed32PerfectSquareFormula_v7(),
			 new Algebraic.Signed64PerfectSquareFormula(),
			 new Algebraic.Signed64PerfectSquareFormula_v1(),
			 new Algebraic.Signed64PerfectSquareFormula_v2(),
			 new Algebraic.Signed64PerfectSquareFormula_v3(),
			 new Algebraic.Signed64PerfectSquareFormula_v4(),
			 new Algebraic.Signed64PerfectSquareFormula_v5(),
			 new Algebraic.Signed64PerfectSquareFormula_v6(),
			 new Algebraic.Signed64PerfectSquareFormula_v7(),
		};
	}
}
