using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Common;
using System.Collections.Generic;
using System;
using Mosa.Compiler.Framework;
using System.Linq;
using Mosa.Compiler.Framework.CompilerStages;

namespace Mosa.Compiler.Extensions.Dwarf
{

	public class DwarfCompilerExtension : BaseCompilerExtension
	{

		public override void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> pipeline)
		{
			pipeline.InsertBefore<LinkerFinalizationStage>(new DwarfCompilerStage());
		}

		public override void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline)
		{
		}

	}
}
