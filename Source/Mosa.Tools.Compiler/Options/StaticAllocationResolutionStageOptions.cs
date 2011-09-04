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
using Mosa.Runtime.Options;

namespace Mosa.Tools.Compiler.Options
{

	public class StaticAllocationResolutionStageOptions : BaseCompilerWithEnableOptions
	{

		public StaticAllocationResolutionStageOptions(OptionSet optionSet)
		{
			this.Enabled = false;

			optionSet.Add(
				@"sa|enable-static-alloc",
				@"Performs static allocations at compile time.",
				enable => this.Enabled = enable != null);
		}
		
	}
}
