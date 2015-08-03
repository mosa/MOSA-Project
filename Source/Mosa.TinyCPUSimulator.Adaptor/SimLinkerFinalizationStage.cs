// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			var file = new SimStream(simCPU, Linker.BaseAddress);

			Linker.Emit(file);
		}
	}
}