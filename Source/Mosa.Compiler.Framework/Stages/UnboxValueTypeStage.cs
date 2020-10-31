// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Unboxes value types when we are virtually calling a method on a boxed value type.
	/// </summary>
	public class UnboxValueTypeStage : BaseMethodCompilerStage
	{
		private readonly Counter TriggeredCount = new Counter("UnboxValueTypeStage.Triggered");

		protected override void Run()
		{
			// The method declaring type must be a valuetype, not a constructor, or static method, or not virual
			if (!Method.DeclaringType.IsValueType
				|| Method.IsConstructor
				|| Method.IsStatic
				|| !Method.IsVirtual)
				return;

			// If the method is empty then don't process
			if (BasicBlocks.PrologueBlock.NextBlocks.Count == 0 || BasicBlocks.PrologueBlock.NextBlocks[0] == BasicBlocks.EpilogueBlock)
				return;

			TriggeredCount.Set(1);

			// Get the this pointer
			var thisPtr = MethodCompiler.Parameters[0];

			var context = new Context(BasicBlocks.PrologueBlock.NextBlocks[0].First);

			// Now push the this pointer by two native pointer sizes
			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.TypedRef);

			context.AppendInstruction(LoadInstruction, v1, StackFrame, thisPtr);
			context.AppendInstruction(AddInstruction, v1, v1, CreateConstant32(NativePointerSize * 2));
			context.AppendInstruction(StoreInstruction, null, StackFrame, thisPtr, v1);
		}
	}
}
