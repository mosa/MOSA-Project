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
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Test.System
{
	class TestCaseMethodCompiler : BaseMethodCompiler
	{

		private IntPtr address = IntPtr.Zero;

		public IntPtr Address { get { return address; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="TestCaseMethodCompiler"/> class.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="method">The method.</param>
		public TestCaseMethodCompiler(TestCaseCompiler compiler, RuntimeMethod method)
			: base(compiler, method, null)
		{
			// Populate the pipeline
			Pipeline.AddRange(new IMethodCompilerStage[] {
				new CILDecodingStage(),
				new BasicBlockBuilderStage(),
				new ExceptionPrologueStage(),
				new OperandAssignmentStage(),
				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
				
				new LocalVariablePromotionStage(), 
				new	EdgeSplitStage(),
				new DominanceCalculationStage(),
				new PhiPlacementStage(),
				new EnterSSAStage(),
				new SSAOptimizations(),
				new LeaveSSA(),
				
				new StackLayoutStage(),
				new PlatformIntrinsicTransformationStage(),
				new PlatformStubStage(),
				new LoopAwareBlockOrderStage(),
				//new SimpleTraceBlockOrderStage(),
				//new ReverseBlockOrderStage(),  // reverse all the basic blocks and see if it breaks anything
				new CodeGenerationStage(),				
			});
		}

		#region BaseMethodCompiler Overrides

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
				(Compiler as TestCaseCompiler).QueueCCtorForInvocationAfterCompilation(cctor);
			}

			base.EndCompile();
		}

		#endregion // BaseMethodCompiler Overrides
	}
}
