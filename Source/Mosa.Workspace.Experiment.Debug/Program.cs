// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Expression;
using Mosa.Compiler.Framework.IR;
using Mosa.Platform.x86;
using System.IO;
using System.Text;

namespace Mosa.Workspace.Experiment.Debug
{
	internal static class Program
	{
		private static void Main()
		{
			var tree = ExpressionTest.GetTestExpression2();
			var basicBlocks = ExpressionTest.CreateBasicBlockInstructionSet();

			//var match = tree.Transform(basicBlocks[0].Last.Previous, null);

			//ExpressionTest.GetTestExpression5();
			//ExpressionTest.GetTestExpression4();
			//ExpressionTest.GetTestExpression3();
			//ExpressionTest.GetTestExpression2();

			DumpIRInstruction();
			DumpX86Instruction();

			return;
		}

		private static void DumpIRInstruction()
		{
			var sb = new StringBuilder();

			sb.AppendLine("{ \"Instructions\": [");

			foreach (var instruction in IRInstructionMap.Map.Values)
			{
				var inst = new Instruction()
				{
					Name = instruction.GetType().Name,
					FamilyName = instruction.FamilyName,
					ResultCount = instruction.DefaultResultCount,
					OperandCount = instruction.DefaultOperandCount,
					ResultType = instruction.ResultType.ToString(),
					ResultType2 = instruction.ResultType2.ToString(),
					FlowControl = instruction.FlowControl.ToString(),
					IgnoreDuringCodeGeneration = instruction.IgnoreDuringCodeGeneration,
					IgnoreInstructionBasicBlockTargets = instruction.IgnoreInstructionBasicBlockTargets,
					VariableOperands = instruction.VariableOperands,
					Commutative = instruction.IsCommutative,
					MemoryWrite = instruction.IsMemoryWrite,
					MemoryRead = instruction.IsMemoryRead,
					IOOperation = instruction.IsIOOperation,
					UnspecifiedSideEffect = instruction.HasIRUnspecifiedSideEffect,
				};

				sb.Append(inst.ToString());
				sb.AppendLine(",");
			}

			sb.Length--;
			sb.Length--;
			sb.Length--;
			sb.AppendLine();
			sb.AppendLine("] }");

			File.WriteAllText("IRInstructions.json", sb.ToString());
		}

		private static void DumpX86Instruction()
		{
			var sb = new StringBuilder();

			sb.AppendLine("{ \"Instructions\": [");

			foreach (var instruction in X86InstructionMap.Map.Values) // replaces X86Instructions
			{
				var inst = new Instruction()
				{
					Name = instruction.GetType().Name,
					FamilyName = instruction.FamilyName,
					ResultCount = instruction.DefaultResultCount,
					OperandCount = instruction.DefaultOperandCount,
					ResultType = instruction.ResultType.ToString(),
					ResultType2 = instruction.ResultType2.ToString(),
					FlowControl = instruction.FlowControl.ToString(),
					IgnoreDuringCodeGeneration = instruction.IgnoreDuringCodeGeneration,
					IgnoreInstructionBasicBlockTargets = instruction.IgnoreInstructionBasicBlockTargets,
					VariableOperands = false,

					Commutative = instruction.IsCommutative,
					MemoryWrite = instruction.IsMemoryWrite,
					MemoryRead = instruction.IsMemoryRead,
					IOOperation = instruction.IsIOOperation,
					UnspecifiedSideEffect = instruction.HasIRUnspecifiedSideEffect,

					X86ThreeTwoAddressConversion = instruction.ThreeTwoAddressConversion,

					//X86EmitBytes = instruction.__opcode,
					//X86LegacyOpcode = (instruction.__legacyopcode != null) ? instruction.__legacyopcode.Code : null,
					//X86LegacyOpcodeRegField = (instruction.__legacyopcode != null) ? instruction.__legacyopcode.RegField : null,
					//StaticEmitMethod = instruction.__staticEmitMethod,

					//X86EmitMethodType =
					//	instruction.__staticMethodName != null ? "CallInternalMethod" :
					//	GetX86Bytes(instruction) != null ? "SimpleByteCode" :
					//	instruction.__legacyopcode != null ? "LegacyOpcode" :
					//	instruction.__opcode != null ? "SimpleByteCode" :
					//	null,
				};

				sb.Append(inst.ToString());
				sb.AppendLine(",");
			}

			sb.Length--;
			sb.Length--;
			sb.Length--;
			sb.AppendLine();
			sb.AppendLine("] }");

			File.WriteAllText("X86Instructions.json", sb.ToString());
		}
	}
}
