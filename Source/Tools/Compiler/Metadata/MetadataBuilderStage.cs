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
	public sealed partial class MetadataBuilderStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, IPipelineStage
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

		private IAssemblyLinker linker;

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
		/// Holds the heap size bitmask.
		/// </summary>
		private int heapSizes;

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

		#region IPipelineStage members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"Metadata Builder Stage"; } }

		#endregion // IPipelineStage

		#region IAssemblyCompilerStage

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);

			linker = RetrieveAssemblyLinkerFromCompiler();

			if (linker == null)
				throw new InvalidOperationException(@"Can't build metadata without a linker.");
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IAssemblyCompilerStage.Run()
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

		#endregion IAssemblyCompilerStage

		/// <summary>
		/// Initializes the metadata builder stage using the given assembly compiler.
		/// </summary>
		private void Initialize()
		{
			metadataStream = AllocateMetadataStream();
			metadataWriter = new BinaryWriter(metadataStream, Encoding.UTF8);
			metadataSource = compiler.Metadata;
		}

		/// <summary>
		/// Uninitializes this instance.
		/// </summary>
		private void Uninitialize()
		{
			metadataWriter.Close();
			metadataWriter = null;
			metadataStream = null;
			metadataSource = null;
		}

		/// <summary>
		/// Allocates the metadata stream in the output assembly.
		/// </summary>
		/// <returns>The allocated metadata stream.</returns>
		private Stream AllocateMetadataStream()
		{
			return linker.Allocate(Symbol.Name, SectionKind.Text, 0, 0);
		}

		/// <summary>
		/// Retrieves the assembly linker from compiler.
		/// </summary>
		/// <returns>The retrieved assembly linker.</returns>
		private IAssemblyLinker RetrieveAssemblyLinkerFromCompiler()
		{
			return compiler.Pipeline.FindFirst<IAssemblyLinker>();
		}

		/// <summary>
		/// Writes the metadata to the output file.
		/// </summary>
		private void WriteMetadata()
		{
			ReserveSpaceForMetadataRoot();

			//WriteHeaps();
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
			Debug.Assert(metadataStream.Position == 0, @"Metadata stream is not at the start.");
			metadataRootHeaderPosition = metadataStream.Position;
			metadataStream.Position += CalculateMetadataHeaderLength();
		}

		/// <summary>
		/// Calculates the length of the metadata header.
		/// </summary>
		/// <returns>The length of the metadata header.</returns>
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
			metadataStream.Position = metadataRootHeaderPosition;
			WriteMetadataRootHeader();
			WriteStreamHeaders();
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

			metadataWriter.Write(MetadataSignature);
			metadataWriter.Write(MetadataMajorVersion);
			metadataWriter.Write(MetadataMinorVersion);
			metadataWriter.Write(ReservedValue);
			metadataWriter.Write(paddedMetadataVersionLength);
			metadataWriter.Write(metadataVersion);
			metadataWriter.Write(new byte[padding]);
			metadataWriter.Write(MetadataFlags);
			metadataWriter.Write(MetadataStreams);
		}

		/// <summary>
		/// Writes the metadata stream headers.
		/// </summary>
		private void WriteStreamHeaders()
		{
			int index = 0;
			foreach (MetadataStreamPosition streamPosition in metadataStreamPositions)
			{
				metadataWriter.Write((uint)streamPosition.Position);
				metadataWriter.Write((uint)streamPosition.Size);

				byte[] streamName = Encoding.ASCII.GetBytes(MetadataStreamNames[index++]);
				int nameLength = Math.Min(streamName.Length + 1, MaxMetadataStreamNameLengthInBytes);
				int padding = nameLength % 4;
				if (padding != 0)
				{
					padding = 4 - padding;
				}

				metadataWriter.Write(streamName, 0, nameLength - 1);
				metadataWriter.Write((byte)0);
				if (padding != 0)
				{
					metadataWriter.Write(new byte[padding]);
				}
			}
		}

		/// <summary>
		/// Hook function, which is called before a stream is being written.
		/// </summary>
		/// <param name="streamIndex">Index of the stream.</param>
		private void StartWritingStream(int streamIndex)
		{
			metadataStreamPositions[streamIndex].Position = metadataStream.Position;
		}

		/// <summary>
		/// Hook function, which is called after writing the stream has completed.
		/// </summary>
		/// <param name="streamIndex">Index of the stream.</param>
		private void StoppedWritingStream(int streamIndex)
		{
			metadataStreamPositions[streamIndex].Size = metadataStream.Position - metadataStreamPositions[streamIndex].Position;
		}

		/// <summary>
		/// Writes the strings heap.
		/// </summary>
		private void WriteStringsHeap()
		{
			// Notify that we're starting to write a stream
			StartWritingStream(StringStream);

			const byte zero = 0;
			TokenTypes lastToken = metadataSource.GetMaxTokenValue(TokenTypes.String);
			for (TokenTypes token = TokenTypes.String; token < lastToken; token++)
			{
				string value = metadataSource.ReadString(token);

				byte[] valueInUtf8Format = Encoding.UTF8.GetBytes(value);
				metadataWriter.Write(valueInUtf8Format);
				metadataWriter.Write(zero);

				token += valueInUtf8Format.Length;
			}

			// Notify that we've completed writing the stream
			StoppedWritingStream(StringStream);
		}

		/// <summary>
		/// Writes the user string heap.
		/// </summary>
		private void WriteUserStringHeap()
		{
			// Notify that we're starting to write a stream
			StartWritingStream(UserStringStream);

			//TODO
			//string value;
			//TokenTypes lastToken = metadataSource.GetMaxTokenValue(TokenTypes.UserString);
			//for (TokenTypes token = TokenTypes.UserString; token < lastToken; )
			//{
			//    token = metadataSource.Read(token, out value);

			//    // Convert the user string to a UTF-16 BLOB
			//    byte[] blob = new byte[Encoding.Unicode.GetByteCount(value) + 1];
			//    Encoding.Unicode.GetBytes(value, 0, value.Length, blob, 0);
			//}

			// Notify that we've completed writing the stream
			StoppedWritingStream(UserStringStream);
		}

		/// <summary>
		/// Writes the BLOB heap.
		/// </summary>
		private void WriteBlobHeap()
		{
			// Notify that we're starting to write a stream
			StartWritingStream(BlobStream);

			//TODO
			//byte[] value;
			//TokenTypes lastToken = metadataSource.GetMaxTokenValue(TokenTypes.Blob);
			//for (TokenTypes token = TokenTypes.Blob; token < lastToken; )
			//{
			//    token = metadataSource.ReadBlob(token, out value);
			//    WriteBlobRow(value);
			//}

			// Notify that we've completed writing the stream
			StoppedWritingStream(BlobStream);
		}

		/// <summary>
		/// Writes the BLOB row.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The number of bytes written.</returns>
		private int WriteBlobRow(byte[] value)
		{
			int size = WriteBlobLength(value.Length);
			metadataWriter.Write(value);
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
				metadataWriter.Write((byte)length);
				return 1;
			}
			else if (length <= 0x0CFF)
			{
				metadataWriter.Write((ushort)(length | 0x8000));
				return 2;
			}
			else if (length <= 0x1FFFFFFF)
			{
				metadataWriter.Write((uint)length | (uint)0xC0000000);
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

			//TODO
			//Guid value;
			//TokenTypes lastToken = metadataSource.GetMaxTokenValue(TokenTypes.Guid);
			//for (TokenTypes token = TokenTypes.Guid; token < lastToken; token++)
			//{
			//    metadataSource.Read(token, out value);
			//    metadataWriter.Write(value.ToByteArray());
			//}

			// Notify that we've completed writing the stream
			StoppedWritingStream(GuidStream);
		}

		/// <summary>
		/// Writes the default metadata tables.
		/// </summary>
		private void WriteTableStream()
		{
			// Notify that we're starting to write a stream
			StartWritingStream(TableStream);

			WriteMetadataTableHeader();
			WriteMetadataTables();

			// Notify that we've completed writing the stream
			StoppedWritingStream(TableStream);
		}

		/// <summary>
		/// Calculates the heap sizes.
		/// </summary>
		/// <returns>The calculated heap size.</returns>
		private byte CalculateHeapSizes()
		{
			byte result = 0;

			if (metadataStreamPositions[StringStream].Size > 0xFFFF)
			{
				result |= 1;
			}
			if (metadataStreamPositions[GuidStream].Size > 0xFFFF)
			{
				result |= 2;
			}
			if (metadataStreamPositions[BlobStream].Size > 0xFFFF)
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
			metadataWriter.Write(ReservedValue);
			metadataWriter.Write(MetadataTableSchemaMajorVersion);
			metadataWriter.Write(MetadataTableSchemaMinorVersion);

			heapSizes = CalculateHeapSizes();
			metadataWriter.Write(heapSizes);
			metadataWriter.Write((byte)ReservedValue);

			ulong availableTables = BuildAvailableTableBitMask();
			metadataWriter.Write(availableTables);

			// HACK: Indicate that all tables are sorted. This may not be
			// true however for our metadata source. We need some API on
			// IMetadataSource to determine if the tables are sorted or
			// not. An alternative would be to decorate IMetadataSource
			// with a sorting IMetadataSource.
			metadataWriter.Write(availableTables);

			WriteTableRowCounts(availableTables);

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
				TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
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
					TokenTypes lastToken = metadataSource.GetMaxTokenValue((TokenTypes)(table << 24));
					int count = ((int)lastToken - table);
					metadataWriter.Write(count);
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
				tableHandler(metadataSource, this);
			}
		}

		/// <summary>
		/// Writes the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		private void Write(byte value)
		{
			metadataWriter.Write(value);
		}

		/// <summary>
		/// Writes the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		private void Write(ushort value)
		{
			metadataWriter.Write(value);
		}

		/// <summary>
		/// Writes the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		private void Write(uint value)
		{
			metadataWriter.Write(value);
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
					WriteVariableToken(token, (heapSizes & 1) == 1);
					break;

				case TokenTypes.Guid:
					WriteVariableToken(token, (heapSizes & 2) == 2);
					break;

				case TokenTypes.Blob:
					WriteVariableToken(token, (heapSizes & 4) == 4);
					break;

				default:
					WriteVariableToken(token, true);
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
				metadataWriter.Write((uint)token);
			}
			else
			{
				metadataWriter.Write((ushort)token);
			}
		}

		/// <summary>
		/// Writes the MOSA specific metadata tables.
		/// </summary>
		private void WriteMosaTables()
		{
			// Notify that we're starting to write a stream
			StartWritingStream(MosaStream);

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
			StoppedWritingStream(MosaStream);
		}
	}
}
