/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Basic base class for pipeline stages
	/// </summary>
	public abstract class BaseStage
	{
		#region Data members

		/// <summary>
		/// Hold the method compiler
		/// </summary>
		protected IMethodCompiler MethodCompiler;

		/// <summary>
		/// The architecture of the compilation process.
		/// </summary>
		protected IArchitecture Architecture;

		/// <summary>
		/// Holds the instruction set
		/// </summary>
		protected InstructionSet InstructionSet;

		/// <summary>
		/// List of basic blocks found during decoding.
		/// </summary>
		protected List<BasicBlock> BasicBlocks;

		#endregion // Data members

		#region IMethodCompilerStage members

		/// <summary>
		/// Setups the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public void Setup(IMethodCompiler compiler)
		{
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");

			MethodCompiler = compiler;
			InstructionSet = compiler.InstructionSet;
			BasicBlocks = compiler.BasicBlocks;
			Architecture = compiler.Architecture;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets block by label
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected BasicBlock FindBlock(int label)
		{
			return MethodCompiler.FromLabel(label);
		}

		/// <summary>
		/// Creates the context.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		protected Context CreateContext(BasicBlock block)
		{
			return new Context(InstructionSet, block);
		}

		/// <summary>
		/// Creates the context.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		protected Context CreateContext(int index)
		{
			return new Context(InstructionSet, index);
		}

		/// <summary>
		/// Creates the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		protected BasicBlock CreateBlock(int label, int index)
		{
			return new BasicBlock(label, index);
		}

		/// <summary>
		/// Creates the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected BasicBlock CreateBlock(int label)
		{
			return new BasicBlock(label);
		}

		#endregion

		#region Utility Methods

		/// <summary>
		/// Gets the unsigned condition code.
		/// </summary>
		/// <param name="conditionCode">The condition code to get an unsigned form from.</param>
		/// <returns>The unsigned form of the given condition code.</returns>
		protected IR.ConditionCode GetUnsignedConditionCode(IR.ConditionCode conditionCode)
		{
			switch (conditionCode) {
				case IR.ConditionCode.Equal: break;
				case IR.ConditionCode.NotEqual: break;
				case IR.ConditionCode.GreaterOrEqual: return IR.ConditionCode.UnsignedGreaterOrEqual;
				case IR.ConditionCode.GreaterThan: return IR.ConditionCode.UnsignedGreaterThan;
				case IR.ConditionCode.LessOrEqual: return IR.ConditionCode.UnsignedLessOrEqual;
				case IR.ConditionCode.LessThan: return IR.ConditionCode.UnsignedLessThan;
				case IR.ConditionCode.UnsignedGreaterOrEqual: break;
				case IR.ConditionCode.UnsignedGreaterThan: break;
				case IR.ConditionCode.UnsignedLessOrEqual: break;
				case IR.ConditionCode.UnsignedLessThan: break;
				default: throw new NotSupportedException();
			}

			return conditionCode;
		}

		#endregion // Utility Methods

	}
}
