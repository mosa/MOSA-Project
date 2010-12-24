/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Tools.Compiler.Stages
{
	using Mosa.Runtime.CompilerFramework;

	public class StaticAllocationResolutionStageWrapper : MethodCompilerStageWrapper<StaticAllocationResolutionStage>
	{
		public static readonly StaticAllocationResolutionStageWrapper Instance = new StaticAllocationResolutionStageWrapper();

		private StaticAllocationResolutionStageWrapper()
		{
			this.Enabled = false;
		}

		public override void AddOptions(NDesk.Options.OptionSet optionSet)
		{
			optionSet.Add(
				@"sa|enable-static-alloc",
				@"Performs static allocations at compile time.",
				enable => this.Enabled = enable != null);
		}
	}
}
