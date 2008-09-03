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
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Runtime.Loader.PE
{
    public sealed class PortableExecutableImage : IMetadataModule, IDisposable
    {
        #region Constants

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
        /// The DOS header of the SharpOS.Runtime.Loader.PE image.
        /// </summary>
        private IMAGE_DOS_HEADER _dosHeader;

        /// <summary>
        /// The SharpOS.Runtime.Loader.PE file header.
        /// </summary>
        private IMAGE_NT_HEADERS _ntHeader;

        /// <summary>
        /// The CLI header of the assembly.
        /// </summary>
        private CLI_HEADER _cliHeader;

        /// <summary>
        /// Sections in the SharpOS.Runtime.Loader.PE file.
        /// </summary>
        private IMAGE_SECTION_HEADER[] _sections;

        /// <summary>
        /// Metadata of the assembly
        /// </summary>
        private IMetadataProvider _metadataRoot;

        #endregion // Data members

        #region Construction

        private PortableExecutableImage(int loadOrder, Stream stream)
        {
            _assemblyStream = stream;
            _assemblyReader = new BinaryReader(stream);

            _loadOrder = loadOrder;

            // Load all headers by visiting them sequentially
            _dosHeader.Load(_assemblyReader);

            _assemblyReader.BaseStream.Seek(_dosHeader.e_lfanew, SeekOrigin.Begin);
            _ntHeader.Load(_assemblyReader);

            if (CLI_HEADER_DATA_DIRECTORY >= _ntHeader.OptionalHeader.NumberOfRvaAndSizes)
                throw new BadImageFormatException();

            _sections = new IMAGE_SECTION_HEADER[_ntHeader.FileHeader.NumberOfSections];
            for (int i = 0; i < _ntHeader.FileHeader.NumberOfSections; i++)
            {
                _sections[i].Load(_assemblyReader);
            }

            long position = ResolveVirtualAddress(_ntHeader.OptionalHeader.DataDirectory[CLI_HEADER_DATA_DIRECTORY].VirtualAddress);
            _assemblyReader.BaseStream.Seek(position, SeekOrigin.Begin);
            _cliHeader.Load(_assemblyReader);

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

        public int LoadOrder
        {
            get { return _loadOrder; }
        }

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

        public IMetadataProvider Metadata
        {
            get
            {
                return _metadataRoot;
            }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Retrieves an instruction for the specified relative virtual address.
        /// </summary>
        /// <param name="method">The method to retrieve the instruction stream for.</param>
        /// <returns>A new instance of CILInstructionStream, which represents the stream.</returns>
        public Stream GetInstructionStream(long rva)
        {
            return new InstructionStream(_assemblyStream, ResolveVirtualAddress(rva));
        }

        public static PortableExecutableImage Load(int loadOrder, Stream stream)
        {
            // Check preconditions
            if (null == stream)
                throw new ArgumentNullException("stream");

            // Create a new assembly instance
            return new PortableExecutableImage(loadOrder, stream);
        }

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

            throw new ArgumentException(@"Failed to resolve virtual address to disk position.", @"address");
        }

        #endregion // Methods

        #region IDisposable Members

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
