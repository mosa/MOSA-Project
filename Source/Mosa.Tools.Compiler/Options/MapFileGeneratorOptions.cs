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
	public class MapFileGeneratorOptions : BaseCompilerStageWithEnableOptions
	{

		public string MapFile { get; set; }

		/// <summary>
		/// Adds the additional options for the parsing process to the given OptionSet.
		/// </summary>
		/// <param name="optionSet">A given OptionSet to add the options to.</param>
		public override void AddOptions(OptionSet optionSet)
		{
			optionSet.Add(
				"map=",
				"Generate a map {file} of the produced binary.",
				delegate(string file)
				{
					this.MapFile = file;
					this.Enabled = string.IsNullOrEmpty(file);
				}
			);
		}
	}
}
