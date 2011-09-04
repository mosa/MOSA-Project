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
	public class MapFileGeneratorOptions : BaseCompilerWithEnableOptions
	{

		public string MapFile { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MapFileGeneratorOptions"/> class.
		/// </summary>
		/// <param name="optionSet">The option set.</param>
		public MapFileGeneratorOptions(OptionSet optionSet)
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
