/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	/// <summary>
	///
	/// </summary>
	public class SimCompiler : BaseCompiler
	{
		protected ISimAdapter simAdapter;

		/// <summary>
		/// Prevents a default instance of the <see cref="SimCompiler" /> class from being created.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="simAdapter">The sim adapter.</param>
		public SimCompiler(ISimAdapter simAdapter)
		{
			this.simAdapter = simAdapter;
		}

		/// <summary>
		/// Extends the compiler setup.
		/// </summary>
		public override void ExtendCompilerSetup()
		{
			// Build the assembly compiler pipeline
			PreCompilePipeline.Add(new ICompilerStage[] {
				new PlugStage(),
				//new MethodCompilerSchedulerStage(),
			});

			PostCompilePipeline.Add(new ICompilerStage[] {
				new TypeInitializerSchedulerStage(),
				new SimPowerUpStage(),
				new MethodLookupTableStage(),
				new MethodExceptionLookupTableStage(),
				new MetadataStage(),
				new SimLinkerFinalizationStage(simAdapter.SimCPU),
			});
		}

		/// <summary>
		/// Creates a method compiler
		/// </summary>
		/// <param name="method">The method to compile.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="threadID"></param>
		/// <returns>
		/// An instance of a MethodCompilerBase for the given type/method pair.
		/// </returns>
		protected override BaseMethodCompiler CreateMethodCompiler(MosaMethod method, BasicBlocks basicBlocks, int threadID)
		{
			return new SimMethodCompiler(this, method, simAdapter, basicBlocks, threadID);
		}
	}
}