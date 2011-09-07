/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Compiler.Framework;
using Mosa.Compiler.Options;

using NDesk.Options;

namespace Mosa.Tools.Compiler.Options
{
	/// <summary>
	/// </summary>
	public class ArchitectureOptions : BaseCompilerWithEnableOptions
	{

		/// <summary>
		/// Holds the real stage implementation to use.
		/// </summary>
		public IArchitecture Architecture { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MapFileGeneratorOptions"/> class.
		/// </summary>
		/// <param name="optionSet">The option set.</param>
		public ArchitectureOptions(OptionSet optionSet)
		{
			optionSet.Add(
				"a|Architecture=",
				"Select one of the MOSA architectures to compile for [{x86}].",
				delegate(string arch)
				{
					this.Architecture = SelectImplementation(arch);
				}
			);
		}

		#region Internals

		private IArchitecture SelectImplementation(string architecture)
		{
			switch (architecture.ToLower())
			{
				case "x86":
					return Mosa.Platform.x86.Architecture.CreateArchitecture(Mosa.Platform.x86.ArchitectureFeatureFlags.AutoDetect);

				case "x64":
				default:
					throw new OptionException(String.Format("Unknown or unsupported architecture {0}.", architecture), "Architecture");
			}
		}

		#endregion // Internals

	}
}
