/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai P. Reisert <kpreisert@googlemail.com>
 */

using System;

using Mosa.Runtime.CompilerFramework;
using Mosa.Tools.Compiler.MethodCompilerStage;

using CIL = Mosa.Runtime.CompilerFramework.CIL;

using NDesk.Options;

namespace Mosa.Tools.Compiler.MethodCompilerStage
{
	/// <summary>
	/// Wraps the constant folding optimization stage and adds an option to disable it.
	/// </summary>
	public class ConstantFoldingWrapper : MethodCompilerStageWrapper<ConstantFoldingStage>
	{
		/// <summary>
		/// Initializes a new instance of the ConstantFoldingWrapper class.
		/// </summary>
		public ConstantFoldingWrapper()
		{
		}

		/// <summary>
		/// Adds the additional options for the parsing process to the given OptionSet.
		/// </summary>
		/// <param name="optionSet">A given OptionSet to add the options to.</param>
		public override void AddOptions(OptionSet optionSet)
		{
			optionSet.Add(
				"opt-no-cf",
				"Disable constant folding.",
				delegate(string v)
				{
					if (v != null)
					{
						this.Enabled = false;
					}
				}
			);
		}
	}
}
