/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.CIL;
using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Compiler.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.InternalLog;
using Mosa.Tools.Compiler.Stage;

namespace Mosa.Tools.Compiler
{
	/// <summary>
	/// Specializes <see cref="AotMethodCompiler"/> for AOT purposes.
	/// </summary>
	public sealed class AotMethodCompiler : BaseMethodCompiler
	{
		#region Data Members

		/// <summary>
		/// Holds the aot compiler, which started this method compiler.
		/// </summary>
		private readonly AssemblyCompiler assemblyCompiler;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AotMethodCompiler"/> class.
		/// </summary>
		public AotMethodCompiler(AssemblyCompiler compiler, ICompilationSchedulerStage compilationScheduler, RuntimeType type, RuntimeMethod method, IInternalLog internalLog)
			: base(compiler.Pipeline.FindFirst<IAssemblyLinker>(), compiler.Architecture, compilationScheduler, type, method, compiler.TypeSystem, compiler.TypeLayout, internalLog)
		{
			this.assemblyCompiler = compiler;
			this.Pipeline.AddRange(
				new IMethodCompilerStage[] 
				{
					new DecodingStage(),
					//InstructionLogger.Instance,
					new BasicBlockBuilderStage(),
					new OperandDeterminationStage(),
					StaticAllocationResolutionStageWrapper.Instance,
					//InstructionLogger.Instance,
					new CILTransformationStage(),
					//InstructionStatisticsStage.Instance,
					//new DominanceCalculationStage(),
					//InstructionLogger.Instance,
					//new EnterSSA(),
					//InstructionLogger.Instance,
					//new ConstantPropagationStage(),
					//InstructionLogger.Instance,
					//new ConstantFoldingStage(),
					//new StrengthReductionStage(),
					//InstructionLogger.Instance,
					//new LeaveSSA(),
					//InstructionLogger.Instance,
					new StackLayoutStage(),
					//InstructionLogger.Instance,
					new PlatformStubStage(),
					//new BlockReductionStage(),
					new LoopAwareBlockOrderStage(),
					//new SimpleTraceBlockOrderStage(),
					//new ReverseBlockOrderStage(),	
					//new LocalCSE(),
					new CodeGenerationStage(),
				});
		}

		#endregion // Construction

		#region MethodCompilerBase Overrides

		/// <summary>
		/// Called after the method compiler has finished compiling the method.
		/// </summary>
		protected override void EndCompile()
		{
			// If we're compiling a type initializer, run it immediately.
			const MethodAttributes attrs = MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Static;
			if ((Method.Attributes & attrs) == attrs && Method.Name == ".cctor")
			{
				var tiss = this.assemblyCompiler.Pipeline.FindFirst<TypeInitializers.ITypeInitializerSchedulerStage>();
				tiss.Schedule(Method);
			}

			base.EndCompile();
		}

		#endregion // MethodCompilerBase Overrides
	}
}
