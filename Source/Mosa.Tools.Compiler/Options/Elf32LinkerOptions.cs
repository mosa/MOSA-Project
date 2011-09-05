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
using Mosa.Tools.Compiler.Stages;
using Mosa.Tools.Compiler.Linker;
using Mosa.Runtime.Options;

using NDesk.Options;

namespace Mosa.Tools.Compiler.Options
{
	/// <summary>
	/// </summary>
	public class Elf32LinkerOptions : BaseCompilerOptions
	{

		public uint FileAlignment { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Elf32LinkerOptions"/> class.
		/// </summary>
		/// <param name="optionSet">The option set.</param>
		public Elf32LinkerOptions(OptionSet optionSet)
		{
			optionSet.Add<uint>(
				"elf-file-alignment=",
				"Determines the alignment of sections within the ELF file. Must be a multiple of 512 bytes.",
				delegate(uint alignment)
				{
					try
					{
						this.FileAlignment = alignment;
					}
					catch (System.Exception x)
					{
						throw new OptionException(@"The specified file alignment is invalid.", @"elf-file-alignment", x);
					}
				}
			);

		}

		public void ApplyTo(Elf32LinkerStage elf32Linker)
		{
			elf32Linker.FileAlignment = this.FileAlignment;
		}

		public override void Apply(IPipelineStage stage)
		{
			Elf32LinkerStage linker = stage as Elf32LinkerStage;
			if (linker != null)
				ApplyTo(linker);
		}
	}
}
