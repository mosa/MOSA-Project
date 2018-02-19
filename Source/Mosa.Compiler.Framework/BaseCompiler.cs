// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Trace;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base class for just-in-time and ahead-of-time compilers, which use
	/// the Mosa.Compiler.Framework framework.
	/// </summary>
	public abstract class BaseCompiler
	{
		private CompilerPipeline[] methodStagePipelines;

		#region Properties

		/// <summary>
		/// Returns the architecture used by the compiler.
		/// </summary>
		public BaseArchitecture Architecture { get; private set; }

		/// <summary>
		/// Gets the pre compile pipeline.
		/// </summary>
		public CompilerPipeline CompilePipeline { get; private set; }

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
		public Counters GlobalCounters { get; private set; }

		/// <summary>
		/// Gets the scheduler.
		/// </summary>
		public CompilationScheduler CompilationScheduler { get; private set; }

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

		/// <summary>
		/// Gets the type of the platform internal runtime.
		/// </summary>
		/// <value>
		/// The type of the platform internal runtime.
		/// </value>
		public MosaType PlatformInternalRuntimeType { get; private set; }

		/// <summary>
		/// Gets the type of the internal runtime.
		/// </summary>
		/// <value>
		/// The type of the internal runtime.
		/// </value>
		public MosaType InternalRuntimeType { get; private set; }

		/// <summary>
		/// Gets the compiler data.
		/// </summary>
		public CompilerData CompilerData { get; private set; }

		#endregion Properties

		#region Methods

		public void Initialize(MosaCompiler compiler)
		{
			Architecture = compiler.CompilerOptions.Architecture;

			TypeSystem = compiler.TypeSystem;
			TypeLayout = compiler.TypeLayout;
			CompilerTrace = compiler.CompilerTrace;
			CompilerOptions = compiler.CompilerOptions;
			CompilationScheduler = compiler.CompilationScheduler;
			Linker = compiler.Linker;

			methodStagePipelines = new CompilerPipeline[compiler.MaxThreads];

			CompilePipeline = new CompilerPipeline();
			GlobalCounters = new Counters();
			PlugSystem = new PlugSystem();
			CompilerData = new CompilerData();

			// Create new dictionary
			IntrinsicTypes = new Dictionary<string, Type>();

			foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
			{
				if (type.IsClass && typeof(IIntrinsicInternalMethod).IsAssignableFrom(type))
				{
					// Now get all the ReplacementTarget attributes
					var attributes = (ReplacementTargetAttribute[])type.GetCustomAttributes(typeof(ReplacementTargetAttribute), true);
					for (int i = 0; i < attributes.Length; i++)
					{
						// Finally add the dictionary entry mapping the target string and the type
						IntrinsicTypes.Add(attributes[i].Target, type);
					}
				}
			}

			PlatformInternalRuntimeType = GetPlatformInternalRuntimeType();
			InternalRuntimeType = GeInternalRuntimeType();

			// Extended Setup
			ExtendCompilerSetup();

			// Build the default pre-compiler pipeline
			Architecture.ExtendCompilerPipeline(CompilePipeline);
		}

		/// <summary>
		/// Extends the compiler setup.
		/// </summary>
		public virtual void ExtendCompilerSetup()
		{
		}

		/// <summary>
		/// Compiles the method.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="threadID">The thread identifier.</param>
		public void CompileMethod(MosaMethod method, BasicBlocks basicBlocks, int threadID = 0)
		{
			NewCompilerTraceEvent(CompilerEvent.CompilingMethod, method.FullName, threadID);

			var methodCompiler = GetMethodCompiler(method, basicBlocks, threadID);

			methodCompiler.Compile();
		}

		protected MethodCompiler GetMethodCompiler(MosaMethod method, BasicBlocks basicBlocks, int threadID = 0)
		{
			var methodCompiler = new MethodCompiler(this, method, basicBlocks, threadID);

			var pipeline = methodStagePipelines[threadID];

			if (pipeline == null)
			{
				var stages = CreateMethodPipeline();

				pipeline = new CompilerPipeline
				{
					stages
				};

				Architecture.ExtendMethodCompilerPipeline(pipeline);

				methodStagePipelines[threadID] = pipeline;

				foreach (BaseMethodCompilerStage stage in pipeline)
				{
					stage.Initialize(this);
				}
			}

			methodCompiler.Pipeline = pipeline;

			return methodCompiler;
		}

		protected abstract BaseMethodCompilerStage[] CreateMethodPipeline();

		/// <summary>
		/// Compiles the linker method.
		/// </summary>
		/// <param name="methodName">Name of the method.</param>
		/// <returns></returns>
		public MosaMethod CreateLinkerMethod(string methodName)
		{
			return TypeSystem.CreateLinkerMethod(methodName, TypeSystem.BuiltIn.Void, false, null);
		}

		/// <summary>
		/// Executes the compiler pre compiler stages.
		/// </summary>
		/// <remarks>
		/// The method iterates the compilation stage chain and runs each
		/// stage on the input.
		/// </remarks>
		internal void PreCompile()
		{
			foreach (BaseCompilerStage stage in CompilePipeline)
			{
				stage.Initialize(this);
			}

			foreach (BaseCompilerStage stage in CompilePipeline)
			{
				NewCompilerTraceEvent(CompilerEvent.PreCompileStageStart, stage.Name);

				// Execute stage
				stage.ExecutePreCompile();

				NewCompilerTraceEvent(CompilerEvent.PreCompileStageEnd, stage.Name);
			}
		}

		public void ExecuteCompile()
		{
			ExecuteCompilePass();

			while (CompilationScheduler.StartNextPass())
			{
				ExecuteCompilePass();
			}
		}

		private void ExecuteCompilePass()
		{
			while (true)
			{
				var method = CompilationScheduler.GetMethodToCompile();

				if (method == null)
					return;

				CompileMethod(method, null, 0);

				CompilerTrace.UpdatedCompilerProgress(
					CompilationScheduler.TotalMethods,
					CompilationScheduler.TotalMethods - CompilationScheduler.TotalQueuedMethods
				);
			}
		}

		public void ExecuteThreadedCompile(int threads)
		{
			ExecuteThreadedCompilePass(threads);

			while (CompilationScheduler.StartNextPass())
			{
				ExecuteThreadedCompilePass(threads);
			}
		}

		private void ExecuteThreadedCompilePass(int threads)
		{
			using (var finished = new CountdownEvent(1))
			{
				for (int threadID = 0; threadID < threads; threadID++)
				{
					finished.AddCount();

					int tid = threadID;

					ThreadPool.QueueUserWorkItem(
						new WaitCallback(delegate
						{
							//try
							//{
							CompileWorker(tid);

							//}
							//catch (Exception e)
							//{
							//	this.CompilerTrace.NewCompilerTraceEvent(CompilerEvent.Exception, e.ToString(), threadID);
							//}
							//finally
							//{
							finished.Signal();

							//}
						}
					));
				}

				finished.Signal();
				finished.Wait();
			}
		}

		private void CompileWorker(int threadID)
		{
			while (true)
			{
				var method = CompilationScheduler.GetMethodToCompile();

				if (method == null)
					return;

				// only one method can be compiled at a time
				lock (method)
				{
					CompileMethod(method, null, threadID);
				}

				CompilerTrace.UpdatedCompilerProgress(CompilationScheduler.TotalMethods, CompilationScheduler.TotalMethods - CompilationScheduler.TotalQueuedMethods);
			}
		}

		/// <summary>
		/// Executes the compiler post compiler stages.
		/// </summary>
		/// <remarks>
		/// The method iterates the compilation stage chain and runs each
		/// stage on the input.
		/// </remarks>
		internal void PostCompile()
		{
			foreach (BaseCompilerStage stage in CompilePipeline)
			{
				NewCompilerTraceEvent(CompilerEvent.PostCompileStageStart, stage.Name);

				// Execute stage
				stage.ExecutePostCompile();

				NewCompilerTraceEvent(CompilerEvent.PostCompileStageEnd, stage.Name);
			}

			// Sum up the counters
			foreach (var methodData in CompilerData.MethodData)
			{
				GlobalCounters.Merge(methodData.Counters);
			}

			ExportCounters();
		}

		#endregion Methods

		protected void ExportCounters()
		{
			foreach (var counter in GlobalCounters.Export())
			{
				NewCompilerTraceEvent(CompilerEvent.Counter, counter);
			}
		}

		#region Helper Methods

		/// <summary>
		/// Traces the specified compiler event.
		/// </summary>
		/// <param name="compilerEvent">The compiler event.</param>
		/// <param name="message">The message.</param>
		protected void NewCompilerTraceEvent(CompilerEvent compilerEvent, string message)
		{
			CompilerTrace.NewCompilerTraceEvent(compilerEvent, message, 0);
		}

		/// <summary>
		/// Traces the specified compiler event.
		/// </summary>
		/// <param name="compilerEvent">The compiler event.</param>
		/// <param name="message">The message.</param>
		/// <param name="threadID">The thread identifier.</param>
		protected void NewCompilerTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			CompilerTrace.NewCompilerTraceEvent(compilerEvent, message, threadID);
		}

		/// <summary>
		/// Updates the counter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="count">The count.</param>
		protected void UpdateCounter(string name, int count)
		{
			GlobalCounters.Update(name, count);
		}

		protected MosaType GetPlatformInternalRuntimeType()
		{
			return TypeSystem.GetTypeByName("Mosa.Runtime." + Architecture.PlatformName, "Internal");
		}

		protected MosaType GeInternalRuntimeType()
		{
			return TypeSystem.GetTypeByName("Mosa.Runtime", "Internal");
		}

		#endregion Helper Methods
	}
}
