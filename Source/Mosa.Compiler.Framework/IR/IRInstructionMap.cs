// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// IR Instruction Map
	/// </summary>
	public static class IRInstructionMap
	{
		public static readonly Dictionary<string, BaseInstruction> Map = new Dictionary<string, BaseInstruction>() {
			{ "AddFloatR4", IRInstruction.AddFloatR4 },
			{ "AddFloatR8", IRInstruction.AddFloatR8 },
			{ "AddressOf", IRInstruction.AddressOf },
			{ "Add32", IRInstruction.Add32 },
			{ "Add64", IRInstruction.Add64 },
			{ "AddCarryOut32", IRInstruction.AddCarryOut32 },
			{ "AddWithCarry32", IRInstruction.AddWithCarry32 },
			{ "ArithShiftRight32", IRInstruction.ArithShiftRight32 },
			{ "ArithShiftRight64", IRInstruction.ArithShiftRight64 },
			{ "BlockEnd", IRInstruction.BlockEnd },
			{ "BlockStart", IRInstruction.BlockStart },
			{ "Break", IRInstruction.Break },
			{ "Call", IRInstruction.Call },
			{ "CallDirect", IRInstruction.CallDirect },
			{ "CallDynamic", IRInstruction.CallDynamic },
			{ "CallInterface", IRInstruction.CallInterface },
			{ "CallStatic", IRInstruction.CallStatic },
			{ "CallVirtual", IRInstruction.CallVirtual },
			{ "CompareFloatR4", IRInstruction.CompareFloatR4 },
			{ "CompareFloatR8", IRInstruction.CompareFloatR8 },
			{ "CompareInt32x32", IRInstruction.CompareInt32x32 },
			{ "CompareInt64x32", IRInstruction.CompareInt64x32 },
			{ "CompareInt64x64", IRInstruction.CompareInt64x64 },
			{ "CompareIntBranch32", IRInstruction.CompareIntBranch32 },
			{ "CompareIntBranch64", IRInstruction.CompareIntBranch64 },
			{ "ConvertFloatR4ToFloatR8", IRInstruction.ConvertFloatR4ToFloatR8 },
			{ "ConvertFloatR4ToInt32", IRInstruction.ConvertFloatR4ToInt32 },
			{ "ConvertFloatR4ToInt64", IRInstruction.ConvertFloatR4ToInt64 },
			{ "ConvertFloatR8ToFloatR4", IRInstruction.ConvertFloatR8ToFloatR4 },
			{ "ConvertFloatR8ToInt32", IRInstruction.ConvertFloatR8ToInt32 },
			{ "ConvertFloatR8ToInt64", IRInstruction.ConvertFloatR8ToInt64 },
			{ "ConvertInt32ToFloatR4", IRInstruction.ConvertInt32ToFloatR4 },
			{ "ConvertInt64ToFloatR4", IRInstruction.ConvertInt64ToFloatR4 },
			{ "ConvertInt32ToFloatR8", IRInstruction.ConvertInt32ToFloatR8 },
			{ "ConvertInt64ToFloatR8", IRInstruction.ConvertInt64ToFloatR8 },
			{ "DivFloatR4", IRInstruction.DivFloatR4 },
			{ "DivFloatR8", IRInstruction.DivFloatR8 },
			{ "DivSigned32", IRInstruction.DivSigned32 },
			{ "DivSigned64", IRInstruction.DivSigned64 },
			{ "DivUnsigned32", IRInstruction.DivUnsigned32 },
			{ "DivUnsigned64", IRInstruction.DivUnsigned64 },
			{ "Epilogue", IRInstruction.Epilogue },
			{ "ExceptionEnd", IRInstruction.ExceptionEnd },
			{ "ExceptionStart", IRInstruction.ExceptionStart },
			{ "FilterEnd", IRInstruction.FilterEnd },
			{ "FilterStart", IRInstruction.FilterStart },
			{ "FinallyEnd", IRInstruction.FinallyEnd },
			{ "FinallyStart", IRInstruction.FinallyStart },
			{ "Flow", IRInstruction.Flow },
			{ "Gen", IRInstruction.Gen },
			{ "GotoLeaveTarget", IRInstruction.GotoLeaveTarget },
			{ "IntrinsicMethodCall", IRInstruction.IntrinsicMethodCall },
			{ "IsInstanceOfType", IRInstruction.IsInstanceOfType },
			{ "IsInstanceOfInterfaceType", IRInstruction.IsInstanceOfInterfaceType },
			{ "Jmp", IRInstruction.Jmp },
			{ "Kill", IRInstruction.Kill },
			{ "KillAll", IRInstruction.KillAll },
			{ "KillAllExcept", IRInstruction.KillAllExcept },
			{ "LoadConstant32", IRInstruction.LoadConstant32 },
			{ "LoadConstant64", IRInstruction.LoadConstant64 },
			{ "LoadCompound", IRInstruction.LoadCompound },
			{ "LoadFloatR4", IRInstruction.LoadFloatR4 },
			{ "LoadFloatR8", IRInstruction.LoadFloatR8 },
			{ "LoadInt32", IRInstruction.LoadInt32 },
			{ "LoadInt64", IRInstruction.LoadInt64 },
			{ "LoadSignExtend8x32", IRInstruction.LoadSignExtend8x32 },
			{ "LoadSignExtend16x32", IRInstruction.LoadSignExtend16x32 },
			{ "LoadSignExtend8x64", IRInstruction.LoadSignExtend8x64 },
			{ "LoadSignExtend16x64", IRInstruction.LoadSignExtend16x64 },
			{ "LoadSignExtend32x64", IRInstruction.LoadSignExtend32x64 },
			{ "LoadZeroExtend8x32", IRInstruction.LoadZeroExtend8x32 },
			{ "LoadZeroExtend16x32", IRInstruction.LoadZeroExtend16x32 },
			{ "LoadZeroExtend8x64", IRInstruction.LoadZeroExtend8x64 },
			{ "LoadZeroExtend16x64", IRInstruction.LoadZeroExtend16x64 },
			{ "LoadZeroExtend32x64", IRInstruction.LoadZeroExtend32x64 },
			{ "LoadParamCompound", IRInstruction.LoadParamCompound },
			{ "LoadParamFloatR4", IRInstruction.LoadParamFloatR4 },
			{ "LoadParamFloatR8", IRInstruction.LoadParamFloatR8 },
			{ "LoadParamInt32", IRInstruction.LoadParamInt32 },
			{ "LoadParamInt64", IRInstruction.LoadParamInt64 },
			{ "LoadParamSignExtend8x32", IRInstruction.LoadParamSignExtend8x32 },
			{ "LoadParamSignExtend16x32", IRInstruction.LoadParamSignExtend16x32 },
			{ "LoadParamSignExtend8x64", IRInstruction.LoadParamSignExtend8x64 },
			{ "LoadParamSignExtend16x64", IRInstruction.LoadParamSignExtend16x64 },
			{ "LoadParamSignExtend32x64", IRInstruction.LoadParamSignExtend32x64 },
			{ "LoadParamZeroExtend8x32", IRInstruction.LoadParamZeroExtend8x32 },
			{ "LoadParamZeroExtend16x32", IRInstruction.LoadParamZeroExtend16x32 },
			{ "LoadParamZeroExtend8x64", IRInstruction.LoadParamZeroExtend8x64 },
			{ "LoadParamZeroExtend16x64", IRInstruction.LoadParamZeroExtend16x64 },
			{ "LoadParamZeroExtend32x64", IRInstruction.LoadParamZeroExtend32x64 },
			{ "LogicalAnd32", IRInstruction.LogicalAnd32 },
			{ "LogicalAnd64", IRInstruction.LogicalAnd64 },
			{ "LogicalNot32", IRInstruction.LogicalNot32 },
			{ "LogicalNot64", IRInstruction.LogicalNot64 },
			{ "LogicalOr32", IRInstruction.LogicalOr32 },
			{ "LogicalOr64", IRInstruction.LogicalOr64 },
			{ "LogicalXor32", IRInstruction.LogicalXor32 },
			{ "LogicalXor64", IRInstruction.LogicalXor64 },
			{ "MemorySet", IRInstruction.MemorySet },
			{ "MoveCompound", IRInstruction.MoveCompound },
			{ "MoveFloatR4", IRInstruction.MoveFloatR4 },
			{ "MoveFloatR8", IRInstruction.MoveFloatR8 },
			{ "SignExtend8x32", IRInstruction.SignExtend8x32 },
			{ "SignExtend16x32", IRInstruction.SignExtend16x32 },
			{ "SignExtend8x64", IRInstruction.SignExtend8x64 },
			{ "SignExtend16x64", IRInstruction.SignExtend16x64 },
			{ "SignExtend32x64", IRInstruction.SignExtend32x64 },
			{ "ZeroExtend8x32", IRInstruction.ZeroExtend8x32 },
			{ "ZeroExtend16x32", IRInstruction.ZeroExtend16x32 },
			{ "ZeroExtend8x64", IRInstruction.ZeroExtend8x64 },
			{ "ZeroExtend16x64", IRInstruction.ZeroExtend16x64 },
			{ "ZeroExtend32x64", IRInstruction.ZeroExtend32x64 },
			{ "MoveInt32", IRInstruction.MoveInt32 },
			{ "MoveInt64", IRInstruction.MoveInt64 },
			{ "MulFloatR4", IRInstruction.MulFloatR4 },
			{ "MulFloatR8", IRInstruction.MulFloatR8 },
			{ "MulSigned32", IRInstruction.MulSigned32 },
			{ "MulSigned64", IRInstruction.MulSigned64 },
			{ "MulUnsigned64", IRInstruction.MulUnsigned64 },
			{ "MulUnsigned32", IRInstruction.MulUnsigned32 },
			{ "NewArray", IRInstruction.NewArray },
			{ "NewObject", IRInstruction.NewObject },
			{ "NewString", IRInstruction.NewString },
			{ "Nop", IRInstruction.Nop },
			{ "Phi", IRInstruction.Phi },
			{ "Prologue", IRInstruction.Prologue },
			{ "RemFloatR4", IRInstruction.RemFloatR4 },
			{ "RemFloatR8", IRInstruction.RemFloatR8 },
			{ "RemSigned32", IRInstruction.RemSigned32 },
			{ "RemSigned64", IRInstruction.RemSigned64 },
			{ "RemUnsigned32", IRInstruction.RemUnsigned32 },
			{ "RemUnsigned64", IRInstruction.RemUnsigned64 },
			{ "SetReturnR4", IRInstruction.SetReturnR4 },
			{ "SetReturnR8", IRInstruction.SetReturnR8 },
			{ "SetReturn32", IRInstruction.SetReturn32 },
			{ "SetReturn64", IRInstruction.SetReturn64 },
			{ "SetReturnCompound", IRInstruction.SetReturnCompound },
			{ "SetLeaveTarget", IRInstruction.SetLeaveTarget },
			{ "ShiftLeft32", IRInstruction.ShiftLeft32 },
			{ "ShiftLeft64", IRInstruction.ShiftLeft64 },
			{ "ShiftRight32", IRInstruction.ShiftRight32 },
			{ "ShiftRight64", IRInstruction.ShiftRight64 },
			{ "StableObjectTracking", IRInstruction.StableObjectTracking },
			{ "StoreCompound", IRInstruction.StoreCompound },
			{ "StoreFloatR4", IRInstruction.StoreFloatR4 },
			{ "StoreFloatR8", IRInstruction.StoreFloatR8 },
			{ "StoreInt8", IRInstruction.StoreInt8 },
			{ "StoreInt16", IRInstruction.StoreInt16 },
			{ "StoreInt32", IRInstruction.StoreInt32 },
			{ "StoreInt64", IRInstruction.StoreInt64 },
			{ "StoreParamCompound", IRInstruction.StoreParamCompound },
			{ "StoreParamFloatR4", IRInstruction.StoreParamFloatR4 },
			{ "StoreParamFloatR8", IRInstruction.StoreParamFloatR8 },
			{ "StoreParamInt8", IRInstruction.StoreParamInt8 },
			{ "StoreParamInt16", IRInstruction.StoreParamInt16 },
			{ "StoreParamInt32", IRInstruction.StoreParamInt32 },
			{ "StoreParamInt64", IRInstruction.StoreParamInt64 },
			{ "SubFloatR4", IRInstruction.SubFloatR4 },
			{ "SubFloatR8", IRInstruction.SubFloatR8 },
			{ "Sub32", IRInstruction.Sub32 },
			{ "Sub64", IRInstruction.Sub64 },
			{ "SubCarryOut32", IRInstruction.SubCarryOut32 },
			{ "SubWithCarry32", IRInstruction.SubWithCarry32 },
			{ "Switch", IRInstruction.Switch },
			{ "Throw", IRInstruction.Throw },
			{ "Truncation64x32", IRInstruction.Truncation64x32 },
			{ "TryEnd", IRInstruction.TryEnd },
			{ "TryStart", IRInstruction.TryStart },
			{ "UnstableObjectTracking", IRInstruction.UnstableObjectTracking },
			{ "Rethrow", IRInstruction.Rethrow },
			{ "GetVirtualFunctionPtr", IRInstruction.GetVirtualFunctionPtr },
			{ "MemoryCopy", IRInstruction.MemoryCopy },
			{ "Box", IRInstruction.Box },
			{ "Box32", IRInstruction.Box32 },
			{ "Box64", IRInstruction.Box64 },
			{ "BoxR4", IRInstruction.BoxR4 },
			{ "BoxR8", IRInstruction.BoxR8 },
			{ "Unbox", IRInstruction.Unbox },
			{ "Unbox32", IRInstruction.Unbox32 },
			{ "Unbox64", IRInstruction.Unbox64 },
			{ "To64", IRInstruction.To64 },
			{ "GetLow64", IRInstruction.GetLow64 },
			{ "GetHigh64", IRInstruction.GetHigh64 },
			{ "IfThenElse32", IRInstruction.IfThenElse32 },
			{ "IfThenElse64", IRInstruction.IfThenElse64 },
		};
	}
}
