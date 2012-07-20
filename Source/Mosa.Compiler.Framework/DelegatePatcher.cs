/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System.Collections.Generic;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// 
	/// </summary>
	public static class DelegatePatcher
	{

		/// <summary>
		/// Patches the delegate.
		/// </summary>
		/// <param name="methodCompiler">The method compiler.</param>
		/// <returns></returns>
		public static bool PatchDelegate(BaseMethodCompiler methodCompiler)
		{
			if (!methodCompiler.Method.DeclaringType.IsDelegate)
				return false;

			switch (methodCompiler.Method.Name)
			{
				case ".ctor": PatchConstructor(methodCompiler); return true;
				case "Invoke": PatchInvoke(methodCompiler); return false;
				case "BeginInvoke": PatchBeginInvoke(methodCompiler); return true;
				case "EndInvoke": PatchEndInvoke(methodCompiler); return true;
				default: return false;
			}
		}

		private static void PatchConstructor(BaseMethodCompiler methodCompiler)
		{
			Context context = CreateMethodStructure(methodCompiler);

			Operand thisOperand = methodCompiler.Parameters[0];
			Operand instanceOperand = methodCompiler.Parameters[1];
			Operand methodPointerOperand = methodCompiler.Parameters[2];

			RuntimeField instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
			int instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
			Operand instanceOffsetOperand = Operand.CreateConstant(BuiltInSigType.IntPtr, instanceOffset);

			RuntimeField methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
			int methodPointerOffset = methodCompiler.TypeLayout.GetFieldOffset(methodPointerField);
			Operand methodPointerOffsetOperand = Operand.CreateConstant(BuiltInSigType.IntPtr, methodPointerOffset);

			context.AppendInstruction(IRInstruction.Store, null, thisOperand, instanceOffsetOperand, instanceOperand);
			context.AppendInstruction(IRInstruction.Store, null, thisOperand, methodPointerOffsetOperand, methodPointerOperand);
			context.AppendInstruction(IRInstruction.Return);
			context.SetBranch(BasicBlock.EpilogueLabel);
		}

		private static void PatchInvoke(BaseMethodCompiler methodCompiler)
		{
			// check if instance is null (if so, it's a static call to the methodPointer)

			// let's get the field
			RuntimeField instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
			var instanceOperand = methodCompiler.CreateVirtualRegister(instanceField.SignatureType);
			int instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
			Operand instanceOffsetOperand = Operand.CreateConstant(BuiltInSigType.IntPtr, instanceOffset);

			RuntimeField methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
			var methodPointerOperand = methodCompiler.CreateVirtualRegister(methodPointerField.SignatureType);

			//Context context = CreateMethodStructure(methodCompiler);

			//context.AppendInstruction(IRInstruction.Load, result, objectOperand, instanceOffsetOperand);
			//context.SigType = instanceField.SignatureType;
			//context.AppendInstruction(IRInstruction.Move, resultOperand, result);

			//Operand target = methodCompiler.CreateVirtualRegister(BuiltInSigType.Ptr);
			//Operand result = methodCompiler.CreateVirtualRegister(BuiltInSigType.Ptr);

			return;
		}


		private static void PatchBeginInvoke(BaseMethodCompiler methodCompiler)
		{
			Context context = CreateMethodStructure(methodCompiler);
			context.AppendInstruction(IRInstruction.Return, Operand.GetNull());
		}

		private static void PatchEndInvoke(BaseMethodCompiler methodCompiler)
		{
			Context context = CreateMethodStructure(methodCompiler);
			context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		private static Context CreateMethodStructure(BaseMethodCompiler methodCompiler)
		{
			var basicBlocks = methodCompiler.BasicBlocks;
			CreatePrologueAndEpilogueBlocks(methodCompiler.InstructionSet, basicBlocks);

			Context context = new Context(methodCompiler.InstructionSet);
			context.AppendInstruction(null);
			context.Label = 0;

			var newblock = basicBlocks.CreateBlock(0, context.Index);
			basicBlocks.LinkBlocks(basicBlocks.PrologueBlock, newblock);
			basicBlocks.LinkBlocks(newblock, basicBlocks.EpilogueBlock);

			return context;
		}

		private static void CreatePrologueAndEpilogueBlocks(InstructionSet instructionSet, BasicBlocks basicBlocks)
		{
			// Create the prologue block
			Context context = new Context(instructionSet);
			// Add a jump instruction to the first block from the prologue
			context.AppendInstruction(IRInstruction.Jmp);
			context.SetBranch(0);
			context.Label = BasicBlock.PrologueLabel;
			var prologue = basicBlocks.CreateBlock(BasicBlock.PrologueLabel, context.Index);
			basicBlocks.AddHeaderBlock(prologue);

			// Create the epilogue block
			context = new Context(instructionSet);
			// Add null instruction, necessary to generate a block index
			context.AppendInstruction(null);
			context.Label = BasicBlock.EpilogueLabel;
			var epilogue = basicBlocks.CreateBlock(BasicBlock.EpilogueLabel, context.Index);
		}

		private static RuntimeField GetField(RuntimeType type, string name)
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
