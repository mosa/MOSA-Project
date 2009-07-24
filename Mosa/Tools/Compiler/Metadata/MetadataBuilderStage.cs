/*
 * (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Linker;
using System.IO;
using Mosa.Runtime.Loader;
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
    public sealed partial class MetadataBuilderStage : IAssemblyCompilerStage
    {
        /// <summary>
        /// Holds the signature of the metadata.
        /// </summary>
        private const int METADATA_SIGNATURE = 0x424A5342;

        /// <summary>
        /// Holds the major metadata version number according ECMA-335.
        /// </summary>
        private const int METADATA_MAJOR_VERSION = 1;

        /// <summary>
        /// Holds the minor metadata version according to ECMA-335.
        /// </summary>
        private const int METADATA_MINOR_VERSION = 1;

        /// <summary>
        /// The value of reseved metadata fields.
        /// </summary>
        private const int RESERVED_VALUE = 0;

        /// <summary>
        /// The value of metadata flags written by this stage.
        /// </summary>
        private const int METADATA_FLAGS = 0;

        /// <summary>
        /// The version string emitted by this stage in the metadata root header.
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
        private const int STRING_STREAM = 0;

        /// <summary>
        /// Holds the index of the user string stream in all stream information arrays.
        /// </summary>
        private const int USER_STRING_STREAM = 1;

        /// <summary>
        /// Holds the index of the blob stream in all stream information arrays.
        /// </summary>
        private const int BLOB_STREAM = 2;

        /// <summary>
        /// Holds the index of the guid stream in all stream information arrays.
        /// </summary>
        private const int GUID_STREAM = 3;

        /// <summary>
        /// Holds the index of the table stream in all stream information arrays.
        /// </summary>
        private const int TABLE_STREAM = 4;

        /// <summary>
        /// Holds the index of the MOSA stream in all stream information arrays.
        /// </summary>
        private const int MOSA_STREAM = 5;

        /// <summary>
        /// This stage emits 6 metadata streams.
        /// </summary>
        private const int METADATA_STREAMS = 6;

        /// <summary>
        /// The maximum length of a metadata stream name in ASCII characters (bytes)
        /// </summary>
        private const int MAX_METADATA_STREAM_NAME_LENGTH_IN_BYTES = 32;

        /// <summary>
        /// The size of the fixed metadata table header.
        /// </summary>
        private const int METADATA_TABLE_HEADER_SIZE = 24;

        /// <summary>
        /// Contains the names of the metadata streams emitted by this stage.
        /// </summary>
        private static readonly string[] MetadataStreamNames = new string[] 
        {
            @"#Strings",
            @"#US",
            @"#Blob",
            @"#GUID",
            @"#~",
            @"#MOSA"
        };

        /// <summary>
        /// Holds the source metadata to emit in the target module.
        /// </summary>
        private IMetadataProvider metadataSource;

        /// <summary>
        /// The stream, where the metadata is emitted to.
        /// </summary>
        private Stream metadataStream;

        /// <summary>
        /// The binary writer used to write to the metadata stream.
        /// </summary>
        private BinaryWriter metadataWriter;

        /// <summary>
        /// Holds the position of the metadata root header in the metadata stream.
        /// </summary>
        private long metadataRootHeaderPosition;

        /// <summary>
        /// Holds the position of the metadata table header.
        /// </summary>
        private long metadataTableHeaderPosition;

        /// <summary>
        /// Holds positional information about each metadata stream.
        /// </summary>
        private MetadataStreamPosition[] metadataStreamPositions;

        /// <summary>
        /// Holds the size flags for the heaps.
        /// </summary>
        private int heapSizes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataBuilderStage"/> class.
        /// </summary>
        public MetadataBuilderStage()
        {
            this.metadataStreamPositions = new MetadataStreamPosition[METADATA_STREAMS];
        }

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"Metadata Builder Stage"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(AssemblyCompiler compiler)
        {
            this.Initialize(compiler);
            try
            {
                this.WriteMetadata();
            }
            finally
            {
                this.Uninitialize();
            }
        }

        /// <summary>
        /// Initializes the metadata builder stage using the given assembly compiler.
        /// </summary>
        /// <param name="compiler">The assembly compiler.</param>
        private void Initialize(AssemblyCompiler compiler)
        {
            IAssemblyLinker linker = this.RetrieveAssemblyLinkerFromCompiler(compiler);
            if (linker == null)
            {
                throw new InvalidOperationException(@"Can't build metadata without a linker.");
            }

            this.metadataStream = this.AllocateMetadataStream(linker);
            this.metadataWriter = new BinaryWriter(this.metadataStream, Encoding.UTF8);
            this.metadataSource = compiler.Metadata;
        }

        /// <summary>
        /// Uninitializes this instance.
        /// </summary>
        private void Uninitialize()
        {
            this.metadataWriter.Close();
            this.metadataWriter = null;
            this.metadataStream = null;
            this.metadataSource = null;
        }

        /// <summary>
        /// Allocates the metadata stream in the output assembly.
        /// </summary>
        /// <param name="linker">The linker to allocate the metadata stream in.</param>
        /// <returns>The allocated metadata stream.</returns>
        private Stream AllocateMetadataStream(IAssemblyLinker linker)
        {
            return linker.Allocate(Mosa.Runtime.Metadata.Symbol.Name, SectionKind.Text, 0, 0);
        }

        /// <summary>
        /// Retrieves the assembly linker from compiler.
        /// </summary>
        /// <param name="compiler">The compiler.</param>
        /// <returns>The retrieved assembly linker.</returns>
        private IAssemblyLinker RetrieveAssemblyLinkerFromCompiler(AssemblyCompiler compiler)
        {
            return compiler.Pipeline.Find<IAssemblyLinker>();
        }

        /// <summary>
        /// Writes the metadata to the output file.
        /// </summary>
        private void WriteMetadata()
        {
            this.ReserveSpaceForMetadataRoot();
            this.WriteHeaps();
            this.WriteTableStream();
            this.WriteMosaTables();
            this.WriteMetadataRoot();
        }

        /// <summary>
        /// Writes the metadata heaps to the output file.
        /// </summary>
        private void WriteHeaps()
        {
            this.WriteStringsHeap();
            this.WriteUserStringHeap();
            this.WriteBlobHeap();
            this.WriteGuidHeap();
        }

        /// <summary>
        /// Reserves the space for metadata root structure and the stream headers at the beginning of the metadata stream.
        /// </summary>
        private void ReserveSpaceForMetadataRoot()
        {
            // Make sure we are at the start of the stream
            Debug.Assert(this.metadataStream.Position == 0, @"Metadata stream is not at the start.");
            this.metadataRootHeaderPosition = this.metadataStream.Position;
            this.metadataStream.Position += this.CalculateMetadataHeaderLength();
        }

        /// <summary>
        /// Calculates the length of the metadata header.
        /// </summary>
        /// <returns>The length of the metadata header.</returns>
        private int CalculateMetadataHeaderLength()
        {
            const int metadataHeaderLength = 20;

            int length = metadataHeaderLength + Encoding.UTF8.GetBytes(MetadataVersion).Length;
            int padding = MetadataVersion.Length % 4;
            if (padding != 0)
            {
                padding = 4 - padding;
            }

            length += padding;
            length += 2 + 2;

            foreach (string streamName in MetadataStreamNames)
            {
                int streamNameLength = Math.Min(Encoding.ASCII.GetBytes(streamName).Length, 32);
                padding = streamNameLength % 4;
                if (padding != 0)
                {
                    padding = 4 - padding;
                }

                length += 4 + 4 + streamNameLength + padding;
            }

            return length;
        }

        /// <summary>
        /// Writes the metadata root.
        /// </summary>
        private void WriteMetadataRoot()
        {
            this.metadataStream.Position = this.metadataRootHeaderPosition;
            this.WriteMetadataRootHeader();
            this.WriteStreamHeaders();
        }

        /// <summary>
        /// Writes the metadata root header.
        /// </summary>
        private void WriteMetadataRootHeader()
        {
            byte[] metadataVersion = Encoding.UTF8.GetBytes(MetadataVersion);
            int paddedMetadataVersionLength = metadataVersion.Length;
            int padding = 4 - (paddedMetadataVersionLength % 4);
            paddedMetadataVersionLength += padding;

            this.metadataWriter.Write(METADATA_SIGNATURE);
            this.metadataWriter.Write(METADATA_MAJOR_VERSION);
            this.metadataWriter.Write(METADATA_MINOR_VERSION);
            this.metadataWriter.Write(RESERVED_VALUE);
            this.metadataWriter.Write(paddedMetadataVersionLength);
            this.metadataWriter.Write(metadataVersion);
            this.metadataWriter.Write(new byte[padding]);
            this.metadataWriter.Write(METADATA_FLAGS);
            this.metadataWriter.Write(METADATA_STREAMS);
        }

        /// <summary>
        /// Writes the metadata stream headers.
        /// </summary>
        private void WriteStreamHeaders()
        {
            int index = 0;
            foreach (MetadataStreamPosition streamPosition in this.metadataStreamPositions)
            {
                this.metadataWriter.Write((uint)streamPosition.Position);
                this.metadataWriter.Write((uint)streamPosition.Size);

                byte[] streamName = Encoding.ASCII.GetBytes(MetadataStreamNames[index++]);
                int nameLength = Math.Min(streamName.Length, MAX_METADATA_STREAM_NAME_LENGTH_IN_BYTES);
                int padding = MAX_METADATA_STREAM_NAME_LENGTH_IN_BYTES - nameLength;

                this.metadataWriter.Write(streamName, 0, nameLength);
                if (padding != 0)
                {
                    this.metadataWriter.Write(new byte[padding]);
                }
            }
        }

        /// <summary>
        /// Hook function, which is called before a stream is being written.
        /// </summary>
        /// <param name="streamIndex">Index of the stream.</param>
        private void StartWritingStream(int streamIndex)
        {
            this.metadataStreamPositions[streamIndex].Position = this.metadataStream.Position;
        }

        /// <summary>
        /// Hook function, which is called after writing the stream has completed.
        /// </summary>
        /// <param name="streamIndex">Index of the stream.</param>
        private void StoppedWritingStream(int streamIndex)
        {
            this.metadataStreamPositions[streamIndex].Size = this.metadataStream.Position - this.metadataStreamPositions[streamIndex].Position;
        }

        /// <summary>
        /// Writes the strings heap.
        /// </summary>
        private void WriteStringsHeap()
        {
            // Notify that we're starting to write a stream
            this.StartWritingStream(STRING_STREAM);

            const byte zero = 0;
            string value;
            TokenTypes lastToken = this.metadataSource.GetMaxTokenValue(TokenTypes.String);
            for (TokenTypes token = TokenTypes.String; token < lastToken; token++)
            {
                this.metadataSource.Read(token, out value);

                byte[] valueInUtf8Format = Encoding.UTF8.GetBytes(value);
                this.metadataWriter.Write(valueInUtf8Format);
                this.metadataWriter.Write(zero);
            }

            // Notify that we've completed writing the stream
            this.StoppedWritingStream(STRING_STREAM);
        }

        /// <summary>
        /// Writes the user string heap.
        /// </summary>
        private void WriteUserStringHeap()
        {
            // Notify that we're starting to write a stream
            this.StartWritingStream(USER_STRING_STREAM);

            string value;
            TokenTypes lastToken = this.metadataSource.GetMaxTokenValue(TokenTypes.UserString);
            for (TokenTypes token = TokenTypes.UserString; token < lastToken; token++)
            {
                this.metadataSource.Read(token, out value);

                // Convert the user string to a UTF-16 BLOB
                byte[] blob = new byte[Encoding.Unicode.GetByteCount(value) + 1];
                Encoding.Unicode.GetBytes(value, 0, value.Length, blob, 0);
                this.WriteBlobRow(blob);
            }

            // Notify that we've completed writing the stream
            this.StoppedWritingStream(USER_STRING_STREAM);
        }

        /// <summary>
        /// Writes the BLOB heap.
        /// </summary>
        private void WriteBlobHeap()
        {
            // Notify that we're starting to write a stream
            this.StartWritingStream(BLOB_STREAM);

            byte[] value;
            TokenTypes lastToken = this.metadataSource.GetMaxTokenValue(TokenTypes.Blob);
            for (TokenTypes token = TokenTypes.Blob; token < lastToken; token++)
            {
                this.metadataSource.Read(token, out value);
                this.WriteBlobRow(value);
            }

            // Notify that we've completed writing the stream
            this.StoppedWritingStream(BLOB_STREAM);
        }

        /// <summary>
        /// Writes the BLOB row.
        /// </summary>
        /// <param name="value">The value.</param>
        private void WriteBlobRow(byte[] value)
        {
            this.WriteBlobLength(value.Length);
            this.metadataWriter.Write(value);
        }

        /// <summary>
        /// Writes the length of the BLOB.
        /// </summary>
        /// <param name="length">The length.</param>
        private void WriteBlobLength(int length)
        {
            if (length <= 0x7F)
            {
                this.metadataWriter.Write((byte)length);
            }
            else if (length <= 0x0CFF)
            {
                this.metadataWriter.Write((ushort)(length | 0x8000));
            }
            else if (length <= 0x1FFFFFFF)
            {
                this.metadataWriter.Write((uint)length | (uint)0xC0000000);
            }
        }

        /// <summary>
        /// Writes the GUID heap.
        /// </summary>
        private void WriteGuidHeap()
        {
            // Notify that we're starting to write a stream
            this.StartWritingStream(GUID_STREAM);

            Guid value;
            TokenTypes lastToken = this.metadataSource.GetMaxTokenValue(TokenTypes.Guid);
            for (TokenTypes token = TokenTypes.Guid; token < lastToken; token++)
            {
                this.metadataSource.Read(token, out value);
                this.metadataWriter.Write(value.ToByteArray());
            }

            // Notify that we've completed writing the stream
            this.StoppedWritingStream(GUID_STREAM);
        }

        /// <summary>
        /// Writes the default metadata tables.
        /// </summary>
        private void WriteTableStream()
        {
            // Notify that we're starting to write a stream
            this.StartWritingStream(TABLE_STREAM);

            this.SkipMetadataTableHeaderSpace();
            this.heapSizes = this.CalculateHeapSizes();
            this.WriteMetadataTables();
            this.WriteMetadataTableHeader();

            // Notify that we've completed writing the stream
            this.StoppedWritingStream(TABLE_STREAM);
        }

        /// <summary>
        /// Calculates the heap sizes.
        /// </summary>
        /// <returns>The calculated heap size.</returns>
        private byte CalculateHeapSizes()
        {
            byte result = 0;

            if (this.metadataStreamPositions[STRING_STREAM].Size > 0xFFFF)
            {
                result |= 1;
            }
            if (this.metadataStreamPositions[GUID_STREAM].Size > 0xFFFF)
            {
                result |= 2;
            }
            if (this.metadataStreamPositions[BLOB_STREAM].Size > 0xFFFF)
            {
                result |= 4;
            }

            return result;
        }

        /// <summary>
        /// Skips the space of the metadata table header.
        /// </summary>
        private void SkipMetadataTableHeaderSpace()
        {
            this.metadataTableHeaderPosition = this.metadataStream.Position;
            this.metadataWriter.Write(new byte[METADATA_TABLE_HEADER_SIZE]);
        }

        /// <summary>
        /// Writes the metadata table header.
        /// </summary>
        private void WriteMetadataTableHeader()
        {
            this.metadataStream.Position = this.metadataTableHeaderPosition;
        }

        /// <summary>
        /// Writes the metadata tables.
        /// </summary>
        private void WriteMetadataTables()
        {
            // Invoke all metadata table handlers in order of definition
            foreach (Action<IMetadataProvider, MetadataBuilderStage> tableHandler in MetadataTableHandlers)
            {
                tableHandler(this.metadataSource, this);
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void Write(byte value)
        {
            this.metadataWriter.Write(value);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void Write(ushort value)
        {
            this.metadataWriter.Write(value);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void Write(uint value)
        {
            this.metadataWriter.Write(value);
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
                    this.WriteVariableToken(token, (this.heapSizes & 1) == 1);
                    break;

                case TokenTypes.Guid:
                    this.WriteVariableToken(token, (this.heapSizes & 2) == 2);
                    break;

                case TokenTypes.Blob:
                    this.WriteVariableToken(token, (this.heapSizes & 4) == 4);
                    break;

                default:
                    this.metadataWriter.Write((uint)token);
                    break;
            }
        }

        /// <summary>
        /// Writes the variable token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="wide">if set to <c>true</c> the token is emitted using 4 bytes, otherwise only 2.</param>
        private void WriteVariableToken(TokenTypes token,bool wide)
        {
            if (wide == true)
            {
                this.metadataWriter.Write((uint)token);
            }
            else
            {
                this.metadataWriter.Write((ushort)token);
            }
        }

        /// <summary>
        /// Writes the MOSA specific metadata tables.
        /// </summary>
        private void WriteMosaTables()
        {
            // Notify that we're starting to write a stream
            this.StartWritingStream(MOSA_STREAM);

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
            this.StoppedWritingStream(MOSA_STREAM);
        }
    }
}
