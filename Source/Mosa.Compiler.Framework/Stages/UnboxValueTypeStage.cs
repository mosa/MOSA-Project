/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Unboxes value types when we are virtually calling a method on a boxed value type.
	/// </summary>
	public class UnboxValueTypeStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			// The method declaring type must be a valuetype
			if (!MethodCompiler.Method.DeclaringType.IsValueType)
				return;

			// The method and method declaring type must not have generic parameters
			//if (MethodCompiler.Method.HasOpenGenericParams || MethodCompiler.Method.DeclaringType.HasOpenGenericParams)
			//	return;

			// If the method is static, non-virtual or is a constructor then don't process
			if (MethodCompiler.Method.IsStatic || !MethodCompiler.Method.IsVirtual || MethodCompiler.Method.Name.Equals(".ctor"))
				return;

			// If the method is empty then don't process
			if (BasicBlocks.PrologueBlock.NextBlocks.Count == 0 || BasicBlocks.PrologueBlock.NextBlocks[0] == BasicBlocks.EpilogueBlock)
				return;

			// Get our first viable context
			var context = GetFirstContext(BasicBlocks.PrologueBlock.NextBlocks[0]);

			// If we didn't get a viable context then the method is empty
			if (context == null)
			{
				Debug.Assert(false, MethodCompiler.Method.FullName);
				return;
			}

			// Get the this pointer
			var thisPtr = MethodCompiler.StackLayout.GetStackParameter(0);

			// Insert a new context before the viable context
			context = context.InsertBefore();

			// Now push the this pointer by two native pointer sizes
			context.SetInstruction(IRInstruction.AddSigned, thisPtr, thisPtr, Operand.CreateConstantSignedInt(TypeSystem, NativePointerSize * 2));
		}

		private Context GetFirstContext(BasicBlock block)
		{
			for (Context context = new Context(InstructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
			{
				if (context.IsBlockStartInstruction)
					continue;
				return context;
			}
			return null;
		}
	}
}