/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using Mosa.Compiler.Linker;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.CIL;
using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Tools.Compiler;
using Mosa.Tools.Compiler.Stage;

namespace Mosa.Tools.CompilerHelper
{
	class MethodCompiler : BaseMethodCompiler
	{
		private readonly CompilerHelper assemblyCompiler;

		private IntPtr address = IntPtr.Zero;

		public IntPtr Address { get { return address; } }

		public MethodCompiler(CompilerHelper compiler, IArchitecture architecture, ICompilationSchedulerStage compilationScheduler, RuntimeType type, RuntimeMethod method)
			: base(compiler.Pipeline.FindFirst<IAssemblyLinker>(), architecture, compilationScheduler, type, method, compiler.TypeSystem, compiler.TypeLayout, null)
		{
			this.assemblyCompiler = compiler;

			// Populate the pipeline
			this.Pipeline.AddRange(new IMethodCompilerStage[] {
				new DecodingStage(),
				//new InstructionLogger(),
				new BasicBlockBuilderStage(),
				//new InstructionLogger(),
				new OperandDeterminationStage(),
				new StaticAllocationResolutionStage(),
				//new InstructionLogger(),
				//new ConstantFoldingStage(),
				new CILTransformationStage(),
				new CILLeakGuardStage() { MustThrowCompilationException = true },
				//new InstructionLogger(),
				//InstructionStatisticsStage.Instance,
				//new DominanceCalculationStage(),
				//new EnterSSA(),
				//new ConstantPropagationStage(),
				//new ConstantFoldingStage(),
				//new LeaveSSA(),
				new StackLayoutStage(),
				new PlatformStubStage(),
				//new InstructionLogger(),
				//new BlockReductionStage(),
				new LoopAwareBlockOrderStage(),
				//new SimpleTraceBlockOrderStage(),
				//new ReverseBlockOrderStage(),  // reverse all the basic blocks and see if it breaks anything
				//new BasicBlockOrderStage()	
				//new CodeGenerationStage(),
			});
		}

	}
}
