// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Stages;
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
	/// Mosa Compiler
	/// </summary>
	public sealed class Compiler
	{
		#region Data Members

		private Pipeline<BaseMethodCompilerStage>[] methodStagePipelines;

		#endregion Data Members

		#region Properties

		/// <summary>
		/// Returns the architecture used by the compiler.
		/// </summary>
		public BaseArchitecture Architecture { get; private set; }

		/// <summary>
		/// Gets the pre compile pipeline.
		/// </summary>
		public Pipeline<BaseCompilerStage> CompilerPipeline { get; } = new Pipeline<BaseCompilerStage>();

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
		public Counters GlobalCounters { get; } = new Counters();

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
		public PlugSystem PlugSystem { get; } = new PlugSystem();

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
		public CompilerData CompilerData { get; } = new CompilerData();

		/// <summary>
		/// Gets the compiler extensions.
		/// </summary>
		private List<BaseCompilerExtension> CompilerExtensions { get; } = new List<BaseCompilerExtension>();

		#endregion Properties

		#region Static Methods

		private static List<BaseCompilerStage> GetDefaultCompilerPipeline(CompilerOptions compilerOptions)
		{
			var bootStage = compilerOptions.BootStageFactory != null ? compilerOptions.BootStageFactory() : null;

			return new List<BaseCompilerStage> {
				bootStage,
				new PlugStage(),
				new TypeInitializerSchedulerStage(),
				new MethodLookupTableStage(),
				new MethodExceptionLookupTableStage(),
				new MetadataStage(),
				(compilerOptions.OutputFile!= null && compilerOptions.EmitBinary) ? new LinkerFinalizationStage() : null,
				(compilerOptions.MapFile != null) ? new MapFileGenerationStage() : null,
				(compilerOptions.DebugFile != null) ? new DebugFileGenerationStage() : null
			};
		}

		private static List<BaseMethodCompilerStage> GetDefaultMethodPipeline(CompilerOptions compilerOptions)
		{
			return new List<BaseMethodCompilerStage>() {
				new CILDecodingStage(),
				new ExceptionPrologueStage(),
				new OperandAssignmentStage(),
				new StackSetupStage(),
				new CILProtectedRegionStage(),
				new ExceptionStage(),
				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
				new UnboxValueTypeStage(),
				(compilerOptions.EnableInlinedMethods) ? new InlineStage() : null,
				new PromoteTemporaryVariables(),
				(compilerOptions.EnableSSA) ? new EdgeSplitStage() : null,
				(compilerOptions.EnableSSA) ? new EnterSSAStage() : null,
				(compilerOptions.EnableSparseConditionalConstantPropagation && compilerOptions.EnableSSA) ? new SparseConditionalConstantPropagationStage() : null,
				(compilerOptions.EnableIROptimizations) ? new IROptimizationStage() : null,
				(compilerOptions.EnableSSA) ? new LeaveSSAStage() : null,
				new BlockMergeStage(),
				new IRCleanupStage(),
				new LowerIRStage(),
				(compilerOptions.IRLongExpansion && compilerOptions.Architecture.NativePointerSize == 4) ? new IRLongDecomposeStage() : null,
				(compilerOptions.TwoPassOptimizations && compilerOptions.EnableSSA) ? new EnterSSAStage() : null,
				(compilerOptions.TwoPassOptimizations && compilerOptions.EnableIROptimizations && compilerOptions.EnableSSA) ? new SparseConditionalConstantPropagationStage() : null,
				(compilerOptions.TwoPassOptimizations && compilerOptions.EnableIROptimizations && compilerOptions.EnableSparseConditionalConstantPropagation && compilerOptions.EnableSSA) ? new IROptimizationStage() : null,
				(compilerOptions.TwoPassOptimizations && compilerOptions.EnableSSA) ? new LeaveSSAStage() : null,
				(compilerOptions.TwoPassOptimizations ) ?new BlockMergeStage():  null,
				(compilerOptions.TwoPassOptimizations ) ?new IRCleanupStage() :  null,
				(compilerOptions.EnableInlinedMethods) ? new InlineEvaluationStage() : null,
				new DevirtualizeCallStage(),
				new CallStage(),
				new PlatformIntrinsicStage(),
				new PlatformEdgeSplitStage(),
				new VirtualRegisterRenameStage(),

				//new StopStage(),
				new GreedyRegisterAllocatorStage(),
				new StackLayoutStage(),
				new EmptyBlockRemovalStage(),
				new BlockOrderingStage(),
				new CodeGenerationStage(compilerOptions.EmitBinary),

				//new PreciseGCStage(),
				(compilerOptions.EmitBinary) ? new ProtectedRegionLayoutStage() : null,
			};
		}

		#endregion Static Methods

		#region Methods

		public Compiler(MosaCompiler mosaCompiler)
		{
			Architecture = mosaCompiler.CompilerOptions.Architecture;

			TypeSystem = mosaCompiler.TypeSystem;
			TypeLayout = mosaCompiler.TypeLayout;
			CompilerTrace = mosaCompiler.CompilerTrace;
			CompilerOptions = mosaCompiler.CompilerOptions;
			CompilationScheduler = mosaCompiler.CompilationScheduler;
			Linker = mosaCompiler.Linker;

			CompilerExtensions.AddRange(mosaCompiler.CompilerExtensions);

			methodStagePipelines = new Pipeline<BaseMethodCompilerStage>[mosaCompiler.MaxThreads];

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

			// Build the default compiler pipeline
			CompilerPipeline.Add(GetDefaultCompilerPipeline(CompilerOptions));

			foreach (var extension in CompilerExtensions)
			{
				extension.ExtendCompilerPipeline(CompilerPipeline);
			}

			Architecture.ExtendCompilerPipeline(CompilerPipeline);
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

			NewCompilerTraceEvent(CompilerEvent.CompiledMethod, method.FullName, threadID);
			CompilerTrace.TraceListener.OnMethodcompiled(method);
		}

		private MethodCompiler GetMethodCompiler(MosaMethod method, BasicBlocks basicBlocks, int threadID = 0)
		{
			var methodCompiler = new MethodCompiler(this, method, basicBlocks, threadID);

			var pipeline = methodStagePipelines[threadID];

			if (pipeline == null)
			{
				pipeline = new Pipeline<BaseMethodCompilerStage>();

				methodStagePipelines[threadID] = pipeline;

				pipeline.Add(GetDefaultMethodPipeline(CompilerOptions));

				foreach (var extension in CompilerExtensions)
				{
					extension.ExtendMethodCompilerPipeline(pipeline);
				}

				Architecture.ExtendMethodCompilerPipeline(pipeline);

				foreach (var stage in pipeline)
				{
					stage.Initialize(this);
				}
			}

			methodCompiler.Pipeline = pipeline;

			return methodCompiler;
		}

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
			foreach (var stage in CompilerPipeline)
			{
				stage.Initialize(this);
			}

			foreach (var stage in CompilerPipeline)
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
			foreach (BaseCompilerStage stage in CompilerPipeline)
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

		private void ExportCounters()
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
		private void NewCompilerTraceEvent(CompilerEvent compilerEvent, string message)
		{
			CompilerTrace.NewCompilerTraceEvent(compilerEvent, message, 0);
		}

		/// <summary>
		/// Traces the specified compiler event.
		/// </summary>
		/// <param name="compilerEvent">The compiler event.</param>
		/// <param name="message">The message.</param>
		/// <param name="threadID">The thread identifier.</param>
		private void NewCompilerTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			CompilerTrace.NewCompilerTraceEvent(compilerEvent, message, threadID);
		}

		private MosaType GetPlatformInternalRuntimeType()
		{
			return TypeSystem.GetTypeByName("Mosa.Runtime." + Architecture.PlatformName, "Internal");
		}

		private MosaType GeInternalRuntimeType()
		{
			return TypeSystem.GetTypeByName("Mosa.Runtime", "Internal");
		}

		#endregion Helper Methods
	}
}
