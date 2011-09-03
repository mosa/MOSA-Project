/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Runtime.CompilerFramework;
using Mosa.Tools.Compiler.MethodCompilerStage;

using NDesk.Options;

namespace Mosa.Tools.Compiler.Options
{
	/// <summary>
	/// </summary>
	public class InstructionStatisticsOptions : BaseCompilerStageWithEnableOptions
	{

		public string StatisticsFile { get; set; }

		/// <summary>
		/// Adds the additional options for the parsing process to the given OptionSet.
		/// </summary>
		/// <param name="optionSet">A given OptionSet to add the options to.</param>
		public override void AddOptions(OptionSet optionSet)
		{
			optionSet.Add(
				"stats=",
				"Generate instruction statistics {file} of the produced binary.",
				delegate(string file)
				{
					this.StatisticsFile = file;
					this.Enabled = string.IsNullOrEmpty(file);
				}
			);

		}
	}
}
