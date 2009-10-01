/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *
 */

using System.Collections.Generic;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Tools.Compiler.LinkTimeCodeGeneration
{
	/// <summary>
	/// This class provides a source of an instruction list for the method compiler.
	/// </summary>
	/// <remarks>
	/// This instruction source is used during link time code generation in order to 
	/// have a source of instructions to compile. This source acts on a previously built
	/// list of instructions to pass through the following compilation stages.
	/// </remarks>
	sealed class LinkerInstructionSource : BaseStage, IMethodCompilerStage
	{
		#region Data Members

		/// <summary>
		/// Holds the instructions to emit during the linker process.
		/// </summary>
		private InstructionSet _instructionSet;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerInstructionSource"/> class.
		/// </summary>
		/// <param name="instructionSet">The instruction set.</param>
		public LinkerInstructionSource(InstructionSet instructionSet)
		{
			_instructionSet = instructionSet;
		}

		#endregion // Construction

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Link Time Code Generation Instruction Source"; }
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			// Nothing to do here, normally an instruction source would parse some source code
			// or intermediate form of it. We've already got the instructions in the ctor.

			compiler.InstructionSet = _instructionSet;
		}

		/// <summary>
		/// Adds the stage to the pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add to.</param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.Add(this);
		}

		#endregion // IMethodCompilerStage Members
	}
}
