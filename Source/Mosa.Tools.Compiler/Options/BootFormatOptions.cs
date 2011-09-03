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
	public class BootFormatOptions : BaseCompilerStageOptions
	{

		/// <summary>
		/// Holds the real stage implementation to use.
		/// </summary>
		public IAssemblyCompilerStage BootCompilerStage { get; set; }

		/// <summary>
		/// Adds the additional options for the parsing process to the given OptionSet.
		/// </summary>
		/// <param name="optionSet">A given OptionSet to add the options to.</param>
		public override void AddOptions(OptionSet optionSet)
		{
			optionSet.Add(
				"b|boot=",
				"Specify the bootable format of the produced binary [{mb0.7}].",
				delegate(string format)
				{
					this.BootCompilerStage = SelectImplementation(format);
				}
			);
		}

		#region Internals

		/// <summary>
		/// Selects the implementation.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		private IAssemblyCompilerStage SelectImplementation(string format)
		{
			switch (format.ToLower())
			{
				case "multiboot-0.7":
				case "mb0.7":
					return new Boot.Multiboot0695AssemblyStage();

				default:
					throw new OptionException(String.Format("Unknown or unsupported boot format {0}.", format), "boot");
			}
		}

		#endregion // Internals
	}
}
