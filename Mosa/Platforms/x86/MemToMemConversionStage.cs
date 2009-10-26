/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Scott Balmos <sbalmos@fastmail.fm>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class MemToMemConversionStage :
		BaseTransformationStage,
		IMethodCompilerStage,
		IPlatformTransformationStage
	{

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name
		{
			get { return @"X86.MemToMemConversionStage"; }
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertAfter<TweakTransformationStage>(this);
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			foreach (BasicBlock block in BasicBlocks)
				for (Context ctx = new Context(InstructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
					if (ctx.Instruction != null)
						if (!ctx.Ignore && ctx.Instruction is CPUx86.IX86Instruction)
							if (ctx.Result is MemoryOperand && ctx.Operand1 is MemoryOperand)
								HandleMemoryToMemoryOperation(ctx, null, true);
		}

		#endregion // IMethodCompilerStage Members

		private void HandleMemoryToMemoryOperation(Context ctx, Operand register, bool useStack)
		{
			Operand destination = ctx.Result;
			Operand source = ctx.Operand1;

			Debug.Assert(destination is MemoryOperand && source is MemoryOperand);

			if (register == null)
				register = new RegisterOperand(destination.Type, GeneralPurposeRegister.EDX);

			ctx.Operand1 = register;

			Context before = ctx.InsertBefore();

            if (useStack)
            {
                before.SetInstruction(CPUx86.Instruction.PushInstruction, register);
                before.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, register, source);
            }
            else 
                before.SetInstruction(CPUx86.Instruction.MovInstruction, register, source);

			if (useStack)
				ctx.InsertInstructionAfter(CPUx86.Instruction.PopInstruction, register);
		}
	}
}
