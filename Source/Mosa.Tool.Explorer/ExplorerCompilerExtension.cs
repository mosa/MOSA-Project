// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Stages;
using Mosa.Tool.Explorer.Stages;

namespace Mosa.Tool.Explorer
{
	public class ExplorerCompilerExtension : BaseCompilerExtension
	{
		public override void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> pipeline)
		{
			pipeline.InsertAfterFirst<TypeInitializerStage>(
					new ExplorerMethodCompileTimeStage()
			);
		}

		public override void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline)
		{
			pipeline.Add(new DisassemblyStage());
			pipeline.Add(new DebugInfoStage());

			//pipeline.InsertAfterLast<GreedyRegisterAllocatorStage>(new StopStage());

			//new DominanceOutputStage(),
		}
	}
}
