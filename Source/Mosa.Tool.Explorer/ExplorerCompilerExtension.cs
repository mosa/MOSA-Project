// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Tool.Explorer
{
	public class ExplorerCompilerExtension : BaseCompilerExtension
	{
		public override void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> pipeline)
		{ }

		public override void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline)
		{
			pipeline.Add(
				new DisassemblyStage()
			);
		}
	}
}
