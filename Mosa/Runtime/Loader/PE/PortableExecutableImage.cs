/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker.PE;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Loader.PE
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

        private int _loadOrder;

        /// <summary>
        /// The stream, which provides the assembly data.
        /// </summary>
        private Stream _assemblyStream;

        /// <summary>
        /// Reader used to read the assembly data.
        /// </summary>
        private BinaryReader _assemblyReader;

        /// <summary>
        /// The DOS header of the Mosa.Runtime.Loader.PE image.
        /// </summary>
        private IMAGE_DOS_HEADER _dosHeader;

        /// <summary>
		/// The Mosa.Runtime.Loader.PE file header.
        /// </summary>
        private IMAGE_NT_HEADERS _ntHeader;

        /// <summary>
        /// The CLI header of the assembly.
        /// </summary>
        private CLI_HEADER _cliHeader;

        /// <summary>
		/// Sections in the Mosa.Runtime.Loader.PE file.
        /// </summary>
        private IMAGE_SECTION_HEADER[] _sections;

        /// <summary>
        /// Metadata of the assembly
        /// </summary>
        private IMetadataProvider _metadataRoot;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PortableExecutableImage"/> class.
        /// </summary>
        /// <param name="loadOrder">The load order.</param>
        /// <param name="stream">The stream.</param>
        private PortableExecutableImage(int loadOrder, Stream stream)
        {
            _assemblyStream = stream;
            _assemblyReader = new BinaryReader(stream);

            _loadOrder = loadOrder;

            // Load all headers by visiting them sequentially
            _dosHeader.Read(_assemblyReader);

            _assemblyReader.BaseStream.Seek(_dosHeader.e_lfanew, SeekOrigin.Begin);
            _ntHeader.Read(_assemblyReader);

            if (CLI_HEADER_DATA_DIRECTORY >= _ntHeader.OptionalHeader.NumberOfRvaAndSizes)
                throw new BadImageFormatException();

            _sections = new IMAGE_SECTION_HEADER[_ntHeader.FileHeader.NumberOfSections];
            for (int i = 0; i < _ntHeader.FileHeader.NumberOfSections; i++)
            {
                _sections[i].Read(_assemblyReader);
            }

            long position = ResolveVirtualAddress(_ntHeader.OptionalHeader.DataDirectory[CLI_HEADER_DATA_DIRECTORY].VirtualAddress);
            _assemblyReader.BaseStream.Seek(position, SeekOrigin.Begin);
            _cliHeader.Read(_assemblyReader);

            // Load the provider...
            position = ResolveVirtualAddress(_cliHeader.Metadata.VirtualAddress);
            _assemblyReader.BaseStream.Position = position;
            byte[] metadata = _assemblyReader.ReadBytes(_cliHeader.Metadata.Size);

            MetadataRoot mdr = new MetadataRoot(this);
            mdr.Initialize(metadata);
            _metadataRoot = mdr;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the entry point of the module.
        /// </summary>
        /// <value>The entry point.</value>
        public RuntimeMethod EntryPoint
        {
            get
            {
                if (_cliHeader.EntryPointToken == 0)
                    return null;

                return RuntimeBase.Instance.TypeLoader.GetMethod(this, (TokenTypes)_cliHeader.EntryPointToken);
            }
        }

        /// <summary>
        /// Retrieves the load order index of the module.
        /// </summary>
        /// <value></value>
        public int LoadOrder
        {
            get { return _loadOrder; }
        }

        /// <summary>
        /// Retrieves the name of the module.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get
            {
                string result;
                AssemblyRow arow;
                _metadataRoot.Read(TokenTypes.Assembly + 1, out arow);
                _metadataRoot.Read(arow.NameIdx, out result);
                return result;
            }
        }

        /// <summary>
        /// Provides access to the provider contained in the assembly.
        /// </summary>
        /// <value></value>
        public IMetadataProvider Metadata
        {
            get
            {
                return _metadataRoot;
            }
        }

        /// <summary>
        /// Gets the type of the module.
        /// </summary>
        /// <value>The type of the module.</value>
        public ModuleType ModuleType
        {
            get
            {
                if ((_ntHeader.FileHeader.Characteristics & IMAGE_FILE_HEADER.IMAGE_FILE_DLL) == IMAGE_FILE_HEADER.IMAGE_FILE_DLL)
                    return ModuleType.Library;

                return ModuleType.Executable;
            }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Retrieves an instruction for the specified relative virtual virtualAddress.
        /// </summary>
        /// <param name="rva">The method to retrieve the instruction stream for.</param>
        /// <returns>A new instance of CILInstructionStream, which represents the stream.</returns>
        public Stream GetInstructionStream(long rva)
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
        public Stream GetDataSection(long rva)
        {
            return new InstructionStream(_assemblyStream, ResolveVirtualAddress(rva));
        }

        /// <summary>
        /// Loads the specified load order.
        /// </summary>
        /// <param name="loadOrder">The load order.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static PortableExecutableImage Load(int loadOrder, Stream stream)
        {
            // Check preconditions
            if (null == stream)
                throw new ArgumentNullException("stream");

            // Create a new assembly instance
            return new PortableExecutableImage(loadOrder, stream);
        }

        /// <summary>
        /// Resolves the virtual address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        internal long ResolveVirtualAddress(long address)
        {
            if (null == _sections)
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
        /// Führt anwendungsspezifische Aufgaben durch, die mit der Freigabe, der Zurückgabe oder dem Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (null != _assemblyReader)
                _assemblyReader.Close();
            _assemblyReader = null;
            _assemblyStream = null;
        }

        #endregion // IDisposable Members
    }
}
