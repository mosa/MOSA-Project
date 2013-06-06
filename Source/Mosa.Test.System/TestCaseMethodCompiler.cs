/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Mosa.Test.System
{
	internal class TestCaseMethodCompiler : BaseMethodCompiler
	{
		/// <summary>
		///
		/// </summary>
		private long address = 0;

		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>
		/// The address.
		/// </value>
		public long Address { get { return address; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="TestCaseMethodCompiler" /> class.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="method">The method.</param>
		public TestCaseMethodCompiler(TestCaseCompiler compiler, RuntimeMethod method)
			: base(compiler, method, null, null)
		{
			// Populate the pipeline
			Pipeline.AddRange(new IMethodCompilerStage[] {
				new CILDecodingStage(),
				new BasicBlockBuilderStage(),
				new StackSetupStage(),
				new ExceptionPrologueStage(),
				new OperandAssignmentStage(),
				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
				new EdgeSplitStage(),
				new DominanceCalculationStage(),
				new PhiPlacementStage(),
				new EnterSSAStage(),
				new SSAOptimizations(),
				new LeaveSSA(),
				new PlatformStubStage(),
				new	PlatformEdgeSplitStage(),
				new GreedyRegisterAllocatorStage(),
				new StackLayoutStage(),
				new CodeGenerationStage(),
			});
		}

		#region BaseMethodCompiler Overrides

		public override Stream RequestCodeStream()
		{
			LinkerStream stream = base.RequestCodeStream() as LinkerStream;
			VirtualMemoryStream vms = (VirtualMemoryStream)stream.BaseStream;

			if (address == 0)
				address = vms.Base + vms.Position;

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
				Debug.WriteLine("Executing: " + Method.FullName);
				CCtor cctor = (CCtor)Marshal.GetDelegateForFunctionPointer(new IntPtr(address), typeof(CCtor));
				(Compiler as TestCaseCompiler).QueueCCtorForInvocationAfterCompilation(cctor);
			}

			base.EndCompile();
		}

		#endregion BaseMethodCompiler Overrides
	}
}