// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Patches delegates
	/// </summary>
	public sealed class DelegatePatcher
	{
		/// <summary>
		/// The compiler
		/// </summary>
		private readonly BaseCompiler Compiler;

		/// <summary>
		/// The delegate proxy type
		/// </summary>
		private MosaType delegateProxyType;

		/// <summary>
		/// The deligate proxy methods
		/// </summary>
		public readonly Dictionary<MosaMethod, Tuple<MosaMethod, MosaMethod>> delegateProxyMethods = new Dictionary<MosaMethod, Tuple<MosaMethod, MosaMethod>>();

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegatePatcher"/> class.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public DelegatePatcher(BaseCompiler compiler)
		{
			Compiler = compiler;
		}

		private MosaMethod GetDelegateProxyMethod(MosaMethod delegateMethod, bool instance)
		{
			if (delegateProxyType == null)
			{
				delegateProxyType = Compiler.TypeSystem.CreateLinkerType("DelegateProxy");
			}

			if (!delegateProxyMethods.TryGetValue(delegateMethod, out Tuple<MosaMethod, MosaMethod> tuple))
			{
				var staticProxy = Compiler.TypeSystem.CreateLinkerMethod(delegateProxyType, delegateMethod.FullName + "@Static@Proxy", delegateMethod.Signature.ReturnType, false, delegateMethod.Signature.Parameters);
				var instanceProxy = Compiler.TypeSystem.CreateLinkerMethod(delegateProxyType, delegateMethod.FullName + "@Instance@Proxy", delegateMethod.Signature.ReturnType, true, delegateMethod.Signature.Parameters);

				tuple = new Tuple<MosaMethod, MosaMethod>(instanceProxy, staticProxy);

				delegateProxyMethods.Add(delegateMethod, tuple);
			}

			return instance ? tuple.Item1 : tuple.Item2;
		}

		/// <summary>
		/// Patches the delegate.
		/// </summary>
		/// <param name="methodCompiler">The method compiler.</param>
		/// <returns></returns>
		public bool PatchDelegate(BaseMethodCompiler methodCompiler)
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
			var thisOperand = methodCompiler.Parameters[0];
			var instanceOperand = methodCompiler.Parameters[1];
			var methodPointerOperand = methodCompiler.Parameters[2];

			var size = methodCompiler.Architecture.NativeInstructionSize;

			var methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
			int methodPointerOffset = methodCompiler.TypeLayout.GetFieldOffset(methodPointerField);
			var methodPointerOffsetOperand = Operand.CreateConstant(methodCompiler.TypeSystem, methodPointerOffset);

			var instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
			int instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
			var instanceOffsetOperand = Operand.CreateConstant(methodCompiler.TypeSystem, instanceOffset);

			var context = new Context(CreateMethodStructure(methodCompiler));

			var v1 = methodCompiler.CreateVirtualRegister(thisOperand.Type);
			var v2 = methodCompiler.CreateVirtualRegister(methodPointerOperand.Type);
			var v3 = methodCompiler.CreateVirtualRegister(instanceOperand.Type);

			context.AppendInstruction(IRInstruction.LoadParameterInteger, v1, thisOperand);
			context.AppendInstruction(IRInstruction.LoadParameterInteger, v2, methodPointerOperand);
			context.AppendInstruction(IRInstruction.LoadParameterInteger, v3, instanceOperand);

			context.AppendInstruction(IRInstruction.StoreInteger, size, null, v1, methodPointerOffsetOperand, v2);
			context.MosaType = methodPointerOperand.Type;
			context.AppendInstruction(IRInstruction.StoreInteger, size, null, v1, instanceOffsetOperand, v3);
			context.MosaType = instanceOperand.Type;
			context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		private void PatchInvoke(BaseMethodCompiler methodCompiler)
		{
			// check if instance is null (if so, it's a static call to the methodPointer)

			var methodPointerField = GetField(methodCompiler.Method.DeclaringType, "methodPointer");
			int methodPointerOffset = methodCompiler.TypeLayout.GetFieldOffset(methodPointerField);
			var methodPointerOffsetOperand = Operand.CreateConstant(methodCompiler.TypeSystem, methodPointerOffset);

			var instanceField = GetField(methodCompiler.Method.DeclaringType, "instance");
			int instanceOffset = methodCompiler.TypeLayout.GetFieldOffset(instanceField);
			var instanceOffsetOperand = Operand.CreateConstant(methodCompiler.TypeSystem, instanceOffset);

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
					b0.AppendInstruction(IRInstruction.LoadParameterCompound, vrs[i], methodCompiler.Parameters[i]);
					b0.MosaType = type;
				}
				else
				{
					vrs[i] = methodCompiler.VirtualRegisters.Allocate(methodCompiler.Parameters[i].Type);

					var loadInstruction = BaseMethodCompilerStage.GetLoadParameterInstruction(vrs[i].Type);
					var loadsize = BaseMethodCompilerStage.GetInstructionSize(vrs[i].Type);

					b0.AppendInstruction(loadInstruction, loadsize, vrs[i], methodCompiler.Parameters[i]);
					b0.MosaType = type;
				}
			}

			var thisOperand = vrs[0];

			var opMethod = methodCompiler.VirtualRegisters.Allocate(methodCompiler.TypeSystem.BuiltIn.U4);
			var opInstance = methodCompiler.VirtualRegisters.Allocate(thisOperand.Type);
			var opCompare = methodCompiler.VirtualRegisters.Allocate(methodCompiler.TypeSystem.BuiltIn.I4);

			var opReturn = withReturn ? methodCompiler.AllocateVirtualRegisterOrStackSlot(methodCompiler.Method.Signature.ReturnType) : null;
			var c0 = Operand.CreateConstant(methodCompiler.TypeSystem, 0);

			b0.AppendInstruction(IRInstruction.LoadInteger, size, opMethod, thisOperand, methodPointerOffsetOperand);
			b0.AppendInstruction(IRInstruction.LoadInteger, size, opInstance, thisOperand, instanceOffsetOperand);
			b0.AppendInstruction(IRInstruction.CompareInteger, ConditionCode.Equal, opCompare, opInstance, c0);
			b0.AppendInstruction(IRInstruction.CompareIntegerBranch, ConditionCode.Equal, null, opCompare, c0);
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
				b3.AppendInstruction(IRInstruction.SetReturn, null, opReturn);

			b3.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		private static void PatchBeginInvoke(BaseMethodCompiler methodCompiler)
		{
			var nullOperand = Operand.GetNull(methodCompiler.TypeSystem);
			var context = new Context(CreateMethodStructure(methodCompiler));

			context.AppendInstruction(IRInstruction.SetReturn, null, nullOperand);
			context.AppendInstruction(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock);
		}

		private static void PatchEndInvoke(BaseMethodCompiler methodCompiler)
		{
			var start = CreateMethodStructure(methodCompiler);

			start.First.Insert(new InstructionNode(IRInstruction.Jmp, methodCompiler.BasicBlocks.EpilogueBlock));
		}

		private static BasicBlock CreateMethodStructure(BaseMethodCompiler methodCompiler)
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
