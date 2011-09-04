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
using Mosa.Runtime.Options;

using NDesk.Options;

namespace Mosa.Tools.Compiler.Options
{
	/// <summary>
	/// </summary>
	public class InstructionStatisticsOptions : BaseCompilerWithEnableOptions
	{

		public string StatisticsFile { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="InstructionStatisticsOptions"/> class.
		/// </summary>
		/// <param name="optionSet">The option set.</param>
		public InstructionStatisticsOptions(OptionSet optionSet)
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
