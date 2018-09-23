// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Unboxes value types when we are virtually calling a method on a boxed value type.
	/// </summary>
	public class UnboxValueTypeStage : BaseMethodCompilerStage
	{
		private Counter TriggeredCount = new Counter("UnboxValueTypeStage.Triggered");

		protected override void Run()
		{
			// The method declaring type must be a valuetype
			if (!MethodCompiler.Type.IsValueType)
				return;

			// If the method is static, non-virtual or is a constructor then don't process
			if (Method.IsStatic || !Method.IsVirtual || Method.Name.Equals(".ctor"))
				return;

			// If the method does not belong to an interface then don't process
			if (!(IsInterfaceMethod() || OverridesMethod()))
				return;

			// If the method is empty then don't process
			if (BasicBlocks.PrologueBlock.NextBlocks.Count == 0 || BasicBlocks.PrologueBlock.NextBlocks[0] == BasicBlocks.EpilogueBlock)
				return;

			TriggeredCount.Set(1);

			// Get the this pointer
			var thisPtr = MethodCompiler.Parameters[0];

			// FUTURE: move this to the end of prologue
			var context = new Context(BasicBlocks.PrologueBlock.NextBlocks[0].First);

			// Now push the this pointer by two native pointer sizes
			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.TypedRef);

			context.AppendInstruction(Select(IRInstruction.LoadInt32, IRInstruction.LoadInt64), v1, StackFrame, thisPtr);
			context.AppendInstruction(Select(IRInstruction.Add32, IRInstruction.Add64), v1, v1, CreateConstant(NativePointerSize * 2));

			// FUTURE: Change all thisPtr to v1
			context.AppendInstruction(Select(IRInstruction.StoreInt32, IRInstruction.StoreInt64), null, StackFrame, thisPtr, v1);
		}

		private bool IsInterfaceMethod()
		{
			foreach (var iface in MethodCompiler.Type.Interfaces)
			{
				foreach (var method in TypeLayout.GetInterfaceTable(MethodCompiler.Type, iface))
				{
					if (method == MethodCompiler.Method)
						return true;
				}
			}

			return false;
		}

		private bool OverridesMethod()
		{
			if (MethodCompiler.Method.Overrides == null)
				return false;
			if (MethodCompiler.Type.BaseType.Name.Equals("ValueType"))
				return true;
			if (MethodCompiler.Type.BaseType.Name.Equals("Object"))
				return true;
			if (MethodCompiler.Type.BaseType.Name.Equals("Enum"))
				return true;
			return false;
		}
	}
}
