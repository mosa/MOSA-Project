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

		private readonly Pipeline<BaseMethodCompilerStage>[] MethodStagePipelines;

		private Dictionary<string, InstrinsicMethodDelegate> InternalIntrinsicMethods { get; } = new Dictionary<string, InstrinsicMethodDelegate>();

		private volatile int WorkCount;

		#endregion Data Members

		#region Properties

		/// <summary>
		/// Returns the architecture used by the compiler.
		/// </summary>
		public BaseArchitecture Architecture { get; }

		/// <summary>
		/// Gets the compiler pipeline.
		/// </summary>
		public Pipeline<BaseCompilerStage> CompilerPipeline { get; } = new Pipeline<BaseCompilerStage>();

		/// <summary>
		/// Gets the type system.
		/// </summary>
		public TypeSystem TypeSystem { get; }

		/// <summary>
		/// Gets the type layout.
		/// </summary>
		public MosaTypeLayout TypeLayout { get; }

		/// <summary>
		/// Gets the compiler trace.
		/// </summary>
		public CompilerTrace CompilerTrace { get; }

		/// <summary>
		/// Gets the compiler options.
		/// </summary>
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
		public MethodScheduler MethodScheduler { get; }

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
		public MosaType PlatformInternalRuntimeType { get; }

		/// <summary>
		/// Gets the type of the internal runtime.
		/// </summary>
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
				new StaticLoadOptimizationStage(),
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
			TypeSystem = mosaCompiler.TypeSystem;
			TypeLayout = mosaCompiler.TypeLayout;
			CompilerOptions = mosaCompiler.CompilerOptions;
			Linker = mosaCompiler.Linker;
			CompilerTrace = mosaCompiler.CompilerTrace;
			Architecture = CompilerOptions.Architecture;

			PostCompilerTraceEvent(CompilerEvent.CompilerStart);

			CompilerExtensions.AddRange(mosaCompiler.CompilerExtensions);

			MethodStagePipelines = new Pipeline<BaseMethodCompilerStage>[mosaCompiler.MaxThreads + 1];

			MethodScheduler = new MethodScheduler(this);
			MethodScanner = new MethodScanner(this);

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
			WorkCount = 0;
		}

		/// <summary>
		/// Compiles the method.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="threadID">The thread identifier.</param>
		public void CompileMethod(MosaMethod method, BasicBlocks basicBlocks, int threadID = 0)
		{
			PostCompilerTraceEvent(CompilerEvent.MethodCompileStart, method.FullName, threadID);

			var pipeline = GetOrCreateMethodStagePipeline(threadID);

			var methodCompiler = new MethodCompiler(this, method, basicBlocks, threadID)
			{
				Pipeline = pipeline
			};

			methodCompiler.Compile();

			PostCompilerTraceEvent(CompilerEvent.MethodCompileEnd, method.FullName, threadID);

			CompilerTrace.PostMethodCompiled(method);
		}

		private Pipeline<BaseMethodCompilerStage> GetOrCreateMethodStagePipeline(int threadID)
		{
			var pipeline = MethodStagePipelines[threadID];

			if (pipeline == null)
			{
				pipeline = new Pipeline<BaseMethodCompilerStage>();

				MethodStagePipelines[threadID] = pipeline;

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
		internal void Setup()
		{
			PostCompilerTraceEvent(CompilerEvent.SetupStart);

			foreach (var stage in CompilerPipeline)
			{
				stage.ExecuteInitialization(this);
			}

			foreach (var stage in CompilerPipeline)
			{
				PostCompilerTraceEvent(CompilerEvent.SetupStageStart, stage.Name);

				// Execute stage
				stage.ExecuteSetup();

				PostCompilerTraceEvent(CompilerEvent.SetupStageEnd, stage.Name);
			}

			PostCompilerTraceEvent(CompilerEvent.SetupEnd);
		}

		public void ExecuteCompile()
		{
			PostCompilerTraceEvent(CompilerEvent.CompilingMethods);

			while (true)
			{
				if (IsStopped)
					return;

				if (ProcessQueue() == null)
					break;
			}

			PostCompilerTraceEvent(CompilerEvent.CompilingMethodsCompleted);
		}

		private MosaMethod ProcessQueue(int threadID = 0)
		{
			try
			{
				WorkIncrement();

				var method = MethodScheduler.GetMethodToCompile();

				if (method == null)
					return null;

				return CompileMethod(method, threadID);
			}
			finally
			{
				WorkDecrement();
			}
		}

		public MosaMethod CompileMethod(MosaMethod method)
		{
			return CompileMethod(method, 0);
		}

		private MosaMethod CompileMethod(MosaMethod method, int threadID)
		{
			if (method.IsCompilerGenerated)
				return method;

			lock (method)
			{
				CompileMethod(method, null, threadID);
			}

			CompilerTrace.UpdatedCompilerProgress(
				MethodScheduler.TotalMethods,
				MethodScheduler.TotalMethods - MethodScheduler.TotalQueuedMethods);

			return method;
		}

		public void ExecuteThreadedCompile(int threads)
		{
			PostCompilerTraceEvent(CompilerEvent.CompilingMethods);

			ExecuteThreadedCompilePass(threads);

			PostCompilerTraceEvent(CompilerEvent.CompilingMethodsCompleted);
		}

		private int WorkIncrement()
		{
			return Interlocked.Increment(ref WorkCount);
		}

		private int WorkDecrement()
		{
			return Interlocked.Decrement(ref WorkCount);
		}

		private bool IsWorkDone()
		{
			return WorkCount == 0;
		}

		private void ExecuteThreadedCompilePass(int threads)
		{
			using (var finished = new CountdownEvent(1))
			{
				WorkIncrement();

				for (int threadID = 1; threadID <= threads; threadID++)
				{
					finished.AddCount();

					int tid = threadID;

					ThreadPool.QueueUserWorkItem(
						new WaitCallback(
							delegate
							{
								CompileWorker(tid);
								finished.Signal();
							}
						)
					);
				}

				WorkDecrement();

				finished.Signal();
				finished.Wait();
			}
		}

		private void CompileWorker(int threadID)
		{
			while (true)
			{
				var method = ProcessQueue(threadID);

				if (method != null)
					continue;

				if (IsWorkDone())
					return;

				Thread.Yield();
			}
		}

		/// <summary>
		/// Executes the compiler post compiler stages.
		/// </summary>
		/// <remarks>
		/// The method iterates the compilation stage chain and runs each
		/// stage on the input.
		/// </remarks>
		internal void Finalization()
		{
			PostCompilerTraceEvent(CompilerEvent.FinalizationStart);

			foreach (BaseCompilerStage stage in CompilerPipeline)
			{
				PostCompilerTraceEvent(CompilerEvent.FinalizationStageStart, stage.Name);

				// Execute stage
				stage.ExecuteFinalization();

				PostCompilerTraceEvent(CompilerEvent.FinalizationStageEnd, stage.Name);
			}

			MethodScanner.Complete();

			// Sum up the counters
			foreach (var methodData in CompilerData.MethodData)
			{
				GlobalCounters.Merge(methodData.Counters);
			}

			EmitCounters();

			PostCompilerTraceEvent(CompilerEvent.FinalizationEnd);
			PostCompilerTraceEvent(CompilerEvent.CompilerEnd);
		}

		public void Stop()
		{
			PostCompilerTraceEvent(CompilerEvent.Stopped);
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

		public void PostCompilerTraceEvent(CompilerEvent compilerEvent)
		{
			CompilerTrace.PostCompilerTraceEvent(compilerEvent, string.Empty, 0);
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
