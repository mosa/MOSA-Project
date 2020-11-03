// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
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

		private enum StackType { Int32, Int64, R4, R8, ManagedPointer, Object = 7, ValueType };

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

		protected override void Finish()
		{
		}

		protected override void Run()
		{
			if (!MethodCompiler.IsCILStream)
				return;

			MethodCompiler.SetLocalVariables(Method.LocalVariables);

			// Create the prologue block
			var prologue = CreateNewBlock(BasicBlock.PrologueLabel);
			BasicBlocks.AddHeadBlock(prologue);

			var jmpNode = new InstructionNode()
			{
				Label = BasicBlock.PrologueLabel,
				Block = prologue
			};
			prologue.First.Insert(jmpNode);

			// Create starting block
			var startBlock = CreateNewBlock(0);

			jmpNode.SetInstruction(IRInstruction.Jmp, startBlock);

			CreateBasicBlocks();

			CreateHandlersBlocks();

			CreateInstructions();
		}

		protected override void Setup()
		{
		}

		private void CreateBasicBlocks()
		{
			for (int label = 0; label < Method.Code.Count; label++)
			{
				var instruction = Method.Code[label];

				var opcode = (OpCode)instruction.OpCode;

				if (opcode == OpCode.Br)
				{
					GetOrCreateBlock((int)instruction.Operand);
				}
				else if (IsBranch(opcode))
				{
					GetOrCreateBlock((int)instruction.Operand);
					GetOrCreateBlock(label + 1);
				}
				else if (opcode == OpCode.Switch)
				{
					foreach (var target in (int[])instruction.Operand)
					{
						GetOrCreateBlock((int)instruction.Operand);
					}

					GetOrCreateBlock(label + 1);
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

			for (int label = 0; label < Method.Code.Count; label++)
			{
				if (block == null)
				{
					block = BasicBlocks.GetByLabel(label);
					stack = GetEvaluationStack(block);
					context.Node = block.AfterFirst;
				}

				var instruction = Method.Code[label];
				var opcode = (OpCode)instruction.OpCode;

				bool processed = Translate(stack, context, instruction, opcode);

				if (!processed)
					throw new CompilerException($"Error: Unknown or unprocessable opcode: {opcode}");

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
				case OpCode.Castclass: return false;
				case OpCode.Ceq: return Compare(context, stack, ConditionCode.Equal);
				case OpCode.Cgt: return Compare(context, stack, ConditionCode.Greater);
				case OpCode.Cgt_un: return Compare(context, stack, ConditionCode.UnsignedGreater);
				case OpCode.Ckfinite: return false;
				case OpCode.Clt: return Compare(context, stack, ConditionCode.Less);
				case OpCode.Clt_un: return Compare(context, stack, ConditionCode.Less);
				case OpCode.Conv_i: return false;
				case OpCode.Conv_i1: return ConvertI1(context, stack);
				case OpCode.Conv_i2: return ConvertI2(context, stack);
				case OpCode.Conv_i4: return ConvertI4(context, stack);
				case OpCode.Conv_i8: return ConvertI8(context, stack);
				case OpCode.Conv_ovf_i: return false;
				case OpCode.Conv_ovf_i_un: return false;
				case OpCode.Conv_ovf_i1: return ConvertI1(context, stack);  // TODO: implement overflow check
				case OpCode.Conv_ovf_i1_un: return false;
				case OpCode.Conv_ovf_i2: return ConvertI2(context, stack);  // TODO: implement overflow check
				case OpCode.Conv_ovf_i2_un: return false;
				case OpCode.Conv_ovf_i4: return ConvertI4(context, stack);  // TODO: implement overflow check
				case OpCode.Conv_ovf_i4_un: return false;
				case OpCode.Conv_ovf_i8: return ConvertI8(context, stack);  // TODO: implement overflow check
				case OpCode.Conv_ovf_i8_un: return false;
				case OpCode.Conv_ovf_u: return false;
				case OpCode.Conv_ovf_u_un: return false;
				case OpCode.Conv_ovf_u1: return ConvertU1(context, stack);  // TODO: implement overflow check
				case OpCode.Conv_ovf_u1_un: return false;
				case OpCode.Conv_ovf_u2: return ConvertU2(context, stack);  // TODO: implement overflow check
				case OpCode.Conv_ovf_u2_un: return false;
				case OpCode.Conv_ovf_u4: return ConvertU4(context, stack);  // TODO: implement overflow check
				case OpCode.Conv_ovf_u4_un: return false;
				case OpCode.Conv_ovf_u8: return ConvertU8(context, stack);  // TODO: implement overflow check
				case OpCode.Conv_ovf_u8_un: return false;
				case OpCode.Conv_r_un: return false;
				case OpCode.Conv_r4: return false;
				case OpCode.Conv_r8: return false;
				case OpCode.Conv_u: return false;
				case OpCode.Conv_u1: return ConvertU1(context, stack);
				case OpCode.Conv_u2: return ConvertU2(context, stack);
				case OpCode.Conv_u4: return ConvertU4(context, stack);
				case OpCode.Conv_u8: return ConvertU8(context, stack);
				case OpCode.Cpblk: return false;
				case OpCode.Cpobj: return false;
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
				case OpCode.Ldarg: return false;
				case OpCode.Ldarg_0: return false;
				case OpCode.Ldarg_1: return false;
				case OpCode.Ldarg_2: return false;
				case OpCode.Ldarg_3: return false;
				case OpCode.Ldarg_s: return false;
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
				case OpCode.Ldloc: return false;
				case OpCode.Ldloc_0: return false;
				case OpCode.Ldloc_1: return false;
				case OpCode.Ldloc_2: return false;
				case OpCode.Ldloc_3: return false;
				case OpCode.Ldloc_s: return false;
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
				case OpCode.Newarr: return false;
				case OpCode.Newobj: return false;
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
				case OpCode.Shl: return false;
				case OpCode.Shr: return false;
				case OpCode.Shr_un: return false;
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
				case OpCode.Stloc: return false;
				case OpCode.Stloc_0: return false;
				case OpCode.Stloc_1: return false;
				case OpCode.Stloc_2: return false;
				case OpCode.Stloc_3: return false;
				case OpCode.Stloc_s: return false;
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
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			context.SetInstruction(IRInstruction.Move32, result, CreateConstant32(value));
			stack.Push(new StackEntry(StackType.Int32, result));
			return true;
		}

		private bool Constant64(Context context, Stack<StackEntry> stack, long value)
		{
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
			context.SetInstruction(IRInstruction.Move64, result, CreateConstant64(value));
			stack.Push(new StackEntry(StackType.Int64, result));
			return true;
		}

		private bool ConstantNull(Context context, Stack<StackEntry> stack)
		{
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);
			context.SetInstruction(IRInstruction.MoveObject, result, Operand.GetNullObject(TypeSystem));
			stack.Push(new StackEntry(StackType.Object, result));
			return true;
		}

		private bool ConstantR4(Context context, Stack<StackEntry> stack, float value)
		{
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
			context.SetInstruction(IRInstruction.MoveR4, result, CreateConstantR4(value));
			stack.Push(new StackEntry(StackType.R4, result));
			return true;
		}

		private bool ConstantR8(Context context, Stack<StackEntry> stack, double value)
		{
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R8);
			context.SetInstruction(IRInstruction.MoveR8, result, CreateConstantR8(value));
			stack.Push(new StackEntry(StackType.R8, result));
			return true;
		}

		#endregion CIL Shortcuts

		#region CIL

		private bool Add(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			if (entry1.StackType == StackType.R4 && entry2.StackType == StackType.R4 && entry1.Operand.IsR4)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
				context.AppendInstruction(IRInstruction.AddR4, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R4, result));
				return true;
			}

			if (entry1.StackType == StackType.R8 && entry2.StackType == StackType.R8 && entry1.Operand.IsR8)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R8);
				context.AppendInstruction(IRInstruction.AddR8, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R8, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int32)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Add32, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int64)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.Add64, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.Add64, result, v1, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
				context.AppendInstruction(IRInstruction.Add64, result, entry1.Operand, v1);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if ((entry1.StackType == StackType.Int32 && entry2.StackType == StackType.ManagedPointer && Is32BitPlatform)
				|| (entry1.StackType == StackType.ManagedPointer && entry2.StackType == StackType.Int32 && Is32BitPlatform))
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Add32, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.ManagedPointer, result));
				return true;
			}

			if ((entry1.StackType == StackType.Int64 && entry2.StackType == StackType.ManagedPointer && Is64BitPlatform)
				|| (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.ManagedPointer && Is64BitPlatform))
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.Add64, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.ManagedPointer, result));
				return true;
			}

			if ((entry1.StackType == StackType.Int32 && entry2.StackType == StackType.ManagedPointer && Is64BitPlatform))
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.Add64, result, v1, entry2.Operand);
				stack.Push(new StackEntry(StackType.ManagedPointer, result));
				return true;
			}

			if ((entry1.StackType == StackType.ManagedPointer && entry2.StackType == StackType.Int32 && Is64BitPlatform))
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
				context.AppendInstruction(IRInstruction.Add32, result, entry1.Operand, v1);
				stack.Push(new StackEntry(StackType.ManagedPointer, result));
				return true;
			}

			return false;
		}

		private bool And(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int32)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.And32, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int64)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.And64, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.And64, result, v1, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
				context.AppendInstruction(IRInstruction.And64, result, entry1.Operand, v1);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			return false;
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

			if (entry1.StackType == StackType.R4 && entry2.StackType == StackType.R4 && entry1.Operand.IsR4)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.CompareR4, result, entry1.Operand, entry2.Operand);
				context.AppendInstruction(IRInstruction.Branch32, ConditionCode.NotZero, null, result, ConstantZero32, block);
				context.AppendInstruction(IRInstruction.Jmp, nextblock);
				return true;
			}

			if (entry1.StackType == StackType.R8 && entry2.StackType == StackType.R8 && entry1.Operand.IsR8)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.CompareR8, conditionCode, result, entry1.Operand, entry2.Operand);
				context.AppendInstruction(IRInstruction.Branch32, ConditionCode.NotZero, null, result, ConstantZero32, block);
				context.AppendInstruction(IRInstruction.Jmp, nextblock);
				return true;
			}

			if (entry1.StackType == StackType.Object && entry2.StackType == StackType.Object)
			{
				context.AppendInstruction(IRInstruction.BranchObject, conditionCode, null, entry1.Operand, entry2.Operand, block);
				context.AppendInstruction(IRInstruction.Jmp, nextblock);
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int32)
			{
				context.AppendInstruction(IRInstruction.Branch32, conditionCode, null, entry1.Operand, entry2.Operand, block);
				context.AppendInstruction(IRInstruction.Jmp, nextblock);
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int64)
			{
				context.AppendInstruction(IRInstruction.Branch64, conditionCode, null, entry1.Operand, entry2.Operand, block);
				context.AppendInstruction(IRInstruction.Jmp, nextblock);
				return true;
			}

			// TODO: Managed Pointers

			return false;
		}

		private bool Branch1(Context context, Stack<StackEntry> stack, ConditionCode conditionCode, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();

			var target = (int)instruction.Operand;
			var block = BasicBlocks.GetByLabel(target);
			var nextblock = BasicBlocks.GetByLabel(instruction.Next.Value);

			if (entry1.StackType == StackType.Int32)
			{
				context.AppendInstruction(IRInstruction.Branch32, conditionCode, null, entry1.Operand, ConstantZero32, block);
				context.AppendInstruction(IRInstruction.Jmp, nextblock);
				return true;
			}

			if (entry1.StackType == StackType.Int64)
			{
				context.AppendInstruction(IRInstruction.Branch64, conditionCode, null, entry1.Operand, ConstantZero64, block);
				context.AppendInstruction(IRInstruction.Jmp, nextblock);
				return true;
			}

			if (entry1.StackType == StackType.Object)
			{
				context.AppendInstruction(IRInstruction.BranchObject, conditionCode, null, entry1.Operand, ConstantZero, block);
				context.AppendInstruction(IRInstruction.Jmp, nextblock);
				return true;
			}

			return false;
		}

		private bool Compare(Context context, Stack<StackEntry> stack, ConditionCode conditionCode)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			stack.Push(new StackEntry(StackType.Int32, result));

			if (entry1.StackType == StackType.R4 && entry2.StackType == StackType.R4 && entry1.Operand.IsR4)
			{
				context.AppendInstruction(IRInstruction.CompareR4, result, entry1.Operand, entry2.Operand);
				return true;
			}

			if (entry1.StackType == StackType.R8 && entry2.StackType == StackType.R8 && entry1.Operand.IsR8)
			{
				context.AppendInstruction(IRInstruction.CompareR8, conditionCode, result, entry1.Operand, entry2.Operand);
				return true;
			}

			if (entry1.StackType == StackType.Object && entry2.StackType == StackType.Object)
			{
				context.AppendInstruction(IRInstruction.CompareObject, result, entry1.Operand, entry2.Operand);
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int32)
			{
				context.AppendInstruction(IRInstruction.Compare32x32, conditionCode, result, entry1.Operand, entry2.Operand);
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int64)
			{
				context.AppendInstruction(IRInstruction.Compare64x32, conditionCode, result, entry1.Operand, entry2.Operand);
				return true;
			}

			// TODO: Managed Pointers

			return false;
		}

		private bool ConvertI1(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			stack.Push(new StackEntry(StackType.Int32, result));

			if (entry1.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.And32, v1, entry1.Operand, CreateConstant32(0xFF));
				context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(0xFFFFFF00));
				return true;
			}

			if (entry1.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFF));
				context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFFFF00));
				return true;
			}

			if (entry1.StackType == StackType.R4)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.ConvertR4ToI32, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFF));
				context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFFFF00));
				return true;
			}

			if (entry1.StackType == StackType.R8)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.ConvertR8ToI32, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFF));
				context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFFFF00));
				return true;
			}
			return false;
		}

		private bool ConvertI2(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			stack.Push(new StackEntry(StackType.Int32, result));

			if (entry1.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.And32, v1, entry1.Operand, CreateConstant32(0xFFFF));
				context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(0xFFFF0000));
				return true;
			}

			if (entry1.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFFFF));
				context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFF0000));
				return true;
			}

			if (entry1.StackType == StackType.R4)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.ConvertR4ToI32, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFFFF));
				context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFF0000));
				return true;
			}

			if (entry1.StackType == StackType.R8)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.ConvertR8ToI32, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFFFF));
				context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFF0000));
				return true;
			}

			return false;
		}

		private bool ConvertI4(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			stack.Push(new StackEntry(StackType.Int32, result));

			if (entry1.StackType == StackType.Int32)
			{
				context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
				return true;
			}

			if (entry1.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.Move32, result, v1);
				return true;
			}

			if (entry1.StackType == StackType.R4)
			{
				context.AppendInstruction(IRInstruction.ConvertR4ToI32, result, entry1.Operand);
				return true;
			}

			if (entry1.StackType == StackType.R8)
			{
				context.AppendInstruction(IRInstruction.ConvertR8ToI32, result, entry1.Operand);
				return true;
			}

			return false;
		}

		private bool ConvertI8(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
			stack.Push(new StackEntry(StackType.Int64, result));

			if (entry1.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.Move32, result, v1);
				return true;
			}

			if (entry1.StackType == StackType.Int64)
			{
				context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
				return true;
			}

			if (entry1.StackType == StackType.R4)
			{
				context.AppendInstruction(IRInstruction.ConvertR4ToI64, result, entry1.Operand);
				return true;
			}

			if (entry1.StackType == StackType.R8)
			{
				context.AppendInstruction(IRInstruction.ConvertR8ToI64, result, entry1.Operand);
				return true;
			}

			return false;
		}

		private bool ConvertI(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			if (Is32BitPlatform)
			{
				stack.Push(new StackEntry(StackType.Int32, result));

				if (entry1.StackType == StackType.Int32)
				{
					context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
					return true;
				}

				if (entry1.StackType == StackType.Int64)
				{
					var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
					context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Move32, result, v1);
					return true;
				}

				if (entry1.StackType == StackType.R4)
				{
					context.AppendInstruction(IRInstruction.ConvertR4ToI32, result, entry1.Operand);
					return true;
				}

				if (entry1.StackType == StackType.R8)
				{
					context.AppendInstruction(IRInstruction.ConvertR8ToI32, result, entry1.Operand);
					return true;
				}
			}
			else
			{
				stack.Push(new StackEntry(StackType.Int64, result));

				if (entry1.StackType == StackType.Int32)
				{
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry1.Operand);
					return true;
				}

				if (entry1.StackType == StackType.Int64)
				{
					context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);
					return true;
				}

				if (entry1.StackType == StackType.R4)
				{
					context.AppendInstruction(IRInstruction.ConvertR4ToI64, result, entry1.Operand);
					return true;
				}

				if (entry1.StackType == StackType.R8)
				{
					context.AppendInstruction(IRInstruction.ConvertR8ToI64, result, entry1.Operand);
					return true;
				}
			}

			return false;
		}

		private bool ConvertU(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			if (Is32BitPlatform)
			{
				stack.Push(new StackEntry(StackType.Int32, result));

				if (entry1.StackType == StackType.Int32)
				{
					context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
					return true;
				}

				if (entry1.StackType == StackType.Int64)
				{
					var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
					context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Move32, result, v1);
					return true;
				}

				if (entry1.StackType == StackType.R4)
				{
					context.AppendInstruction(IRInstruction.ConvertR4ToI32, result, entry1.Operand);
					return true;
				}

				if (entry1.StackType == StackType.R8)
				{
					context.AppendInstruction(IRInstruction.ConvertR8ToI32, result, entry1.Operand);
					return true;
				}

				// TODO: Float
			}
			else
			{
				stack.Push(new StackEntry(StackType.Int64, result));

				if (entry1.StackType == StackType.Int32)
				{
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry1.Operand);
					return true;
				}

				if (entry1.StackType == StackType.Int64)
				{
					context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);
					return true;
				}

				if (entry1.StackType == StackType.R4)
				{
					context.AppendInstruction(IRInstruction.ConvertR4ToI64, result, entry1.Operand);
					return true;
				}

				if (entry1.StackType == StackType.R8)
				{
					context.AppendInstruction(IRInstruction.ConvertR8ToI64, result, entry1.Operand);
					return true;
				}

				// TODO: Float
			}

			return false;
		}

		private bool ConvertU1(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			stack.Push(new StackEntry(StackType.Int32, result));

			if (entry1.StackType == StackType.Int32)
			{
				context.AppendInstruction(IRInstruction.And32, result, entry1.Operand, CreateConstant32(0xFF));
				return true;
			}

			if (entry1.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
				return true;
			}

			if (entry1.StackType == StackType.R4)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.ConvertR4ToI32, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
				return true;
			}

			if (entry1.StackType == StackType.R8)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.ConvertR8ToI32, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
				return true;
			}

			return false;
		}

		private bool ConvertU2(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			stack.Push(new StackEntry(StackType.Int32, result));

			if (entry1.StackType == StackType.Int32)
			{
				context.AppendInstruction(IRInstruction.And32, result, entry1.Operand, CreateConstant32(0xFFFF));
				return true;
			}

			if (entry1.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));
				return true;
			}

			if (entry1.StackType == StackType.R4)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.ConvertR4ToI32, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));

				return true;
			}

			if (entry1.StackType == StackType.R8)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.ConvertR8ToI32, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));

				return true;
			}

			return false;
		}

		private bool ConvertU4(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			stack.Push(new StackEntry(StackType.Int32, result));

			if (entry1.StackType == StackType.Int32)
			{
				context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
				return true;
			}

			if (entry1.StackType == StackType.Int64)
			{
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry1.Operand);
				return true;
			}

			if (entry1.StackType == StackType.R4)
			{
				context.AppendInstruction(IRInstruction.ConvertR4ToI32, result, entry1.Operand);
				return true;
			}

			if (entry1.StackType == StackType.R8)
			{
				context.AppendInstruction(IRInstruction.ConvertR8ToI32, result, entry1.Operand);
				return true;
			}

			return false;
		}

		private bool ConvertU8(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
			stack.Push(new StackEntry(StackType.Int64, result));

			if (entry1.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.Move32, result, v1);
				return true;
			}

			if (entry1.StackType == StackType.Int64)
			{
				context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);
				return true;
			}

			if (entry1.StackType == StackType.R4)
			{
				context.AppendInstruction(IRInstruction.ConvertR4ToI64, result, entry1.Operand);
				return true;
			}

			if (entry1.StackType == StackType.R8)
			{
				context.AppendInstruction(IRInstruction.ConvertR8ToI64, result, entry1.Operand);
				return true;
			}

			return false;
		}

		private bool Div(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			if (entry1.StackType == StackType.R4 && entry2.StackType == StackType.R4 && entry1.Operand.IsR4)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
				context.AppendInstruction(IRInstruction.DivR4, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R4, result));
				return true;
			}

			if (entry1.StackType == StackType.R8 && entry2.StackType == StackType.R8 && entry1.Operand.IsR8)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R8);
				context.AppendInstruction(IRInstruction.DivR8, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R8, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int32)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.DivSigned32, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int64)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.DivSigned64, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.DivSigned64, result, v1, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
				context.AppendInstruction(IRInstruction.DivSigned64, result, entry1.Operand, v1);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			return false;
		}

		private bool DivUnsigned(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			if (entry1.StackType == StackType.R4 && entry2.StackType == StackType.R4 && entry1.Operand.IsR4)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
				context.AppendInstruction(IRInstruction.DivR4, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R4, result));
				return true;
			}

			if (entry1.StackType == StackType.R8 && entry2.StackType == StackType.R8 && entry1.Operand.IsR8)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R8);
				context.AppendInstruction(IRInstruction.DivR8, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R8, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int32)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.DivUnsigned32, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int64)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.DivUnsigned64, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.DivUnsigned64, result, v1, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
				context.AppendInstruction(IRInstruction.DivUnsigned64, result, entry1.Operand, v1);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			return false;
		}

		private bool Dup(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();

			if (entry1.StackType == StackType.Int32)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.R4)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
				context.AppendInstruction(IRInstruction.MoveR4, result, entry1.Operand);
				stack.Push(new StackEntry(StackType.R4, result));
				return true;
			}

			if (entry1.StackType == StackType.R8)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R8);
				context.AppendInstruction(IRInstruction.MoveR8, result, entry1.Operand);
				stack.Push(new StackEntry(StackType.R8, result));
				return true;
			}

			if (entry1.StackType == StackType.Object)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);
				context.AppendInstruction(IRInstruction.MoveObject, result, entry1.Operand);
				stack.Push(new StackEntry(StackType.Object, result));
				return true;
			}

			if (entry1.StackType == StackType.ManagedPointer && Is32BitPlatform)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);    // FUTURE - Use MoveManagedPointer
				stack.Push(new StackEntry(StackType.ManagedPointer, result));
				return true;
			}

			if (entry1.StackType == StackType.ManagedPointer && Is64BitPlatform)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);    // FUTURE - Use MoveManagedPointer
				stack.Push(new StackEntry(StackType.ManagedPointer, result));
				return true;
			}

			// TODO: ValueTypes

			return false;
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

		private bool Mul(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			if (entry1.StackType == StackType.R4 && entry2.StackType == StackType.R4 && entry1.Operand.IsR4)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
				context.AppendInstruction(IRInstruction.MulR4, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R4, result));
				return true;
			}

			if (entry1.StackType == StackType.R8 && entry2.StackType == StackType.R8 && entry1.Operand.IsR8)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R8);
				context.AppendInstruction(IRInstruction.MulR8, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R8, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int32)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.MulUnsigned32, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int64)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.MulUnsigned64, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.MulUnsigned64, result, v1, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
				context.AppendInstruction(IRInstruction.MulUnsigned64, result, entry1.Operand, v1);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			return false;
		}

		private bool Neg(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();

			if (entry1.StackType == StackType.R4)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
				var zero = CreateConstantR4(0);
				context.AppendInstruction(IRInstruction.SubR4, result, zero, entry1.Operand);
				stack.Push(new StackEntry(StackType.R4, result));
				return true;
			}

			if (entry1.StackType == StackType.R8)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R8);
				var zero = CreateConstantR8(0);
				context.AppendInstruction(IRInstruction.SubR8, result, zero, entry1.Operand);
				stack.Push(new StackEntry(StackType.R8, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Sub32, result, ConstantZero32, entry1.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.Sub32, result, ConstantZero64, entry1.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			return false;
		}

		private bool Not(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();

			if (entry1.StackType == StackType.Int32)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Not32, result, entry1.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.Not64, result, entry1.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			return false;
		}

		private bool Or(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int32)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Or32, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int64)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.Or64, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.Or64, result, v1, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
				context.AppendInstruction(IRInstruction.Or64, result, entry1.Operand, v1);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			return false;
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

			if (entry1.StackType == StackType.R4 && entry2.StackType == StackType.R4 && entry1.Operand.IsR4)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
				context.AppendInstruction(IRInstruction.RemR4, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R4, result));
				return true;
			}

			if (entry1.StackType == StackType.R8 && entry2.StackType == StackType.R8 && entry1.Operand.IsR8)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R8);
				context.AppendInstruction(IRInstruction.RemR8, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R8, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int32)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.RemSigned32, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int64)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.RemSigned64, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.RemSigned64, result, v1, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
				context.AppendInstruction(IRInstruction.RemSigned64, result, entry1.Operand, v1);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			return false;
		}

		private bool RemUnsigned(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			if (entry1.StackType == StackType.R4 && entry2.StackType == StackType.R4 && entry1.Operand.IsR4)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
				context.AppendInstruction(IRInstruction.RemR4, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R4, result));
				return true;
			}

			if (entry1.StackType == StackType.R8 && entry2.StackType == StackType.R8 && entry1.Operand.IsR8)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R8);
				context.AppendInstruction(IRInstruction.RemR8, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R8, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int32)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.RemUnsigned32, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int64)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.RemUnsigned64, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.RemUnsigned64, result, v1, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
				context.AppendInstruction(IRInstruction.RemUnsigned64, result, entry1.Operand, v1);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			return false;
		}

		private bool Ret(Context context, Stack<StackEntry> stack)
		{
			if (!Method.Signature.ReturnType.IsVoid)
				stack.Pop();

			var block = BasicBlocks.GetByLabel(BasicBlock.EpilogueLabel);
			context.AppendInstruction(IRInstruction.Jmp, block);

			return true;
		}

		private bool Sub(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			if (entry1.StackType == StackType.R4 && entry2.StackType == StackType.R4 && entry1.Operand.IsR4)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
				context.AppendInstruction(IRInstruction.SubR4, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R4, result));
				return true;
			}

			if (entry1.StackType == StackType.R8 && entry2.StackType == StackType.R8 && entry1.Operand.IsR8)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R8);
				context.AppendInstruction(IRInstruction.SubR8, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.R8, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int32)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Sub32, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int64)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.Sub64, result, v1, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
				context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, v1);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.ManagedPointer && entry2.StackType == StackType.ManagedPointer && Is32BitPlatform)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Sub32, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.ManagedPointer && entry2.StackType == StackType.ManagedPointer && Is64BitPlatform)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if ((entry1.StackType == StackType.Int32 && entry2.StackType == StackType.ManagedPointer && Is32BitPlatform)
				|| (entry1.StackType == StackType.ManagedPointer && entry2.StackType == StackType.Int32 && Is32BitPlatform))
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Sub32, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.ManagedPointer, result));
				return true;
			}

			if ((entry1.StackType == StackType.Int64 && entry2.StackType == StackType.ManagedPointer && Is64BitPlatform)
				|| (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.ManagedPointer && Is64BitPlatform))
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.ManagedPointer, result));
				return true;
			}

			if ((entry1.StackType == StackType.Int32 && entry2.StackType == StackType.ManagedPointer && Is64BitPlatform))
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.Sub64, result, v1, entry2.Operand);
				stack.Push(new StackEntry(StackType.ManagedPointer, result));
				return true;
			}

			if ((entry1.StackType == StackType.ManagedPointer && entry2.StackType == StackType.Int32 && Is64BitPlatform))
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
				context.AppendInstruction(IRInstruction.Sub32, result, entry1.Operand, v1);
				stack.Push(new StackEntry(StackType.ManagedPointer, result));
				return true;
			}

			return false;
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

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int32)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(IRInstruction.Xor32, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int32, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int64)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.Xor64, result, entry1.Operand, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int32 && entry2.StackType == StackType.Int64)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
				context.AppendInstruction(IRInstruction.Xor64, result, v1, entry2.Operand);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			if (entry1.StackType == StackType.Int64 && entry2.StackType == StackType.Int32)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I8);
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
				context.AppendInstruction(IRInstruction.Xor64, result, entry1.Operand, v1);
				stack.Push(new StackEntry(StackType.Int64, result));
				return true;
			}

			return false;
		}

		#endregion CIL
	}
}
