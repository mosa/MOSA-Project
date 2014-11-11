/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;

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

			var size = methodCompiler.Architecture.NativePointerSize == 4 ? InstructionSize.Size32 : InstructionSize.Size64;

			MosaField methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
			int methodPointerOffset = methodCompiler.TypeLayout.GetFieldOffset(methodPointerField);
			Operand methodPointerOffsetOperand = Operand.CreateConstantSignedInt(methodCompiler.TypeSystem, methodPointerOffset);

			MosaField instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
			int instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
			Operand instanceOffsetOperand = Operand.CreateConstantSignedInt(methodCompiler.TypeSystem, instanceOffset);

			Context context = CreateMethodStructure(methodCompiler, true);

			Operand v1 = methodCompiler.CreateVirtualRegister(thisOperand.Type);

			context.AppendInstruction(IRInstruction.Move, v1, thisOperand);
			context.AppendInstruction(IRInstruction.Store, size, null, v1, methodPointerOffsetOperand, methodPointerOperand);
			context.MosaType = methodPointerOperand.Type;
			context.AppendInstruction(IRInstruction.Store, size, null, v1, instanceOffsetOperand, instanceOperand);
			context.MosaType = instanceOperand.Type; 
			context.AppendInstruction(IRInstruction.Return, methodCompiler.BasicBlocks.EpilogueBlock);
			context.SetBranch(BasicBlock.EpilogueLabel);
		}

		private static void PatchInvoke(BaseMethodCompiler methodCompiler, bool withReturn)
		{
			// check if instance is null (if so, it's a static call to the methodPointer)

			MosaField methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
			int methodPointerOffset = methodCompiler.TypeLayout.GetFieldOffset(methodPointerField);
			Operand methodPointerOffsetOperand = Operand.CreateConstantSignedInt(methodCompiler.TypeSystem, methodPointerOffset);

			MosaField instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
			int instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
			Operand instanceOffsetOperand = Operand.CreateConstantSignedInt(methodCompiler.TypeSystem, instanceOffset);

			var size = methodCompiler.Architecture.NativePointerSize == 4 ? InstructionSize.Size32 : InstructionSize.Size64;

			Context b0 = CreateMethodStructure(methodCompiler, false);
			Context b1 = CreateNewBlock(methodCompiler);
			Context b2 = CreateNewBlock(methodCompiler);
			Context b3 = CreateNewBlock(methodCompiler);

			Operand[] vrs = new Operand[methodCompiler.Parameters.Length];

			for (int i = 0; i < methodCompiler.Parameters.Length; i++)
			{
				vrs[i] = methodCompiler.VirtualRegisters.Allocate(methodCompiler.Parameters[i].Type);

				if (methodCompiler.TypeLayout.IsCompoundType(methodCompiler.Parameters[i].Type))
				{
					b0.AppendInstruction(IRInstruction.CompoundMove, vrs[i], methodCompiler.Parameters[i]);
				}
				else
				{
					b0.AppendInstruction(IRInstruction.Move, vrs[i], methodCompiler.Parameters[i]);
				}
			}

			Operand thisOperand = vrs[0];

			Operand opMethod = methodCompiler.VirtualRegisters.Allocate(methodCompiler.TypeSystem.BuiltIn.U4);
			Operand opInstance = methodCompiler.VirtualRegisters.Allocate(thisOperand.Type);
			Operand opCompare = methodCompiler.VirtualRegisters.Allocate(methodCompiler.TypeSystem.BuiltIn.I4);

			Operand opReturn = withReturn ? methodCompiler.VirtualRegisters.Allocate(methodCompiler.TypeSystem.BuiltIn.Object) : null;
			Operand c0 = Operand.CreateConstantSignedInt(methodCompiler.TypeSystem, 0);

			b0.AppendInstruction(IRInstruction.Load, size, opMethod, thisOperand, methodPointerOffsetOperand);
			b0.AppendInstruction(IRInstruction.Load, size, opInstance, thisOperand, instanceOffsetOperand);
			b0.AppendInstruction(IRInstruction.IntegerCompare, ConditionCode.Equal, opCompare, opInstance, c0);
			b0.AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, opCompare, c0);
			b0.SetBranch(b2.BasicBlock);
			b0.AppendInstruction(IRInstruction.Jmp, b1.BasicBlock);
			methodCompiler.BasicBlocks.LinkBlocks(b0.BasicBlock, b1.BasicBlock);
			methodCompiler.BasicBlocks.LinkBlocks(b0.BasicBlock, b2.BasicBlock);

			// no instance
			b1.AppendInstruction(IRInstruction.Call, opReturn, opMethod);
			b1.MosaMethod = methodCompiler.Method;
			for (int i = 1; i < methodCompiler.Parameters.Length; i++)
			{
				b1.AddOperand(vrs[i]);
			}
			b1.AppendInstruction(IRInstruction.Jmp, b3.BasicBlock);
			methodCompiler.BasicBlocks.LinkBlocks(b1.BasicBlock, b3.BasicBlock);

			// instance
			b2.AppendInstruction(IRInstruction.Call, opReturn, opMethod);
			b2.MosaMethod = methodCompiler.Method;
			b2.AddOperand(opInstance);
			for (int i = 1; i < methodCompiler.Parameters.Length; i++)
			{
				b2.AddOperand(vrs[i]);
			}
			b2.AppendInstruction(IRInstruction.Jmp, b3.BasicBlock);
			methodCompiler.BasicBlocks.LinkBlocks(b2.BasicBlock, b3.BasicBlock);

			// return
			b3.AppendInstruction(IRInstruction.Return, methodCompiler.BasicBlocks.EpilogueBlock);
			if (withReturn)
			{
				b3.SetOperand(0, opReturn);
			}
			methodCompiler.BasicBlocks.LinkBlocks(b3.BasicBlock, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		private static void PatchBeginInvoke(BaseMethodCompiler methodCompiler)
		{
			var nullOperand = Operand.GetNull(methodCompiler.TypeSystem);

			Context context = CreateMethodStructure(methodCompiler, true);
			context.AppendInstruction(IRInstruction.Return, null, nullOperand);
			context.SetBranch(methodCompiler.BasicBlocks.EpilogueBlock);
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

			var context = methodCompiler.InstructionSet.CreateNewBlock(basicBlocks, 0);

			basicBlocks.LinkBlocks(basicBlocks.PrologueBlock, context.BasicBlock);

			if (linkEpilogueBlock)
				basicBlocks.LinkBlocks(context.BasicBlock, basicBlocks.EpilogueBlock);

			return context;
		}

		private static void CreatePrologueAndEpilogueBlocks(InstructionSet instructionSet, BasicBlocks basicBlocks)
		{
			// Create the prologue block
			var context = instructionSet.CreateNewBlock(basicBlocks, BasicBlock.PrologueLabel);

			// Add a jump instruction to the first block from the prologue
			context.AppendInstruction(IRInstruction.Jmp);
			context.SetBranch(0);
			var prologue = context.BasicBlock;
			basicBlocks.AddHeaderBlock(prologue);

			// Create the epilogue block
			context = instructionSet.CreateNewBlock(basicBlocks, BasicBlock.EpilogueLabel);
			var epilogue = context.BasicBlock;
		}

		private static Context CreateNewBlock(BaseMethodCompiler methodCompiler)
		{
			return methodCompiler.InstructionSet.CreateNewBlock(methodCompiler.BasicBlocks);
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