// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Tool.Explorer.Stages;

namespace Mosa.Tool.Explorer
{
	public class ExplorerCompilerExtension : BaseCompilerExtension
	{
		public override void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline)
		{
			pipeline.Add(
				new DisassemblyStage()
			);

			pipeline.Add(
				new DebugInfoStage()
			);
		}
	}
}
