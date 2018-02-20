// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base Compiler Extension
	/// </summary>
	public abstract class BaseCompilerExtension
	{
		public abstract void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> pipeline);

		public abstract void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline);
	}
}
