// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using System.Collections.Generic;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.IR;

/// <summary>
/// IR Instruction Map
/// </summary>
public static class IRInstructions
{
	public static readonly List<BaseInstruction> List = new List<BaseInstruction> {
		IRInstruction.AddR4,
		IRInstruction.AddR8,
		IRInstruction.AddressOf,
		IRInstruction.Add32,
		IRInstruction.Add64,
		IRInstruction.AddManagedPointer,
		IRInstruction.AddCarryOut32,
		IRInstruction.AddCarryOut64,
		IRInstruction.AddCarryIn32,
		IRInstruction.AddCarryIn64,
		IRInstruction.AddOverflowOut32,
		IRInstruction.AddOverflowOut64,
		IRInstruction.ArithShiftRight32,
		IRInstruction.ArithShiftRight64,
		IRInstruction.BlockEnd,
		IRInstruction.BlockStart,
		IRInstruction.Call,
		IRInstruction.CallDirect,
		IRInstruction.CallDynamic,
		IRInstruction.CallInterface,
		IRInstruction.CallStatic,
		IRInstruction.CallVirtual,
		IRInstruction.CompareR4,
		IRInstruction.CompareR8,
		IRInstruction.CompareObject,
		IRInstruction.CompareManagedPointer,
		IRInstruction.Compare32x32,
		IRInstruction.Compare32x64,
		IRInstruction.Compare64x32,
		IRInstruction.Compare64x64,
		IRInstruction.Branch32,
		IRInstruction.Branch64,
		IRInstruction.BranchObject,
		IRInstruction.BranchManagedPointer,
		IRInstruction.ConvertR4ToR8,
		IRInstruction.ConvertR4ToI32,
		IRInstruction.ConvertR4ToI64,
		IRInstruction.ConvertR8ToR4,
		IRInstruction.ConvertR8ToI32,
		IRInstruction.ConvertR8ToI64,
		IRInstruction.ConvertI32ToR4,
		IRInstruction.ConvertI64ToR4,
		IRInstruction.ConvertI32ToR8,
		IRInstruction.ConvertI64ToR8,
		IRInstruction.ConvertR4ToU32,
		IRInstruction.ConvertR4ToU64,
		IRInstruction.ConvertR8ToU32,
		IRInstruction.ConvertR8ToU64,
		IRInstruction.ConvertU32ToR4,
		IRInstruction.ConvertU64ToR4,
		IRInstruction.ConvertU32ToR8,
		IRInstruction.ConvertU64ToR8,
		IRInstruction.DivR4,
		IRInstruction.DivR8,
		IRInstruction.DivSigned32,
		IRInstruction.DivSigned64,
		IRInstruction.DivUnsigned32,
		IRInstruction.DivUnsigned64,
		IRInstruction.Epilogue,
		IRInstruction.ExceptionEnd,
		IRInstruction.ExceptionStart,
		IRInstruction.FilterEnd,
		IRInstruction.FilterStart,
		IRInstruction.FinallyEnd,
		IRInstruction.FinallyStart,
		IRInstruction.Flow,
		IRInstruction.Gen,
		IRInstruction.IntrinsicMethodCall,
		IRInstruction.IsInstanceOfType,
		IRInstruction.IsInstanceOfInterfaceType,
		IRInstruction.Jmp,
		IRInstruction.Kill,
		IRInstruction.KillAll,
		IRInstruction.KillAllExcept,
		IRInstruction.LoadCompound,
		IRInstruction.LoadR4,
		IRInstruction.LoadR8,
		IRInstruction.Load32,
		IRInstruction.Load64,
		IRInstruction.LoadObject,
		IRInstruction.LoadManagedPointer,
		IRInstruction.LoadSignExtend8x32,
		IRInstruction.LoadSignExtend16x32,
		IRInstruction.LoadSignExtend8x64,
		IRInstruction.LoadSignExtend16x64,
		IRInstruction.LoadSignExtend32x64,
		IRInstruction.LoadZeroExtend8x32,
		IRInstruction.LoadZeroExtend16x32,
		IRInstruction.LoadZeroExtend8x64,
		IRInstruction.LoadZeroExtend16x64,
		IRInstruction.LoadZeroExtend32x64,
		IRInstruction.LoadParamCompound,
		IRInstruction.LoadParamObject,
		IRInstruction.LoadParamManagedPointer,
		IRInstruction.LoadParamR4,
		IRInstruction.LoadParamR8,
		IRInstruction.LoadParam32,
		IRInstruction.LoadParam64,
		IRInstruction.LoadParamSignExtend8x32,
		IRInstruction.LoadParamSignExtend16x32,
		IRInstruction.LoadParamSignExtend8x64,
		IRInstruction.LoadParamSignExtend16x64,
		IRInstruction.LoadParamSignExtend32x64,
		IRInstruction.LoadParamZeroExtend8x32,
		IRInstruction.LoadParamZeroExtend16x32,
		IRInstruction.LoadParamZeroExtend8x64,
		IRInstruction.LoadParamZeroExtend16x64,
		IRInstruction.LoadParamZeroExtend32x64,
		IRInstruction.And32,
		IRInstruction.And64,
		IRInstruction.Not32,
		IRInstruction.Not64,
		IRInstruction.Or32,
		IRInstruction.Or64,
		IRInstruction.Xor32,
		IRInstruction.Xor64,
		IRInstruction.MemorySet,
		IRInstruction.MoveCompound,
		IRInstruction.MoveR4,
		IRInstruction.MoveR8,
		IRInstruction.SignExtend8x32,
		IRInstruction.SignExtend16x32,
		IRInstruction.SignExtend8x64,
		IRInstruction.SignExtend16x64,
		IRInstruction.SignExtend32x64,
		IRInstruction.ZeroExtend8x32,
		IRInstruction.ZeroExtend16x32,
		IRInstruction.ZeroExtend8x64,
		IRInstruction.ZeroExtend16x64,
		IRInstruction.ZeroExtend32x64,
		IRInstruction.Move32,
		IRInstruction.Move64,
		IRInstruction.MoveObject,
		IRInstruction.MoveManagedPointer,
		IRInstruction.MulCarryOut32,
		IRInstruction.MulCarryOut64,
		IRInstruction.MulOverflowOut32,
		IRInstruction.MulOverflowOut64,
		IRInstruction.MulR4,
		IRInstruction.MulR8,
		IRInstruction.MulSigned32,
		IRInstruction.MulSigned64,
		IRInstruction.MulUnsigned64,
		IRInstruction.MulUnsigned32,
		IRInstruction.NewArray,
		IRInstruction.NewObject,
		IRInstruction.NewString,
		IRInstruction.Nop,
		IRInstruction.PhiObject,
		IRInstruction.PhiManagedPointer,
		IRInstruction.Phi32,
		IRInstruction.Phi64,
		IRInstruction.PhiR4,
		IRInstruction.PhiR8,
		IRInstruction.Prologue,
		IRInstruction.RemR4,
		IRInstruction.RemR8,
		IRInstruction.RemSigned32,
		IRInstruction.RemSigned64,
		IRInstruction.RemUnsigned32,
		IRInstruction.RemUnsigned64,
		IRInstruction.SetReturnR4,
		IRInstruction.SetReturnR8,
		IRInstruction.SetReturn32,
		IRInstruction.SetReturn64,
		IRInstruction.SetReturnObject,
		IRInstruction.SetReturnManagedPointer,
		IRInstruction.SetReturnCompound,
		IRInstruction.ShiftLeft32,
		IRInstruction.ShiftLeft64,
		IRInstruction.ShiftRight32,
		IRInstruction.ShiftRight64,
		IRInstruction.StableObjectTracking,
		IRInstruction.StoreCompound,
		IRInstruction.StoreR4,
		IRInstruction.StoreR8,
		IRInstruction.Store8,
		IRInstruction.Store16,
		IRInstruction.Store32,
		IRInstruction.Store64,
		IRInstruction.StoreObject,
		IRInstruction.StoreManagedPointer,
		IRInstruction.StoreParamCompound,
		IRInstruction.StoreParamR4,
		IRInstruction.StoreParamR8,
		IRInstruction.StoreParamObject,
		IRInstruction.StoreParamManagedPointer,
		IRInstruction.StoreParam8,
		IRInstruction.StoreParam16,
		IRInstruction.StoreParam32,
		IRInstruction.StoreParam64,
		IRInstruction.SubR4,
		IRInstruction.SubR8,
		IRInstruction.Sub32,
		IRInstruction.Sub64,
		IRInstruction.SubManagedPointer,
		IRInstruction.SubCarryOut32,
		IRInstruction.SubCarryOut64,
		IRInstruction.SubCarryIn32,
		IRInstruction.SubCarryIn64,
		IRInstruction.SubOverflowOut32,
		IRInstruction.SubOverflowOut64,
		IRInstruction.Switch,
		IRInstruction.Throw,
		IRInstruction.Truncate64x32,
		IRInstruction.TryEnd,
		IRInstruction.TryStart,
		IRInstruction.UnstableObjectTracking,
		IRInstruction.Rethrow,
		IRInstruction.GetVirtualFunctionPtr,
		IRInstruction.MemoryCopy,
		IRInstruction.Box,
		IRInstruction.Box32,
		IRInstruction.Box64,
		IRInstruction.BoxR4,
		IRInstruction.BoxR8,
		IRInstruction.Unbox,
		IRInstruction.UnboxAny,
		IRInstruction.To64,
		IRInstruction.GetLow32,
		IRInstruction.GetHigh32,
		IRInstruction.IfThenElse32,
		IRInstruction.IfThenElse64,
		IRInstruction.IfThenElseObject,
		IRInstruction.IfThenElseManagedPointer,
		IRInstruction.BitCopyR4To32,
		IRInstruction.BitCopyR8To64,
		IRInstruction.BitCopy32ToR4,
		IRInstruction.BitCopy64ToR8,
		IRInstruction.ThrowOverflow,
		IRInstruction.ThrowIndexOutOfRange,
		IRInstruction.ThrowDivideByZero,
		IRInstruction.CheckArrayBounds,
		IRInstruction.CheckThrowOverflow,
		IRInstruction.CheckThrowIndexOutOfRange,
		IRInstruction.CheckThrowDivideByZero,
		IRInstruction.CheckedConversionI32ToU8,
		IRInstruction.CheckedConversionI64ToU8,
		IRInstruction.CheckedConversionR4ToU8,
		IRInstruction.CheckedConversionR8ToU8,
		IRInstruction.CheckedConversionU32ToU8,
		IRInstruction.CheckedConversionU64ToU8,
		IRInstruction.CheckedConversionI64ToI32,
		IRInstruction.CheckedConversionR4ToI32,
		IRInstruction.CheckedConversionR8ToI32,
		IRInstruction.CheckedConversionU32ToI32,
		IRInstruction.CheckedConversionU64ToI32,
		IRInstruction.CheckedConversionR4ToI64,
		IRInstruction.CheckedConversionR8ToI64,
		IRInstruction.CheckedConversionU64ToI64,
		IRInstruction.CheckedConversionI32ToI8,
		IRInstruction.CheckedConversionI64ToI8,
		IRInstruction.CheckedConversionR4ToI8,
		IRInstruction.CheckedConversionR8ToI8,
		IRInstruction.CheckedConversionU32ToI8,
		IRInstruction.CheckedConversionU64ToI8,
		IRInstruction.CheckedConversionI32ToI16,
		IRInstruction.CheckedConversionI64ToI16,
		IRInstruction.CheckedConversionR4ToI16,
		IRInstruction.CheckedConversionR8ToI16,
		IRInstruction.CheckedConversionU32ToI16,
		IRInstruction.CheckedConversionU64ToI16,
		IRInstruction.CheckedConversionI32ToU32,
		IRInstruction.CheckedConversionI64ToU32,
		IRInstruction.CheckedConversionR4ToU32,
		IRInstruction.CheckedConversionR8ToU32,
		IRInstruction.CheckedConversionU64ToU32,
		IRInstruction.CheckedConversionI32ToU64,
		IRInstruction.CheckedConversionI64ToU64,
		IRInstruction.CheckedConversionR4ToU64,
		IRInstruction.CheckedConversionR8ToU64,
		IRInstruction.CheckedConversionI32ToU16,
		IRInstruction.CheckedConversionI64ToU16,
		IRInstruction.CheckedConversionR4ToU16,
		IRInstruction.CheckedConversionR8ToU16,
		IRInstruction.CheckedConversionU32ToU16,
		IRInstruction.CheckedConversionU64ToU16,
	};
}
