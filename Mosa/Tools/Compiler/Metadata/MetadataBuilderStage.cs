/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
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
    sealed class MetadataBuilderStage : IAssemblyCompilerStage
    {
        /// <summary>
        /// Holds the signature of the metadata.
        /// </summary>
        private const int METADATA_SIGNATURE = 0x424A5342;

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
        private static readonly int METADATA_STREAMS = 6;

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
        /// Holds positional information about each metadata stream.
        /// </summary>
        private MetadataStreamPosition[] metadataStreamPositions;

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
        }

        /// <summary>
        /// Uninitializes this instance.
        /// </summary>
        private void Uninitialize()
        {
            this.metadataWriter.Close();
            this.metadataWriter = null;
            this.metadataStream = null;
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
            this.WriteMetadataTables();
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

            // HACK: Assume the length of the metadata version string doesn't change when the name is converted to UTF-8.
            int length = metadataHeaderLength + MetadataVersion.Length;
            int padding = MetadataVersion.Length % 4;
            if (padding != 0)
            {
                padding = 4 - padding;
            }

            length += padding;
            length += 2 + 2;

            foreach (string streamName in MetadataStreamNames)
            {
                // HACK: Assume the length of the streamName doesn't change when the name is converted to ASCII.
                int streamNameLength = Math.Min(streamName.Length, 32);
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
            this.metadataWriter.Write(METADATA_SIGNATURE);
            this.metadataWriter.Write(1);
            this.metadataWriter.Write(1);
            this.metadataWriter.Write(0);
            this.metadataWriter.Write(MetadataVersion.Length);
            this.metadataWriter.Write(Encoding.UTF8.GetBytes(MetadataVersion));
            this.metadataWriter.Write(23);
            this.metadataWriter.Write(0);
            this.metadataWriter.Write(METADATA_STREAMS);
        }

        /// <summary>
        /// Writes the metadata stream headers.
        /// </summary>
        private void WriteStreamHeaders()
        {
        }

        /// <summary>
        /// Writes the strings heap.
        /// </summary>
        private void WriteStringsHeap()
        {
        }

        /// <summary>
        /// Writes the user string heap.
        /// </summary>
        private void WriteUserStringHeap()
        {
        }

        /// <summary>
        /// Writes the BLOB heap.
        /// </summary>
        private void WriteBlobHeap()
        {
        }

        /// <summary>
        /// Writes the GUID heap.
        /// </summary>
        private void WriteGuidHeap()
        {
        }

        /// <summary>
        /// Writes the default metadata tables.
        /// </summary>
        private void WriteMetadataTables()
        {
        }

        /// <summary>
        /// Writes the MOSA specific metadata tables.
        /// </summary>
        private void WriteMosaTables()
        {
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
        }
    }
}
