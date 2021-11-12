// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Mosa Compiler
	/// </summary>
	public sealed class Compiler
	{
		private const uint MaxThreads = 1024;

		#region Data Members

		private readonly Pipeline<BaseMethodCompilerStage>[] MethodStagePipelines;

		private Dictionary<string, IntrinsicMethodDelegate> InternalIntrinsicMethods { get; } = new Dictionary<string, IntrinsicMethodDelegate>();

		private Dictionary<string, StubMethodDelegate> InternalStubMethods { get; } = new Dictionary<string, StubMethodDelegate>();

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
		/// Gets the compiler options.
		/// </summary>
		public CompilerSettings CompilerSettings { get; }

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
		/// Gets or sets a value indicating whether [all stop].
		/// </summary>
		public bool IsStopped { get; private set; }

		/// <summary>
		/// The stack frame
		/// </summary>
		internal Operand StackFrame { get; }

		/// <summary>
		/// The stack frame
		/// </summary>
		internal Operand StackPointer { get; }

		/// <summary>
		/// The program counter
		/// </summary>
		internal Operand ProgramCounter { get; }

		/// <summary>
		/// The link register
		/// </summary>
		internal Operand LinkRegister { get; }

		/// <summary>
		/// The exception register
		/// </summary>
		public Operand ExceptionRegister { get; }

		/// <summary>
		/// The ;eave target register
		/// </summary>
		public Operand LeaveTargetRegister { get; }

		public CompilerHooks CompilerHooks { get; }

		public int TraceLevel { get; }

		public bool Statistics { get; }

		public uint ObjectHeaderSize { get; }

		#endregion Properties

		#region Static Methods

		private static List<BaseCompilerStage> GetDefaultCompilerPipeline(CompilerSettings compilerSettings, bool is64BitPlatform)
		{
			return new List<BaseCompilerStage> {
				new InlinedSetupStage(),
				new UnitTestStage(),
				new TypeInitializerStage(),
				compilerSettings.Devirtualization ? new DevirtualizationStage() : null,
				new StaticFieldStage(),
				new MethodTableStage(),
				new ExceptionTableStage(),
				new MetadataStage(),
				(!string.IsNullOrEmpty(compilerSettings.PreLinkHashFile)) ? new PreLinkHashFileStage() : null,
				new LinkerLayoutStage(),
				(!string.IsNullOrEmpty(compilerSettings.PostLinkHashFile)) ? new PostLinkHashFileStage() : null,
				(!string.IsNullOrEmpty(compilerSettings.CompileTimeFile)) ? new MethodCompileTimeStage() : null,
				(!string.IsNullOrEmpty(compilerSettings.OutputFile) && compilerSettings.EmitBinary) ? new LinkerEmitStage() : null,
				(!string.IsNullOrEmpty(compilerSettings.MapFile)) ? new MapFileStage() : null,
				(!string.IsNullOrEmpty(compilerSettings.DebugFile)) ? new DebugFileStage() : null,
				(!string.IsNullOrEmpty(compilerSettings.InlinedFile)) ? new InlinedFileStage() : null,
			};
		}

		private static List<BaseMethodCompilerStage> GetDefaultMethodPipeline(CompilerSettings compilerSettings, bool is64BitPlatform)
		{
			return new List<BaseMethodCompilerStage>() {
				(!compilerSettings.CILDecodingStageV2) ? new CILDecodingStage() : null,
				(compilerSettings.CILDecodingStageV2) ? new CILDecodingStageV2() : null,
				(!compilerSettings.CILDecodingStageV2) ? new CILOperandAssignmentStage(): null,
				(!compilerSettings.CILDecodingStageV2) ? new CILProtectedRegionStage(): null,
				(!compilerSettings.CILDecodingStageV2) ? new CILTransformationStage(): null,

				new ExceptionStage(),
				new StackSetupStage(),
				compilerSettings.Devirtualization ? new DevirtualizeCallStage() : null,
				new PlugStage(),
				new RuntimeCallStage(),
				(compilerSettings.InlineMethods || compilerSettings.InlineExplicit) ? new InlineStage() : null,
				new PromoteTemporaryVariables(),
				new StaticLoadOptimizationStage(),

				(compilerSettings.BasicOptimizations) ? new OptimizationStage(false) : null,
				(compilerSettings.SSA) ? new EdgeSplitStage() : null,
				(compilerSettings.SSA) ? new EnterSSAStage() : null,
				(compilerSettings.BasicOptimizations && compilerSettings.SSA) ? new OptimizationStage(false) : null,
				(compilerSettings.ValueNumbering && compilerSettings.SSA) ? new ValueNumberingStage() : null,
				(compilerSettings.LoopInvariantCodeMotion && compilerSettings.SSA) ? new LoopInvariantCodeMotionStage() : null,
				(compilerSettings.SparseConditionalConstantPropagation && compilerSettings.SSA) ? new SparseConditionalConstantPropagationStage() : null,
				(compilerSettings.BasicOptimizations && compilerSettings.SSA) ? new OptimizationStage(false) : null,
				(compilerSettings.BitTracker) ? new BitTrackerStage() : null,
				(compilerSettings.BasicOptimizations && compilerSettings.SSA && compilerSettings.BitTracker && (compilerSettings.ValueNumbering || compilerSettings.LoopInvariantCodeMotion || compilerSettings.SparseConditionalConstantPropagation || compilerSettings.BitTracker)) ? new OptimizationStage(compilerSettings.LongExpansion) : null,
				(compilerSettings.TwoPass && compilerSettings.ValueNumbering && compilerSettings.SSA) ? new ValueNumberingStage() : null,
				(compilerSettings.TwoPass && compilerSettings.LoopInvariantCodeMotion && compilerSettings.SSA) ? new LoopInvariantCodeMotionStage() : null,
				(compilerSettings.TwoPass && compilerSettings.SparseConditionalConstantPropagation && compilerSettings.SSA) ? new SparseConditionalConstantPropagationStage() : null,
				(compilerSettings.TwoPass && compilerSettings.BitTracker) ? new BitTrackerStage() : null,
				(compilerSettings.TwoPass && compilerSettings.BasicOptimizations && compilerSettings.SSA) ? new OptimizationStage(compilerSettings.LongExpansion) : null,

				(compilerSettings.SSA) ? new ExitSSAStage() : null,
				new IRCleanupStage(),
				new NewObjectStage(),
				(compilerSettings.InlineMethods || compilerSettings.InlineExplicit) ? new InlineEvaluationStage() : null,

				//new StopStage(),

				new CallStage(),
				new CompoundStage(),
				new PlatformIntrinsicStage(),
				new PlatformEdgeSplitStage(),
				new VirtualRegisterRenameStage(),
				new GreedyRegisterAllocatorStage(),
				new StackLayoutStage(),
				new DeadBlockStage(),
				new BlockOrderingStage(),

				//new PreciseGCStage(),

				new CodeGenerationStage(compilerSettings.EmitBinary),
				(compilerSettings.EmitBinary) ? new ProtectedRegionLayoutStage() : null,
			};
		}

		#endregion Static Methods

		#region Methods

		public Compiler(MosaCompiler mosaCompiler)
		{
			TypeSystem = mosaCompiler.TypeSystem;
			TypeLayout = mosaCompiler.TypeLayout;
			CompilerSettings = mosaCompiler.CompilerSettings;
			Architecture = mosaCompiler.Platform;
			CompilerHooks = mosaCompiler.CompilerHooks;
			TraceLevel = CompilerSettings.TraceLevel;
			Statistics = CompilerSettings.Statistics;

			Linker = new MosaLinker(this);

			ObjectHeaderSize = Architecture.NativePointerSize + 4 + 4; // Hash Value (32-bit) + Lock & Status (32-bit) + Method Table

			StackFrame = Operand.CreateCPURegister(TypeSystem.BuiltIn.Pointer, Architecture.StackFrameRegister);
			StackPointer = Operand.CreateCPURegister(TypeSystem.BuiltIn.Pointer, Architecture.StackPointerRegister);
			LinkRegister = Architecture.LinkRegister == null ? null : Operand.CreateCPURegister(TypeSystem.BuiltIn.Object, Architecture.LinkRegister);
			ProgramCounter = Architecture.ProgramCounter == null ? null : Operand.CreateCPURegister(TypeSystem.BuiltIn.Object, Architecture.ProgramCounter);

			ExceptionRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.Object, Architecture.ExceptionRegister);
			LeaveTargetRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.Object, Architecture.LeaveTargetRegister);

			PostEvent(CompilerEvent.CompileStart);

			MethodStagePipelines = new Pipeline<BaseMethodCompilerStage>[MaxThreads];

			MethodScheduler = new MethodScheduler(this);
			MethodScanner = new MethodScanner(this);

			foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
			{
				if (!type.IsClass)
					continue;

				foreach (var method in type.GetRuntimeMethods())
				{
					// Now get all the IntrinsicMethodAttribute attributes
					var intrinsicMethodAttributes = (IntrinsicMethodAttribute[])method.GetCustomAttributes(typeof(IntrinsicMethodAttribute), true);

					for (int i = 0; i < intrinsicMethodAttributes.Length; i++)
					{
						var d = (IntrinsicMethodDelegate)System.Delegate.CreateDelegate(typeof(IntrinsicMethodDelegate), method);

						// Finally add the dictionary entry mapping the target name and the delegate
						InternalIntrinsicMethods.Add(intrinsicMethodAttributes[i].Target, d);
					}

					// Now get all the StubMethodAttribute attributes
					var stubMethodAttributes = (StubMethodAttribute[])method.GetCustomAttributes(typeof(StubMethodAttribute), true);

					for (int i = 0; i < stubMethodAttributes.Length; i++)
					{
						var d = (StubMethodDelegate)System.Delegate.CreateDelegate(typeof(StubMethodDelegate), method);

						// Finally add the dictionary entry mapping the target name and the delegate
						InternalStubMethods.Add(stubMethodAttributes[i].Target, d);
					}
				}
			}

			PlugSystem = new PlugSystem(TypeSystem);

			PlatformInternalRuntimeType = GetPlatformInternalRuntimeType();
			InternalRuntimeType = GeInternalRuntimeType();

			// Build the default compiler pipeline
			CompilerPipeline.Add(GetDefaultCompilerPipeline(CompilerSettings, Architecture.Is64BitPlatform));

			// Call hook to allow for the extension of the pipeline
			CompilerHooks.ExtendCompilerPipeline?.Invoke(CompilerPipeline);

			Architecture.ExtendCompilerPipeline(CompilerPipeline, CompilerSettings);

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
			PostEvent(CompilerEvent.MethodCompileStart, method.FullName, threadID);

			var pipeline = GetOrCreateMethodStagePipeline(threadID);

			var methodCompiler = new MethodCompiler(this, method, basicBlocks, threadID)
			{
				Pipeline = pipeline
			};

			methodCompiler.Compile();

			PostEvent(CompilerEvent.MethodCompileEnd, method.FullName, threadID);

			CompilerHooks.NotifyMethodCompiled?.Invoke(method);
		}

		private Pipeline<BaseMethodCompilerStage> GetOrCreateMethodStagePipeline(int threadID)
		{
			var pipeline = MethodStagePipelines[threadID];

			if (pipeline == null)
			{
				pipeline = new Pipeline<BaseMethodCompilerStage>();

				MethodStagePipelines[threadID] = pipeline;

				pipeline.Add(GetDefaultMethodPipeline(CompilerSettings, Architecture.Is64BitPlatform));

				// Call hook to allow for the extension of the pipeline
				CompilerHooks.ExtendMethodCompilerPipeline?.Invoke(pipeline);

				Architecture.ExtendMethodCompilerPipeline(pipeline, CompilerSettings);

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
			PostEvent(CompilerEvent.SetupStart);

			foreach (var stage in CompilerPipeline)
			{
				stage.ExecuteInitialization(this);
			}

			foreach (var stage in CompilerPipeline)
			{
				PostEvent(CompilerEvent.SetupStageStart, stage.Name);

				// Execute stage
				stage.ExecuteSetup();

				PostEvent(CompilerEvent.SetupStageEnd, stage.Name);
			}

			PostEvent(CompilerEvent.SetupEnd);
		}

		public void ExecuteCompile()
		{
			PostEvent(CompilerEvent.CompilingMethods);

			while (true)
			{
				if (IsStopped)
					return;

				if (ProcessQueue() == null)
					break;
			}

			PostEvent(CompilerEvent.CompilingMethodsCompleted);
		}

		private MosaMethod ProcessQueue(int threadID = 0)
		{
			try
			{
				var methodData = MethodScheduler.GetMethodToCompile();

				if (methodData == null)
					return null;

				CompileMethod(methodData.Method, threadID);

				return methodData.Method;
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public void CompileMethod(MosaMethod method)
		{
			if (!MethodScheduler.IsCompilable(method))
				return;

			Debug.Assert(!method.HasOpenGenericParams);

			CompileMethod(method, 0);
		}

		private MosaMethod CompileMethod(MosaMethod method, int threadID)
		{
			if (method.IsCompilerGenerated)
				return method;

			Debug.Assert(!method.HasOpenGenericParams);

			lock (method)
			{
				CompileMethod(method, null, threadID);
			}

			CompilerHooks.NotifyProgress?.Invoke(MethodScheduler.TotalMethods, MethodScheduler.TotalMethods - MethodScheduler.TotalQueuedMethods);

			return method;
		}

		public void ExecuteCompile(int maxThreads)
		{
			PostEvent(CompilerEvent.CompilingMethods);

			if (maxThreads > 0)
			{
				var threads = Enumerable
					.Range(0, maxThreads)
					.Select(x => new Thread(CompilePass))
					.ToList();

				threads.ForEach(x => x.Start());
				threads.ForEach(x => x.Join());
			}
			else
				CompilePass();

			PostEvent(CompilerEvent.CompilingMethodsCompleted);
		}

		private void CompilePass() //TODO: Add IProgress<> to report progress
		{
			var threadID = Thread.CurrentThread.ManagedThreadId;
			int success = 0;

			while (true)
			{
				var result = ProcessQueue(threadID);

				if (result == null)
					return;

				success++;
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
			PostEvent(CompilerEvent.FinalizationStart);

			foreach (BaseCompilerStage stage in CompilerPipeline)
			{
				PostEvent(CompilerEvent.FinalizationStageStart, stage.Name);

				// Execute stage
				stage.ExecuteFinalization();

				PostEvent(CompilerEvent.FinalizationStageEnd, stage.Name);
			}

			MethodScanner.Complete();

			// Sum up the counters
			foreach (var methodData in CompilerData.MethodData)
			{
				GlobalCounters.Merge(methodData.Counters);
			}

			EmitCounters();

			PostEvent(CompilerEvent.FinalizationEnd);
			PostEvent(CompilerEvent.CompileEnd);
		}

		public void Stop()
		{
			IsStopped = true;
			PostEvent(CompilerEvent.Stopped);
		}

		public IntrinsicMethodDelegate GetInstrincMethod(string name)
		{
			InternalIntrinsicMethods.TryGetValue(name, out IntrinsicMethodDelegate value);

			return value;
		}

		public StubMethodDelegate GetStubMethod(string name)
		{
			InternalStubMethods.TryGetValue(name, out StubMethodDelegate value);

			return value;
		}

		private void EmitCounters()
		{
			foreach (var counter in GlobalCounters.Export())
			{
				PostEvent(CompilerEvent.Counter, counter);
			}
		}

		public string SearchPathsForFile(string filename)
		{
			foreach (var path in CompilerSettings.SearchPaths)
			{
				var file = Path.Combine(path, filename);

				if (File.Exists(file))
				{
					return file;
				}
			}

			return null;
		}

		public byte[] SearchPathsForFileAndLoad(string filename)
		{
			var file = SearchPathsForFile(filename);

			if (file == null)
				return null;

			return File.ReadAllBytes(file);
		}

		#endregion Methods

		#region Helper Methods

		public bool IsTraceable(int traceLevel)
		{
			return TraceLevel >= traceLevel;
		}

		public void PostTraceLog(TraceLog traceLog)
		{
			if (traceLog == null)
				return;

			CompilerHooks.NotifyTraceLog?.Invoke(traceLog);
		}

		/// <summary>
		/// Traces the specified compiler event.
		/// </summary>
		/// <param name="compilerEvent">The compiler event.</param>
		/// <param name="message">The message.</param>
		/// <param name="threadID">The thread identifier.</param>
		public void PostEvent(CompilerEvent compilerEvent, string message = null, int threadID = 0)
		{
			CompilerHooks.NotifyEvent?.Invoke(compilerEvent, message ?? string.Empty, threadID);
		}

		private MosaType GetPlatformInternalRuntimeType()
		{
			return TypeSystem.GetTypeByName("Mosa.Runtime." + Architecture.PlatformName, "Internal");
		}

		private MosaType GeInternalRuntimeType()
		{
			return TypeSystem.GetTypeByName("Mosa.Runtime", "Internal");
		}

		public MethodData GetMethodData(MosaMethod method)
		{
			return CompilerData.GetMethodData(method);
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
