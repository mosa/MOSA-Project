// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

// FIXME -- not 64bit compatible

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Patches delegates
	/// </summary>
	public static class DelegatePatcher
	{
		public static bool PatchDelegate(MethodCompiler methodCompiler)
		{
			Debug.Assert(methodCompiler.Method.DeclaringType.IsDelegate);

			//if (!methodCompiler.Method.DeclaringType.IsDelegate)
			//	return false;

			switch (methodCompiler.Method.Name)
			{
				case ".ctor": PatchConstructor(methodCompiler); return true;
				case "Invoke": PatchInvoke(methodCompiler); return true;
				case "InvokeWithReturn": PatchInvoke(methodCompiler); return true;
				case "BeginInvoke": PatchBeginInvoke(methodCompiler); return true;
				case "EndInvoke": PatchEndInvoke(methodCompiler); return true;
				default: return false;
			}
		}

		private static void PatchConstructor(MethodCompiler methodCompiler)
		{
			var thisOperand = methodCompiler.Parameters[0];
			var instanceOperand = methodCompiler.Parameters[1];
			var methodPointerOperand = methodCompiler.Parameters[2];

			var methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
			int methodPointerOffset = methodCompiler.TypeLayout.GetFieldOffset(methodPointerField);
			var methodPointerOffsetOperand = methodCompiler.CreateConstant(methodPointerOffset);

			var instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
			int instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
			var instanceOffsetOperand = methodCompiler.CreateConstant(instanceOffset);

			var context = new Context(CreateMethodStructure(methodCompiler));

			var v1 = methodCompiler.CreateVirtualRegister(thisOperand.Type);
			var v2 = methodCompiler.CreateVirtualRegister(methodPointerOperand.Type);
			var v3 = methodCompiler.CreateVirtualRegister(instanceOperand.Type);

			var loadParameterInstruction = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.LoadParam32 : IRInstruction.LoadParam64;

			context.AppendInstruction(loadParameterInstruction, v1, thisOperand);
			context.AppendInstruction(loadParameterInstruction, v2, methodPointerOperand);
			context.AppendInstruction(loadParameterInstruction, v3, instanceOperand);

			var storeIntegerInstruction = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.Store32 : IRInstruction.Store64;

			context.AppendInstruction(storeIntegerInstruction, null, v1, methodPointerOffsetOperand, v2);
			context.MosaType = methodPointerOperand.Type;
			context.AppendInstruction(storeIntegerInstruction, null, v1, instanceOffsetOperand, v3);
			context.MosaType = instanceOperand.Type;
			context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		private static void PatchInvoke(MethodCompiler methodCompiler)
		{
			// check if instance is null (if so, it's a static call to the methodPointer)

			var loadInstruction = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.Load32 : IRInstruction.Load64;
			var compareInstruction = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.Compare32x32 : IRInstruction.Compare64x64;
			var branchInstruction = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.CompareIntBranch32 : IRInstruction.CompareIntBranch64;
			var nativeIntegerType = methodCompiler.Architecture.Is32BitPlatform ? methodCompiler.TypeSystem.BuiltIn.U4 : methodCompiler.TypeSystem.BuiltIn.U8;

			var methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
			int methodPointerOffset = methodCompiler.TypeLayout.GetFieldOffset(methodPointerField);
			var methodPointerOffsetOperand = methodCompiler.CreateConstant(methodPointerOffset);

			var instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
			int instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
			var instanceOffsetOperand = methodCompiler.CreateConstant(instanceOffset);

			var size = methodCompiler.Architecture.NativeInstructionSize;
			bool withReturn = (methodCompiler.Method.Signature.ReturnType == null) ? false : !methodCompiler.Method.Signature.ReturnType.IsVoid;

			var b0 = new Context(CreateMethodStructure(methodCompiler));
			var b1 = new Context(methodCompiler.BasicBlocks.CreateBlock());
			var b2 = new Context(methodCompiler.BasicBlocks.CreateBlock());
			var b3 = new Context(methodCompiler.BasicBlocks.CreateBlock());

			var vrs = new Operand[methodCompiler.Parameters.Length];

			for (int i = 0; i < methodCompiler.Parameters.Length; i++)
			{
				var type = methodCompiler.Parameters[i].Type;

				if (MosaTypeLayout.IsStoredOnStack(type))
				{
					b0.AppendInstruction(IRInstruction.LoadParamCompound, vrs[i], methodCompiler.Parameters[i]);
					b0.MosaType = type;
				}
				else
				{
					vrs[i] = methodCompiler.VirtualRegisters.Allocate(methodCompiler.Parameters[i].Type);

					var paramLoadInstruction = BaseMethodCompilerStage.GetLoadParameterInstruction(vrs[i].Type, methodCompiler.Architecture.Is32BitPlatform);

					b0.AppendInstruction(paramLoadInstruction, vrs[i], methodCompiler.Parameters[i]);
					b0.MosaType = type;
				}
			}

			var thisOperand = vrs[0];

			var opMethod = methodCompiler.VirtualRegisters.Allocate(nativeIntegerType);
			var opInstance = methodCompiler.VirtualRegisters.Allocate(thisOperand.Type);
			var opCompare = methodCompiler.VirtualRegisters.Allocate(nativeIntegerType);

			var opReturn = withReturn ? methodCompiler.AllocateVirtualRegisterOrStackSlot(methodCompiler.Method.Signature.ReturnType) : null;
			var c0 = methodCompiler.ConstantZero;

			b0.AppendInstruction(loadInstruction, opMethod, thisOperand, methodPointerOffsetOperand);
			b0.AppendInstruction(loadInstruction, opInstance, thisOperand, instanceOffsetOperand);
			b0.AppendInstruction(compareInstruction, ConditionCode.Equal, opCompare, opInstance, c0);
			b0.AppendInstruction(branchInstruction, ConditionCode.Equal, null, opCompare, c0);
			b0.AddBranchTarget(b2.Block);
			b0.AppendInstruction(IRInstruction.Jmp, b1.Block);

			var operands = new List<Operand>(methodCompiler.Parameters.Length + 1);
			for (int i = 1; i < methodCompiler.Parameters.Length; i++)
			{
				operands.Add(vrs[i]);
			}

			var result = withReturn ? opReturn : null;

			// no instance
			b1.AppendInstruction(IRInstruction.CallDynamic, result, opMethod, operands);
			b1.AppendInstruction(IRInstruction.Jmp, b3.Block);

			// instance
			b2.AppendInstruction(IRInstruction.CallDynamic, result, opMethod, opInstance, operands);
			b2.AppendInstruction(IRInstruction.Jmp, b3.Block);

			// return
			if (opReturn != null)
			{
				var setReturn = BaseMethodCompilerStage.GetSetReturnInstruction(opReturn.Type, methodCompiler.Architecture.Is32BitPlatform);
				b3.AppendInstruction(setReturn, null, opReturn);
			}

			b3.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		private static void PatchBeginInvoke(MethodCompiler methodCompiler)
		{
			var nullOperand = Operand.GetNullObject(methodCompiler.TypeSystem);
			var context = new Context(CreateMethodStructure(methodCompiler));

			var setReturn = BaseMethodCompilerStage.GetSetReturnInstruction(nullOperand.Type, methodCompiler.Architecture.Is32BitPlatform);

			context.AppendInstruction(setReturn, null, nullOperand);
			context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		private static void PatchEndInvoke(MethodCompiler methodCompiler)
		{
			var start = CreateMethodStructure(methodCompiler);

			start.First.Insert(new InstructionNode(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock));
		}

		private static BasicBlock CreateMethodStructure(MethodCompiler methodCompiler)
		{
			var basicBlocks = methodCompiler.BasicBlocks;

			// Create the prologue block
			var prologue = basicBlocks.CreateBlock(BasicBlock.PrologueLabel);
			basicBlocks.AddHeadBlock(prologue);

			// Create the epilogue block
			var epiologue = basicBlocks.CreateBlock(BasicBlock.EpilogueLabel);

			var start = basicBlocks.CreateBlock(BasicBlock.StartLabel);

			// Add a jump instruction to the first block from the prologue
			prologue.First.Insert(new InstructionNode(IRInstruction.Jmp, start));

			return start;
		}

		private static MosaField GetField(MosaType type, string name)
		{
			foreach (var field in type.Fields)
			{
				if (field.Name == name)
					return field;
			}

			return GetField(type.BaseType, name);
		}
	}
}
