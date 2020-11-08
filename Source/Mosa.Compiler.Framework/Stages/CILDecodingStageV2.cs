// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Represents the CIL decoding compilation stage.
	/// </summary>
	/// <remarks>
	/// The CIL decoding stage takes a stream of bytes and decodes the instructions represented into an MSIL based intermediate
	/// representation. The instructions are grouped into basic Blocks for easier local optimizations in later compiler stages.
	/// </remarks>
	public sealed class CILDecodingStageV2 : BaseMethodCompilerStage
	{
		#region Stack classes

		private enum StackType { Int32, Int64, R4, R8, ManagedPointer, Object, ValueType };

		private class StackEntry
		{
			public Operand Operand;
			public StackType StackType;
			public MosaType Type;   // for ValueTypes

			public StackEntry(StackType stackType, Operand operand)
			{
				StackType = stackType;
				Operand = operand;
			}

			public StackEntry(StackType stackType, Operand operand, MosaType type)
			{
				StackType = stackType;
				Operand = operand;
				Type = type;
			}
		}

		#endregion Stack classes

		private readonly Dictionary<BasicBlock, Stack<StackEntry>> stacks = new Dictionary<BasicBlock, Stack<StackEntry>>();

		private Operand[] LocalStack;
		private StackType[] LocalStackType;

		protected override void Finish()
		{
			MethodCompiler.Stop();
		}

		protected override void Run()
		{
			if (!MethodCompiler.IsCILStream)
				return;

			// Create the prologue block
			var prologue = CreateNewBlock(BasicBlock.PrologueLabel);
			BasicBlocks.AddHeadBlock(prologue);

			var jmpNode = new InstructionNode()
			{
				Label = BasicBlock.PrologueLabel,
				Block = prologue
			};
			prologue.First.Insert(jmpNode);

			var startBlock = CreateNewBlock(0);

			jmpNode.SetInstruction(IRInstruction.Jmp, startBlock);

			CreateBasicBlocks();

			CreateHandlersBlocks();

			CreateLocalVariables();

			InitializeLocalVariables();

			CreateInstructions();
		}

		protected override void Setup()
		{
		}

		private void CreateBasicBlocks()
		{
			for (int index = 0; index < Method.Code.Count; index++)
			{
				var instruction = Method.Code[index];

				var opcode = (OpCode)instruction.OpCode;

				if (opcode == OpCode.Br || opcode == OpCode.Br_s)
				{
					GetOrCreateBlock((int)instruction.Operand);
				}
				else if (IsBranch(opcode))
				{
					GetOrCreateBlock((int)instruction.Operand);
					GetOrCreateBlock(instruction.Offset + 1);
				}
				else if (opcode == OpCode.Switch)
				{
					foreach (var target in (int[])instruction.Operand)
					{
						GetOrCreateBlock((int)instruction.Operand);
					}

					GetOrCreateBlock(instruction.Offset + 1);
				}
			}
		}

		private void CreateHandlersBlocks()
		{
			foreach (var clause in Method.ExceptionHandlers)
			{
				if (clause.TryStart != 0)
				{
					GetOrCreateBlock(clause.TryStart);
				}

				if (clause.TryEnd != 0)
				{
					GetOrCreateBlock(clause.TryEnd);
				}

				if (clause.HandlerStart != 0)
				{
					var block = GetOrCreateBlock(clause.HandlerStart);
					BasicBlocks.AddHeadBlock(block);
					BasicBlocks.AddHandlerHeadBlock(block);
				}

				if (clause.FilterStart != null)
				{
					var block = GetOrCreateBlock(clause.FilterStart.Value);
					BasicBlocks.AddHeadBlock(block);
					BasicBlocks.AddHandlerHeadBlock(block);
				}

				if (clause.ExceptionHandlerType == ExceptionHandlerType.Exception)
				{
					var handler = GetOrCreateBlock(clause.HandlerStart);
					var context = new Context(handler);
					var exceptionObject = AllocateVirtualRegister(clause.Type);

					context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);
				}

				if (clause.ExceptionHandlerType == ExceptionHandlerType.Filter)
				{
					{
						var handler = GetOrCreateBlock(clause.HandlerStart);
						var context = new Context(handler);
						var exceptionObject = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);

						context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);
					}

					{
						var handler = GetOrCreateBlock(clause.FilterStart.Value);
						var context = new Context(handler);
						var exceptionObject = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);

						context.AppendInstruction(IRInstruction.FilterStart, exceptionObject);
					}
				}
			}
		}

		private void CreateInstructions()
		{
			BasicBlock block = BasicBlocks.GetByLabel(0);
			Stack<StackEntry> stack = new Stack<StackEntry>();
			Context context = new Context(block.AfterFirst);

			InstructionNode endNode = block.First;

			for (int index = 0; index < Method.Code.Count; index++)
			{
				var instruction = Method.Code[index];
				var opcode = (OpCode)instruction.OpCode;
				var label = instruction.Offset;

				if (block == null)
				{
					block = BasicBlocks.GetByLabel(label);
					stack = GetEvaluationStack(block);
					context.Node = block.AfterFirst;
					endNode = block.First;
				}

				bool processed = Translate(stack, context, instruction, opcode);

				if (!processed)
					throw new CompilerException($"Error: Unknown or unprocessable opcode: {opcode}");

				UpdateLabel(context.Node, label, endNode);
				endNode = context.Node;

				//if (IsBranch(opcode) || opcode == OpCode.Br || opcode == OpCode.Throw || opcode == OpCode.Endfilter || opcode == OpCode.Endfinally || opcode == OpCode.Ret)

				if (block.HasNextBlocks || opcode == OpCode.Throw || opcode == OpCode.Endfilter || opcode == OpCode.Endfinally)
				{
					foreach (var nextblock in block.NextBlocks)
					{
						SaveEvaluationStack(nextblock, stack);
					}

					stack = null;
					block = null;
				}
			}
		}

		private void CreateLocalVariables()
		{
			int count = Method.LocalVariables.Count;

			LocalStack = new Operand[count];

			if (count == 0)
				return;

			var arg = new bool[count];
			var argCount = 0;

			for (int label = 0; label < Method.Code.Count; label++)
			{
				var instruction = Method.Code[label];

				var opcode = (OpCode)instruction.OpCode;

				if (opcode == OpCode.Ldloca)
				{
					var index = (int)instruction.Operand;

					if (!arg[index])
					{
						arg[index] = true;
						argCount++;

						if (argCount == count)  // early out
							break;
					}
				}
			}

			LocalStackType = new StackType[count];

			for (int index = 0; index < count; index++)
			{
				var type = Method.LocalVariables[index];
				var registerType = MosaTypeLayout.GetTypeForRegister(type.Type);

				var stackType = ConvertToStackType(registerType);
				LocalStackType[index] = stackType;

				if (arg[index] || type.IsPinned || stackType == StackType.ValueType)
				{
					LocalStack[index] = MethodCompiler.AddStackLocal(registerType, type.IsPinned);
				}
				else
				{
					LocalStack[index] = MethodCompiler.CreateVirtualRegister(type.Type);
				}
			}
		}

		private void InitializeLocalVariables()
		{
			var prologue = new Context(BasicBlocks.PrologueBlock.First);

			for (int index = 0; index < LocalStack.Length; index++)
			{
				var local = LocalStack[index];
				var localstacktype = LocalStackType[index];

				if (!local.IsVirtualRegister)
					continue;

				switch (localstacktype)
				{
					case StackType.Object:
						prologue.AppendInstruction(IRInstruction.MoveObject, local, Operand.GetNull(local.Type));
						break;

					case StackType.Int32:
						prologue.AppendInstruction(IRInstruction.Move64, local, ConstantZero64);
						break;

					case StackType.Int64:
						prologue.AppendInstruction(IRInstruction.Move32, local, ConstantZero64);
						break;

					case StackType.R4:
						prologue.AppendInstruction(IRInstruction.MoveR4, local, CreateConstantR4(0.0f));
						break;

					case StackType.R8:
						prologue.AppendInstruction(IRInstruction.MoveR8, local, CreateConstantR8(0.0d));
						break;

					default:
						prologue.AppendInstruction(IRInstruction.Move32, local, ConstantZero32);
						break;
				}
			}
		}

		private bool Translate(Stack<StackEntry> stack, Context context, MosaInstruction instruction, OpCode opcode)
		{
			switch (opcode)
			{
				case OpCode.Add: return Add(context, stack);
				case OpCode.Add_ovf: return Add(context, stack);        // TODO: implement overflow check
				case OpCode.Add_ovf_un: return Add(context, stack);     // TODO: implement overflow check
				case OpCode.And: return And(context, stack);
				case OpCode.Arglist: return false;
				case OpCode.Beq: return Branch(context, stack, ConditionCode.Equal, instruction);
				case OpCode.Beq_s: return Branch(context, stack, ConditionCode.Equal, instruction);
				case OpCode.Bge: return Branch(context, stack, ConditionCode.GreaterOrEqual, instruction);
				case OpCode.Bge_s: return Branch(context, stack, ConditionCode.GreaterOrEqual, instruction);
				case OpCode.Bge_un: return Branch(context, stack, ConditionCode.UnsignedGreaterOrEqual, instruction);
				case OpCode.Bge_un_s: return Branch(context, stack, ConditionCode.UnsignedGreaterOrEqual, instruction);
				case OpCode.Bgt: return Branch(context, stack, ConditionCode.Greater, instruction);
				case OpCode.Bgt_s: return Branch(context, stack, ConditionCode.Greater, instruction);
				case OpCode.Bgt_un: return Branch(context, stack, ConditionCode.UnsignedGreater, instruction);
				case OpCode.Bgt_un_s: return Branch(context, stack, ConditionCode.UnsignedGreater, instruction);
				case OpCode.Ble: return Branch(context, stack, ConditionCode.LessOrEqual, instruction);
				case OpCode.Ble_s: return Branch(context, stack, ConditionCode.LessOrEqual, instruction);
				case OpCode.Ble_un: return Branch(context, stack, ConditionCode.UnsignedLessOrEqual, instruction);
				case OpCode.Ble_un_s: return Branch(context, stack, ConditionCode.UnsignedLessOrEqual, instruction);
				case OpCode.Blt: return Branch(context, stack, ConditionCode.Less, instruction);
				case OpCode.Blt_s: return Branch(context, stack, ConditionCode.Less, instruction);
				case OpCode.Blt_un: return Branch(context, stack, ConditionCode.UnsignedLess, instruction);
				case OpCode.Blt_un_s: return Branch(context, stack, ConditionCode.UnsignedLess, instruction);
				case OpCode.Bne_un: return Branch(context, stack, ConditionCode.NotEqual, instruction);
				case OpCode.Bne_un_s: return Branch(context, stack, ConditionCode.NotEqual, instruction);
				case OpCode.Box: return Box(context, stack, instruction);
				case OpCode.Br: return Branch(context, stack, instruction);
				case OpCode.Br_s: return Branch(context, stack, instruction);
				case OpCode.Break: return true;
				case OpCode.Brfalse: return Branch1(context, stack, ConditionCode.Equal, instruction);
				case OpCode.Brfalse_s: return Branch1(context, stack, ConditionCode.Equal, instruction);
				case OpCode.Brtrue: return Branch1(context, stack, ConditionCode.NotEqual, instruction);
				case OpCode.Brtrue_s: return Branch1(context, stack, ConditionCode.NotEqual, instruction);
				case OpCode.Call: return false;
				case OpCode.Calli: return false;
				case OpCode.Callvirt: return false;
				case OpCode.Castclass: return Castclass(context, stack);
				case OpCode.Ceq: return Compare(context, stack, ConditionCode.Equal);
				case OpCode.Cgt: return Compare(context, stack, ConditionCode.Greater);
				case OpCode.Cgt_un: return Compare(context, stack, ConditionCode.UnsignedGreater);
				case OpCode.Ckfinite: return false;
				case OpCode.Clt: return Compare(context, stack, ConditionCode.Less);
				case OpCode.Clt_un: return Compare(context, stack, ConditionCode.UnsignedLess);
				case OpCode.Conv_i: return ConvertI(context, stack);
				case OpCode.Conv_i1: return ConvertI1(context, stack);
				case OpCode.Conv_i2: return ConvertI2(context, stack);
				case OpCode.Conv_i4: return ConvertI4(context, stack);
				case OpCode.Conv_i8: return ConvertI8(context, stack);
				case OpCode.Conv_ovf_i: return ConvertI(context, stack);            // TODO: implement overflow check
				case OpCode.Conv_ovf_i_un: return false;
				case OpCode.Conv_ovf_i1: return ConvertI1(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_i1_un: return ConvertUToI1(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_i2: return ConvertI2(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_i2_un: return ConvertUToI2(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_i4: return ConvertI4(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_i4_un: return ConvertUToI4(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_i8: return ConvertI8(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_i8_un: return ConvertUToI8(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_u: return ConvertU(context, stack);            // TODO: implement overflow check
				case OpCode.Conv_ovf_u_un: return false;
				case OpCode.Conv_ovf_u1: return ConvertU1(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_u1_un: return ConvertUToU1(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_u2: return ConvertU2(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_u2_un: return ConvertUToU2(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_u4: return ConvertU4(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_u4_un: return ConvertUToU4(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_u8: return ConvertU8(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_u8_un: return ConvertUToU8(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_r_un: return ConvertUToF(context, stack);
				case OpCode.Conv_r4: return ConvertR4(context, stack);
				case OpCode.Conv_r8: return ConvertR8(context, stack);
				case OpCode.Conv_u: return ConvertU(context, stack);
				case OpCode.Conv_u1: return ConvertU1(context, stack);
				case OpCode.Conv_u2: return ConvertU2(context, stack);
				case OpCode.Conv_u4: return ConvertU4(context, stack);
				case OpCode.Conv_u8: return ConvertU8(context, stack);
				case OpCode.Cpblk: return Cpblk(context, stack, instruction);
				case OpCode.Cpobj: return Cpobj(context, stack, instruction);
				case OpCode.Div: return Div(context, stack);
				case OpCode.Div_un: return DivUnsigned(context, stack);
				case OpCode.Dup: return Dup(context, stack);
				case OpCode.Endfilter: return false;
				case OpCode.Endfinally: return false;
				case OpCode.Extop: return false;
				case OpCode.Initblk: return false;
				case OpCode.InitObj: return false;
				case OpCode.Isinst: return false;
				case OpCode.Jmp: return false;
				case OpCode.Ldarg: return LoadArgument(context, stack, (int)instruction.Operand);
				case OpCode.Ldarg_0: return LoadArgument(context, stack, 0);
				case OpCode.Ldarg_1: return LoadArgument(context, stack, 1);
				case OpCode.Ldarg_2: return LoadArgument(context, stack, 2);
				case OpCode.Ldarg_3: return LoadArgument(context, stack, 3);
				case OpCode.Ldarg_s: return LoadArgument(context, stack, (int)instruction.Operand);
				case OpCode.Ldarga: return false;
				case OpCode.Ldarga_s: return false;
				case OpCode.Ldc_i4: return Constant32(context, stack, (int)instruction.Operand);
				case OpCode.Ldc_i4_0: return Constant32(context, stack, 0);
				case OpCode.Ldc_i4_1: return Constant32(context, stack, 1);
				case OpCode.Ldc_i4_2: return Constant32(context, stack, 2);
				case OpCode.Ldc_i4_3: return Constant32(context, stack, 3);
				case OpCode.Ldc_i4_4: return Constant32(context, stack, 4);
				case OpCode.Ldc_i4_5: return Constant32(context, stack, 5);
				case OpCode.Ldc_i4_6: return Constant32(context, stack, 6);
				case OpCode.Ldc_i4_7: return Constant32(context, stack, 7);
				case OpCode.Ldc_i4_8: return Constant32(context, stack, 8);
				case OpCode.Ldc_i4_m1: return Constant32(context, stack, -1);
				case OpCode.Ldc_i4_s: return Constant32(context, stack, (sbyte)instruction.Operand);
				case OpCode.Ldc_i8: return Constant64(context, stack, (long)instruction.Operand);
				case OpCode.Ldc_r4: return ConstantR4(context, stack, (float)instruction.Operand);
				case OpCode.Ldc_r8: return ConstantR8(context, stack, (double)instruction.Operand);
				case OpCode.Ldelem: return false;
				case OpCode.Ldelem_i: return false;
				case OpCode.Ldelem_i1: return false;
				case OpCode.Ldelem_i2: return false;
				case OpCode.Ldelem_i4: return false;
				case OpCode.Ldelem_i8: return false;
				case OpCode.Ldelem_r4: return false;
				case OpCode.Ldelem_r8: return false;
				case OpCode.Ldelem_ref: return false;
				case OpCode.Ldelem_u1: return false;
				case OpCode.Ldelem_u2: return false;
				case OpCode.Ldelem_u4: return false;
				case OpCode.Ldelema: return false;
				case OpCode.Ldfld: return false;
				case OpCode.Ldflda: return false;
				case OpCode.Ldftn: return false;
				case OpCode.Ldind_i: return false;
				case OpCode.Ldind_i1: return false;
				case OpCode.Ldind_i2: return false;
				case OpCode.Ldind_i4: return false;
				case OpCode.Ldind_i8: return false;
				case OpCode.Ldind_r4: return false;
				case OpCode.Ldind_r8: return false;
				case OpCode.Ldind_ref: return false;
				case OpCode.Ldind_u1: return false;
				case OpCode.Ldind_u2: return false;
				case OpCode.Ldind_u4: return false;
				case OpCode.Ldlen: return false;
				case OpCode.Ldloc: return LoadLocal(context, stack, (int)instruction.Operand);
				case OpCode.Ldloc_0: return LoadLocal(context, stack, 0);
				case OpCode.Ldloc_1: return LoadLocal(context, stack, 0);
				case OpCode.Ldloc_2: return LoadLocal(context, stack, 0);
				case OpCode.Ldloc_3: return LoadLocal(context, stack, 0);
				case OpCode.Ldloc_s: return LoadLocal(context, stack, (int)instruction.Operand);
				case OpCode.Ldloca: return false;
				case OpCode.Ldloca_s: return false;
				case OpCode.Ldnull: return ConstantNull(context, stack);
				case OpCode.Ldobj: return false;
				case OpCode.Ldsfld: return false;
				case OpCode.Ldsflda: return false;
				case OpCode.Ldstr: return Ldstr(context, stack, instruction);
				case OpCode.Ldtoken: return false;
				case OpCode.Ldvirtftn: return false;
				case OpCode.Leave: return false;
				case OpCode.Leave_s: return false;
				case OpCode.Localalloc: return false;
				case OpCode.Mkrefany: return false;
				case OpCode.Mul: return Mul(context, stack);
				case OpCode.Mul_ovf: return Mul(context, stack);        // TODO: implement overflow check
				case OpCode.Mul_ovf_un: return Mul(context, stack);     // TODO: implement overflow check
				case OpCode.Neg: return Neg(context, stack);
				case OpCode.Newarr: return NewArray(context, stack, instruction);
				case OpCode.Newobj: return NewObject(context, stack, instruction);
				case OpCode.Nop: return true;
				case OpCode.Not: return Not(context, stack);
				case OpCode.Or: return Or(context, stack);
				case OpCode.Pop: return Pop(context, stack);
				case OpCode.PreConstrained: return false;
				case OpCode.PreNo: return false;
				case OpCode.PreReadOnly: return false;
				case OpCode.PreTail: return false;
				case OpCode.PreUnaligned: return false;
				case OpCode.PreVolatile: return false;
				case OpCode.Refanytype: return false;
				case OpCode.Refanyval: return false;
				case OpCode.Rem: return RemOperand(context, stack);
				case OpCode.Rem_un: return RemUnsigned(context, stack);
				case OpCode.Ret: return Ret(context, stack);
				case OpCode.Rethrow: return false;
				case OpCode.Shl: return Shl(context, stack, instruction);
				case OpCode.Shr: return Shr(context, stack, instruction);
				case OpCode.Shr_un: return ShrU(context, stack, instruction);
				case OpCode.Sizeof: return false;
				case OpCode.Starg: return false;
				case OpCode.Starg_s: return false;
				case OpCode.Stelem: return false;
				case OpCode.Stelem_i: return false;
				case OpCode.Stelem_i1: return false;
				case OpCode.Stelem_i2: return false;
				case OpCode.Stelem_i4: return false;
				case OpCode.Stelem_i8: return false;
				case OpCode.Stelem_r4: return false;
				case OpCode.Stelem_r8: return false;
				case OpCode.Stelem_ref: return false;
				case OpCode.Stfld: return false;
				case OpCode.Stind_i: return false;
				case OpCode.Stind_i1: return false;
				case OpCode.Stind_i2: return false;
				case OpCode.Stind_i4: return false;
				case OpCode.Stind_i8: return false;
				case OpCode.Stind_r4: return false;
				case OpCode.Stind_r8: return false;
				case OpCode.Stind_ref: return false;
				case OpCode.Stloc: return SaveLocal(context, stack, (int)instruction.Operand);
				case OpCode.Stloc_0: return SaveLocal(context, stack, 0);
				case OpCode.Stloc_1: return SaveLocal(context, stack, 1);
				case OpCode.Stloc_2: return SaveLocal(context, stack, 2);
				case OpCode.Stloc_3: return SaveLocal(context, stack, 3);
				case OpCode.Stloc_s: return SaveLocal(context, stack, 4);
				case OpCode.Stobj: return false;
				case OpCode.Stsfld: return false;
				case OpCode.Sub: return Sub(context, stack);
				case OpCode.Sub_ovf: return Sub(context, stack);        // TODO: implement overflow check
				case OpCode.Sub_ovf_un: return Sub(context, stack);     // TODO: implement overflow check
				case OpCode.Switch: return false;
				case OpCode.Throw: return Throw(context, stack);
				case OpCode.Unbox: return false;
				case OpCode.Unbox_any: return false;
				case OpCode.Xor: return Xor(context, stack);

				default: return false;
			}
		}

		private void UpdateLabel(InstructionNode node, int label, InstructionNode endNode)
		{
			while (node != endNode)
			{
				node.Label = label;
				node = node.Previous;
			}
		}

		#region Helpers

		private static bool IsBranch(OpCode opcode)
		{
			switch (opcode)
			{
				case OpCode.Beq: return true;
				case OpCode.Beq_s: return true;
				case OpCode.Bge: return true;
				case OpCode.Bge_s: return true;
				case OpCode.Bge_un: return true;
				case OpCode.Bge_un_s: return true;
				case OpCode.Bgt: return true;
				case OpCode.Bgt_s: return true;
				case OpCode.Bgt_un: return true;
				case OpCode.Bgt_un_s: return true;
				case OpCode.Ble: return true;
				case OpCode.Ble_s: return true;
				case OpCode.Ble_un: return true;
				case OpCode.Ble_un_s: return true;
				case OpCode.Blt: return true;
				case OpCode.Blt_s: return true;
				case OpCode.Blt_un: return true;
				case OpCode.Blt_un_s: return true;
				case OpCode.Bne_un: return true;
				case OpCode.Bne_un_s: return true;
				case OpCode.Brfalse_s: return true;
				case OpCode.Brtrue_s: return true;
				case OpCode.Brfalse: return true;
				case OpCode.Brtrue: return true;
				default: return false;
			}
		}

		private StackType ConvertToStackType(MosaType type)
		{
			if (type.IsReferenceType)
				return StackType.Object;
			else if (type.IsI1 || type.IsI2 || type.IsI4 || type.IsU1 || type.IsU2 || type.IsU4 || type.IsChar || type.IsBoolean)
				return StackType.Int32;
			else if (type.IsI8 || type.IsU8)
				return StackType.Int64;
			else if (type.IsR8)
				return StackType.Int64;
			else if (type.IsR4)
				return StackType.R4;

			if (type.IsI)
				return Is32BitPlatform ? StackType.Int32 : StackType.Int64;

			// TODO --- enums and other value types that fit into 32 or 64 bit register

			if (type.IsValueType)
				return StackType.ValueType;

			throw new CompilerException($"Can not translate to stacktype {type}");
		}

		private string EmitString(string data, uint token)
		{
			string symbolName = $"$ldstr${Method.Module.Name}${token}";
			var linkerSymbol = Linker.DefineSymbol(symbolName, SectionKind.ROData, NativeAlignment, (NativePointerSize * 2) + 4 + (data.Length * 2));
			var stream = linkerSymbol.Stream;
			Linker.Link(LinkType.AbsoluteAddress, PatchType.I32, linkerSymbol, 0, $"{Metadata.TypeDefinition}System.String", 0);
			stream.WriteZeroBytes(NativePointerSize * 2);
			stream.Write(BitConverter.GetBytes(data.Length), 0, 4);
			var stringData = Encoding.Unicode.GetBytes(data);
			stream.Write(stringData);
			return symbolName;
		}

		private Stack<StackEntry> GetEvaluationStack(BasicBlock block)
		{
			if (!stacks.TryGetValue(block, out Stack<StackEntry> stack))
			{
				stack = new Stack<StackEntry>(stack);
			}

			var newstack = new Stack<StackEntry>(stack);

			return newstack;
		}

		private BasicBlock GetOrCreateBlock(int label)
		{
			var block = BasicBlocks.GetByLabel(label);

			if (block == null)
			{
				block = CreateNewBlock(label, label);
			}

			return block;
		}

		private Operand GetRuntimeTypeHandle(MosaType runtimeType)
		{
			return Operand.CreateSymbol(TypeSystem.GetTypeByName("System", "RuntimeTypeHandle"), Metadata.TypeDefinition + runtimeType.FullName);
		}

		private MosaType GetType(StackType stackType)
		{
			switch (stackType)
			{
				case StackType.Int32: return TypeSystem.BuiltIn.I4;
				case StackType.Int64: return TypeSystem.BuiltIn.I8;
				case StackType.R4: return TypeSystem.BuiltIn.R4;
				case StackType.R8: return TypeSystem.BuiltIn.R8;
				case StackType.Object: return TypeSystem.BuiltIn.Object;
				default: return null;
			}
		}

		private MosaType GetType(StackEntry stackEntry)
		{
			if (stackEntry.StackType == StackType.ValueType)
				return stackEntry.Type;

			return GetType(stackEntry.StackType);
		}

		private void SaveEvaluationStack(BasicBlock block, Stack<StackEntry> stack)
		{
			if (stacks.TryGetValue(block, out var foundstack))
			{
				// TODO: Check stack size
			}
			else
			{
				stacks.Add(block, stack);
			}
		}

		#endregion Helpers

		#region CIL Shortcuts

		private bool Constant32(Context context, Stack<StackEntry> stack, int value)
		{
			var result = AllocateVirtualRegisterI32();
			context.AppendInstruction(IRInstruction.Move32, result, CreateConstant32(value));
			stack.Push(new StackEntry(StackType.Int32, result));
			return true;
		}

		private bool Constant64(Context context, Stack<StackEntry> stack, long value)
		{
			var result = AllocateVirtualRegisterI64();
			context.AppendInstruction(IRInstruction.Move64, result, CreateConstant64(value));
			stack.Push(new StackEntry(StackType.Int64, result));
			return true;
		}

		private bool ConstantNull(Context context, Stack<StackEntry> stack)
		{
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);
			context.AppendInstruction(IRInstruction.MoveObject, result, Operand.GetNullObject(TypeSystem));
			stack.Push(new StackEntry(StackType.Object, result));
			return true;
		}

		private bool ConstantR4(Context context, Stack<StackEntry> stack, float value)
		{
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
			context.AppendInstruction(IRInstruction.MoveR4, result, CreateConstantR4(value));
			stack.Push(new StackEntry(StackType.R4, result));
			return true;
		}

		private bool ConstantR8(Context context, Stack<StackEntry> stack, double value)
		{
			var result = AllocateVirtualRegisterR8();
			context.AppendInstruction(IRInstruction.MoveR8, result, CreateConstantR8(value));
			stack.Push(new StackEntry(StackType.R8, result));
			return true;
		}

		#endregion CIL Shortcuts

		#region CIL

		private bool Add(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					{
						var result = AllocateVirtualRegisterR4();
						context.AppendInstruction(IRInstruction.AddR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.AddR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Add32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Add64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Add64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.Add64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
				case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is32BitPlatform:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Add32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Add64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						var v1 = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Add64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						var v1 = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.Add32, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool And(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.And32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.And64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.And64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Box(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();
			var type = (MosaType)instruction.Operand;

			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);
			stack.Push(new StackEntry(StackType.Object, result));

			// TODO: if the HasValue property is false, then return null reference

			if (!type.IsValueType)
			{
				var moveInstruction = GetMoveInstruction(type);
				context.AppendInstruction(moveInstruction, context.Result, context.Operand1);
				return true;
			}

			var typeSize = Alignment.AlignUp(TypeLayout.GetTypeSize(type), TypeLayout.NativePointerAlignment);
			var runtimeType = GetRuntimeTypeHandle(type);

			if (typeSize <= 8 || type.IsR)
			{
				BaseIRInstruction boxInstruction;

				if (type.IsR4)
					boxInstruction = IRInstruction.BoxR4;
				else if (type.IsR8)
					boxInstruction = IRInstruction.BoxR8;
				else if (typeSize <= 4)
					boxInstruction = IRInstruction.Box32;
				else if (typeSize == 8)
					boxInstruction = IRInstruction.Box64;
				else
					throw new InvalidOperationException();

				context.AppendInstruction(boxInstruction, result, runtimeType, entry1.Operand);
			}
			else
			{
				var adr = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

				context.AppendInstruction(IRInstruction.AddressOf, adr, entry1.Operand);
				context.AppendInstruction(IRInstruction.Box, result, runtimeType, adr, CreateConstant32(typeSize));
			}

			return true;
		}

		private bool Branch(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var target = (int)instruction.Operand;
			var block = BasicBlocks.GetByLabel(target);

			context.AppendInstruction(IRInstruction.Jmp, block);

			return true;
		}

		private bool Branch(Context context, Stack<StackEntry> stack, ConditionCode conditionCode, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var target = (int)instruction.Operand;
			var block = BasicBlocks.GetByLabel(target);
			var nextblock = BasicBlocks.GetByLabel(instruction.Next.Value);

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.CompareR4, result, entry1.Operand, entry2.Operand);
						context.AppendInstruction(IRInstruction.Branch32, ConditionCode.NotZero, null, result, ConstantZero32, block);
						context.AppendInstruction(IRInstruction.Jmp, nextblock);
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.CompareR8, conditionCode, result, entry1.Operand, entry2.Operand);
						context.AppendInstruction(IRInstruction.Branch32, ConditionCode.NotZero, null, result, ConstantZero32, block);
						context.AppendInstruction(IRInstruction.Jmp, nextblock);
						return true;
					}

				case StackType.Object when entry2.StackType == StackType.Object:
					context.AppendInstruction(IRInstruction.BranchObject, conditionCode, null, entry1.Operand, entry2.Operand, block);
					context.AppendInstruction(IRInstruction.Jmp, nextblock);
					return true;

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					context.AppendInstruction(IRInstruction.Branch32, conditionCode, null, entry1.Operand, entry2.Operand, block);
					context.AppendInstruction(IRInstruction.Jmp, nextblock);
					return true;

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					context.AppendInstruction(IRInstruction.Branch64, conditionCode, null, entry1.Operand, entry2.Operand, block);
					context.AppendInstruction(IRInstruction.Jmp, nextblock);
					return true;

				default:

					// TODO: Managed Pointers

					return false;
			}
		}

		private bool Branch1(Context context, Stack<StackEntry> stack, ConditionCode conditionCode, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();

			var target = (int)instruction.Operand;
			var block = BasicBlocks.GetByLabel(target);
			var nextblock = BasicBlocks.GetByLabel(instruction.Next.Value);

			switch (entry1.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.Branch32, conditionCode, null, entry1.Operand, ConstantZero32, block);
					context.AppendInstruction(IRInstruction.Jmp, nextblock);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.Branch64, conditionCode, null, entry1.Operand, ConstantZero64, block);
					context.AppendInstruction(IRInstruction.Jmp, nextblock);
					return true;

				case StackType.Object:
					context.AppendInstruction(IRInstruction.BranchObject, conditionCode, null, entry1.Operand, ConstantZero, block);
					context.AppendInstruction(IRInstruction.Jmp, nextblock);
					return true;

				default:
					return false;
			}
		}

		private bool Castclass(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();

			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);
			stack.Push(new StackEntry(StackType.Object, result));

			if (entry1.StackType == StackType.Object)
			{
				// TODO: Do this right
				context.AppendInstruction(IRInstruction.MoveObject, result, entry1.Operand);
				return true;
			}

			return false;
		}

		private bool Compare(Context context, Stack<StackEntry> stack, ConditionCode conditionCode)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var result = AllocateVirtualRegisterI32();
			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					context.AppendInstruction(IRInstruction.CompareR4, result, entry1.Operand, entry2.Operand);
					return true;

				case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
					context.AppendInstruction(IRInstruction.CompareR8, conditionCode, result, entry1.Operand, entry2.Operand);
					return true;

				case StackType.Object when entry2.StackType == StackType.Object:
					context.AppendInstruction(IRInstruction.CompareObject, conditionCode, result, entry1.Operand, entry2.Operand);
					return true;

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					context.AppendInstruction(IRInstruction.Compare32x32, conditionCode, result, entry1.Operand, entry2.Operand);
					return true;

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					context.AppendInstruction(IRInstruction.Compare64x32, conditionCode, result, entry1.Operand, entry2.Operand);
					return true;

				default:

					// TODO: Managed Pointers

					return false;
			}
		}

		private bool ConvertI(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();

			if (Is32BitPlatform)
			{
				var result = AllocateVirtualRegisterI32();
				stack.Push(new StackEntry(StackType.Int32, result));

				switch (entry1.StackType)
				{
					case StackType.Int32:
						context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
						return true;

					case StackType.Int64:
						context.AppendInstruction(IRInstruction.Truncate64x32, result, entry1.Operand);
						return true;

					case StackType.R4:
						context.AppendInstruction(IRInstruction.ConvertR4ToI32, result, entry1.Operand);
						return true;

					case StackType.R8:
						context.AppendInstruction(IRInstruction.ConvertR8ToI32, result, entry1.Operand);
						return true;
				}
			}
			else
			{
				var result = AllocateVirtualRegisterI64();
				stack.Push(new StackEntry(StackType.Int64, result));

				switch (entry1.StackType)
				{
					case StackType.Int32:
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry1.Operand);
						return true;

					case StackType.Int64:
						context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);
						return true;

					case StackType.R4:
						context.AppendInstruction(IRInstruction.ConvertR4ToI64, result, entry1.Operand);
						return true;

					case StackType.R8:
						context.AppendInstruction(IRInstruction.ConvertR8ToI64, result, entry1.Operand);
						return true;
				}
			}

			return false;
		}

		private bool ConvertI1(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.And32, v1, entry1.Operand, CreateConstant32(0xFF));
						context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(0xFFFFFF00));
						return true;
					}

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFFFF00));
						return true;
					}

				case StackType.R4:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR4ToI32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFFFF00));
						return true;
					}

				case StackType.R8:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR8ToI32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFFFF00));
						return true;
					}

				default:
					return false;
			}
		}

		private bool ConvertI2(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.And32, v1, entry1.Operand, CreateConstant32(0xFFFF));
						context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(0xFFFF0000));
						return true;
					}

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFFFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFF0000));
						return true;
					}

				case StackType.R4:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR4ToI32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFFFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFF0000));
						return true;
					}

				case StackType.R8:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR8ToI32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFFFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFF0000));
						return true;
					}

				default:
					return false;
			}
		}

		private bool ConvertI4(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
					return true;

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Move32, result, v1);
						return true;
					}

				case StackType.R4:
					context.AppendInstruction(IRInstruction.ConvertR4ToI32, result, entry1.Operand);
					return true;

				case StackType.R8:
					context.AppendInstruction(IRInstruction.ConvertR8ToI32, result, entry1.Operand);
					return true;

				default:
					return false;
			}
		}

		private bool ConvertI8(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI64();
			stack.Push(new StackEntry(StackType.Int64, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Move32, result, v1);
						return true;
					}

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
					return true;

				case StackType.R4:
					context.AppendInstruction(IRInstruction.ConvertR4ToI64, result, entry1.Operand);
					return true;

				case StackType.R8:
					context.AppendInstruction(IRInstruction.ConvertR8ToI64, result, entry1.Operand);
					return true;

				default:
					return false;
			}
		}

		private bool ConvertR4(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
			stack.Push(new StackEntry(StackType.R4, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.ConvertI32ToR4, result, entry1.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.ConvertI64ToR4, result, entry1.Operand);
					return true;

				case StackType.R4:
					context.AppendInstruction(IRInstruction.MoveR4, result, entry1.Operand);
					return true;

				case StackType.R8:
					context.AppendInstruction(IRInstruction.ConvertR8ToR4, result, entry1.Operand);
					return true;

				default:
					return false;
			}
		}

		private bool ConvertR8(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterR8();
			stack.Push(new StackEntry(StackType.R8, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.ConvertI32ToR8, result, entry1.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.ConvertI64ToR8, result, entry1.Operand);
					return true;

				case StackType.R4:
					context.AppendInstruction(IRInstruction.ConvertR4ToR8, result, entry1.Operand);
					return true;

				case StackType.R8:
					context.AppendInstruction(IRInstruction.MoveR8, result, entry1.Operand);
					return true;

				default:
					return false;
			}
		}

		private bool ConvertU(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();

			if (Is32BitPlatform)
			{
				var result = AllocateVirtualRegisterI32();
				stack.Push(new StackEntry(StackType.Int32, result));

				switch (entry1.StackType)
				{
					case StackType.Int32:
						context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
						return true;

					case StackType.Int64:
						context.AppendInstruction(IRInstruction.Truncate64x32, result, entry1.Operand);
						return true;

					case StackType.R4:
						context.AppendInstruction(IRInstruction.ConvertR4ToU32, result, entry1.Operand);
						return true;

					case StackType.R8:
						context.AppendInstruction(IRInstruction.ConvertR8ToU32, result, entry1.Operand);
						return true;
				}

				// TODO: Float
			}
			else
			{
				var result = AllocateVirtualRegisterI64();
				stack.Push(new StackEntry(StackType.Int64, result));

				switch (entry1.StackType)
				{
					case StackType.Int32:
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry1.Operand);
						return true;

					case StackType.Int64:
						context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);
						return true;

					case StackType.R4:
						context.AppendInstruction(IRInstruction.ConvertR4ToU64, result, entry1.Operand);
						return true;

					case StackType.R8:
						context.AppendInstruction(IRInstruction.ConvertR8ToU64, result, entry1.Operand);
						return true;
				}

				// TODO: Float
			}

			return false;
		}

		private bool ConvertU1(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.And32, result, entry1.Operand, CreateConstant32(0xFF));
					return true;

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
						return true;
					}

				case StackType.R4:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR4ToU32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
						return true;
					}

				case StackType.R8:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR8ToU32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
						return true;
					}

				default:
					return false;
			}
		}

		private bool ConvertU2(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.And32, result, entry1.Operand, CreateConstant32(0xFFFF));
					return true;

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));
						return true;
					}

				case StackType.R4:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR4ToU32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));

						return true;
					}

				case StackType.R8:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR8ToI32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));

						return true;
					}

				default:
					return false;
			}
		}

		private bool ConvertU4(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry1.Operand);
					return true;

				case StackType.R4:
					context.AppendInstruction(IRInstruction.ConvertR4ToU32, result, entry1.Operand);
					return true;

				case StackType.R8:
					context.AppendInstruction(IRInstruction.ConvertR8ToU32, result, entry1.Operand);
					return true;

				default:
					return false;
			}
		}

		private bool ConvertU8(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI64();
			stack.Push(new StackEntry(StackType.Int64, result));

			if (entry1.StackType != StackType.Int32)
			{
				if (entry1.StackType == StackType.Int64)
				{
					context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);
					return true;
				}

				if (entry1.StackType == StackType.R4)
				{
					context.AppendInstruction(IRInstruction.ConvertR4ToU64, result, entry1.Operand);
					return true;
				}

				if (entry1.StackType == StackType.R8)
				{
					context.AppendInstruction(IRInstruction.ConvertR8ToU64, result, entry1.Operand);
					return true;
				}

				return false;
			}
			var v1 = AllocateVirtualRegisterI64();
			context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
			context.AppendInstruction(IRInstruction.Move32, result, v1);
			return true;
		}

		private bool ConvertUToF(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterR8();
			stack.Push(new StackEntry(StackType.R8, result));

			if (entry1.StackType != StackType.Int32)
			{
				if (entry1.StackType == StackType.Int64)
				{
					context.AppendInstruction(IRInstruction.ConvertU64ToR8, result, entry1.Operand);
					return true;
				}

				return false;
			}
			context.AppendInstruction(IRInstruction.ConvertU32ToR8, result, entry1.Operand);
			return true;
		}

		private bool ConvertUToI1(Context context, Stack<StackEntry> stack)
		{
			// convert unsigned to an int8 (on the stack as int32)

			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.And32, v1, entry1.Operand, CreateConstant32(0xFF));
						context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(~0xFF));
						return true;
					}

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(~0xFF));
						return true;
					}

				default:
					return false;
			}
		}

		private bool ConvertUToI2(Context context, Stack<StackEntry> stack)
		{
			// convert unsigned to an int8 (on the stack as int32)

			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.And32, v1, entry1.Operand, CreateConstant32(0xFFFF));
						context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(~0xFFFF));
						return true;
					}

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFFFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(~0xFFFF));
						return true;
					}

				default:
					return false;
			}
		}

		private bool ConvertUToI4(Context context, Stack<StackEntry> stack)
		{
			// convert unsigned to an int4 (on the stack as int32)

			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.Truncate64x32, result, entry1.Operand);
					return true;

				default:
					return false;
			}
		}

		private bool ConvertUToI8(Context context, Stack<StackEntry> stack)
		{
			// convert unsigned to an int64 (on the stack as int32)

			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI64();

			stack.Push(new StackEntry(StackType.Int64, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.SignExtend32x64, result, entry1.Operand, CreateConstant32(0xFFFF));
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);
					return true;

				default:
					return false;
			}
		}

		private bool ConvertUToU1(Context context, Stack<StackEntry> stack)
		{
			// convert unsigned to an unsigned int8 (on the stack as int32)

			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.And32, result, entry1.Operand, CreateConstant32(0xFF));
						return true;
					}

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
						return true;
					}

				default:
					return false;
			}
		}

		private bool ConvertUToU2(Context context, Stack<StackEntry> stack)
		{
			// convert unsigned to an unsigned int8 (on the stack as int32)

			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.And32, result, entry1.Operand, CreateConstant32(0xFFFF));
						return true;
					}

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));
						return true;
					}

				default:
					return false;
			}
		}

		private bool ConvertUToU4(Context context, Stack<StackEntry> stack)
		{
			// convert unsigned to an unsigned int4 (on the stack as int32)

			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.Truncate64x32, result, entry1.Operand);
					return true;

				default:
					return false;
			}
		}

		private bool ConvertUToU8(Context context, Stack<StackEntry> stack)
		{
			// convert unsigned to an unsigned int64 (on the stack as int32)

			var entry1 = stack.Pop();
			var result = AllocateVirtualRegisterI64();

			stack.Push(new StackEntry(StackType.Int64, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry1.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);
					return true;

				default:
					return false;
			}
		}

		private bool Cpblk(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var source = stack.Pop();
			var destination = stack.Pop();

			if ((source.StackType == StackType.Int32 || source.StackType == StackType.Int64) && (destination.StackType == StackType.Int32 || destination.StackType == StackType.Int64))
			{
				context.AppendInstruction(IRInstruction.MemoryCopy, null, source.Operand, destination.Operand);
				return true;
			}

			return false;
		}

		private bool Cpobj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var source = stack.Pop();
			var destination = stack.Pop();

			// TODO

			return false;
		}

		private bool Div(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						context.AppendInstruction(IRInstruction.DivR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.DivR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.DivSigned32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.DivSigned64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.DivSigned64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.DivSigned64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool DivUnsigned(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						context.AppendInstruction(IRInstruction.DivR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.DivR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.DivUnsigned32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.DivUnsigned64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.DivUnsigned64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.DivUnsigned64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Dup(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.R4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						context.AppendInstruction(IRInstruction.MoveR4, result, entry1.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.MoveR8, result, entry1.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Object:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);
						context.AppendInstruction(IRInstruction.MoveObject, result, entry1.Operand);
						stack.Push(new StackEntry(StackType.Object, result));
						return true;
					}

				case StackType.ManagedPointer when Is32BitPlatform:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);    // FUTURE - Use MoveManagedPointer
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				case StackType.ManagedPointer when Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);    // FUTURE - Use MoveManagedPointer
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				default:

					// TODO: ValueTypes

					return false;
			}
		}

		private bool Ldstr(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.String);
			stack.Push(new StackEntry(StackType.Object, result));

			var token = (uint)instruction.Operand;

			var stringdata = TypeSystem.LookupUserString(Method.Module, token);
			var symbolName = EmitString(stringdata, token);

			var symbol = Operand.CreateSymbol(TypeSystem.BuiltIn.String, symbolName);

			context.AppendInstruction(IRInstruction.MoveObject, result, symbol);

			return true;
		}

		private bool LoadArgument(Context context, Stack<StackEntry> stack, int index)
		{
			var parameter = MethodCompiler.Parameters[index];
			var type = parameter.Type;

			if (type.IsValueType)
			{
				var basetype = MosaTypeLayout.GetTypeForRegister(type);

				if (MosaTypeLayout.CanFitInRegister(basetype))
				{
					return LoadArgumentByType(context, stack, parameter, basetype);
				}
				else
				{
					var result = AllocateVirtualRegister(TypeSystem.BuiltIn.ValueType);
					context.AppendInstruction(IRInstruction.LoadParamCompound, result, parameter);
					context.MosaType = type;
					stack.Push(new StackEntry(StackType.ValueType, result, type));
					return true;
				}
			}

			return LoadArgumentByType(context, stack, parameter, type);
		}

		private bool LoadArgumentByType(Context context, Stack<StackEntry> stack, Operand parameter, MosaType type)
		{
			if (type.IsU1 || type.IsBoolean)
			{
				var result = AllocateVirtualRegisterI32();
				context.AppendInstruction(IRInstruction.LoadParamZeroExtend8x32, result, parameter);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (type.IsI1)
			{
				var result = AllocateVirtualRegisterI32();
				context.AppendInstruction(IRInstruction.LoadParamSignExtend8x32, result, parameter);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (type.IsU2 || type.IsChar)
			{
				var result = AllocateVirtualRegisterI32();
				context.AppendInstruction(IRInstruction.LoadParamZeroExtend16x32, result, parameter);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (type.IsI2)
			{
				var result = AllocateVirtualRegisterI32();
				context.AppendInstruction(IRInstruction.LoadParamSignExtend16x32, result, parameter);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (type.IsU4 || type.IsI4 || (type.IsEnum && (type.ElementType.IsI4 || type.ElementType.IsU4)))
			{
				var result = AllocateVirtualRegisterI32();
				context.AppendInstruction(IRInstruction.LoadParam32, result, parameter);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (type.IsU8 || type.IsI8 || (type.IsEnum && (type.ElementType.IsI8 || type.ElementType.IsU8)))
			{
				var result = AllocateVirtualRegisterI64();
				context.AppendInstruction(IRInstruction.LoadParam64, result, parameter);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (type.IsR4)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
				context.AppendInstruction(IRInstruction.LoadParamR4, result, parameter);
				stack.Push(new StackEntry(StackType.R4, result));
				return true;
			}

			if (type.IsR8)
			{
				var result = AllocateVirtualRegisterR8();
				context.AppendInstruction(IRInstruction.LoadParamR8, result, parameter);
				stack.Push(new StackEntry(StackType.R8, result));
				return true;
			}

			if (type.IsReferenceType)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);
				context.AppendInstruction(IRInstruction.LoadParamObject, result, parameter);
				stack.Push(new StackEntry(StackType.Object, result));
				return true;
			}

			if ((type.IsI || type.IsU) && Is32BitPlatform)
			{
				var result = AllocateVirtualRegisterI32();
				context.AppendInstruction(IRInstruction.LoadParam32, result, parameter);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if ((type.IsI || type.IsU) && Is64BitPlatform)
			{
				var result = AllocateVirtualRegisterI64();
				context.AppendInstruction(IRInstruction.LoadParam64, result, parameter);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			return false;
		}

		private bool LoadLocal(Context context, Stack<StackEntry> stack, int index)
		{
			var stacktype = LocalStackType[index];
			var local = LocalStack[index];

			if (stacktype == StackType.ValueType)
			{
				var result2 = AllocateVirtualRegister(local.Type);
				context.AppendInstruction(IRInstruction.LoadParamCompound, result2, local);
				context.MosaType = local.Type;

				stack.Push(new StackEntry(stacktype, result2, local.Type));

				return true;
			}

			var resultType = GetType(stacktype);
			var result = AllocateVirtualRegister(resultType);

			stack.Push(new StackEntry(stacktype, result));

			if (local.IsVirtualRegister)
			{
				switch (stacktype)
				{
					case StackType.Int32:
						context.AppendInstruction(IRInstruction.Move32, result, local);
						return true;

					case StackType.Int64:
						context.AppendInstruction(IRInstruction.Move64, result, local);
						return true;

					case StackType.R4:
						context.AppendInstruction(IRInstruction.MoveR4, result, local);
						return true;

					case StackType.R8:
						context.AppendInstruction(IRInstruction.MoveR8, result, local);
						return true;

					case StackType.Object:
						context.AppendInstruction(IRInstruction.MoveObject, result, local);
						return true;
				}
			}
			else
			{
				switch (stacktype)
				{
					case StackType.Int32:
						context.AppendInstruction(IRInstruction.LoadParam32, result, local);
						return true;

					case StackType.Int64:
						context.AppendInstruction(IRInstruction.LoadParam64, result, local);
						return true;

					case StackType.R4:
						context.AppendInstruction(IRInstruction.LoadParamR4, result, local);
						return true;

					case StackType.R8:
						context.AppendInstruction(IRInstruction.LoadParamR8, result, local);
						return true;
				}
			}

			return false;
		}

		private bool Mul(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						context.AppendInstruction(IRInstruction.MulR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.MulR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.MulUnsigned32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.MulUnsigned64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.MulUnsigned64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.MulUnsigned64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Neg(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						var zero = CreateConstantR4(0);
						context.AppendInstruction(IRInstruction.SubR4, result, zero, entry1.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8:
					{
						var result = AllocateVirtualRegisterR8();
						var zero = CreateConstantR8(0);
						context.AppendInstruction(IRInstruction.SubR8, result, zero, entry1.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Sub32, result, ConstantZero32, entry1.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Sub32, result, ConstantZero64, entry1.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Not(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Not32, result, entry1.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Not64, result, entry1.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Or(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Or32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Or64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Or64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.Or64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Pop(Context context, Stack<StackEntry> stack)
		{
			stack.Pop();

			return true;
		}

		private bool RemOperand(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						context.AppendInstruction(IRInstruction.RemR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.RemR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.RemSigned32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.RemSigned64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.RemSigned64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.RemSigned64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool RemUnsigned(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						context.AppendInstruction(IRInstruction.RemR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.RemR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.RemUnsigned32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.RemUnsigned64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.RemUnsigned64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.RemUnsigned64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Ret(Context context, Stack<StackEntry> stack)
		{
			if (!Method.Signature.ReturnType.IsVoid)
			{
				var entry = stack.Pop();

				switch (entry.StackType)
				{
					case StackType.Int32:
						context.AppendInstruction(IRInstruction.SetReturn32, null, entry.Operand);
						break;

					case StackType.Int64:
						context.AppendInstruction(IRInstruction.SetReturn64, null, entry.Operand);
						break;

					case StackType.R4:
						context.AppendInstruction(IRInstruction.SetReturnR4, null, entry.Operand);
						break;

					case StackType.R8:
						context.AppendInstruction(IRInstruction.SetReturnR8, null, entry.Operand);
						break;

					case StackType.Object:
						context.AppendInstruction(IRInstruction.SetReturnObject, null, entry.Operand);
						break;

					case StackType.ValueType:
						context.AppendInstruction(IRInstruction.SetReturnCompound, null, entry.Operand);
						context.MosaType = entry.Type;
						break;
				}
			}

			var block = GetOrCreateBlock(BasicBlock.EpilogueLabel);
			context.AppendInstruction(IRInstruction.Jmp, block);

			return true;
		}

		private bool SaveLocal(Context context, Stack<StackEntry> stack, int index)
		{
			var entry1 = stack.Pop();
			var source = entry1.Operand;

			var stacktype = LocalStackType[index];
			var local = LocalStack[index];

			if (stacktype == StackType.ValueType)
			{
				context.AppendInstruction(IRInstruction.StoreCompound, null, local, source);
				context.MosaType = local.Type;

				return true;
			}

			if (local.IsVirtualRegister)
			{
				switch (stacktype)
				{
					case StackType.Int32:
						context.AppendInstruction(IRInstruction.Move32, local, source);
						return true;

					case StackType.Int64:
						context.AppendInstruction(IRInstruction.Move64, local, source);
						return true;

					case StackType.Object:
						context.AppendInstruction(IRInstruction.MoveObject, local, source);
						return true;

					case StackType.R4:
						context.AppendInstruction(IRInstruction.MoveR4, local, source);
						return true;

					case StackType.R8:
						context.AppendInstruction(IRInstruction.MoveR8, local, source);
						return true;
				}
			}
			else
			{
				switch (stacktype)
				{
					case StackType.Int32:
						context.AppendInstruction(IRInstruction.StoreParam32, null, local, source);
						return true;

					case StackType.Int64:
						context.AppendInstruction(IRInstruction.StoreParam64, null, local, source);
						return true;

					case StackType.Object:
						context.AppendInstruction(IRInstruction.StoreParamObject, local, source);
						return true;

					case StackType.R4:
						context.AppendInstruction(IRInstruction.StoreParamR4, null, local, source);
						return true;

					case StackType.R8:
						context.AppendInstruction(IRInstruction.StoreParamR8, null, local, source);
						return true;
				}
			}

			return false;
		}

		private bool Shl(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var shiftAmount = entry1.Operand;
			var shiftValue = entry2.Operand;

			switch (entry2.StackType)
			{
				case StackType.Int32 when entry1.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ShiftLeft32, result, shiftValue, shiftAmount);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry1.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ShiftLeft64, result, shiftValue, shiftAmount);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry1.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, shiftAmount);
						context.AppendInstruction(IRInstruction.ShiftLeft32, result, shiftValue, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry1.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, shiftAmount);
						context.AppendInstruction(IRInstruction.ShiftLeft64, result, shiftValue, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Shr(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var shiftAmount = entry1.Operand;
			var shiftValue = entry2.Operand;

			switch (entry1.StackType)
			{
				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ArithShiftRight32, result, shiftValue, shiftAmount);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ArithShiftRight32, result, shiftValue, shiftAmount);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, shiftAmount);
						context.AppendInstruction(IRInstruction.ArithShiftRight32, result, shiftValue, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, shiftAmount);
						context.AppendInstruction(IRInstruction.ArithShiftRight32, result, shiftValue, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool ShrU(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var shiftAmount = entry1.Operand;
			var shiftValue = entry2.Operand;

			switch (entry1.StackType)
			{
				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ShiftRight32, result, shiftValue, shiftAmount);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ShiftRight32, result, shiftValue, shiftAmount);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, shiftAmount);
						context.AppendInstruction(IRInstruction.ShiftRight32, result, shiftValue, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, shiftAmount);
						context.AppendInstruction(IRInstruction.ShiftRight32, result, shiftValue, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Sub(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						context.AppendInstruction(IRInstruction.SubR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.SubR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Sub32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Sub64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.ManagedPointer when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Sub32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.ManagedPointer when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
				case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is32BitPlatform:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Sub32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						var v1 = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Sub64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						var v1 = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.Sub32, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Throw(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();

			if (entry1.StackType == StackType.Object)
			{
				context.AppendInstruction(IRInstruction.Throw, null, entry1.Operand);
				stack.Clear();
				return true;
			}

			return false;
		}

		private bool Xor(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Xor32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Xor64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Xor64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.Xor64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool NewArray(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var elements = stack.Pop();

			var arrayType = (MosaType)instruction.Operand;

			var elementSize = GetTypeSize(arrayType.ElementType, false);
			var runtimeTypeHandle = GetRuntimeTypeHandle(arrayType);
			var size = CreateConstant32(elementSize);
			var result = AllocateVirtualRegisterObject();

			context.SetInstruction(IRInstruction.NewArray, result, runtimeTypeHandle, size, elements.Operand);
			context.MosaType = arrayType;

			stack.Push(new StackEntry(StackType.Object, result));

			return true;
		}

		private bool NewObject(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var method = (MosaMethod)instruction.Operand;
			var classType = method.DeclaringType;
			int paramCount = method.Signature.Parameters.Count;

			var registerType = MosaTypeLayout.GetTypeForRegister(classType);
			var stackType = ConvertToStackType(registerType);

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			var operands = new List<Operand>();

			for (int i = 0; i < paramCount; i++)
			{
				var param = stack.Pop();
				operands.Add(param.Operand);
			}

			if (stackType == StackType.Object)
			{
				var result = AllocateVirtualRegisterObject();

				var runtimeTypeHandle = GetRuntimeTypeHandle(classType);
				var size = CreateConstant32(TypeLayout.GetTypeSize(classType));

				context.AppendInstruction(IRInstruction.NewObject, result, runtimeTypeHandle, size);

				operands.Insert(0, result);

				context.AppendInstruction(IRInstruction.CallStatic, null, symbol, operands);
				context.InvokeMethod = method;

				stack.Push(new StackEntry(StackType.Object, result));

				return true;
			}
			else if (stackType == StackType.ValueType)
			{
				var newThisLocal = MethodCompiler.AddStackLocal(classType);
				var newThis = MethodCompiler.CreateVirtualRegister(classType.ToManagedPointer());

				context.AppendInstruction(IRInstruction.AddressOf, newThis, newThisLocal);

				operands.Insert(0, newThis);

				context.AppendInstruction(IRInstruction.CallStatic, null, symbol, operands);
				context.InvokeMethod = method;

				stack.Push(new StackEntry(StackType.ManagedPointer, newThis));   // ManagedPointer??
			}
			else if (stackType == StackType.Int32)
			{
				// INCOMPLETE

				var newThisLocal = MethodCompiler.AddStackLocal(classType);
				var newThis = MethodCompiler.CreateVirtualRegister(classType.ToManagedPointer());

				context.AppendInstruction(IRInstruction.AddressOf, newThis, newThisLocal);

				//context.AppendInstruction(IRInstruction.Load32,  )

				operands.Insert(0, newThis);

				context.AppendInstruction(IRInstruction.CallStatic, null, symbol, operands);
				context.InvokeMethod = method;

				stack.Push(new StackEntry(StackType.ManagedPointer, newThis));   // ManagedPointer??
			}
			else if (stackType != StackType.ValueType)
			{
				// INCOMPLETE

				var newThisLocal = MethodCompiler.AddStackLocal(classType);
				var newThis = MethodCompiler.CreateVirtualRegister(classType.ToManagedPointer());

				context.AppendInstruction(IRInstruction.AddressOf, newThis, newThisLocal);

				// TODO: unboxing it (kinda of)

				operands.Insert(0, newThis);

				context.AppendInstruction(IRInstruction.CallStatic, null, symbol, operands);
				context.InvokeMethod = method;

				stack.Push(new StackEntry(StackType.ManagedPointer, newThis));   // ManagedPointer??
			}

			return false;
		}

		#endregion CIL
	}
}
