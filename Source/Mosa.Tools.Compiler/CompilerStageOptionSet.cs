/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;

using NDesk.Options;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Tools.Compiler.MethodCompilerStage
{

	public class CompilerStageOptionSet
	{

		private Dictionary<Type, BaseCompilerStageOptions> compilerStageOptions = new Dictionary<Type, BaseCompilerStageOptions>();

		public CompilerStageOptionSet()
		{

		}
		
		public void Add(Type stage, BaseCompilerStageOptions options)
		{
			compilerStageOptions.Add(stage, options);
		}

		public BaseCompilerStageOptions GetOptions(IPipelineStage stage)
		{
			return compilerStageOptions[stage.GetType()];
		}

		public BaseCompilerStageOptions GetOptions<T>(IPipelineStage stage) where T : BaseCompilerStageOptions
		{
			return (T)compilerStageOptions[stage.GetType()];
		}

	}
}
