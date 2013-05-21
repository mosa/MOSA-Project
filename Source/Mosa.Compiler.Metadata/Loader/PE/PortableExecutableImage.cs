/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.IO;

using Mosa.Compiler.Common;
using Mosa.Compiler.LinkerFormat.PE;
using Mosa.Compiler.Metadata.Tables;

namespace Mosa.Compiler.Metadata.Loader.PE
{
	/// <summary>
	///
	/// </summary>
	public sealed class PortableExecutableImage : IMetadataModule, IDisposable
	{
		#region Constants

		/// <summary>
		///
		/// </summary>
		private const int CLI_HEADER_DATA_DIRECTORY = 0x0e;

		#endregion Constants

		#region Data members

		private int loadOrder = -1;

		private string name;

		/// <summary>
		/// The stream, which provides the assembly data.
		/// </summary>
		private Stream assemblyStream;

		/// <summary>
		/// Reader used to read the assembly data.
		/// </summary>
		private EndianAwareBinaryReader assemblyReader;

		/// <summary>
		/// The DOS header of the Mosa.Runtime.Metadata.Loader.PE image.
		/// </summary>
		private ImageDosHeader dosHeader;

		/// <summary>
		/// The Mosa.Runtime.Metadata.Loader.PE file header.
		/// </summary>
		private ImageNtHeaders ntHeader;

		/// <summary>
		/// The CLI header of the assembly.
		/// </summary>
		private CliHeader cliHeader;

		/// <summary>
		/// Sections in the Mosa.Runtime.Metadata.Loader.PE file.
		/// </summary>
		private ImageSectionHeader[] sections;

		/// <summary>
		/// Metadata of the assembly
		/// </summary>
		private IMetadataProvider metadataRoot;

		/// <summary>
		/// Metadata of the assembly
		/// </summary>
		private byte[] metadata;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PortableExecutableImage"/> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="codeBase">The code base.</param>
		public PortableExecutableImage(Stream stream)
		{
			// Check preconditions
			if (stream == null)
				throw new ArgumentNullException("stream");

			assemblyStream = stream;
			assemblyReader = new EndianAwareBinaryReader(stream, true);

			// Load all headers by visiting them sequentially
			dosHeader.Read(assemblyReader);

			assemblyReader.BaseStream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin);
			ntHeader.Read(assemblyReader);

			if (CLI_HEADER_DATA_DIRECTORY >= ntHeader.OptionalHeader.NumberOfRvaAndSizes)
				throw new BadImageFormatException();

			sections = new ImageSectionHeader[ntHeader.FileHeader.NumberOfSections];
			for (int i = 0; i < ntHeader.FileHeader.NumberOfSections; i++)
				sections[i].Read(assemblyReader);

			long position = ResolveVirtualAddress(ntHeader.OptionalHeader.DataDirectory[CLI_HEADER_DATA_DIRECTORY].VirtualAddress);
			assemblyReader.BaseStream.Seek(position, SeekOrigin.Begin);
			cliHeader.Read(assemblyReader);

			// Load the provider...
			position = ResolveVirtualAddress(cliHeader.Metadata.VirtualAddress);
			assemblyReader.BaseStream.Position = position;
			metadata = assemblyReader.ReadBytes(cliHeader.Metadata.Size);

			metadataRoot = new MetadataRoot(metadata);
		}

		#endregion Construction

		#region Properties

		/// <summary>
		///
		/// </summary>
		Token IMetadataModule.EntryPoint
		{
			get
			{
				if (cliHeader.EntryPointToken == 0)
					return Token.Zero;

				return new Token(cliHeader.EntryPointToken);
			}
		}

		/// <summary>
		/// Retrieves the load order index of the module.
		/// </summary>
		/// <value></value>
		int IMetadataModule.LoadOrder { get { return loadOrder; } set { loadOrder = value; } }

		/// <summary>
		/// Retrieves the name of the module.
		/// </summary>
		/// <value></value>
		string IMetadataModule.Name
		{
			get
			{
				if (name == null)
				{
					AssemblyRow arow = metadataRoot.ReadAssemblyRow(new Token(TableType.Assembly, 1));
					name = metadataRoot.ReadString(arow.Name);
				}

				return name;
			}
		}

		/// <summary>
		/// Provides access to the provider contained in the assembly.
		/// </summary>
		/// <value></value>
		IMetadataProvider IMetadataModule.Metadata { get { return metadataRoot; } }

		/// <summary>
		/// Gets the type of the module.
		/// </summary>
		/// <value>The type of the module.</value>
		ModuleType IMetadataModule.ModuleType
		{
			get
			{
				if ((ntHeader.FileHeader.Characteristics & ImageFileHeader.IMAGE_FILE_DLL) == ImageFileHeader.IMAGE_FILE_DLL)
					return ModuleType.Library;

				return ModuleType.Executable;
			}
		}

		/// <summary>
		/// Provides access to the metadata binary array.
		/// </summary>
		/// <value></value>
		public byte[] MetadataBinary { get { return metadata; } }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Retrieves an instruction for the specified relative virtual address.
		/// </summary>
		/// <param name="rva">The method to retrieve the instruction stream for.</param>
		/// <returns>A new instance of CILInstructionStream, which represents the stream.</returns>
		Stream IMetadataModule.GetInstructionStream(long rva)
		{
			return new InstructionStream(assemblyStream, ResolveVirtualAddress(rva));
		}

		/// <summary>
		/// Gets a stream into the data section, beginning at the specified RVA.
		/// </summary>
		/// <param name="rva">The rva.</param>
		/// <returns>
		/// A stream into the data section, pointed to the requested RVA.
		/// </returns>
		Stream IMetadataModule.GetDataSection(long rva)
		{
			return new InstructionStream(assemblyStream, ResolveVirtualAddress(rva));
		}

		/// <summary>
		/// Resolves the virtual address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		internal long ResolveVirtualAddress(long address)
		{
			if (sections == null)
			{
				return ((address / ntHeader.OptionalHeader.SectionAlignment) * ntHeader.OptionalHeader.FileAlignment) + (address % ntHeader.OptionalHeader.SectionAlignment);
			}
			else
			{
				foreach (var section in sections)
				{
					if (section.VirtualAddress <= address && address < section.VirtualAddress + section.VirtualSize)
					{
						return (address + section.PointerToRawData) - section.VirtualAddress;
					}
				}
			}

			throw new ArgumentException(@"Failed to resolve virtual virtualAddress to disk position.", @"virtualAddress");
		}

		#endregion Methods

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		void IDisposable.Dispose()
		{
			if (null != assemblyReader)
				assemblyReader.Close();
			assemblyReader = null;
			assemblyStream = null;
		}

		#endregion IDisposable Members

		public override string ToString()
		{
			return ((IMetadataModule)this).Name;
		}
	}
}