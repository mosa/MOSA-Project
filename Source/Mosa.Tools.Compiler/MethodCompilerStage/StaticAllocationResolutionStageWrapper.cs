/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using NDesk.Options;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Tools.Compiler.MethodCompilerStage
{

	public class StaticAllocationResolutionStageWrapper : MethodCompilerStageWrapper<StaticAllocationResolutionStage>
	{
		// TODO: Remove instance usage; it is not thread safe
		public static readonly StaticAllocationResolutionStageWrapper Instance = new StaticAllocationResolutionStageWrapper();

		private StaticAllocationResolutionStageWrapper()
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
