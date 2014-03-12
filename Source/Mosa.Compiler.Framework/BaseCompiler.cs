/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base class for just-in-time and ahead-of-time compilers, which use
	/// the Mosa.Compiler.Framework framework.
	/// </summary>
	public abstract class BaseCompiler
	{
		#region Data members

		/// <summary>
		/// The compiler target architecture.
		/// </summary>
		private BaseArchitecture architecture;

		/// <summary>
		/// The pipeline of the compiler.
		/// </summary>
		private CompilerPipeline pipeline;

		/// <summary>
		/// Holds the current type system during compilation.
		/// </summary>
		private TypeSystem typeSystem;

		/// <summary>
		/// Holds the current type layout during complication
		/// </summary>
		private MosaTypeLayout typeLayout;

		/// <summary>
		/// Holds the current internal log
		/// </summary>
		private IInternalTrace internalTrace;

		/// <summary>
		/// Holds the compiler option set
		/// </summary>
		private CompilerOptions compilerOptions;

		/// <summary>
		/// Holds the counters
		/// </summary>
		private Counters counters;

		/// <summary>
		/// Holds the compilation scheduler
		/// </summary>
		private ICompilationScheduler compilationScheduler;

		/// <summary>
		/// Holds the linker
		/// </summary>
		private ILinker linker;

		/// <summary>
		/// Holds the plug system
		/// </summary>
		private readonly PlugSystem plugSystem;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new compiler instance.
		/// </summary>
		/// <param name="architecture">The compiler target architecture.</param>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="typeLayout">The type layout.</param>
		/// <param name="compilationScheduler">The compilation scheduler.</param>
		/// <param name="internalTrace">The internal trace.</param>
		/// <param name="compilerOptions">The compiler options.</param>
		protected BaseCompiler(BaseArchitecture architecture, TypeSystem typeSystem, MosaTypeLayout typeLayout, ICompilationScheduler compilationScheduler, IInternalTrace internalTrace, ILinker linker, CompilerOptions compilerOptions)
		{
			if (architecture == null)
				throw new ArgumentNullException(@"architecture");

			this.pipeline = new CompilerPipeline();
			this.architecture = architecture;
			this.typeSystem = typeSystem;
			this.typeLayout = typeLayout;
			this.internalTrace = internalTrace;
			this.compilerOptions = compilerOptions;
			this.counters = new Counters();
			this.compilationScheduler = compilationScheduler;
			this.linker = (linker != null) ? linker : LinkerFactory.Create(compilerOptions.LinkerType, compilerOptions, architecture);
			this.plugSystem = new PlugSystem();
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Returns the architecture used by the compiler.
		/// </summary>
		public BaseArchitecture Architecture { get { return architecture; } }

		/// <summary>
		/// Gets the pipeline.
		/// </summary>
		/// <value>The pipeline.</value>
		public CompilerPipeline Pipeline { get { return pipeline; } }

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		public TypeSystem TypeSystem { get { return typeSystem; } }

		/// <summary>
		/// Gets the type layout.
		/// </summary>
		/// <value>The type layout.</value>
		public MosaTypeLayout TypeLayout { get { return typeLayout; } }

		/// <summary>
		/// Gets the internal log.
		/// </summary>
		/// <value>The internal log.</value>
		public IInternalTrace InternalTrace { get { return internalTrace; } }

		/// <summary>
		/// Gets the compiler options.
		/// </summary>
		/// <value>The compiler options.</value>
		public CompilerOptions CompilerOptions { get { return compilerOptions; } }

		/// <summary>
		/// Gets the counters.
		/// </summary>
		public Counters Counters { get { return counters; } }

		/// <summary>
		/// Gets the scheduler.
		/// </summary>
		public ICompilationScheduler Scheduler { get { return compilationScheduler; } }

		/// <summary>
		/// Gets the linker.
		/// </summary>
		public ILinker Linker { get { return linker; } }

		/// <summary>
		/// Gets the plug system.
		/// </summary>
		public PlugSystem PlugSystem { get { return plugSystem; } }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Compiles the method.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="instructionSet">The instruction set.</param>
		public void CompileMethod(MosaMethod method, BasicBlocks basicBlocks, InstructionSet instructionSet)
		{
			Trace(CompilerEvent.CompilingMethod, method.FullName);

			BaseMethodCompiler methodCompiler = CreateMethodCompiler(method, basicBlocks, instructionSet);
			Architecture.ExtendMethodCompilerPipeline(methodCompiler.Pipeline);

			methodCompiler.Compile();

			//try
			//{
			//    methodCompiler.Compile();
			//}
			//catch (Exception e)
			//{
			//    HandleCompilationException(e);
			//    throw;
			//}
		}

		/// <summary>
		/// Creates a method compiler
		/// </summary>
		/// <param name="method">The method to compile.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="instructionSet">The instruction set.</param>
		/// <returns></returns>
		public abstract BaseMethodCompiler CreateMethodCompiler(MosaMethod method, BasicBlocks basicBlocks, InstructionSet instructionSet);

		/// <summary>
		/// Compiles the linker method.
		/// </summary>
		/// <param name="methodName">Name of the method.</param>
		/// <returns></returns>
		public MosaMethod CreateLinkerMethod(string methodName)
		{
			var method = typeSystem.CreateLinkerMethod(methodName, typeSystem.BuiltIn.Void, null);
			return method;
		}

		/// <summary>
		/// Executes the compiler using the configured stages.
		/// </summary>
		/// <remarks>
		/// The method iterates the compilation stage chain and runs each
		/// stage on the input.
		/// </remarks>
		public void Compile()
		{
			BeginCompile();

			foreach (ICompilerStage stage in Pipeline)
			{
				// Setup Compiler
				stage.Setup(this);
			}

			foreach (ICompilerStage stage in Pipeline)
			{
				Trace(CompilerEvent.CompilerStageStart, stage.Name);

				// Execute stage
				stage.Run();

				Trace(CompilerEvent.CompilerStageEnd, stage.Name);
			}

			EndCompile();

			ExportCounters();
		}

		/// <summary>
		/// Called when compilation is about to begin.
		/// </summary>
		protected virtual void BeginCompile()
		{
		}

		/// <summary>
		/// Called when compilation has completed.
		/// </summary>
		protected virtual void EndCompile()
		{
		}

		#endregion Methods

		protected void ExportCounters()
		{
			foreach (var counter in Counters.Export())
			{
				Trace(CompilerEvent.Counter, counter);
			}
		}

		#region Helper Methods

		/// <summary>
		/// Traces the specified compiler event.
		/// </summary>
		/// <param name="compilerEvent">The compiler event.</param>
		/// <param name="message">The message.</param>
		protected void Trace(CompilerEvent compilerEvent, string message)
		{
			internalTrace.CompilerEventListener.SubmitTraceEvent(compilerEvent, message);
		}

		/// <summary>
		/// Updates the counter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="count">The count.</param>
		protected void UpdateCounter(string name, int count)
		{
			counters.UpdateCounter(name, count);
		}

		#endregion Helper Methods
	}
}