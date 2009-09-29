/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Basic base class for other stages
	/// </summary>
	public class BaseStage
	{
		#region Data members

		/// <summary>
		/// Holds the instruction set
		/// </summary>
		protected InstructionSet _instructionset;

		/// <summary>
		/// List of basic Blocks found during decoding.
		/// </summary>
		protected List<BasicBlock> _basicBlocks;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseStage"/> class.
		/// </summary>
		public BaseStage()
		{
		}

		#endregion // Construction

		#region IMethodCompilerStage members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public virtual void Run(IMethodCompiler compiler)
		{
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");

			// Retrieve the instruction set
			IInstructionsProvider instructionsProvider = (compiler.GetPreviousStage(typeof(IInstructionsProvider)) as IInstructionsProvider);
			if (_instructionset == null)
				throw new InvalidOperationException(@"Instruction set provider missing.");
			_instructionset = instructionsProvider.InstructionSet;

			// Retrieve the basic block list
			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
			if (blockProvider == null)
				throw new InvalidOperationException(@"Instruction stream must be split to basic Blocks.");
			_basicBlocks = blockProvider.Blocks;

		}

		#endregion
	}
}
