// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mosa.Workspace.Experiment.Debug
{
	internal static class Program
	{
		private static readonly List<BaseIRInstruction> IRInstructions = new List<BaseIRInstruction>()
		{
			IRInstruction.AddFloatR4,
			IRInstruction.AddFloatR8,
			IRInstruction.AddressOf,
			IRInstruction.AddSigned,
			IRInstruction.AddUnsigned,
			IRInstruction.ArithmeticShiftRight,
			IRInstruction.BlockEnd,
			IRInstruction.BlockStart,
			IRInstruction.Break,
			IRInstruction.Call,
			IRInstruction.CallDirect,
			IRInstruction.CallDynamic,
			IRInstruction.CallInterface,
			IRInstruction.CallStatic,
			IRInstruction.CallVirtual,
			IRInstruction.CompareFloatR4,
			IRInstruction.CompareFloatR8,
			IRInstruction.CompareInteger,
			IRInstruction.CompareIntegerBranch,
			IRInstruction.ConversionFloatR4ToFloatR8,
			IRInstruction.ConversionFloatR4ToInteger,
			IRInstruction.ConversionFloatR8ToFloatR4,
			IRInstruction.ConversionFloatR8ToInteger,
			IRInstruction.ConversionIntegerToFloatR4,
			IRInstruction.ConversionIntegerToFloatR8,
			IRInstruction.DivFloatR4,
			IRInstruction.DivFloatR8,
			IRInstruction.DivSigned,
			IRInstruction.DivUnsigned,
			IRInstruction.Epilogue,
			IRInstruction.ExceptionEnd,
			IRInstruction.ExceptionStart,
			IRInstruction.FilterEnd,
			IRInstruction.FilterStart,
			IRInstruction.FinallyEnd,
			IRInstruction.FinallyStart,
			IRInstruction.Flow,
			IRInstruction.Gen,
			IRInstruction.GotoLeaveTarget,
			IRInstruction.IntrinsicMethodCall,
			IRInstruction.IsInstanceOfType,
			IRInstruction.IsInstanceOfInterfaceType,
			IRInstruction.Jmp,
			IRInstruction.Kill,
			IRInstruction.KillAll,
			IRInstruction.KillAllExcept,
			IRInstruction.LoadCompound,
			IRInstruction.LoadFloatR4,
			IRInstruction.LoadFloatR8,
			IRInstruction.LoadInteger,
			IRInstruction.LoadSignExtended,
			IRInstruction.LoadZeroExtended,
			IRInstruction.LoadParameterCompound,
			IRInstruction.LoadParameterFloatR4,
			IRInstruction.LoadParameterFloatR8,
			IRInstruction.LoadParameterInteger,
			IRInstruction.LoadParameterSignExtended,
			IRInstruction.LoadParameterZeroExtended,
			IRInstruction.LogicalAnd,
			IRInstruction.LogicalNot,
			IRInstruction.LogicalOr,
			IRInstruction.LogicalXor,
			IRInstruction.MemorySet,
			IRInstruction.MoveCompound,
			IRInstruction.MoveFloatR4,
			IRInstruction.MoveFloatR8,
			IRInstruction.MoveInteger,
			IRInstruction.MoveSignExtended,
			IRInstruction.MoveZeroExtended,
			IRInstruction.MulFloatR4,
			IRInstruction.MulFloatR8,
			IRInstruction.MulSigned,
			IRInstruction.MulUnsigned,
			IRInstruction.NewArray,
			IRInstruction.NewObject,
			IRInstruction.NewString,
			IRInstruction.Nop,
			IRInstruction.Phi,
			IRInstruction.Prologue,
			IRInstruction.RemFloatR4,
			IRInstruction.RemFloatR8,
			IRInstruction.RemSigned,
			IRInstruction.RemUnsigned,
			IRInstruction.SetReturn,
			IRInstruction.SetLeaveTarget,
			IRInstruction.ShiftLeft,
			IRInstruction.ShiftRight,
			IRInstruction.StableObjectTracking,
			IRInstruction.StoreCompound,
			IRInstruction.StoreFloatR4,
			IRInstruction.StoreFloatR8,
			IRInstruction.StoreInteger,
			IRInstruction.StoreParameterCompound,
			IRInstruction.StoreParameterFloatR4,
			IRInstruction.StoreParameterFloatR8,
			IRInstruction.StoreParameterInteger,
			IRInstruction.SubFloatR4,
			IRInstruction.SubFloatR8,
			IRInstruction.SubSigned,
			IRInstruction.SubUnsigned,
			IRInstruction.Switch,
			IRInstruction.Throw,
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
			IRInstruction.Unbox32,
			IRInstruction.Unbox64,
			IRInstruction.To64,
			IRInstruction.Split64
		};

		private static readonly List<BaseIRInstruction> Commutative = new List<BaseIRInstruction>()
		{
			IRInstruction.AddFloatR4,
			IRInstruction.AddFloatR8,
			IRInstruction.AddSigned,
			IRInstruction.AddUnsigned,
			IRInstruction.LogicalAnd,
			IRInstruction.LogicalNot,
			IRInstruction.LogicalOr,
			IRInstruction.LogicalXor,
			IRInstruction.MulFloatR4,
			IRInstruction.MulFloatR8,
			IRInstruction.MulSigned,
			IRInstruction.MulUnsigned,
		};

		private static readonly List<BaseIRInstruction> SideEffects = new List<BaseIRInstruction>()
		{
			IRInstruction.Call,
			IRInstruction.CallDirect,
			IRInstruction.CallDynamic,
			IRInstruction.CallInterface,
			IRInstruction.CallStatic,
			IRInstruction.CallVirtual,
			IRInstruction.MemoryCopy,
			IRInstruction.StableObjectTracking,
			IRInstruction.UnstableObjectTracking,
			IRInstruction.StoreCompound,
			IRInstruction.StoreFloatR4,
			IRInstruction.StoreFloatR8,
			IRInstruction.StoreInteger,
			IRInstruction.StoreParameterCompound,
			IRInstruction.StoreParameterFloatR4,
			IRInstruction.StoreParameterFloatR8,
			IRInstruction.StoreParameterInteger,
			IRInstruction.StoreCompound,
			IRInstruction.StoreFloatR4,
			IRInstruction.StoreFloatR8,
			IRInstruction.StoreInteger,
			IRInstruction.IntrinsicMethodCall
		};

		private static readonly List<BaseIRInstruction> VariableOperand = new List<BaseIRInstruction>()
		{
			IRInstruction.Call,
			IRInstruction.CallDirect,
			IRInstruction.CallDynamic,
			IRInstruction.CallInterface,
			IRInstruction.CallStatic,
			IRInstruction.CallVirtual,
			IRInstruction.Phi,
			IRInstruction.Kill,
			IRInstruction.KillAll,
			IRInstruction.KillAllExcept,
			IRInstruction.SetReturn,
			IRInstruction.Gen,
			IRInstruction.IntrinsicMethodCall,
		};

		private static void Main()
		{
			var sb = new StringBuilder();

			const string template = "\"name\": \"{0}\",\n\"familyName\": \"{1}\",\n\"resultCount\": {2},\n\"operandCount\": {3},\n\"flowControl\": \"{4}\",\n\"ignoreDuringCodeGeneration\": \"{5}\",\n\"ignoreInstructionBasicBlockTargets\": \"{6}\",\n\"variableOperandCount\": \"{7}\",\n\"commutative\": \"{8}\",\n\"hasSideEffect\": \"{9}\",\n\"description\": \"{10}\"\n";

			sb.AppendLine("{ \"instructions\": [");

			foreach (var instruction in IRInstructions)
			{
				sb.Append("{ ");
				sb.AppendFormat(template,
					instruction.BaseInstructionName, // 0
					instruction.InstructionFamilyName, // 1
					instruction.DefaultResultCount.ToString(), // 2
					instruction.DefaultOperandCount.ToString(), // 3
					instruction.FlowControl.ToString(), // 4
					instruction.IgnoreDuringCodeGeneration ? "true" : "false", // 5
					instruction.IgnoreInstructionBasicBlockTargets ? "true" : "false", // 6
					VariableOperand.Contains(instruction) ? "true" : "false", // 8
					Commutative.Contains(instruction) ? "true" : "false", // 8
					SideEffects.Contains(instruction) ? "true" : "false", // 9
					string.Empty); //10

				sb.AppendLine("} ,");
			}

			sb.AppendLine("] }");

			File.WriteAllText("instructions.txt", sb.ToString());

			return;
		}
	}
}
