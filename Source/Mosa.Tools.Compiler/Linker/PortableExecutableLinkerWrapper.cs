/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

using Mosa.Compiler.Linker;
using Mosa.Compiler.Linker.PE;

using NDesk.Options;

namespace Mosa.Tools.Compiler.Linker
{
	/// <summary>
	/// Wraps the portable executable linker in the MOSA runtime and adds various options to it.
	/// </summary>
	public sealed class PortableExecutableLinkerWrapper : AssemblyCompilerStageWrapper<PortableExecutableLinkerStage>
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PortableExecutableLinkerWrapper"/> class.
		/// </summary>
		public PortableExecutableLinkerWrapper()
		{
		}

		#endregion // Construction

		#region AssemblyCompilerStageWrapper Overrides

		/// <summary>
		/// Adds the additional options for the parsing process to the given OptionSet.
		/// </summary>
		/// <param name="optionSet">A given OptionSet to add the options to.</param>
		public override void AddOptions(OptionSet optionSet)
		{
			optionSet.Add(
				"pe-no-checksum",
				"Causes no checksum to be written in the generated PE file. MOSA requires the checksum to be set. It is on by default, use this switch to turn it off.",
				delegate(string value)
				{
					this.Wrapped.SetChecksum = false;
				}
			);

			optionSet.Add<uint>(
				"pe-file-alignment=",
				"Determines the alignment of sections within the PE file. Must be a multiple of 512 bytes.",
				delegate(uint alignment)
				{
					try
					{
						this.Wrapped.FileAlignment = alignment;
					}
					catch (Exception x)
					{
						throw new OptionException(@"The specified file alignment is invalid.", @"pe-file-alignment", x);
					}
				}
			);

			optionSet.Add<uint>(
				"pe-section-alignment=",
				"Determines the alignment of sections in memory. Must be a multiple of 4096 bytes.",
				delegate(uint alignment)
				{
					try
					{
						this.Wrapped.SectionAlignment = alignment;
					}
					catch (Exception x)
					{
						throw new OptionException(@"The specified section alignment is invalid.", @"pe-section-alignment", x);
					}
				}
			);
		}

		#endregion // AssemblyCompilerStageWrapper Overrides
	}
}
