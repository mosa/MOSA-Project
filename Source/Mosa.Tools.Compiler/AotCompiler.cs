/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 */

using System.Diagnostics;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Tools.Compiler.Stage;
using Mosa.Compiler.Linker;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.InternalTrace;

namespace Mosa.Tools.Compiler
{

	/// <summary>
	/// Implements the ahead of time compiler.
	/// </summary>
	/// <remarks>
	/// This class implements the ahead of time compiler for MOSA. The AoT uses 
	/// the compiler services offered in Mosa.Runtime.CompilerFramework in order
	/// to share as much code as possible with assembly jit compiler in MOSA. The 
	/// primary difference between the two compilers is primarily the number and
	/// quality of compilation stages used. The AoT compiler makes use of assembly lot
	/// more optimizations than the jit. The jit is tweaked for execution speed. 
	/// </remarks>
	public sealed class AotCompiler : AssemblyCompiler
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AotCompiler"/> class.
		/// </summary>
		/// <param name="architecture">The target compilation architecture.</param>
		/// <param name="typeSystem">The type system.</param>
		public AotCompiler(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalLog) :
			base(architecture, typeSystem, typeLayout, internalLog)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Executes the compiler using the configured stages.
		/// </summary>
		/// <remarks>
		/// The method iterates the compilation stage chain and runs each
		/// stage on the input.
		/// </remarks>
		public void Run()
		{
			//FlowGraphVisualizationStage.Instance.Open();
			InstructionStatisticsStage.Instance.Start();
			// Build the default assembly compiler pipeline
			this.Architecture.ExtendAssemblyCompilerPipeline(this.Pipeline);

			// Run the compiler
			base.Compile();
			InstructionStatisticsStage.Instance.End();
			InstructionStatisticsStage.Instance.PrintStatistics();
			//FlowGraphVisualizationStage.Instance.Close();
		}

		/// <summary>
		/// Creates a method compiler
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="method">The method to compile.</param>
		/// <returns>
		/// An instance of a MethodCompilerBase for the given type/method pair.
		/// </returns>
		public override IMethodCompiler CreateMethodCompiler(ICompilationSchedulerStage compilationScheduler, RuntimeType type, RuntimeMethod method)
		{
			IMethodCompiler mc = new AotMethodCompiler(this, compilationScheduler, type, method, internalTrace);
			this.Architecture.ExtendMethodCompilerPipeline(mc.Pipeline);
			return mc;
		}

		#endregion // Methods
	}
}
