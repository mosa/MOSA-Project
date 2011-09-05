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
using Mosa.Runtime.InternalTrace;
using Mosa.Runtime.CompilerFramework.CIL;
using Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Tools.TypeExplorer
{
	class ExplorerMethodCompiler : BaseMethodCompiler
	{
		private IntPtr address = IntPtr.Zero;

		public ExplorerMethodCompiler(ExplorerAssemblyCompiler compiler, IArchitecture architecture, ICompilationSchedulerStage compilationScheduler, RuntimeType type, RuntimeMethod method, IInternalTrace internalTrace)
			: base(type, method, compiler.Pipeline.FindFirst<IAssemblyLinker>(), architecture, compiler.TypeSystem, compiler.TypeLayout, null, compilationScheduler, internalTrace)
		{

			// Populate the pipeline
			this.Pipeline.AddRange(new IMethodCompilerStage[] {
				new DecodingStage(),
				new BasicBlockBuilderStage(),
				new OperandDeterminationStage(),
				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
				
				//new DominanceCalculationStage(),
				//new PhiPlacementStage(),
				//new EnterSSA(),
				//new ConstantPropagationStage(ConstantPropagationStage.PropagationStage.PreFolding),
				//new ConstantFoldingStage(),
				//new ConstantPropagationStage(ConstantPropagationStage.PropagationStage.PostFolding),
				//new LeaveSSA(),

				new StackLayoutStage(),
				new PlatformStubStage(),

				new LoopAwareBlockOrderStage(),
				new CodeGenerationStage(),
				new ExceptionLayoutStage(),
			});
		}

	}
}
