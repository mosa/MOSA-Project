/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Linker
{
	public sealed class LinkerMethodCompiler : BaseMethodCompiler
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerMethodCompiler"/> class.
		/// </summary>
		/// <param name="assemblyCompiler">The assembly compiler executing this method compiler.</param>
		/// <param name="method">The metadata of the method to compile.</param>
		/// <param name="instructionSet">The instruction set.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="assemblyCompiler"/>, <paramref name="method"/> or <paramref name="instructionSet"/> is null.</exception>
		public LinkerMethodCompiler(AssemblyCompiler assemblyCompiler, ICompilationSchedulerStage compilationScheduler, RuntimeMethod method, InstructionSet instructionSet)
			: base(assemblyCompiler, method.DeclaringType, method,  instructionSet, compilationScheduler)
		{
			this.CreateBlock(-1, 0);

			this.Pipeline.AddRange(new IMethodCompilerStage[] {
				new SimpleTraceBlockOrderStage(),
				new PlatformStubStage(),
				new CodeGenerationStage(),
			});

			assemblyCompiler.Architecture.ExtendMethodCompilerPipeline(this.Pipeline);
		}

		#endregion // Construction
	}
}
