// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	Stop the method compiler - use in development
	/// </summary>
	public class StopStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			MethodCompiler.Stop();
		}
	}
}
