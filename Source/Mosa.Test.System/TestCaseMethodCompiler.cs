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
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Test.System
{
	class TestCaseMethodCompiler : BaseMethodCompiler
	{

		private IntPtr address = IntPtr.Zero;

		public IntPtr Address { get { return address; } }

		public TestCaseMethodCompiler(TestCaseAssemblyCompiler assemblyCompiler, ICompilationSchedulerStage compilationScheduler, RuntimeType type, RuntimeMethod method)
			: base(assemblyCompiler, type, method, null, compilationScheduler)
		{
			// Populate the pipeline
			this.Pipeline.AddRange(new IMethodCompilerStage[] {
				new DecodingStage(),
				new BasicBlockBuilderStage(),
				new ExceptionPrologueStage(),
				new OperandDeterminationStage(),
				new StaticAllocationResolutionStage(),
				//new ConstantFoldingStage(),
				new CILTransformationStage(),
				//new CILLeakGuardStage() { MustThrowCompilationException = true },
				//InstructionStatisticsStage.Instance,
				//new DominanceCalculationStage(),
				//new PhiPlacementStage(),
				//new EnterSSA(),
				
				//new ConstantPropagationStage(ConstantPropagationStage.PropagationStage.PreFolding),
				//new ConstantFoldingStage(),
				//new ConstantPropagationStage(ConstantPropagationStage.PropagationStage.PostFolding),

				//new LeaveSSA(),
				new StackLayoutStage(),
				new PlatformStubStage(),
				//new BlockReductionStage(),
				new LoopAwareBlockOrderStage(),
				//new SimpleTraceBlockOrderStage(),
				//new ReverseBlockOrderStage(),  // reverse all the basic blocks and see if it breaks anything
				//new BasicBlockOrderStage()	
				new CodeGenerationStage(),				
			});
		}

		public override Stream RequestCodeStream()
		{
			LinkerStream stream = base.RequestCodeStream() as LinkerStream;
			VirtualMemoryStream vms = (VirtualMemoryStream)stream.BaseStream;

			if (address == IntPtr.Zero)
				address = new IntPtr(vms.Base.ToInt64() + vms.Position);

			return stream;
		}

		/// <summary>
		/// Called after the method compiler has finished compiling the method.
		/// </summary>
		protected override void EndCompile()
		{
			// If we're compiling a type initializer, run it immediately.
			MethodAttributes attrs = MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Static;
			if ((Method.Attributes & attrs) == attrs && Method.Name == ".cctor")
			{
				CCtor cctor = (CCtor)Marshal.GetDelegateForFunctionPointer(address, typeof(CCtor));
				(AssemblyCompiler as TestCaseAssemblyCompiler).QueueCCtorForInvocationAfterCompilation(cctor);
			}

			base.EndCompile();
		}
	}
}
