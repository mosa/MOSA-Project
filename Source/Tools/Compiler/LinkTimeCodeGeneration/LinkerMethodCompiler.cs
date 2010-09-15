/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler.LinkTimeCodeGeneration
{
	sealed class LinkerMethodCompiler : BaseMethodCompiler
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerMethodCompiler"/> class.
		/// </summary>
		/// <param name="compiler">The assembly compiler executing this method compiler.</param>
		/// <param name="method">The metadata of the method to compile.</param>
		/// <param name="instructionSet">The instruction set.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="compiler"/>, <paramref name="method"/> or <paramref name="instructionSet"/> is null.</exception>
		public LinkerMethodCompiler(AssemblyCompiler compiler, ICompilationSchedulerStage compilationScheduler, RuntimeMethod method, InstructionSet instructionSet) :
			base(compiler.Pipeline.FindFirst<IAssemblyLinker>(), compiler.Architecture, compilationScheduler, method.DeclaringType, method, compiler.TypeSystem)
		{
			this.InstructionSet = instructionSet;
			this.CreateBlock(-1, 0);

			this.Pipeline.AddRange(new IMethodCompilerStage[] {
				new SimpleTraceBlockOrderStage(),
				new PlatformStubStage(),
				new CodeGenerationStage(),
			});
			compiler.Architecture.ExtendMethodCompilerPipeline(this.Pipeline);
		}

		#endregion // Construction
	}
}
