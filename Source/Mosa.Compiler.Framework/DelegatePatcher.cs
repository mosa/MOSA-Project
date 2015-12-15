// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
				case "Invoke": PatchInvoke(methodCompiler); return true;
				case "InvokeWithReturn": PatchInvoke(methodCompiler); return true;
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

			var size = methodCompiler.Architecture.NativeInstructionSize;

			MosaField methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
			int methodPointerOffset = methodCompiler.TypeLayout.GetFieldOffset(methodPointerField);
			Operand methodPointerOffsetOperand = Operand.CreateConstant(methodCompiler.TypeSystem, methodPointerOffset);

			MosaField instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
			int instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
			Operand instanceOffsetOperand = Operand.CreateConstant(methodCompiler.TypeSystem, instanceOffset);

			var context = new Context(CreateMethodStructure(methodCompiler, true));

			Operand v1 = methodCompiler.CreateVirtualRegister(thisOperand.Type);

			context.AppendInstruction(IRInstruction.Move, v1, thisOperand);
			context.AppendInstruction(IRInstruction.Store, size, null, v1, methodPointerOffsetOperand, methodPointerOperand);
			context.MosaType = methodPointerOperand.Type;
			context.AppendInstruction(IRInstruction.Store, size, null, v1, instanceOffsetOperand, instanceOperand);
			context.MosaType = instanceOperand.Type;
			context.AppendInstruction(IRInstruction.Return, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		private static void PatchInvoke(BaseMethodCompiler methodCompiler)
		{
			// check if instance is null (if so, it's a static call to the methodPointer)

			MosaField methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
			int methodPointerOffset = methodCompiler.TypeLayout.GetFieldOffset(methodPointerField);
			Operand methodPointerOffsetOperand = Operand.CreateConstant(methodCompiler.TypeSystem, methodPointerOffset);

			MosaField instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
			int instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
			Operand instanceOffsetOperand = Operand.CreateConstant(methodCompiler.TypeSystem, instanceOffset);

			var size = methodCompiler.Architecture.NativeInstructionSize;
			bool withReturn = (methodCompiler.Method.Signature.ReturnType != null);

			Context b0 = new Context(CreateMethodStructure(methodCompiler, false));
			Context b1 = new Context(methodCompiler.BasicBlocks.CreateBlock());
			Context b2 = new Context(methodCompiler.BasicBlocks.CreateBlock());
			Context b3 = new Context(methodCompiler.BasicBlocks.CreateBlock());

			Operand[] vrs = new Operand[methodCompiler.Parameters.Length];

			for (int i = 0; i < methodCompiler.Parameters.Length; i++)
			{
				vrs[i] = methodCompiler.VirtualRegisters.Allocate(methodCompiler.Parameters[i].Type);
				b0.AppendInstruction(IRInstruction.Move, vrs[i], methodCompiler.Parameters[i]);
			}

			Operand thisOperand = vrs[0];

			Operand opMethod = methodCompiler.VirtualRegisters.Allocate(methodCompiler.TypeSystem.BuiltIn.U4);
			Operand opInstance = methodCompiler.VirtualRegisters.Allocate(thisOperand.Type);
			Operand opCompare = methodCompiler.VirtualRegisters.Allocate(methodCompiler.TypeSystem.BuiltIn.I4);

			Operand opReturn = withReturn ? methodCompiler.VirtualRegisters.Allocate(methodCompiler.Method.Signature.ReturnType) : null;
			Operand c0 = Operand.CreateConstant(methodCompiler.TypeSystem, 0);

			b0.AppendInstruction(IRInstruction.Load, size, opMethod, thisOperand, methodPointerOffsetOperand);
			b0.AppendInstruction(IRInstruction.Load, size, opInstance, thisOperand, instanceOffsetOperand);
			b0.AppendInstruction(IRInstruction.IntegerCompare, ConditionCode.Equal, opCompare, opInstance, c0);
			b0.AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, opCompare, c0);
			b0.AddBranchTarget(b2.Block);
			b0.AppendInstruction(IRInstruction.Jmp, b1.Block);

			// no instance
			b1.AppendInstruction(IRInstruction.Call, opReturn, opMethod);
			b1.InvokeMethod = methodCompiler.Method;
			for (int i = 1; i < methodCompiler.Parameters.Length; i++)
				b1.AddOperand(vrs[i]);
			if (withReturn)
				b1.SetResult(0, opReturn);
			b1.AppendInstruction(IRInstruction.Jmp, b3.Block);

			// instance
			b2.AppendInstruction(IRInstruction.Call, opReturn, opMethod);
			b2.InvokeMethod = methodCompiler.Method;
			b2.AddOperand(opInstance);
			for (int i = 1; i < methodCompiler.Parameters.Length; i++)
				b2.AddOperand(vrs[i]);
			if (withReturn)
				b2.SetResult(0, opReturn);
			b2.AppendInstruction(IRInstruction.Jmp, b3.Block);

			// return
			b3.AppendInstruction(IRInstruction.Return, methodCompiler.BasicBlocks.EpilogueBlock);
			if (withReturn)
				b3.SetOperand(0, opReturn);
		}

		private static void PatchBeginInvoke(BaseMethodCompiler methodCompiler)
		{
			var nullOperand = Operand.GetNull(methodCompiler.TypeSystem);

			var context = new Context(CreateMethodStructure(methodCompiler, true));
			context.AppendInstruction(IRInstruction.Return, null, nullOperand);
			context.AddBranchTarget(methodCompiler.BasicBlocks.EpilogueBlock);
		}

		private static void PatchEndInvoke(BaseMethodCompiler methodCompiler)
		{
			var start = CreateMethodStructure(methodCompiler, true);

			start.First.Insert(new InstructionNode(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock));
		}

		private static BasicBlock CreateMethodStructure(BaseMethodCompiler methodCompiler, bool linkEpilogueBlock)
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
