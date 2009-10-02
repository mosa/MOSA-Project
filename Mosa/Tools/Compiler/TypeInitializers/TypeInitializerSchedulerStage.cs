/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System.Collections.Generic;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;
using Mosa.Tools.Compiler.LinkTimeCodeGeneration;
using Mosa.Runtime.Linker;

using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Tools.Compiler.TypeInitializers
{
	/// <summary>
	/// Schedules type initializers and creates a hidden mosacl_main method,
	/// which runs all type initializers in sequence.
	/// </summary>
	/// <remarks>
	/// Dependencies are not resolved, it is hoped that dependencies are resolved
	/// by the high-level language compiler by placing cctors in some order in
	/// metadata.
	/// </remarks>
	public sealed class TypeInitializerSchedulerStage : BaseStage, IAssemblyCompilerStage
	{
		#region Data Members

		/// <summary>
		/// Holds a list of cctors call instructions.
		/// </summary>
		private InstructionSet _instructionSet;

		/// <summary>
		/// Hold the current context
		/// </summary>
		private Context _ctx;

		/// <summary>
		/// Holds the method for the type initalizer
		/// </summary>
		private CompilerGeneratedMethod _method;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeInitializerSchedulerStage"/> class.
		/// </summary>
		public TypeInitializerSchedulerStage()
		{
			_instructionSet = new InstructionSet(16);
			_ctx = new Context(_instructionSet, -1);

			_ctx.InsertInstructionAfter(IR.Instruction.PrologueInstruction);
			_ctx.Other = 0; // stacksize
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the intializer method.
		/// </summary>
		/// <value>The method.</value>
		public CompilerGeneratedMethod Method
		{
			get
			{
				return _method;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Schedules the specified method for invocation in the main.
		/// </summary>
		/// <param name="method">The method.</param>
		public void Schedule(RuntimeMethod method)
		{
			_ctx.InsertInstructionAfter(IR.Instruction.CallInstruction);
			_ctx.InvokeTarget = method;
		}

		#endregion // Methods

		#region IAssemblyCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Type Initializer Scheduler"; }
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(AssemblyCompiler compiler)
		{
			_ctx.InsertInstructionAfter(IR.Instruction.CallInstruction);
			_ctx.InvokeTarget = compiler.Assembly.EntryPoint;
			_ctx.InsertInstructionAfter(IR.Instruction.EpilogueInstruction);
			_ctx.Other = 0;

			_method = LinkTimeCodeGenerator.Compile(compiler, @"AssemblyInit", _instructionSet);
		}

		#endregion // IAssemblyCompilerStage Members
	}
}
