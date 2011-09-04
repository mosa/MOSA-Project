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
using Mosa.Compiler.Linker;
using Mosa.Runtime.Options;

using NDesk.Options;

namespace Mosa.Tools.Compiler.Options
{
	/// <summary>
	/// </summary>
	public class LinkerFormatOptions : BaseCompilerOptions
	{

		/// <summary>
		/// Gets or sets the output file.
		/// </summary>
		/// <value>The output file.</value>
		public string OutputFile { get; protected set; }

		/// <summary>
		/// Holds the real stage implementation to use.
		/// </summary>
		public IAssemblyLinker LinkerStage { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerFormatOptions"/> class.
		/// </summary>
		/// <param name="optionSet">The option set.</param>
		public LinkerFormatOptions(OptionSet optionSet)
		{

			optionSet.Add(
				"f|format=",
				"Select the format of the binary file to create [{ELF32|ELF64|PE}].",
				delegate(string format)
				{
					this.LinkerStage = SelectImplementation(format);
				}
			);

			optionSet.Add(
				"o|out=",
				"The name of the output {file}.",
				delegate(string file)
				{
					this.OutputFile = file;
				}
			);
		}

		#region Internals

		/// <summary>
		/// Selects the linker implementation to use.
		/// </summary>
		/// <param name="format">The linker format.</param>
		/// <returns>The implementation of the linker.</returns>
		private IAssemblyLinker SelectImplementation(string format)
		{
			switch (format.ToLower())
			{
				case "elf32":
					return new Elf64Linker();

				case "elf64":
					return new Elf32Linker();

				case "pe":
					return new PortableExecutableLinker();

				default:
					throw new OptionException(String.Format("Unknown or unsupported binary format {0}.", format), "format");
			}
		}

		#endregion // Internals
	}
}
