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
using System.Collections.Generic;
using System.Text;
using System.IO;

using Mosa.Runtime.FileFormat.PE;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Runtime.Metadata.Loader.PE
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

		#endregion // Constants

		#region Data members

		private int _loadOrder = -1;

		private string name;

		/// <summary>
		/// The stream, which provides the assembly data.
		/// </summary>
		private Stream _assemblyStream;

		/// <summary>
		/// Reader used to read the assembly data.
		/// </summary>
		private BinaryReader _assemblyReader;

		/// <summary>
		/// The DOS _header of the Mosa.Runtime.Metadata.Loader.PE image.
		/// </summary>
		private IMAGE_DOS_HEADER _dosHeader;

		/// <summary>
		/// The Mosa.Runtime.Metadata.Loader.PE file _header.
		/// </summary>
		private IMAGE_NT_HEADERS _ntHeader;

		/// <summary>
		/// The CLI _header of the assembly.
		/// </summary>
		private CLI_HEADER _cliHeader;

		/// <summary>
		/// Sections in the Mosa.Runtime.Metadata.Loader.PE file.
		/// </summary>
		private IMAGE_SECTION_HEADER[] _sections;

		/// <summary>
		/// Metadata of the assembly
		/// </summary>
		private IMetadataProvider _metadataRoot;

		/// <summary>
		/// Metadata of the assembly
		/// </summary>
		private byte[] _metadata;

		private string codeBase;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PortableExecutableImage"/> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="codeBase">The code base.</param>
		private PortableExecutableImage(Stream stream, string codeBase)
		{
			this.codeBase = codeBase;

			_assemblyStream = stream;
			_assemblyReader = new BinaryReader(stream);

			// Load all headers by visiting them sequentially
			_dosHeader.Read(_assemblyReader);

			_assemblyReader.BaseStream.Seek(_dosHeader.e_lfanew, SeekOrigin.Begin);
			_ntHeader.Read(_assemblyReader);

			if (CLI_HEADER_DATA_DIRECTORY >= _ntHeader.OptionalHeader.NumberOfRvaAndSizes)
				throw new BadImageFormatException();

			_sections = new IMAGE_SECTION_HEADER[_ntHeader.FileHeader.NumberOfSections];
			for (int i = 0; i < _ntHeader.FileHeader.NumberOfSections; i++)
				_sections[i].Read(_assemblyReader);

			long position = ResolveVirtualAddress(_ntHeader.OptionalHeader.DataDirectory[CLI_HEADER_DATA_DIRECTORY].VirtualAddress);
			_assemblyReader.BaseStream.Seek(position, SeekOrigin.Begin);
			_cliHeader.Read(_assemblyReader);

			// Load the provider...
			position = ResolveVirtualAddress(_cliHeader.Metadata.VirtualAddress);
			_assemblyReader.BaseStream.Position = position;
			_metadata = _assemblyReader.ReadBytes(_cliHeader.Metadata.Size);

			_metadataRoot = new MetadataRoot(_metadata);
		}

		#endregion // Construction

		#region Properties

		string IMetadataModule.CodeBase { get { return this.codeBase; } }

		TokenTypes IMetadataModule.EntryPoint
		{
			get
			{
				if (_cliHeader.EntryPointToken == 0)
					return 0;

				return (TokenTypes)_cliHeader.EntryPointToken;
			}
		}

		/// <summary>
		/// Retrieves the load order index of the module.
		/// </summary>
		/// <value></value>
		int IMetadataModule.LoadOrder { get { return _loadOrder; } set { _loadOrder = value; } }

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
					AssemblyRow arow = _metadataRoot.ReadAssemblyRow(TokenTypes.Assembly + 1);
					name = _metadataRoot.ReadString(arow.NameIdx);
				}

				// HACK: Presents Mosa.Test.Korlib as mscorlib
				if (name == @"Mosa.Test.Korlib")
					return @"mscorlib";

				return name;
			}
		}

		/// <summary>
		/// Provides access to the provider contained in the assembly.
		/// </summary>
		/// <value></value>
		IMetadataProvider IMetadataModule.Metadata { get { return _metadataRoot; } }

		/// <summary>
		/// Gets the type of the module.
		/// </summary>
		/// <value>The type of the module.</value>
		ModuleType IMetadataModule.ModuleType
		{
			get
			{
				if ((_ntHeader.FileHeader.Characteristics & IMAGE_FILE_HEADER.IMAGE_FILE_DLL) == IMAGE_FILE_HEADER.IMAGE_FILE_DLL)
					return ModuleType.Library;

				return ModuleType.Executable;
			}
		}

		/// <summary>
		/// Provides access to the metadata binary array.
		/// </summary>
		/// <value></value>
		public byte[] MetadataBinary { get { return _metadata; } }

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Retrieves an instruction for the specified relative virtual address.
		/// </summary>
		/// <param name="rva">The method to retrieve the instruction stream for.</param>
		/// <returns>A new instance of CILInstructionStream, which represents the stream.</returns>
		Stream IMetadataModule.GetInstructionStream(long rva)
		{
			return new InstructionStream(_assemblyStream, ResolveVirtualAddress(rva));
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
			return new InstructionStream(_assemblyStream, ResolveVirtualAddress(rva));
		}

		/// <summary>
		/// Loads the specified load order.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="codeBase">The code base.</param>
		/// <returns></returns>
		public static PortableExecutableImage Load(Stream stream, string codeBase)
		{
			// Check preconditions
			if (stream == null)
				throw new ArgumentNullException("stream");

			// Create a new assembly instance
			return new PortableExecutableImage(stream, codeBase);
		}

		/// <summary>
		/// Resolves the virtual address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		internal long ResolveVirtualAddress(long address)
		{
			if (_sections == null)
			{
				return ((address / _ntHeader.OptionalHeader.SectionAlignment) * _ntHeader.OptionalHeader.FileAlignment) + (address % _ntHeader.OptionalHeader.SectionAlignment);
			}
			else
			{
				foreach (IMAGE_SECTION_HEADER section in _sections)
				{
					if (section.VirtualAddress <= address && address < section.VirtualAddress + section.VirtualSize)
					{
						return (address + section.PointerToRawData) - section.VirtualAddress;
					}
				}
			}

			throw new ArgumentException(@"Failed to resolve virtual virtualAddress to disk position.", @"virtualAddress");
		}

		#endregion // Methods

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		void IDisposable.Dispose()
		{
			if (null != _assemblyReader)
				_assemblyReader.Close();
			_assemblyReader = null;
			_assemblyStream = null;
		}

		#endregion // IDisposable Members

		public override string ToString()
		{
			return ((IMetadataModule)this).Name;
		}
	}
}
