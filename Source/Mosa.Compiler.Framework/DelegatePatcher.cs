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
				case "Invoke": PatchInvoke(methodCompiler, false); return true;
				case "InvokeWithReturn": PatchInvoke(methodCompiler, true); return true;
				case "BeginInvoke": PatchBeginInvoke(methodCompiler); return true;
				case "EndInvoke": PatchEndInvoke(methodCompiler); return true;
				default: return false;
			}
		}

		private static void PatchConstructor(BaseMethodCompiler methodCompiler)
		{
			Operand thisOperand = methodCompiler.Parameters[0];
			Operand instanceOperand = methodCompiler.Parameters[1];
			Operand methodPointerOperand = methodCompiler.Parameters[2];

			RuntimeField methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
			int methodPointerOffset = methodCompiler.TypeLayout.GetFieldOffset(methodPointerField);
			Operand methodPointerOffsetOperand = Operand.CreateConstant(BuiltInSigType.IntPtr, methodPointerOffset);

			RuntimeField instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
			int instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
			Operand instanceOffsetOperand = Operand.CreateConstant(BuiltInSigType.IntPtr, instanceOffset);

			Context context = CreateMethodStructure(methodCompiler, true);

			context.AppendInstruction(IRInstruction.Store, null, thisOperand, methodPointerOffsetOperand, methodPointerOperand);
			context.AppendInstruction(IRInstruction.Store, null, thisOperand, instanceOffsetOperand, instanceOperand);
			context.AppendInstruction(IRInstruction.Return, methodCompiler.BasicBlocks.EpilogueBlock);
			context.SetBranch(BasicBlock.EpilogueLabel);
		}

		private static void PatchInvoke(BaseMethodCompiler methodCompiler, bool withReturn)
		{
			// check if instance is null (if so, it's a static call to the methodPointer)

			Operand thisOperand = methodCompiler.Parameters[0];

			RuntimeField methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
			int methodPointerOffset = methodCompiler.TypeLayout.GetFieldOffset(methodPointerField);
			Operand methodPointerOffsetOperand = Operand.CreateConstant(BuiltInSigType.IntPtr, methodPointerOffset);

			RuntimeField instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
			int instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
			Operand instanceOffsetOperand = Operand.CreateConstant(BuiltInSigType.IntPtr, instanceOffset);

			Operand opMethod = methodCompiler.StackLayout.AllocateStackOperand(BuiltInSigType.UInt32, false);
			Operand opInstance = methodCompiler.StackLayout.AllocateStackOperand(thisOperand.Type, false);
			Operand opCompare = methodCompiler.StackLayout.AllocateStackOperand(BuiltInSigType.Int32, false);

			Operand opReturn = withReturn ? methodCompiler.StackLayout.AllocateStackOperand(BuiltInSigType.Object, false) : null;
			Operand c0 = Operand.CreateConstant(BuiltInSigType.Int32, 0);

			Context b0 = CreateMethodStructure(methodCompiler, false);
			Context b1 = CreateNewBlock(methodCompiler);
			Context b2 = CreateNewBlock(methodCompiler);
			Context b3 = CreateNewBlock(methodCompiler);

			b0.AppendInstruction(IRInstruction.Load, opMethod, thisOperand, methodPointerOffsetOperand);
			b0.AppendInstruction(IRInstruction.Load, opInstance, thisOperand, instanceOffsetOperand);
			b0.AppendInstruction(IRInstruction.IntegerCompare, ConditionCode.Equal, opCompare, opInstance, c0);
			b0.AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, opCompare, c0);
			b0.SetBranch(b2.BasicBlock);
			b0.AppendInstruction(IRInstruction.Jmp, b1.BasicBlock);
			methodCompiler.BasicBlocks.LinkBlocks(b0.BasicBlock, b1.BasicBlock);
			methodCompiler.BasicBlocks.LinkBlocks(b0.BasicBlock, b2.BasicBlock);

			b1.AppendInstruction(IRInstruction.Call, opReturn, opMethod);
			for (int i = 1; i < methodCompiler.Parameters.Length; i++)
			{
				b1.AddOperand(methodCompiler.Parameters[i]);
			}
			b1.AppendInstruction(IRInstruction.Jmp, b3.BasicBlock);
			methodCompiler.BasicBlocks.LinkBlocks(b1.BasicBlock, b3.BasicBlock);

			b2.AppendInstruction(IRInstruction.Call, opReturn, opMethod);
			for (int i = 1; i < methodCompiler.Parameters.Length; i++)
			{
				b2.AddOperand(methodCompiler.Parameters[i]);
			}
			b2.AddOperand(opInstance);

			b2.AppendInstruction(IRInstruction.Jmp, b3.BasicBlock);
			methodCompiler.BasicBlocks.LinkBlocks(b2.BasicBlock, b3.BasicBlock);

			b3.AppendInstruction(IRInstruction.Return, methodCompiler.BasicBlocks.EpilogueBlock);
			if (withReturn)
			{
				b3.SetOperand(0, opReturn);
			}
			methodCompiler.BasicBlocks.LinkBlocks(b3.BasicBlock, methodCompiler.BasicBlocks.EpilogueBlock);

		}

		private static void PatchBeginInvoke(BaseMethodCompiler methodCompiler)
		{
			Context context = CreateMethodStructure(methodCompiler, true);
			context.AppendInstruction(IRInstruction.Return, Operand.GetNull());
		}

		private static void PatchEndInvoke(BaseMethodCompiler methodCompiler)
		{
			Context context = CreateMethodStructure(methodCompiler, true);
			context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		private static Context CreateMethodStructure(BaseMethodCompiler methodCompiler, bool linkEpilogueBlock)
		{
			var basicBlocks = methodCompiler.BasicBlocks;
			CreatePrologueAndEpilogueBlocks(methodCompiler.InstructionSet, basicBlocks);

			Context context = new Context(methodCompiler.InstructionSet);
			context.AppendInstruction(null);
			context.Label = 0;

			var newblock = basicBlocks.CreateBlock(0, context.Index);
			basicBlocks.LinkBlocks(basicBlocks.PrologueBlock, newblock);
			if (linkEpilogueBlock)
				basicBlocks.LinkBlocks(newblock, basicBlocks.EpilogueBlock);
			context.BasicBlock = newblock;

			return context;
		}

		private static Context CreateNewBlock(BaseMethodCompiler methodCompiler)
		{
			var basicBlocks = methodCompiler.BasicBlocks;

			Context context = new Context(methodCompiler.InstructionSet);
			context.AppendInstruction(null);
			context.Label = 0;

			var newblock = basicBlocks.CreateBlock(basicBlocks.Count, context.Index);
			context.BasicBlock = newblock;

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
