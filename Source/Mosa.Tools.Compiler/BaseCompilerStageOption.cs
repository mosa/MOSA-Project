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

	public abstract class BaseCompilerStageOptions
	{

		public bool Enabled { get; set; }

		protected BaseCompilerStageOptions()
		{
			this.Enabled = true;
		}

		public abstract void AddOptions(OptionSet optionSet);
	}
}
