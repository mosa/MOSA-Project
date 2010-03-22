/*
 * (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Text;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Linker;
using System.IO;
using System.Diagnostics;

namespace Mosa.Tools.Compiler.Metadata
{
    /// <summary>
    /// This stage adds the CLI metadata and MOSA AOT metadata to the compiled assembly.
    /// </summary>
    /// <remarks>
    /// The metadata added by this stage conforms to the CLI specification with one addition: MOSA has
    /// an additional metadata heap, which contains tables similar to the CLI metadata tables. This additional
    /// heap maps the compiled code to the CLI metadata.
    /// </remarks>
    public sealed partial class MetadataBuilderStage : IAssemblyCompilerStage, IPipelineStage
    {
        /// <summary>
        /// Holds the signature of the metadata.
        /// </summary>
        private const int MetadataSignature = 0x424A5342;

        /// <summary>
        /// Holds the major metadata version number according ECMA-335.
        /// </summary>
        private const ushort MetadataMajorVersion = 1;

        /// <summary>
        /// Holds the minor metadata version according to ECMA-335.
        /// </summary>
        private const ushort MetadataMinorVersion = 1;

        /// <summary>
        /// The value of reseved metadata fields.
        /// </summary>
        private const int ReservedValue = 0;

        /// <summary>
        /// The value of metadata flags written by this stage.
        /// </summary>
        private const ushort MetadataFlags = 0;

        /// <summary>
        /// The version string emitted by this stage in the metadata root _header.
        /// </summary>
        /// <remarks>
        /// According to ECMA-335, Partition II, §24.2.1 we have to use a vendor specific version string.
        /// We may not use the Standard CLI 2005 or Standard CLI 2002 to identify our AOT'd binaries. The
        /// MOSA loader identifies MOSA CLI 2009 as being equivalent to Standard CLI 2005 with an additional
        /// metadata stream providing AOT data to link native code to the methods in the metadata.
        /// </remarks>
        private const string MetadataVersion = @"MOSA CLI 2009";

        /// <summary>
        /// Holds the index of the string stream in all stream information arrays.
        /// </summary>
        private const int StringStream = 0;

        /// <summary>
        /// Holds the index of the user string stream in all stream information arrays.
        /// </summary>
        private const int UserStringStream = 1;

        /// <summary>
        /// Holds the index of the blob stream in all stream information arrays.
        /// </summary>
        private const int BlobStream = 2;

        /// <summary>
        /// Holds the index of the guid stream in all stream information arrays.
        /// </summary>
        private const int GuidStream = 3;

        /// <summary>
        /// Holds the index of the table stream in all stream information arrays.
        /// </summary>
        private const int TableStream = 4;

        /// <summary>
        /// Holds the index of the MOSA stream in all stream information arrays.
        /// </summary>
        private const int MosaStream = 5;

        /// <summary>
        /// This stage emits 6 metadata streams.
        /// </summary>
        private const ushort MetadataStreams = 6;

        /// <summary>
        /// The maximum length of a metadata stream name in ASCII characters (bytes)
        /// </summary>
        private const int MaxMetadataStreamNameLengthInBytes = 32;

        /// <summary>
        /// The size of the fixed metadata table _header.
        /// </summary>
        private const int MetadataTableHeaderSize = 24;

        /// <summary>
        /// Holds the major version of the metadata table schema.
        /// </summary>
        private const byte MetadataTableSchemaMajorVersion = 2;

        /// <summary>
        /// Holds the minor version of the metadata table schema.
        /// </summary>
        private const byte MetadataTableSchemaMinorVersion = 0;

        /// <summary>
        /// Contains the names of the metadata streams emitted by this stage.
        /// </summary>
        private static readonly string[] MetadataStreamNames = new[] 
        {
            @"#Strings",
            @"#US",
            @"#Blob",
            @"#GUID",
            @"#~",
            @"#MOSA"
        };
		
		private AssemblyCompiler compiler;

		private IAssemblyLinker linker;		
		
        /// <summary>
        /// Holds the source metadata to emit in the target module.
        /// </summary>
        private IMetadataProvider _metadataSource;

        /// <summary>
        /// The stream, where the metadata is emitted to.
        /// </summary>
        private Stream _metadataStream;

        /// <summary>
        /// The binary writer used to write to the metadata stream.
        /// </summary>
        private BinaryWriter _metadataWriter;

        /// <summary>
        /// Holds the position of the metadata root _header in the metadata stream.
        /// </summary>
        private long _metadataRootHeaderPosition;

        /// <summary>
        /// Holds the heap size bitmask.
        /// </summary>
        private int _heapSizes;

        /// <summary>
        /// Holds positional information about each metadata stream.
        /// </summary>
        private readonly MetadataStreamPosition[] metadataStreamPositions;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataBuilderStage"/> class.
        /// </summary>
        public MetadataBuilderStage()
        {
            metadataStreamPositions = new MetadataStreamPosition[MetadataStreams];
        }

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"Metadata Builder Stage"; }
        }
		
		public void Setup(AssemblyCompiler compiler)
		{
			this.compiler = compiler;

            this.linker = RetrieveAssemblyLinkerFromCompiler();
            if (this.linker == null)
            {
                throw new InvalidOperationException(@"Can't build metadata without a linker.");
            }
		}

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        public void Run()
        {
            Initialize();
            try
            {
                WriteMetadata();
            }
            finally
            {
                Uninitialize();
            }
        }

        /// <summary>
        /// Initializes the metadata builder stage using the given assembly compiler.
        /// </summary>
        private void Initialize()
        {
            _metadataStream = AllocateMetadataStream();
            _metadataWriter = new BinaryWriter(_metadataStream, Encoding.UTF8);
            _metadataSource = this.compiler.Metadata;
        }

        /// <summary>
        /// Uninitializes this instance.
        /// </summary>
        private void Uninitialize()
        {
            _metadataWriter.Close();
            _metadataWriter = null;
            _metadataStream = null;
            _metadataSource = null;
        }

        /// <summary>
        /// Allocates the metadata stream in the output assembly.
        /// </summary>
        /// <returns>The allocated metadata stream.</returns>
        private Stream AllocateMetadataStream()
        {
            return this.linker.Allocate(Symbol.Name, SectionKind.Text, 0, 0);
        }

        /// <summary>
        /// Retrieves the assembly linker from compiler.
        /// </summary>
        /// <returns>The retrieved assembly linker.</returns>
        private IAssemblyLinker RetrieveAssemblyLinkerFromCompiler()
        {
            return this.compiler.Pipeline.FindFirst<IAssemblyLinker>();
        }

        /// <summary>
        /// Writes the metadata to the output file.
        /// </summary>
        private void WriteMetadata()
        {
            ReserveSpaceForMetadataRoot();
            WriteHeaps();
            WriteTableStream();
            WriteMosaTables();
            WriteMetadataRoot();
        }

        /// <summary>
        /// Writes the metadata heaps to the output file.
        /// </summary>
        private void WriteHeaps()
        {
            WriteStringsHeap();
            WriteUserStringHeap();
            WriteBlobHeap();
            WriteGuidHeap();
        }

        /// <summary>
        /// Reserves the space for metadata root structure and the stream headers at the beginning of the metadata stream.
        /// </summary>
        private void ReserveSpaceForMetadataRoot()
        {
            // Make sure we are at the start of the stream
            Debug.Assert(_metadataStream.Position == 0, @"Metadata stream is not at the start.");
            _metadataRootHeaderPosition = _metadataStream.Position;
            _metadataStream.Position += CalculateMetadataHeaderLength();
        }

        /// <summary>
        /// Calculates the length of the metadata _header.
        /// </summary>
        /// <returns>The length of the metadata _header.</returns>
        private static int CalculateMetadataHeaderLength()
        {
            const int metadataHeaderLength = 16;

            int versionLength = Encoding.UTF8.GetBytes(MetadataVersion).Length;
            int length = metadataHeaderLength + versionLength;
            int padding = versionLength % 4;
            if (padding != 0)
            {
                padding = 4 - padding;
            }

            length += padding;
            length += 2 + 2;

            foreach (string streamName in MetadataStreamNames)
            {
                int nameLength = Encoding.ASCII.GetBytes(streamName).Length;
                int streamNameLength = Math.Min(nameLength + 1, 32);
                padding = streamNameLength % 4;
                if (padding != 0)
                {
                    padding = 4 - padding;
                }

                length += (4 + 4 + streamNameLength + padding);
            }

            return length;
        }

        /// <summary>
        /// Writes the metadata root.
        /// </summary>
        private void WriteMetadataRoot()
        {
            this._metadataStream.Position = this._metadataRootHeaderPosition;
            this.WriteMetadataRootHeader();
            this.WriteStreamHeaders();
        }

        /// <summary>
        /// Writes the metadata root _header.
        /// </summary>
        private void WriteMetadataRootHeader()
        {
            byte[] metadataVersion = Encoding.UTF8.GetBytes(MetadataVersion);
            int paddedMetadataVersionLength = metadataVersion.Length;
            int padding = 4 - (paddedMetadataVersionLength % 4);
            paddedMetadataVersionLength += padding;

            this._metadataWriter.Write(MetadataSignature);
            this._metadataWriter.Write(MetadataMajorVersion);
            this._metadataWriter.Write(MetadataMinorVersion);
            this._metadataWriter.Write(ReservedValue);
            this._metadataWriter.Write(paddedMetadataVersionLength);
            this._metadataWriter.Write(metadataVersion);
            this._metadataWriter.Write(new byte[padding]);
            this._metadataWriter.Write(MetadataFlags);
            this._metadataWriter.Write(MetadataStreams);
        }

        /// <summary>
        /// Writes the metadata stream headers.
        /// </summary>
        private void WriteStreamHeaders()
        {
            int index = 0;
            foreach (MetadataStreamPosition streamPosition in this.metadataStreamPositions)
            {
                this._metadataWriter.Write((uint)streamPosition.Position);
                this._metadataWriter.Write((uint)streamPosition.Size);

                byte[] streamName = Encoding.ASCII.GetBytes(MetadataStreamNames[index++]);
                int nameLength = Math.Min(streamName.Length + 1, MaxMetadataStreamNameLengthInBytes);
                int padding = nameLength % 4;
                if (padding != 0)
                {
                    padding = 4 - padding;
                }

                this._metadataWriter.Write(streamName, 0, nameLength - 1);
                this._metadataWriter.Write((byte)0);
                if (padding != 0)
                {
                    this._metadataWriter.Write(new byte[padding]);
                }
            }
        }

        /// <summary>
        /// Hook function, which is called before a stream is being written.
        /// </summary>
        /// <param name="streamIndex">Index of the stream.</param>
        private void StartWritingStream(int streamIndex)
        {
            this.metadataStreamPositions[streamIndex].Position = this._metadataStream.Position;
        }

        /// <summary>
        /// Hook function, which is called after writing the stream has completed.
        /// </summary>
        /// <param name="streamIndex">Index of the stream.</param>
        private void StoppedWritingStream(int streamIndex)
        {
            this.metadataStreamPositions[streamIndex].Size = this._metadataStream.Position - this.metadataStreamPositions[streamIndex].Position;
        }

        /// <summary>
        /// Writes the strings heap.
        /// </summary>
        private void WriteStringsHeap()
        {
            // Notify that we're starting to write a stream
            this.StartWritingStream(StringStream);

            const byte zero = 0;
            string value;
            TokenTypes lastToken = this._metadataSource.GetMaxTokenValue(TokenTypes.String);
            for (TokenTypes token = TokenTypes.String; token < lastToken; token++)
            {
                this._metadataSource.Read(token, out value);

                byte[] valueInUtf8Format = Encoding.UTF8.GetBytes(value);
                this._metadataWriter.Write(valueInUtf8Format);
                this._metadataWriter.Write(zero);

                token += valueInUtf8Format.Length;
            }

            // Notify that we've completed writing the stream
            this.StoppedWritingStream(StringStream);
        }

        /// <summary>
        /// Writes the user string heap.
        /// </summary>
        private void WriteUserStringHeap()
        {
            // Notify that we're starting to write a stream
            this.StartWritingStream(UserStringStream);

            string value;
            TokenTypes lastToken = this._metadataSource.GetMaxTokenValue(TokenTypes.UserString);
            for (TokenTypes token = TokenTypes.UserString; token < lastToken; )
            {
                token = this._metadataSource.Read(token, out value);

                // Convert the user string to a UTF-16 BLOB
                byte[] blob = new byte[Encoding.Unicode.GetByteCount(value) + 1];
                Encoding.Unicode.GetBytes(value, 0, value.Length, blob, 0);
            }

            // Notify that we've completed writing the stream
            this.StoppedWritingStream(UserStringStream);
        }

        /// <summary>
        /// Writes the BLOB heap.
        /// </summary>
        private void WriteBlobHeap()
        {
            // Notify that we're starting to write a stream
            this.StartWritingStream(BlobStream);

            byte[] value;
            TokenTypes lastToken = this._metadataSource.GetMaxTokenValue(TokenTypes.Blob);
            for (TokenTypes token = TokenTypes.Blob; token < lastToken; )
            {
                token = this._metadataSource.Read(token, out value);
                this.WriteBlobRow(value);
            }

            // Notify that we've completed writing the stream
            this.StoppedWritingStream(BlobStream);
        }

        /// <summary>
        /// Writes the BLOB row.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The number of bytes written.</returns>
        private int WriteBlobRow(byte[] value)
        {
            int size = this.WriteBlobLength(value.Length);
            this._metadataWriter.Write(value);
            return size + value.Length;
        }

        /// <summary>
        /// Writes the length of the BLOB.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>The number of bytes written.</returns>
        private int WriteBlobLength(int length)
        {
            if (length <= 0x7F)
            {
                this._metadataWriter.Write((byte)length);
                return 1;
            }
            else if (length <= 0x0CFF)
            {
                this._metadataWriter.Write((ushort)(length | 0x8000));
                return 2;
            }
            else if (length <= 0x1FFFFFFF)
            {
                this._metadataWriter.Write((uint)length | (uint)0xC0000000);
                return 4;
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes the GUID heap.
        /// </summary>
        private void WriteGuidHeap()
        {
            // Notify that we're starting to write a stream
            StartWritingStream(GuidStream);

            Guid value;
            TokenTypes lastToken = this._metadataSource.GetMaxTokenValue(TokenTypes.Guid);
            for (TokenTypes token = TokenTypes.Guid; token < lastToken; token++)
            {
                this._metadataSource.Read(token, out value);
                this._metadataWriter.Write(value.ToByteArray());
            }

            // Notify that we've completed writing the stream
            this.StoppedWritingStream(GuidStream);
        }

        /// <summary>
        /// Writes the default metadata tables.
        /// </summary>
        private void WriteTableStream()
        {
            // Notify that we're starting to write a stream
            this.StartWritingStream(TableStream);

            this.WriteMetadataTableHeader();
            this.WriteMetadataTables();

            // Notify that we've completed writing the stream
            this.StoppedWritingStream(TableStream);
        }

        /// <summary>
        /// Calculates the heap sizes.
        /// </summary>
        /// <returns>The calculated heap size.</returns>
        private byte CalculateHeapSizes()
        {
            byte result = 0;

            if (this.metadataStreamPositions[StringStream].Size > 0xFFFF)
            {
                result |= 1;
            }
            if (this.metadataStreamPositions[GuidStream].Size > 0xFFFF)
            {
                result |= 2;
            }
            if (this.metadataStreamPositions[BlobStream].Size > 0xFFFF)
            {
                result |= 4;
            }

            return result;
        }

        /// <summary>
        /// Writes the metadata table _header.
        /// </summary>
        private ulong WriteMetadataTableHeader()
        {
            this._metadataWriter.Write(ReservedValue);
            this._metadataWriter.Write(MetadataTableSchemaMajorVersion);
            this._metadataWriter.Write(MetadataTableSchemaMinorVersion);

            this._heapSizes = this.CalculateHeapSizes();
            this._metadataWriter.Write(this._heapSizes);
            this._metadataWriter.Write((byte)ReservedValue);

            ulong availableTables = this.BuildAvailableTableBitMask();
            this._metadataWriter.Write(availableTables);

            // HACK: Indicate that all tables are sorted. This may not be
            // true however for our metadata source. We need some API on
            // IMetadataSource to determine if the tables are sorted or
            // not. An alternative would be to decorate IMetadataSource
            // with a sorting IMetadataSource.
            this._metadataWriter.Write(availableTables);

            this.WriteTableRowCounts(availableTables);

            return availableTables;
        }

        /// <summary>
        /// Builds a bit mask, where each bit set to one indicates that the table exists in the stream.
        /// </summary>
        /// <returns>The bit mask, that indicates available tables.</returns>
        private ulong BuildAvailableTableBitMask()
        {
            ulong availableTables = 0;
            foreach (TokenTypes table in MetadataTableTokens)
            {
                TokenTypes lastToken = this._metadataSource.GetMaxTokenValue(table);
                if (lastToken != table)
                {
                    // The table has some rows in it - it is available.
                    availableTables |= (1U << ((int)table >> 24));
                }
            }
            return availableTables;
        }

        /// <summary>
        /// Writes the table row counts.
        /// </summary>
        /// <param name="availableTables">The available tables.</param>
        private void WriteTableRowCounts(ulong availableTables)
        {
            for (int table = 0; table < ((int)TokenTypes.MaxTable >> 24); table++)
            {
                if ((availableTables & (ulong)(1 << table)) != 0)
                {
                    TokenTypes lastToken = this._metadataSource.GetMaxTokenValue((TokenTypes)(table << 24));
                    int count = ((int)lastToken - table);
                    this._metadataWriter.Write(count);
                }
            }
        }

        /// <summary>
        /// Writes the metadata tables.
        /// </summary>
        private void WriteMetadataTables()
        {
            // Invoke all metadata table handlers in order of definition
            foreach (Action<IMetadataProvider, MetadataBuilderStage> tableHandler in MetadataTableHandlers)
            {
                tableHandler(this._metadataSource, this);
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void Write(byte value)
        {
            this._metadataWriter.Write(value);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void Write(ushort value)
        {
            this._metadataWriter.Write(value);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void Write(uint value)
        {
            this._metadataWriter.Write(value);
        }

        /// <summary>
        /// Writes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        private void Write(TokenTypes token)
        {
            switch (token & TokenTypes.TableMask)
            {
                case TokenTypes.String:
                    this.WriteVariableToken(token, (this._heapSizes & 1) == 1);
                    break;

                case TokenTypes.Guid:
                    this.WriteVariableToken(token, (this._heapSizes & 2) == 2);
                    break;

                case TokenTypes.Blob:
                    this.WriteVariableToken(token, (this._heapSizes & 4) == 4);
                    break;

                default:
                    this.WriteVariableToken(token, true);
                    break;
            }
        }

        /// <summary>
        /// Writes the variable token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="wide">if set to <c>true</c> the token is emitted using 4 bytes, otherwise only 2.</param>
        private void WriteVariableToken(TokenTypes token, bool wide)
        {
            if (wide == true)
            {
                this._metadataWriter.Write((uint)token);
            }
            else
            {
                this._metadataWriter.Write((ushort)token);
            }
        }

        /// <summary>
        /// Writes the MOSA specific metadata tables.
        /// </summary>
        private void WriteMosaTables()
        {
            // Notify that we're starting to write a stream
            this.StartWritingStream(MosaStream);

            // I'm not sure if this will really need multiple tables, but
            // for future extensibility we should do it. We must record
            // the following information in this stream:
            //
            // 1. Date of compilation
            // 2. Version of mosacl used to compile (build info/date)
            // 3. The compilation settings used to build the binary
            // 4. A table of pairs (Token, RVA) providing the native code entry points
            //    of compiled methods. An RVA of zero would indicate the method was not
            //    compiled ahead of time. The token is any one of MethodDef, MethodRef
            //    or MethodSpec.
            // something else?

            // Notify that we've completed writing the stream
            this.StoppedWritingStream(MosaStream);
        }
    }
}
