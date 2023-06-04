﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.MosaTypeSystem;

// FIXME -- not 64bit compatible

namespace Mosa.Compiler.Framework;

/// <summary>
/// Patches delegates
/// </summary>
public static class DelegatePatcher
{
	public static bool Patch(MethodCompiler methodCompiler)
	{
		Debug.Assert(methodCompiler.Method.DeclaringType.IsDelegate);

		if (!methodCompiler.Method.DeclaringType.IsDelegate)
			return false;

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
		CILDecoderStage.CreateParameters(methodCompiler);

		var thisOperand = methodCompiler.Parameters[0];
		var instanceOperand = methodCompiler.Parameters[1];
		var methodPointerOperand = methodCompiler.Parameters[2];

		var methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
		var methodPointerOffset = methodCompiler.TypeLayout.GetFieldOffset(methodPointerField);
		var methodPointerOffsetOperand = Operand.CreateConstant32(methodPointerOffset);

		var instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
		var instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
		var instanceOffsetOperand = Operand.CreateConstant32(instanceOffset);

		var context = new Context(CreateMethodStructure(methodCompiler));

		var v1 = methodCompiler.VirtualRegisters.AllocateObject();
		var v2 = methodCompiler.VirtualRegisters.AllocateNativeInteger();
		var v3 = methodCompiler.VirtualRegisters.AllocateObject();

		var loadParameterInstruction = methodCompiler.Is32BitPlatform
			? IRInstruction.LoadParam32
			: IRInstruction.LoadParam64;

		context.AppendInstruction(loadParameterInstruction, v1, thisOperand);
		context.AppendInstruction(loadParameterInstruction, v2, methodPointerOperand);
		context.AppendInstruction(loadParameterInstruction, v3, instanceOperand);

		var storeIntegerInstruction = methodCompiler.Is32BitPlatform
			? IRInstruction.Store32
			: IRInstruction.Store64;

		context.AppendInstruction(storeIntegerInstruction, null, v1, methodPointerOffsetOperand, v2);
		context.AppendInstruction(storeIntegerInstruction, null, v1, instanceOffsetOperand, v3);
		context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
	}

	private static void PatchInvoke(MethodCompiler methodCompiler)
	{
		CILDecoderStage.CreateParameters(methodCompiler);

		// check if instance is null (if so, it's a static call to the methodPointer)

		var loadInstruction = methodCompiler.Is32BitPlatform
			? IRInstruction.Load32
			: IRInstruction.Load64;

		var branchInstruction = methodCompiler.Is32BitPlatform
			? IRInstruction.Branch32
			: IRInstruction.Branch64;

		var methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
		var methodPointerOffset = methodCompiler.TypeLayout.GetFieldOffset(methodPointerField);
		var methodPointerOffsetOperand = Operand.CreateConstant32(methodPointerOffset);

		var instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
		var instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
		var instanceOffsetOperand = Operand.CreateConstant32(instanceOffset);

		var b0 = new Context(CreateMethodStructure(methodCompiler));
		var b1 = new Context(methodCompiler.BasicBlocks.CreateBlock());
		var b2 = new Context(methodCompiler.BasicBlocks.CreateBlock());
		var b3 = new Context(methodCompiler.BasicBlocks.CreateBlock());

		var vrs = new Operand[methodCompiler.Parameters.Count];

		for (var i = 0; i < methodCompiler.Parameters.Count; i++)
		{
			var parameter = methodCompiler.Parameters[i];

			if (parameter.IsPrimitive)
			{
				vrs[i] = methodCompiler.VirtualRegisters.Allocate(parameter);

				var paramLoadInstruction = methodCompiler.GetLoadParamInstruction(parameter.Element);

				b0.AppendInstruction(paramLoadInstruction, vrs[i], parameter);
			}
			else
			{
				vrs[i] = methodCompiler.LocalStack.Allocate(parameter);

				b0.AppendInstruction(IRInstruction.LoadParamCompound, vrs[i], parameter);
			}
		}

		var thisOperand = vrs[0];

		var opMethod = methodCompiler.VirtualRegisters.AllocateNativeInteger();
		var opInstance = methodCompiler.VirtualRegisters.AllocateObject();
		var opCompare = methodCompiler.VirtualRegisters.AllocateNativeInteger();

		b0.AppendInstruction(loadInstruction, opMethod, thisOperand, methodPointerOffsetOperand);
		b0.AppendInstruction(loadInstruction, opInstance, thisOperand, instanceOffsetOperand);
		b0.AppendInstruction(IRInstruction.CompareObject, ConditionCode.Equal, opCompare, opInstance, Operand.NullObject);
		b0.AppendInstruction(branchInstruction, ConditionCode.Equal, null, opCompare, methodCompiler.ConstantZero, b2.Block);
		b0.AppendInstruction(IRInstruction.Jmp, b1.Block);

		var operands = new List<Operand>(methodCompiler.Parameters.Count + 1);

		for (var i = 1; i < methodCompiler.Parameters.Count; i++)
		{
			operands.Add(vrs[i]);
		}

		var returnType = methodCompiler.Method.Signature.ReturnType;

		var result = returnType.IsVoid ? null : methodCompiler.AllocateVirtualRegisterOrStackLocal(returnType);

		// no instance
		b1.AppendInstruction(IRInstruction.CallDynamic, result, opMethod, operands);
		b1.AppendInstruction(IRInstruction.Jmp, b3.Block);

		// instance
		b2.AppendInstruction(IRInstruction.CallDynamic, result, opMethod, opInstance, operands);
		b2.AppendInstruction(IRInstruction.Jmp, b3.Block);

		// return
		if (result != null)
		{
			var setReturn = methodCompiler.GetReturnInstruction(result.Primitive);
			b3.AppendInstruction(setReturn, null, result);
		}

		b3.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
	}

	private static void PatchBeginInvoke(MethodCompiler methodCompiler)
	{
		var context = new Context(CreateMethodStructure(methodCompiler));

		var setReturn = methodCompiler.GetReturnInstruction(PrimitiveType.Object);

		context.AppendInstruction(setReturn, null, Operand.NullObject);
		context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
	}

	private static void PatchEndInvoke(MethodCompiler methodCompiler)
	{
		var start = CreateMethodStructure(methodCompiler);

		start.First.Insert(new InstructionNode(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock));
	}

	private static BasicBlock CreateMethodStructure(MethodCompiler methodCompiler)
	{
		// Create the blocks
		var prologueBlock = methodCompiler.BasicBlocks.CreatePrologueBlock();
		var startBlock = methodCompiler.BasicBlocks.CreateStartBlock();
		var epilogueBlock = methodCompiler.BasicBlocks.CreateEpilogueBlock();

		var prologue = new Context(prologueBlock);
		prologue.AppendInstruction(IRInstruction.Prologue);
		prologue.AppendInstruction(IRInstruction.Jmp, startBlock);

		var epilogue = new Context(epilogueBlock);
		epilogue.AppendInstruction(IRInstruction.Epilogue);

		return startBlock;
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
