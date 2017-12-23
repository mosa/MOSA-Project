// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Expression;
using Mosa.Compiler.Framework.IR;
using Mosa.Platform.x86;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mosa.Workspace.Experiment.Debug
{
	internal static class Program
	{
		#region Data

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

		private static readonly List<BaseIRInstruction> IRCommutative = new List<BaseIRInstruction>()
		{
			IRInstruction.AddFloatR4,
			IRInstruction.AddFloatR8,
			IRInstruction.AddSigned,
			IRInstruction.AddUnsigned,
			IRInstruction.LogicalAnd,
			IRInstruction.LogicalOr,
			IRInstruction.LogicalXor,
			IRInstruction.MulFloatR4,
			IRInstruction.MulFloatR8,
			IRInstruction.MulSigned,
			IRInstruction.MulUnsigned,
		};

		private static readonly List<BaseIRInstruction> IRSideEffects = new List<BaseIRInstruction>()
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

		private static readonly List<BaseIRInstruction> IRVariableOperand = new List<BaseIRInstruction>()
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

		private static readonly List<X86Instruction> X86Instructions = new List<X86Instruction>()
		{
			X86.Adc,
			X86.Add,
			X86.Addsd,
			X86.Addss,
			X86.And,
			X86.Branch,
			X86.Break,
			X86.Btr,
			X86.Bts,
			X86.Call,
			X86.Cdq,
			X86.Cld,
			X86.Cli,
			X86.Cmovcc,
			X86.Cmp,
			X86.CmpXchgLoad,
			X86.Comisd,
			X86.Comiss,
			X86.CpuId,
			X86.Cvtsd2ss,
			X86.Cvtsi2sd,
			X86.Cvtsi2ss,
			X86.Cvtss2sd,
			X86.Cvttsd2si,
			X86.Cvttss2si,
			X86.Dec,
			X86.Div,
			X86.Divsd,
			X86.Divss,
			X86.FarJmp,
			X86.Hlt,
			X86.IDiv,
			X86.IMul,
			X86.In,
			X86.Inc,
			X86.Int,
			X86.Invlpg,
			X86.IRetd,
			X86.Jmp,
			X86.Lea,
			X86.Leave,
			X86.Lgdt,
			X86.Lidt,
			X86.Lock,
			X86.Mov,
			X86.Movaps,
			X86.MovapsLoad,
			X86.MovCRLoad,
			X86.MovCRStore,
			X86.Movd,
			X86.MovLoad,
			X86.Movsd,
			X86.MovsdLoad,
			X86.MovsdStore,
			X86.Movss,
			X86.MovssLoad,
			X86.MovssStore,
			X86.MovStore,
			X86.Movsx,
			X86.MovsxLoad,
			X86.Movups,
			X86.MovupsLoad,
			X86.MovupsStore,
			X86.Movzx,
			X86.MovzxLoad,
			X86.Mul,
			X86.Mulsd,
			X86.Mulss,
			X86.Neg,
			X86.Nop,
			X86.Not,
			X86.Or,
			X86.Out,
			X86.Pause,
			X86.Pextrd,
			X86.Pop,
			X86.Popad,
			X86.Push,
			X86.Pushad,
			X86.PXor,
			X86.Rcr,
			X86.Rep,
			X86.Ret,
			X86.Roundsd,
			X86.Roundss,
			X86.Sar,
			X86.Sbb,
			X86.Setcc,
			X86.Shl,
			X86.Shld,
			X86.Shr,
			X86.Shrd,
			X86.Sti,
			X86.Stos,
			X86.Sub,
			X86.Subsd,
			X86.Subss,
			X86.Test,
			X86.Ucomisd,
			X86.Ucomiss,
			X86.Xchg,
			X86.Xor,
		};

		private static readonly List<X86Instruction> X86Commutative = new List<X86Instruction>()
		{
			X86.Add,
			X86.Addsd,
			X86.Addss,
			X86.Adc,
			X86.And,
			X86.Or,
			X86.Xor,
			X86.Mul,
			X86.Mulsd,
			X86.Mulss,
			X86.Xchg,
		};

		private static readonly List<X86Instruction> X86SideEffects = new List<X86Instruction>()
		{
			X86.Call,
			X86.FarJmp,
			X86.Invlpg,
			X86.Int,
			X86.Hlt,
			X86.Out,
			X86.In,
			X86.IRetd,
			X86.Lgdt,
			X86.Leave,
			X86.Break,
			X86.Lidt,
			X86.Lock,
			X86.Rep,
		};

		#endregion Data

		private static void Main()
		{
			var tree = ExpressionTest.GetTestExpression1();
			var basicBlocks = ExpressionTest.CreateBasicBlockInstructionSet();

			var match = tree.Validate(basicBlocks[0].Last.Previous);

			ExpressionTest.GetTestExpression5();
			ExpressionTest.GetTestExpression4();
			ExpressionTest.GetTestExpression3();
			ExpressionTest.GetTestExpression2();

			DumpIRInstruction();
			DumpX86Instruction();

			return;
		}

		private static void DumpIRInstruction()
		{
			var sb = new StringBuilder();

			const string template = "\"name\": \"{0}\",\n\"familyName\": \"{1}\",\n\"resultCount\": {2},\n\"operandCount\": {3},\n\"flowControl\": \"{4}\",\n\"ignoreDuringCodeGeneration\": \"{5}\",\n\"ignoreInstructionBasicBlockTargets\": \"{6}\",\n\"variableOperandCount\": \"{7}\",\n\"commutative\": \"{8}\",\n\"hasSideEffect\": \"{9}\",\n\"description\": \"{10}\"\n";

			sb.AppendLine("{ \"instructions\": [");

			foreach (var instruction in IRInstructions)
			{
				sb.Append("{ ");
				sb.AppendFormat(template,
					instruction.GetType().Name, // 0
					instruction.InstructionFamilyName, // 1
					instruction.DefaultResultCount.ToString(), // 2
					instruction.DefaultOperandCount.ToString(), // 3
					instruction.FlowControl.ToString(), // 4
					instruction.IgnoreDuringCodeGeneration ? "true" : "false", // 5
					instruction.IgnoreInstructionBasicBlockTargets ? "true" : "false", // 6
					IRVariableOperand.Contains(instruction) ? "true" : "false", // 8
					IRCommutative.Contains(instruction) ? "true" : "false", // 8
					IRSideEffects.Contains(instruction) ? "true" : "false", // 9
					string.Empty); //10

				sb.AppendLine("} ,");
			}
			sb.Length--;
			sb.Length--;
			sb.Length--;
			sb.AppendLine("] }");

			File.WriteAllText("IRInstructions.json", sb.ToString());
		}

		private static void DumpX86Instruction()
		{
			var sb = new StringBuilder();

			const string template = "\"name\": \"{0}\",\n\"familyName\": \"{1}\",\n\"resultCount\": {2},\n\"operandCount\": {3},\n\"flowControl\": \"{4}\",\n\"ignoreDuringCodeGeneration\": \"{5}\",\n\"ignoreInstructionBasicBlockTargets\": \"{6}\",\n\"variableOperandCount\": \"{7}\",\n\"commutative\": \"{8}\",\n\"hasSideEffect\": \"{9}\",\n\"description\": \"{10}\"\n";

			sb.AppendLine("{ \"instructions\": [");

			foreach (var instruction in X86Instructions)
			{
				sb.Append("{ ");
				sb.AppendFormat(template,
					instruction.GetType().Name, // 0
					instruction.InstructionFamilyName, // 1
					instruction.DefaultResultCount.ToString(), // 2
					instruction.DefaultOperandCount.ToString(), // 3
					instruction.FlowControl.ToString(), // 4
					instruction.IgnoreDuringCodeGeneration ? "true" : "false", // 5
					instruction.IgnoreInstructionBasicBlockTargets ? "true" : "false", // 6
					"false", // 8
					X86Commutative.Contains(instruction) ? "true" : "false", // 8
					X86SideEffects.Contains(instruction) ? "true" : "false", // 9
					string.Empty); //10

				sb.AppendLine("} ,");
			}
			sb.Length--;
			sb.Length--;
			sb.Length--;
			sb.AppendLine("] }");

			File.WriteAllText("X86Instructions.json", sb.ToString());
		}
	}
}
