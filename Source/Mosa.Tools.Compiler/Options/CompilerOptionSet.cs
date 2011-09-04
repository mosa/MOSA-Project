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

namespace Mosa.Tools.Compiler.Options
{

	public class CompilerOptionSet
	{

		protected List<BaseCompilerOptions> options;

		public CompilerOptionSet(OptionSet optionSet)
		{
			options = new List<BaseCompilerOptions>()
			{
				new BootFormatOptions(optionSet),
				new LinkerFormatOptions(optionSet),
				new Elf32LinkerOptions(optionSet),
				new MapFileGeneratorOptions(optionSet),
				new PortableExecutableOptions(optionSet),
				new StaticAllocationResolutionStageOptions(optionSet),
				new InstructionStatisticsOptions(optionSet)
			};

		}

		public BaseCompilerOptions GetOptions<T>() where T : BaseCompilerOptions
		{
			foreach(BaseCompilerOptions opt in options)
				if (opt is T)
					return (T)opt;

			return (T)null;
		}

	}
}
