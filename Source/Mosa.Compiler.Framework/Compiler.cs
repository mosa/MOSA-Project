// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
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

		private readonly Pipeline<BaseMethodCompilerStage>[] methodStagePipelines;

		private Dictionary<string, InstrinsicMethodDelegate> InternalIntrinsicMethods { get; } = new Dictionary<string, InstrinsicMethodDelegate>();

		#endregion Data Members

		#region Properties

		/// <summary>
		/// Returns the architecture used by the compiler.
		/// </summary>
		public BaseArchitecture Architecture { get; }

		/// <summary>
		/// Gets the pre compile pipeline.
		/// </summary>
		public Pipeline<BaseCompilerStage> CompilerPipeline { get; } = new Pipeline<BaseCompilerStage>();

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		public TypeSystem TypeSystem { get; }

		/// <summary>
		/// Gets the type layout.
		/// </summary>
		/// <value>The type layout.</value>
		public MosaTypeLayout TypeLayout { get; }

		/// <summary>
		/// Gets the compiler trace.
		/// </summary>
		/// <value>
		/// The compiler trace.
		/// </value>
		public CompilerTrace CompilerTrace { get; }

		/// <summary>
		/// Gets the compiler options.
		/// </summary>
		/// <value>The compiler options.</value>
		public CompilerOptions CompilerOptions { get; }

		/// <summary>
		/// Gets the method scanner.
		/// </summary>
		public MethodScanner MethodScanner { get; }

		/// <summary>
		/// Gets the counters.
		/// </summary>
		public Counters GlobalCounters { get; } = new Counters();

		/// <summary>
		/// Gets the scheduler.
		/// </summary>
		public CompilationScheduler CompilationScheduler { get; }

		/// <summary>
		/// Gets the linker.
		/// </summary>
		public MosaLinker Linker { get; }

		/// <summary>
		/// Gets the plug system.
		/// </summary>
		public PlugSystem PlugSystem { get; }

		/// <summary>
		/// Gets the type of the platform internal runtime.
		/// </summary>
		/// <value>
		/// The type of the platform internal runtime.
		/// </value>
		public MosaType PlatformInternalRuntimeType { get; }

		/// <summary>
		/// Gets the type of the internal runtime.
		/// </summary>
		/// <value>
		/// The type of the internal runtime.
		/// </value>
		public MosaType InternalRuntimeType { get; }

		/// <summary>
		/// Gets the compiler data.
		/// </summary>
		public CompilerData CompilerData { get; } = new CompilerData();

		/// <summary>
		/// Gets the compiler extensions.
		/// </summary>
		private List<BaseCompilerExtension> CompilerExtensions { get; } = new List<BaseCompilerExtension>();

		/// <summary>
		/// Gets or sets a value indicating whether [all stop].
		/// </summary>
		/// <value>
		///   <c>true</c> if [all stop]; otherwise, <c>false</c>.
		/// </value>
		public bool IsStopped { get; private set; }

		#endregion Properties

		#region Static Methods

		private static List<BaseCompilerStage> GetDefaultCompilerPipeline(CompilerOptions compilerOptions)
		{
			return new List<BaseCompilerStage> {
				new TypeInitializerStage(),
				new StaticFieldStage(),
				new MethodTableStage(),
				new ExceptionTableStage(),
				new MetadataStage(),
				new LinkerLayoutStage(),
				(compilerOptions.CompileTimeFile != null) ? new MethodCompileTimeStage() : null,
				(compilerOptions.OutputFile != null && compilerOptions.EmitBinary) ? new LinkerEmitStage() : null,
				(compilerOptions.MapFile != null) ? new MapFileStage() : null,
				(compilerOptions.DebugFile != null) ? new DebugFileStage() : null
			};
		}

		private static List<BaseMethodCompilerStage> GetDefaultMethodPipeline(CompilerOptions compilerOptions)
		{
			return new List<BaseMethodCompilerStage>() {
				new CILDecodingStage(),
				new ExceptionPrologueStage(),
				new CILOperandAssignmentStage(),
				new StackSetupStage(),
				new ProtectedRegionStage(),
				new ExceptionStage(),
				new CILStaticAllocationResolutionStage(),
				new CILTransformationStage(),
				new PlugStage(),
				new UnboxValueTypeStage(),
				(compilerOptions.EnableInlinedMethods) ? new InlineStage() : null,
				(compilerOptions.EnableInlinedMethods) ? new BlockMergeStage() : null,
				(compilerOptions.EnableInlinedMethods) ? new DeadBlockStage() : null,
				new PromoteTemporaryVariables(),
				(compilerOptions.EnableSSA) ? new EdgeSplitStage() : null,

				//new PreciseGCStage(),

				(compilerOptions.EnableSSA) ? new EnterSSAStage() : null,
				(compilerOptions.EnableValueNumbering && compilerOptions.EnableSSA) ? new ValueNumberingStage() : null,
				(compilerOptions.EnableLoopInvariantCodeMotion && compilerOptions.EnableSSA) ? new LoopInvariantCodeMotionStage() : null,
				(compilerOptions.EnableSparseConditionalConstantPropagation && compilerOptions.EnableSSA) ? new SparseConditionalConstantPropagationStage() : null,
				(compilerOptions.EnableIROptimizations) ? new IROptimizationStage() : null,
				(compilerOptions.EnableIRLongExpansion && compilerOptions.Architecture.NativePointerSize == 4) ? new IRLongDecompositionStage() : null,
				new LowerIRStage(),
				(compilerOptions.TwoPassOptimizations && compilerOptions.EnableValueNumbering && compilerOptions.EnableSSA) ? new ValueNumberingStage() : null,
				(compilerOptions.TwoPassOptimizations && compilerOptions.EnableLoopInvariantCodeMotion && compilerOptions.EnableSSA) ? new LoopInvariantCodeMotionStage() : null,
				(compilerOptions.TwoPassOptimizations && compilerOptions.EnableSparseConditionalConstantPropagation && compilerOptions.EnableSSA) ? new SparseConditionalConstantPropagationStage() : null,
				(compilerOptions.TwoPassOptimizations && compilerOptions.EnableIROptimizations && compilerOptions.EnableSSA) ? new IROptimizationStage() : null,
				(compilerOptions.EnableSSA) ? new LeaveSSAStage() : null,

				new BlockMergeStage(),
				new IRCleanupStage(),

				//new StopStage(),
				(compilerOptions.EnableInlinedMethods) ? new InlineEvaluationStage() : null,
				new DevirtualizeCallStage(),
				new CallStage(),
				new PlatformIntrinsicStage(),
				new PlatformEdgeSplitStage(),
				new VirtualRegisterRenameStage(),
				new GreedyRegisterAllocatorStage(),
				new StackLayoutStage(),
				new DeadBlockStage(),
				new BlockOrderingStage(),
				new CodeGenerationStage(compilerOptions.EmitBinary),

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
			CompilerOptions = mosaCompiler.CompilerOptions;
			CompilationScheduler = mosaCompiler.CompilationScheduler;
			Linker = mosaCompiler.Linker;
			CompilerTrace = mosaCompiler.CompilerTrace;

			MethodScanner = new MethodScanner(this);

			CompilerExtensions.AddRange(mosaCompiler.CompilerExtensions);

			methodStagePipelines = new Pipeline<BaseMethodCompilerStage>[mosaCompiler.MaxThreads];

			foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
			{
				if (!type.IsClass)
					continue;

				foreach (var method in type.GetRuntimeMethods())
				{
					// Now get all the IntrinsicMethodAttribute attributes
					var attributes = (IntrinsicMethodAttribute[])method.GetCustomAttributes(typeof(IntrinsicMethodAttribute), true);

					for (int i = 0; i < attributes.Length; i++)
					{
						var d = (InstrinsicMethodDelegate)Delegate.CreateDelegate(typeof(InstrinsicMethodDelegate), method);

						// Finally add the dictionary entry mapping the target name and the delegate
						InternalIntrinsicMethods.Add(attributes[i].Target, d);
					}
				}
			}

			PlugSystem = new PlugSystem(TypeSystem);

			PlatformInternalRuntimeType = GetPlatformInternalRuntimeType();
			InternalRuntimeType = GeInternalRuntimeType();

			// Build the default compiler pipeline
			CompilerPipeline.Add(GetDefaultCompilerPipeline(CompilerOptions));

			foreach (var extension in CompilerExtensions)
			{
				extension.ExtendCompilerPipeline(CompilerPipeline);
			}

			Architecture.ExtendCompilerPipeline(CompilerPipeline, CompilerOptions);

			IsStopped = false;
		}

		/// <summary>
		/// Compiles the method.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="threadID">The thread identifier.</param>
		public void CompileMethod(MosaMethod method, BasicBlocks basicBlocks, int threadID = 0)
		{
			PostCompilerTraceEvent(CompilerEvent.CompilingMethod, method.FullName, threadID);

			var pipeline = GetOrCreateMethodStagePipeline(threadID);

			var methodCompiler = new MethodCompiler(this, method, basicBlocks, threadID)
			{
				Pipeline = pipeline
			};

			methodCompiler.Compile();

			PostCompilerTraceEvent(CompilerEvent.CompiledMethod, method.FullName, threadID);

			CompilerTrace.PostMethodCompiled(method);
		}

		private Pipeline<BaseMethodCompilerStage> GetOrCreateMethodStagePipeline(int threadID)
		{
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

				Architecture.ExtendMethodCompilerPipeline(pipeline, CompilerOptions);

				foreach (var stage in pipeline)
				{
					stage.Initialize(this);
				}
			}

			return pipeline;
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
		/// Executes the compiler pre-compiler stages.
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
				PostCompilerTraceEvent(CompilerEvent.PreCompileStageStart, stage.Name);

				// Execute stage
				stage.ExecutePreCompile();

				PostCompilerTraceEvent(CompilerEvent.PreCompileStageEnd, stage.Name);
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
				if (IsStopped)
					return;

				if (CompilerMethodInQueue() == null)
					return;
			}
		}

		private MosaMethod CompilerMethodInQueue(int threadID = 0)
		{
			var method = CompilationScheduler.GetMethodToCompile();

			if (method == null)
				return null;

			return CompileMethod(method, threadID);
		}

		public MosaMethod CompileMethod(MosaMethod method, int threadID = 0)
		{
			lock (method)
			{
				CompileMethod(method, null, threadID);
			}

			CompilerTrace.UpdatedCompilerProgress(
				CompilationScheduler.TotalMethods,
				CompilationScheduler.TotalMethods - CompilationScheduler.TotalQueuedMethods);

			return method;
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

					ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
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
					}));
				}

				finished.Signal();
				finished.Wait();
			}
		}

		private void CompileWorker(int threadID)
		{
			while (true)
			{
				var method = CompilerMethodInQueue(threadID);

				if (method == null)
					return;
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
				PostCompilerTraceEvent(CompilerEvent.PostCompileStageStart, stage.Name);

				// Execute stage
				stage.ExecutePostCompile();

				PostCompilerTraceEvent(CompilerEvent.PostCompileStageEnd, stage.Name);
			}

			MethodScanner.Complete();

			// Sum up the counters
			foreach (var methodData in CompilerData.MethodData)
			{
				GlobalCounters.Merge(methodData.Counters);
			}

			EmitCounters();
		}

		public void Stop()
		{
			IsStopped = true;
		}

		public InstrinsicMethodDelegate GetInstrincMethod(string name)
		{
			InternalIntrinsicMethods.TryGetValue(name, out InstrinsicMethodDelegate value);

			return value;
		}

		private void EmitCounters()
		{
			foreach (var counter in GlobalCounters.Export())
			{
				PostCompilerTraceEvent(CompilerEvent.Counter, counter);
			}
		}

		//public LoadBinary(string filename)
		//{
		//}

		#endregion Methods

		#region Helper Methods

		public void PostTrace(TraceLog traceLog)
		{
			if (traceLog == null)
				return;

			CompilerTrace.PostTraceLog(traceLog);
		}

		/// <summary>
		/// Traces the specified compiler event.
		/// </summary>
		/// <param name="compilerEvent">The compiler event.</param>
		/// <param name="message">The message.</param>
		public void PostCompilerTraceEvent(CompilerEvent compilerEvent, string message)
		{
			CompilerTrace.PostCompilerTraceEvent(compilerEvent, message, 0);
		}

		/// <summary>
		/// Traces the specified compiler event.
		/// </summary>
		/// <param name="compilerEvent">The compiler event.</param>
		/// <param name="message">The message.</param>
		/// <param name="threadID">The thread identifier.</param>
		public void PostCompilerTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			CompilerTrace.PostCompilerTraceEvent(compilerEvent, message, threadID);
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

		#region Type Methods

		public MosaType GetTypeFromTypeCode(MosaTypeCode code)
		{
			switch (code)
			{
				case MosaTypeCode.Void: return TypeSystem.BuiltIn.Void;
				case MosaTypeCode.Boolean: return TypeSystem.BuiltIn.Boolean;
				case MosaTypeCode.Char: return TypeSystem.BuiltIn.Char;
				case MosaTypeCode.I1: return TypeSystem.BuiltIn.I1;
				case MosaTypeCode.U1: return TypeSystem.BuiltIn.U1;
				case MosaTypeCode.I2: return TypeSystem.BuiltIn.I2;
				case MosaTypeCode.U2: return TypeSystem.BuiltIn.U2;
				case MosaTypeCode.I4: return TypeSystem.BuiltIn.I4;
				case MosaTypeCode.U4: return TypeSystem.BuiltIn.U4;
				case MosaTypeCode.I8: return TypeSystem.BuiltIn.I8;
				case MosaTypeCode.U8: return TypeSystem.BuiltIn.U8;
				case MosaTypeCode.R4: return TypeSystem.BuiltIn.R4;
				case MosaTypeCode.R8: return TypeSystem.BuiltIn.R8;
				case MosaTypeCode.I: return TypeSystem.BuiltIn.I;
				case MosaTypeCode.U: return TypeSystem.BuiltIn.U;
				case MosaTypeCode.String: return TypeSystem.BuiltIn.String;
				case MosaTypeCode.TypedRef: return TypeSystem.BuiltIn.TypedRef;
				case MosaTypeCode.Object: return TypeSystem.BuiltIn.Object;
			}

			throw new CompilerException("Can't convert type code {code} to type");
		}

		public StackTypeCode GetStackTypeCode(MosaType type)
		{
			switch (type.IsEnum ? type.GetEnumUnderlyingType().TypeCode : type.TypeCode)
			{
				case MosaTypeCode.Boolean:
				case MosaTypeCode.Char:
				case MosaTypeCode.I1:
				case MosaTypeCode.U1:
				case MosaTypeCode.I2:
				case MosaTypeCode.U2:
				case MosaTypeCode.I4:
				case MosaTypeCode.U4:
					if (Architecture.Is32BitPlatform)
						return StackTypeCode.Int32;
					else
						return StackTypeCode.Int64;

				case MosaTypeCode.I8:
				case MosaTypeCode.U8:
					return StackTypeCode.Int64;

				case MosaTypeCode.R4:
				case MosaTypeCode.R8:
					return StackTypeCode.F;

				case MosaTypeCode.I:
				case MosaTypeCode.U:
					if (Architecture.Is32BitPlatform)
						return StackTypeCode.Int32;
					else
						return StackTypeCode.Int64;

				case MosaTypeCode.ManagedPointer:
					return StackTypeCode.ManagedPointer;

				case MosaTypeCode.UnmanagedPointer:
				case MosaTypeCode.FunctionPointer:
					return StackTypeCode.UnmanagedPointer;

				case MosaTypeCode.String:
				case MosaTypeCode.ValueType:
				case MosaTypeCode.ReferenceType:
				case MosaTypeCode.Array:
				case MosaTypeCode.Object:
				case MosaTypeCode.SZArray:
				case MosaTypeCode.Var:
				case MosaTypeCode.MVar:
					return StackTypeCode.O;

				case MosaTypeCode.Void:
					return StackTypeCode.Unknown;
			}

			throw new CompilerException($"Can't transform Type {type} to StackTypeCode");
		}

		public MosaType GetStackType(MosaType type)
		{
			switch (GetStackTypeCode(type))
			{
				case StackTypeCode.Int32:
					return type.TypeSystem.BuiltIn.I4;

				case StackTypeCode.Int64:
					return type.TypeSystem.BuiltIn.I8;

				case StackTypeCode.N:
					return type.TypeSystem.BuiltIn.I;

				case StackTypeCode.F:
					if (type.IsR4)
						return type.TypeSystem.BuiltIn.R4;
					else
						return type.TypeSystem.BuiltIn.R8;

				case StackTypeCode.O:
					return type;

				case StackTypeCode.UnmanagedPointer:
				case StackTypeCode.ManagedPointer:
					return type;
			}

			throw new CompilerException($"Can't convert {type.FullName} to stack type");
		}

		public MosaType GetStackTypeFromCode(StackTypeCode code)
		{
			switch (code)
			{
				case StackTypeCode.Int32:
					return TypeSystem.BuiltIn.I4;

				case StackTypeCode.Int64:
					return TypeSystem.BuiltIn.I8;

				case StackTypeCode.N:
					return TypeSystem.BuiltIn.I;

				case StackTypeCode.F:
					return TypeSystem.BuiltIn.R8;

				case StackTypeCode.O:
					return TypeSystem.BuiltIn.Object;

				case StackTypeCode.UnmanagedPointer:
					return TypeSystem.BuiltIn.Pointer;

				case StackTypeCode.ManagedPointer:
					return TypeSystem.BuiltIn.Object.ToManagedPointer();
			}

			throw new CompilerException($"Can't convert stack type code {code} to type");
		}

		#endregion Type Methods
	}
}
