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
using Mosa.Compiler.Framework.Stages;
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
		/// <param name="compiler">The compiler.</param>
		/// <param name="method">The method.</param>
		public AotMethodCompiler(BaseCompiler compiler, RuntimeMethod method)
			: base(compiler, method, null)
		{
			var compilerOptions = compiler.CompilerOptions;

			Pipeline.AddRange(new IMethodCompilerStage[] {
				new CILDecodingStage(),
				new BasicBlockBuilderStage(),
				new ExceptionPrologueStage(),
				new OperandAssignmentStage(),
				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
					
				new IRCheckStage(),

				(compilerOptions.EnableSSA && compilerOptions.EnableSSAOptimizations) ? new LocalVariablePromotionStage() : null,
				(compilerOptions.EnableSSA) ? new EdgeSplitStage() : null,
				(compilerOptions.EnableSSA) ? new DominanceCalculationStage() : null,
				(compilerOptions.EnableSSA) ? new PhiPlacementStage() : null,
				(compilerOptions.EnableSSA) ? new EnterSSAStage() : null,
				(compilerOptions.EnableSSA && compilerOptions.EnableSSAOptimizations) ? new SSAOptimizations() : null,
				(compilerOptions.EnableSSA) ? new LeaveSSA() : null,
					
				new StackLayoutStage(),
				new PlatformIntrinsicTransformationStage(),
				new PlatformStubStage(),
				new LoopAwareBlockOrderStage(),
				//new SimpleTraceBlockOrderStage(),
				//new ReverseBlockOrderStage(),	
				//new LocalCSE(),
				new CodeGenerationStage(),
				//new RegisterUsageAnalyzerStage(),
			});
		}

		#endregion // Construction

		#region BaseMethodCompiler Overrides

		/// <summary>
		/// Called after the method compiler has finished compiling the method.
		/// </summary>
		protected override void EndCompile()
		{
			// If we're compiling a type initializer, run it immediately.
			const MethodAttributes attrs = MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Static;
			if ((Method.Attributes & attrs) == attrs && Method.Name == ".cctor")
			{
				var typeInitializerSchedulerStage = Compiler.Pipeline.FindFirst<ITypeInitializerSchedulerStage>();
				typeInitializerSchedulerStage.Schedule(Method);
			}

			base.EndCompile();
		}

		#endregion // BaseMethodCompiler Overrides
	}
}
