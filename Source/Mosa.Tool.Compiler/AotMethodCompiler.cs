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

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Tool.Compiler
{
	/// <summary>
	/// Specializes <see cref="AotMethodCompiler"/> for AOT purposes.
	/// </summary>
	public sealed class AotMethodCompiler : BaseMethodCompiler
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AotMethodCompiler"/> class.
		/// </summary>
		public AotMethodCompiler(AssemblyCompiler assemblyCompiler, ICompilationSchedulerStage compilationScheduler, RuntimeType type, RuntimeMethod method, CompilerOptions compilerOptions)
			: base(assemblyCompiler, type, method, null, compilationScheduler)
		{
			this.Pipeline.AddRange(
				new IMethodCompilerStage[] 
				{
					new DecodingStage(),
					new BasicBlockBuilderStage(),
					new ExceptionPrologueStage(),
					new OperandDeterminationStage(),
					new SingleUseMarkerStage(),
					new OperandUsageAnalyzerStage(),
					new StaticAllocationResolutionStage(),
					new CILTransformationStage(),

					(compilerOptions.EnableSSA) ? new DominanceCalculationStage() : null,
					(compilerOptions.EnableSSA) ? new PhiPlacementStage() : null,
					(compilerOptions.EnableSSA) ? new EnterSSA() : null,

					(compilerOptions.EnableSSA) ? new ConstantPropagationStage(ConstantPropagationStage.PropagationStage.PreFolding) : null,
					(compilerOptions.EnableSSA) ? new ConstantFoldingStage() : null,
					(compilerOptions.EnableSSA) ? new ConstantPropagationStage(ConstantPropagationStage.PropagationStage.PostFolding) : null,

					(compilerOptions.EnableSSA) ? new LeaveSSA() : null,
					
					new StrengthReductionStage(),

					new StackLayoutStage(),
					new PlatformStubStage(),
					new LoopAwareBlockOrderStage(),
					//new SimpleTraceBlockOrderStage(),
					//new ReverseBlockOrderStage(),	
					new LocalCSE(),
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
				var typeInitializerSchedulerStage = AssemblyCompiler.Pipeline.FindFirst<ITypeInitializerSchedulerStage>();
				typeInitializerSchedulerStage.Schedule(Method);
			}

			base.EndCompile();
		}

		#endregion // MethodCompilerBase Overrides
	}
}
