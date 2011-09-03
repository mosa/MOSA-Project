/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using NDesk.Options;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Tools.Compiler.Options
{

	public class StaticAllocationResolutionStageOptions : BaseCompilerStageWithEnableOptions
	{

		private StaticAllocationResolutionStageOptions()
		{
			this.Enabled = false;
		}

		public override void AddOptions(OptionSet optionSet)
		{
			optionSet.Add(
				@"sa|enable-static-alloc",
				@"Performs static allocations at compile time.",
				enable => this.Enabled = enable != null);
		}

		
	}
}
