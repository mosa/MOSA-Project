/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	/// <summary>
	/// Finalizes the sim linking
	/// </summary>
	public sealed class SimLinkerFinalizationStage : BaseCompilerStage
	{
		private SimCPU simCPU;

		public SimLinkerFinalizationStage(SimCPU simCPU)
		{
			this.simCPU = simCPU;
		}

		protected override void Run()
		{
			Linker.FinalizeLayout();

			var file = new SimStream(simCPU, Linker.BaseAddress);

			Linker.Emit(file);
		}
	}
}