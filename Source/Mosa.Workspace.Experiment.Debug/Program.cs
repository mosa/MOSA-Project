// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
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

		private static readonly List<BaseIRInstruction> IRCalls = new List<BaseIRInstruction>()
		{
			IRInstruction.Call,
			IRInstruction.CallDirect,
			IRInstruction.CallDynamic,
			IRInstruction.CallInterface,
			IRInstruction.CallStatic,
			IRInstruction.CallVirtual,
			IRInstruction.IntrinsicMethodCall
		};

		private static readonly List<BaseIRInstruction> IRWriteOperation = new List<BaseIRInstruction>()
		{
			IRInstruction.MemoryCopy,
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
		};

		private static readonly List<BaseIRInstruction> IRReadOperation = new List<BaseIRInstruction>()
		{
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
		};

		private static readonly List<BaseIRInstruction> IRIOOperation = new List<BaseIRInstruction>()
		{
		};

		private static readonly List<BaseIRInstruction> IRUnspecifiedSideEffect = new List<BaseIRInstruction>()
		{
			IRInstruction.StableObjectTracking,
			IRInstruction.UnstableObjectTracking,
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

		//private static readonly List<X86Instruction> X86Instructions = new List<X86Instruction>()
		//{
		//	X86.Adc32,
		//	X86.Add,
		//	X86.Addsd,
		//	X86.Addss,
		//	X86.And,
		//	X86.Branch,
		//	X86.Break,
		//	X86.Btr,
		//	X86.Bts,
		//	X86.Call,
		//	X86.Cdq,
		//	X86.Cld,
		//	X86.Cli,
		//	X86.Cmovcc,
		//	X86.Cmp,
		//	X86.CmpXchgLoad,
		//	X86.Comisd,
		//	X86.Comiss,
		//	X86.CpuId,
		//	X86.Cvtsd2ss,
		//	X86.Cvtsi2sd,
		//	X86.Cvtsi2ss,
		//	X86.Cvtss2sd,
		//	X86.Cvttsd2si,
		//	X86.Cvttss2si,
		//	X86.Dec,
		//	X86.Div,
		//	X86.Divsd,
		//	X86.Divss,
		//	X86.FarJmp,
		//	X86.Hlt,
		//	X86.IDiv,
		//	X86.IMul,
		//	X86.In,
		//	X86.Inc,
		//	X86.Int,
		//	X86.Invlpg,
		//	X86.IRetd,
		//	X86.Jmp,
		//	X86.Lea,
		//	X86.Leave,
		//	X86.Lgdt,
		//	X86.Lidt,
		//	X86.Lock,
		//	X86.Mov,
		//	X86.Movaps,
		//	X86.MovapsLoad,
		//	X86.MovCRLoad,
		//	X86.MovCRStore,
		//	X86.Movd,
		//	X86.MovLoad,
		//	X86.Movsd,
		//	X86.MovsdLoad,
		//	X86.MovsdStore,
		//	X86.Movss,
		//	X86.MovssLoad,
		//	X86.MovssStore,
		//	X86.MovStore,
		//	X86.Movsx,
		//	X86.MovsxLoad,
		//	X86.Movups,
		//	X86.MovupsLoad,
		//	X86.MovupsStore,
		//	X86.Movzx,
		//	X86.MovzxLoad,
		//	X86.Mul,
		//	X86.Mulsd,
		//	X86.Mulss,
		//	X86.Neg,
		//	X86.Nop,
		//	X86.Not,
		//	X86.Or,
		//	X86.Out,
		//	X86.Pause,
		//	X86.Pextrd,
		//	X86.Pop,
		//	X86.Popad,
		//	X86.Push,
		//	X86.Pushad,
		//	X86.PXor,
		//	X86.Rcr,
		//	X86.Rep,
		//	X86.Ret,
		//	X86.Roundsd,
		//	X86.Roundss,
		//	X86.Sar,
		//	X86.Sbb,
		//	X86.Setcc,
		//	X86.Shl,
		//	X86.Shld,
		//	X86.Shr,
		//	X86.Shrd,
		//	X86.Sti,
		//	X86.Stos,
		//	X86.Sub,
		//	X86.Subsd,
		//	X86.Subss,
		//	X86.Test,
		//	X86.Ucomisd,
		//	X86.Ucomiss,
		//	X86.Xchg,
		//	X86.Xor,
		//};

		private static readonly List<X86Instruction> X86Commutative = new List<X86Instruction>()
		{
			//X86.Add,
			//X86.Addsd,
			//X86.Addss,
			//X86.Adc32,
			//X86.And,
			//X86.Or,

			//X86.Xor,
			//X86.Mul,
			X86.Mulsd,
			X86.Mulss,

			//X86.Xchg,
		};

		private static readonly List<X86Instruction> X86WriteOperation = new List<X86Instruction>()
		{
			X86.MovCRStore,
			X86.MovsdStore,
			X86.MovssStore,

			//X86.MovStore,
			X86.MovupsStore,
		};

		private static readonly List<X86Instruction> X86ReadOperation = new List<X86Instruction>()
		{
			X86.MovapsLoad,
			X86.MovCRLoad,

			//X86.MovLoad,
			X86.MovsdLoad,
			X86.MovssLoad,

			//X86.MovsxLoad,
			X86.MovupsLoad,

			//X86.MovzxLoad,
		};

		private static readonly List<X86Instruction> X86IOOperation = new List<X86Instruction>()
		{
			//X86.In,
			//X86.Out,
		};

		private static readonly List<X86Instruction> X86UnspecifiedSideEffect = new List<X86Instruction>()
		{
			//X86.Call,
			X86.FarJmp,
			X86.Invlpg,
			X86.Int,
			X86.Hlt,

			//X86.Out,
			//X86.In,
			X86.IRetd,
			X86.Lgdt,
			X86.Leave,
			X86.Break,
			X86.Lidt,
			X86.Lock,
			X86.Rep,
			X86.Cli,
			X86.Sti,
		};

		private static readonly Dictionary<BaseInstruction, string> ResultType = new Dictionary<BaseInstruction, string>()
		{
			{ IRInstruction.CompareInteger, "Boolean" },
			{ IRInstruction.CompareFloatR4, "Boolean" },
			{ IRInstruction.CompareFloatR8, "Boolean" },
			{ IRInstruction.Split64, "UInt32" },
			{ IRInstruction.To64, "UInt64" },
		};

		private static readonly Dictionary<BaseInstruction, string> ResultType2 = new Dictionary<BaseInstruction, string>()
		{
			{ IRInstruction.Split64, "UInt32" },
		};

		private static readonly Dictionary<BaseInstruction, byte[]> X86Bytes = new Dictionary<BaseInstruction, byte[]>()
		{
			{ X86.Break, new byte[] { 0xCC } },
			{ X86.Pushad, new byte[] { 0x60 } },
			{ X86.Popad, new byte[] { 0x61 } },
			{ X86.Nop, new byte[] { 0x90 } },
			{ X86.Pause, new byte[] { 0xF3, 0x90 } },
			{ X86.Cdq, new byte[] { 0x99 } },
			{ X86.Stos, new byte[] { 0xAB } },
			{ X86.Ret, new byte[] { 0xC3 } },
			{ X86.Leave, new byte[] { 0xC9 } },
			{ X86.IRetd, new byte[] { 0xCF } },
			{ X86.Lock, new byte[] { 0xF0 } },
			{ X86.Rep, new byte[] { 0xF3 } },
			{ X86.Hlt, new byte[] { 0xF4 } },
			{ X86.Cli, new byte[] { 0xFA } },
			{ X86.Sti, new byte[] { 0xFB } },
		};

		#endregion Data

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

		private static string GetResultType(BaseInstruction instruction)
		{
			string value = null;

			ResultType.TryGetValue(instruction, out value);

			return value;
		}

		private static string GetResultType2(BaseInstruction instruction)
		{
			string value = null;

			ResultType2.TryGetValue(instruction, out value);

			return value;
		}

		private static byte[] GetX86Bytes(BaseInstruction instruction)
		{
			X86Bytes.TryGetValue(instruction, out byte[] value);

			return value;
		}

		private static void DumpIRInstruction()
		{
			var sb = new StringBuilder();

			sb.AppendLine("{ \"Instructions\": [");

			foreach (var instruction in IRInstructions)
			{
				var inst = new Instruction()
				{
					Name = instruction.GetType().Name,
					FamilyName = instruction.FamilyName,
					ResultCount = instruction.DefaultResultCount,
					OperandCount = instruction.DefaultOperandCount,
					ResultType = GetResultType(instruction) ?? instruction.ResultType.ToString(),
					ResultType2 = GetResultType2(instruction) ?? instruction.ResultType2.ToString(),
					FlowControl = instruction.FlowControl.ToString(),
					IgnoreDuringCodeGeneration = instruction.IgnoreDuringCodeGeneration,
					IgnoreInstructionBasicBlockTargets = instruction.IgnoreInstructionBasicBlockTargets,
					VariableOperands = instruction.VariableOperands, //IRVariableOperand.Contains(instruction),
					Commutative = instruction.IsCommutative,// IRCommutative.Contains(instruction),

					MemoryWrite = IRWriteOperation.Contains(instruction),
					MemoryRead = IRReadOperation.Contains(instruction),
					IOOperation = IRIOOperation.Contains(instruction),
					UnspecifiedSideEffect = IRUnspecifiedSideEffect.Contains(instruction),
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
					ResultType = GetResultType(instruction) ?? instruction.ResultType.ToString(),
					ResultType2 = GetResultType2(instruction) ?? instruction.ResultType2.ToString(),
					FlowControl = instruction.FlowControl.ToString(),
					IgnoreDuringCodeGeneration = instruction.IgnoreDuringCodeGeneration,
					IgnoreInstructionBasicBlockTargets = instruction.IgnoreInstructionBasicBlockTargets,
					VariableOperands = false,

					//Commutative = instruction.IsCommutative,
					//MemoryWrite = instruction.IsMemoryWrite,
					//MemoryRead = instruction.IsMemoryRead,
					//IOOperation = instruction.IsIOOperation,
					//UnspecifiedSideEffect = instruction.HasIRUnspecifiedSideEffect,

					Commutative = instruction.IsCommutative || X86Commutative.Contains(instruction),
					MemoryWrite = instruction.IsMemoryWrite || X86WriteOperation.Contains(instruction),
					MemoryRead = instruction.IsMemoryRead || X86ReadOperation.Contains(instruction),
					IOOperation = instruction.IsIOOperation || X86IOOperation.Contains(instruction),
					UnspecifiedSideEffect = instruction.HasIRUnspecifiedSideEffect || X86UnspecifiedSideEffect.Contains(instruction),

					X86ThreeTwoAddressConversion = instruction.ThreeTwoAddressConversion,
					X86EmitBytes = instruction.__opcode,
					X86LegacyOpcode = (instruction.__legacyopcode != null) ? instruction.__legacyopcode.Code : null,
					X86LegacyOpcodeRegField = (instruction.__legacyopcode != null) ? instruction.__legacyopcode.RegField : null,
					StaticEmitMethod = instruction.__staticEmitMethod,

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
