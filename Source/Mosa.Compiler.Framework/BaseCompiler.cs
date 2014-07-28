/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base class for just-in-time and ahead-of-time compilers, which use
	/// the Mosa.Compiler.Framework framework.
	/// </summary>
	public abstract class BaseCompiler
	{
		#region Properties

		/// <summary>
		/// Returns the architecture used by the compiler.
		/// </summary>
		public BaseArchitecture Architecture { get; private set; }

		/// <summary>
		/// Gets the pipeline.
		/// </summary>
		/// <value>The pipeline.</value>
		public CompilerPipeline Pipeline { get; private set; }

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		public TypeSystem TypeSystem { get; private set; }

		/// <summary>
		/// Gets the type layout.
		/// </summary>
		/// <value>The type layout.</value>
		public MosaTypeLayout TypeLayout { get; private set; }

		/// <summary>
		/// Gets the compiler trace.
		/// </summary>
		/// <value>
		/// The compiler trace.
		/// </value>
		public CompilerTrace CompilerTrace { get; private set; }

		/// <summary>
		/// Gets the compiler options.
		/// </summary>
		/// <value>The compiler options.</value>
		public CompilerOptions CompilerOptions { get; private set; }

		/// <summary>
		/// Gets the counters.
		/// </summary>
		public Counters Counters { get; private set; }

		/// <summary>
		/// Gets the scheduler.
		/// </summary>
		public ICompilationScheduler CompilationScheduler { get; private set; }

		/// <summary>
		/// Gets the linker.
		/// </summary>
		public BaseLinker Linker { get; private set; }

		/// <summary>
		/// Gets the plug system.
		/// </summary>
		public PlugSystem PlugSystem { get; private set; }

		/// <summary>
		/// Gets the list of Intrinsic Types for internal call replacements.
		/// </summary>
		public Dictionary<string, Type> IntrinsicTypes { get; private set; }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new compiler instance.
		/// </summary>
		/// <param name="architecture">The compiler target architecture.</param>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="typeLayout">The type layout.</param>
		/// <param name="compilationScheduler">The compilation scheduler.</param>
		/// <param name="compilerTrace">The compiler trace.</param>
		/// <param name="linker">The linker.</param>
		/// <param name="compilerOptions">The compiler options.</param>
		/// <exception cref="System.ArgumentNullException">@Architecture</exception>
		protected BaseCompiler(BaseArchitecture architecture, TypeSystem typeSystem, MosaTypeLayout typeLayout, ICompilationScheduler compilationScheduler, CompilerTrace compilerTrace, BaseLinker linker, CompilerOptions compilerOptions)
		{
			if (architecture == null)
				throw new ArgumentNullException(@"Architecture");

			Pipeline = new CompilerPipeline();
			Architecture = architecture;
			TypeSystem = typeSystem;
			TypeLayout = typeLayout;
			CompilerTrace = compilerTrace;
			CompilerOptions = compilerOptions;
			Counters = new Counters();
			CompilationScheduler = compilationScheduler;
			PlugSystem = new PlugSystem();
			Linker = linker;

			if (Linker == null)
			{
				Linker = compilerOptions.LinkerFactory();
				Linker.Initialize(compilerOptions.BaseAddress, architecture.Endianness, architecture.ElfMachineType);
			}

			// Create new dictionary
			IntrinsicTypes = new Dictionary<string, Type>();

			// Get all the classes that implement the IIntrinsicInternalMethod interface
			IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => typeof(IIntrinsicInternalMethod).IsAssignableFrom(p) && p.IsClass);

			// Iterate through all the found types
			foreach (var t in types)
			{
				// Now get all the ReplacementTarget attributes
				var attributes = (ReplacementTargetAttribute[])t.GetCustomAttributes(typeof(ReplacementTargetAttribute), true);
				for (int i = 0; i < attributes.Length; i++)
				{
					// Finally add the dictionary entry mapping the target string and the type
					IntrinsicTypes.Add(attributes[i].Target, t);
				}
			}
		}

		#endregion Construction

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
			return TypeSystem.CreateLinkerMethod(methodName, TypeSystem.BuiltIn.Void, null);
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
				stage.Initialize(this);
			}

			foreach (ICompilerStage stage in Pipeline)
			{
				Trace(CompilerEvent.CompilerStageStart, stage.Name);

				// Execute stage
				stage.Execute();

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
			CompilerTrace.CompilerEventListener.SubmitTraceEvent(compilerEvent, message);
		}

		/// <summary>
		/// Updates the counter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="count">The count.</param>
		protected void UpdateCounter(string name, int count)
		{
			Counters.UpdateCounter(name, count);
		}

		#endregion Helper Methods
	}
}