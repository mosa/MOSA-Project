/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Metadata.Tables;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Metadata
{
	/// <summary>
	/// Provides convenient access to the provider table heap.
	/// </summary>
	public sealed class TableHeap : Heap
	{
		#region Types

		/// <summary>
		/// Delegate used to determine the record sizes with the given heap size.
		/// </summary>
		/// <param name="heap">The table heap making the request.</param>
		/// <returns>The size of the record for the given heap sizes.</returns>
		private delegate int SizeofDelegate(IMetadataProvider heap);

		#endregion Types

		#region Static members

		private static readonly int[][] IndexTables = new[]
		{
			new[] { (int)TableType.TypeDef, (int)TableType.TypeRef, (int)TableType.TypeSpec },
			new[] { (int)TableType.Field, (int)TableType.Param, (int)TableType.Property },
			new[] { (int)TableType.MethodDef, (int)TableType.Field, (int)TableType.TypeRef, (int)TableType.TypeDef, (int)TableType.Param, (int)TableType.InterfaceImpl, (int)TableType.MemberRef, (int)TableType.Module, /*(int)TableTypes.Permission,*/ (int)TableType.Property, (int)TableType.Event, (int)TableType.StandAloneSig, (int)TableType.ModuleRef, (int)TableType.TypeSpec, (int)TableType.Assembly, (int)TableType.AssemblyRef, (int)TableType.File, (int)TableType.ExportedType, (int)TableType.ManifestResource },
			new[] { (int)TableType.Field, (int)TableType.Param },
			new[] { (int)TableType.TypeDef, (int)TableType.MethodDef, (int)TableType.Assembly },
			new[] { (int)TableType.TypeDef, (int)TableType.TypeRef, (int)TableType.ModuleRef, (int)TableType.MethodDef, (int)TableType.TypeSpec },
			new[] { (int)TableType.Event, (int)TableType.Property },
			new[] { (int)TableType.MethodDef, (int)TableType.MemberRef },
			new[] { (int)TableType.Field, (int)TableType.MethodDef },
			new[] { (int)TableType.File, (int)TableType.AssemblyRef, (int)TableType.ExportedType },
			new[] { -1, -1, (int)TableType.MethodDef, (int)TableType.MemberRef, -1 },
			new[] { (int)TableType.Module, (int)TableType.ModuleRef, (int)TableType.AssemblyRef, (int)TableType.TypeRef },
			new[] { (int)TableType.TypeDef, (int)TableType.MethodDef }
		};

		private static readonly int[] IndexBits = new[] {
			2, 2, 5, 1, 2, 3, 1, 1, 1, 2, 3, 2, 1
		};

		private const int TableCount = ((int)TableType.GenericParamConstraint >> 24) + 1;

		#endregion Static members

		#region Data members

		/// <summary>
		/// Holds size flags for the provider heap.
		/// </summary>
		public byte _heapSize;

		/// <summary>
		/// Determines the tables, which are available in the heap.
		/// </summary>
		public long _valid;

		/// <summary>
		/// Determines the tables, which are sorted.
		/// </summary>
		public long _sorted;

		/// <summary>
		/// Holds the row counts.
		/// </summary>
		private int[] _rowCounts;

		/// <summary>
		/// Holds offsets into the heap, where the corresponding table starts.
		/// </summary>
		private int[] _tableOffsets;

		/// <summary>
		/// Size of a single record in the table.
		/// </summary>
		private int[] _recordSize;

		/// <summary>
		/// Size of the corresponding indexes.
		/// </summary>
		private int[] _indexSize;

		private IMetadataProvider _metadataProvider;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TableHeap"/> class.
		/// </summary>
		/// <param name="provider">The provider buffer to use for the heap.</param>
		/// <param name="metadata">The metadata.</param>
		/// <param name="offset">The offset into the provider buffer, where the table heap starts.</param>
		/// <param name="size">The size of the table heap in bytes.</param>
		public TableHeap(IMetadataProvider provider, byte[] metadata, int offset, int size)
			: base(metadata, offset, size)
		{
			// Table offset calculation
			int nextTableOffset = offset;

			// Retrieve the member data values
			using (var reader = new BinaryReader(new MemoryStream(metadata), Encoding.UTF8))
			{
				reader.BaseStream.Position = offset;
				reader.ReadUInt32();
				if (2 != reader.ReadByte() || 0 != reader.ReadByte())
					throw new BadImageFormatException();
				_heapSize = reader.ReadByte();
				reader.ReadByte();
				_valid = reader.ReadInt64();
				_sorted = reader.ReadInt64();

				// Adjust the table offset for the header so far
				nextTableOffset += 4 + 2 + 1 + 1 + 8 + 8;

				CreateRowCountArray(reader, ref nextTableOffset);
				CalculateIndexSizes();
				CalculateHeapIndexSizes();
				CalculateRecordSizes();
				CalculateTableOffsets(ref nextTableOffset);
			}

			_metadataProvider = provider;
		}

		private void CreateRowCountArray(BinaryReader reader, ref int nextTableOffset)
		{
			_rowCounts = new int[TableCount];
			for (long valid = _valid, i = 0; 0 != valid; valid >>= 1, i++)
			{
				if (1 != (valid & 1))
					continue;

				_rowCounts[i] = reader.ReadInt32();
				nextTableOffset += 4;
			}
		}

		private void CalculateIndexSizes()
		{
			_indexSize = new int[(int)IndexType.IndexCount];
			for (int i = 0; i < (int)IndexType.CodedIndexCount; i++)
			{
				int maxrows = 0;
				for (int table = 0; table < IndexTables[i].Length; table++)
				{
					if (-1 != IndexTables[i][table] && maxrows < _rowCounts[(IndexTables[i][table] >> 24)])
					{
						maxrows = _rowCounts[(IndexTables[i][table] >> 24)];
					}
				}

				_indexSize[i] = (maxrows < (1 << (16 - IndexBits[i])) ? 2 : 4);
			}
		}

		private void CalculateHeapIndexSizes()
		{
			_indexSize[(int)IndexType.StringHeap] = 2 + 2 * (_heapSize & 1);
			_indexSize[(int)IndexType.GuidHeap] = 2 + (_heapSize & 2);
			_indexSize[(int)IndexType.BlobHeap] = (0 == (_heapSize & 4) ? 2 : 4);
		}

		private void CalculateRecordSizes()
		{
			_recordSize = CalculateRecordSizes(TableCount);
		}

		private void CalculateTableOffsets(ref int nextTableOffset)
		{
			_tableOffsets = new int[TableCount];
			for (int i = 0; i < TableCount; i++)
			{
				if (0 == _rowCounts[i])
					continue;
				_tableOffsets[i] = nextTableOffset;
				nextTableOffset += _recordSize[i] * _rowCounts[i];
			}
		}

		private int[] CalculateRecordSizes(int tableCount)
		{
			int[] sizes = new int[tableCount];
			int sheapIdx = GetIndexSize(IndexType.StringHeap);
			int gheapIdx = GetIndexSize(IndexType.GuidHeap);
			int bheapIdx = GetIndexSize(IndexType.BlobHeap);

			sizes[(int)TableType.Module >> 24] = (2 + sheapIdx + 3 * gheapIdx);
			sizes[(int)TableType.TypeRef >> 24] = (GetIndexSize(IndexType.ResolutionScope) + 2 * sheapIdx);
			sizes[(int)TableType.TypeDef >> 24] = (4 + 2 * sheapIdx + GetIndexSize(IndexType.TypeDefOrRef) + GetIndexSize(TableType.Field) + GetIndexSize(TableType.MethodDef));
			sizes[(int)TableType.Field >> 24] = (2 + sheapIdx + bheapIdx);
			sizes[(int)TableType.MethodDef >> 24] = (4 + 2 + 2 + sheapIdx + bheapIdx + GetIndexSize(TableType.Param));
			sizes[(int)TableType.Param >> 24] = (2 + 2 + sheapIdx);
			sizes[(int)TableType.InterfaceImpl >> 24] = (GetIndexSize(TableType.TypeDef) + GetIndexSize(IndexType.TypeDefOrRef));
			sizes[(int)TableType.MemberRef >> 24] = (GetIndexSize(IndexType.MemberRefParent) + sheapIdx + bheapIdx);
			sizes[(int)TableType.Constant >> 24] = (2 + GetIndexSize(IndexType.HasConstant) + bheapIdx);
			sizes[(int)TableType.CustomAttribute >> 24] = (GetIndexSize(IndexType.HasCustomAttribute) + GetIndexSize(IndexType.CustomAttributeType) + bheapIdx);
			sizes[(int)TableType.FieldMarshal >> 24] = (GetIndexSize(IndexType.HasFieldMarshal) + bheapIdx);
			sizes[(int)TableType.DeclSecurity >> 24] = (2 + GetIndexSize(IndexType.HasDeclSecurity) + bheapIdx);
			sizes[(int)TableType.ClassLayout >> 24] = (2 + 4 + GetIndexSize(TableType.TypeDef));
			sizes[(int)TableType.FieldLayout >> 24] = (4 + GetIndexSize(TableType.Field));
			sizes[(int)TableType.StandAloneSig >> 24] = (bheapIdx);
			sizes[(int)TableType.EventMap >> 24] = (GetIndexSize(TableType.TypeDef) + GetIndexSize(TableType.Event));
			sizes[(int)TableType.Event >> 24] = (2 + sheapIdx + GetIndexSize(IndexType.TypeDefOrRef));
			sizes[(int)TableType.PropertyMap >> 24] = (GetIndexSize(TableType.TypeDef) + GetIndexSize(TableType.Property));
			sizes[(int)TableType.Property >> 24] = (2 + sheapIdx + bheapIdx);
			sizes[(int)TableType.MethodSemantics >> 24] = (2 + GetIndexSize(TableType.MethodDef) + GetIndexSize(IndexType.HasSemantics));
			sizes[(int)TableType.MethodImpl >> 24] = (GetIndexSize(TableType.TypeDef) + 2 * GetIndexSize(IndexType.MethodDefOrRef));
			sizes[(int)TableType.ModuleRef >> 24] = (sheapIdx);
			sizes[(int)TableType.TypeSpec >> 24] = (bheapIdx);
			sizes[(int)TableType.ImplMap >> 24] = (2 + GetIndexSize(IndexType.MemberForwarded) + sheapIdx + GetIndexSize(TableType.ModuleRef));
			sizes[(int)TableType.FieldRVA >> 24] = (4 + GetIndexSize(TableType.Field));
			sizes[(int)TableType.Assembly >> 24] = (4 + 2 + 2 + 2 + 2 + 4 + bheapIdx + 2 * sheapIdx);
			sizes[(int)TableType.AssemblyProcessor >> 24] = (4);
			sizes[(int)TableType.AssemblyOS >> 24] = (4 + 4 + 4);
			sizes[(int)TableType.AssemblyRef >> 24] = (2 + 2 + 2 + 2 + 4 + 2 * bheapIdx + 2 * sheapIdx);
			sizes[(int)TableType.AssemblyRefProcessor >> 24] = (4 + GetIndexSize(TableType.AssemblyRef));
			sizes[(int)TableType.AssemblyRefOS >> 24] = (4 + 4 + 4 + GetIndexSize(TableType.AssemblyRef));
			sizes[(int)TableType.File >> 24] = (4 + sheapIdx + bheapIdx);
			sizes[(int)TableType.ExportedType >> 24] = (4 + 4 + 2 * sheapIdx + GetIndexSize(IndexType.Implementation));
			sizes[(int)TableType.ManifestResource >> 24] = (4 + 4 + sheapIdx + GetIndexSize(IndexType.Implementation));
			sizes[(int)TableType.NestedClass >> 24] = (2 * GetIndexSize(TableType.TypeDef));
			sizes[(int)TableType.GenericParam >> 24] = (2 + 2 + GetIndexSize(IndexType.TypeOrMethodDef) + sheapIdx);
			sizes[(int)TableType.MethodSpec >> 24] = (GetIndexSize(IndexType.MethodDefOrRef) + bheapIdx);
			sizes[(int)TableType.GenericParamConstraint >> 24] = (GetIndexSize(TableType.GenericParam) + GetIndexSize(IndexType.TypeDefOrRef));

			return sizes;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Determines the number of tables in the provider.
		/// </summary>
		public int Count
		{
			get
			{
				int result = 0;
				long valid = _valid;
				while (0 != valid)
				{
					result += (int)(valid & 1);
					valid >>= 1;
				}
				return result;
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Determines the size of indexes into the named heap.
		/// </summary>
		/// <param name="index">The heap type to retrieve the index size for.</param>
		/// <returns>The size of an index in bytes into the requested index type in bytes.</returns>
		private int GetIndexSize(IndexType index)
		{
			if (0 > index || index >= IndexType.IndexCount)
				throw new ArgumentException(@"Invalid index type specified.", @"index");

			return _indexSize[(int)index];
		}

		/// <summary>
		/// Determines the size of an index into the named table.
		/// </summary>
		/// <param name="tokenTypes">The table to determine the index for.</param>
		/// <returns>The index size in bytes.</returns>
		private int GetIndexSize(TableType table)
		{
			int _table = (int)table >> 24;
			if (_table < 0 || _table > TableCount)
				throw new ArgumentException(@"Invalid token type.");

			if (_rowCounts[_table] > 65535)
				return 4;

			return 2;
		}

		/// <summary>
		/// Read and decode an index value from the reader.
		/// </summary>
		/// <param name="reader">The reader to read From.</param>
		/// <param name="index">The index type to read.</param>
		/// <returns>The index value.</returns>
		private HeapIndexToken ReadHeapToken(BinaryReader reader, IndexType index)
		{
			var value = (HeapIndexToken)(GetIndexSize(index) == 2 ? (0x0000FFFF & (int)reader.ReadInt16()) : reader.ReadInt32());

			switch (index)
			{
				case IndexType.StringHeap:
					value |= HeapIndexToken.String;
					break;

				case IndexType.GuidHeap:
					value |= HeapIndexToken.Guid;
					break;

				case IndexType.BlobHeap:
					value |= HeapIndexToken.Blob;
					break;

				default:
					throw new ArgumentException(@"Invalid IndexType.");
			}

			return value;
		}

		private Token ReadMetadataToken(BinaryReader reader, IndexType index)
		{
			int value = (GetIndexSize(index) == 2) ? (0x0000FFFF & (int)reader.ReadInt16()) : reader.ReadInt32();

			Debug.Assert(index < IndexType.CodedIndexCount);

			int bits = IndexBits[(int)index];
			int mask = 1;

			for (int i = 1; i < bits; i++) mask = (mask << 1) | 1;

			// Get the table
			int table = (int)value & mask;

			// Correct the value
			value = ((int)value >> bits);

			return new Token((TableType)IndexTables[(int)index][table], value);
		}

		/// <summary>
		/// Read and decode an index value from the reader.
		/// </summary>
		/// <param name="reader">The reader to read From.</param>
		/// <param name="table">The index type to read.</param>
		/// <returns>The index value.</returns>
		private Token ReadIndexValue(BinaryReader reader, TableType table)
		{
			return new Token(table, GetIndexSize(table) == 2 ? reader.ReadInt16() : reader.ReadInt32());
		}

		private BinaryReader CreateReaderForToken(Token token)
		{
			if (token.RID > GetRowCount(token.Table))
				throw new ArgumentException(@"Row is out of bounds.", @"token");
			if (token.RID == 0)
				throw new ArgumentException(@"Invalid row index.", @"token");

			int tableIdx = (int)(token.Table) >> 24;
			int tableOffset = _tableOffsets[tableIdx] + ((int)(token.RID - 1) * _recordSize[tableIdx]);

			var reader = new BinaryReader(new MemoryStream(_metadata), Encoding.UTF8);
			reader.BaseStream.Position = tableOffset;
			return reader;
		}

		#endregion Methods

		#region IMetadataProvider members

		/// <summary>
		/// Gets the rows.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns></returns>
		public int GetRowCount(TableType table)
		{
			return _rowCounts[((int)table >> 24)];
		}

		/// <summary>
		/// Retrieves the number of rows in a specified table.
		/// </summary>
		/// <param name="token">The metadata token.</param>
		/// <returns>The row count in the table.</returns>
		/// <exception cref="System.ArgumentException">Invalid token type specified for table.</exception>
		public Token GetMaxTokenValue(TableType table)
		{
			return new Token(table, _rowCounts[((int)table >> 24)]);
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ModuleRow ReadModuleRow(Token token)
		{
			if (token.Table != TableType.Module)
				throw new ArgumentException("Invalid token type for ModuleRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new ModuleRow(
					 reader.ReadUInt16(),
					 ReadHeapToken(reader, IndexType.StringHeap),
					 ReadHeapToken(reader, IndexType.GuidHeap),
					 ReadHeapToken(reader, IndexType.GuidHeap),
					 ReadHeapToken(reader, IndexType.GuidHeap)
				 );
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public TypeRefRow ReadTypeRefRow(Token token)
		{
			if (token.Table != TableType.TypeRef)
				throw new ArgumentException("Invalid token type for TypeRefRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new TypeRefRow(
					ReadMetadataToken(reader, IndexType.ResolutionScope),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadHeapToken(reader, IndexType.StringHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public TypeDefRow ReadTypeDefRow(Token token)
		{
			if (token.Table != TableType.TypeDef)
				throw new ArgumentException("Invalid token type for TypeDefRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new TypeDefRow(
					(TypeAttributes)reader.ReadUInt32(),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadMetadataToken(reader, IndexType.TypeDefOrRef),
					ReadIndexValue(reader, TableType.Field),
					ReadIndexValue(reader, TableType.MethodDef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public FieldRow ReadFieldRow(Token token)
		{
			if (token.Table != TableType.Field)
				throw new ArgumentException("Invalid token type for FieldRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new FieldRow(
					(FieldAttributes)reader.ReadUInt16(),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadHeapToken(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public MethodDefRow ReadMethodDefRow(Token token)
		{
			if (token.Table != TableType.MethodDef)
				throw new ArgumentException("Invalid token type for MethodDefRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new MethodDefRow(
					reader.ReadUInt32(),
					(MethodImplAttributes)reader.ReadUInt16(),
					(MethodAttributes)reader.ReadUInt16(),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadHeapToken(reader, IndexType.BlobHeap),
					ReadIndexValue(reader, TableType.Param)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ParamRow ReadParamRow(Token token)
		{
			if (token.Table != TableType.Param)
				throw new ArgumentException("Invalid token type for ParamRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new ParamRow(
					(ParameterAttributes)reader.ReadUInt16(),
					reader.ReadInt16(),
					ReadHeapToken(reader, IndexType.StringHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public InterfaceImplRow ReadInterfaceImplRow(Token token)
		{
			if (token.Table != TableType.InterfaceImpl)
				throw new ArgumentException("Invalid token type for InterfaceImplRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new InterfaceImplRow(
					ReadIndexValue(reader, TableType.TypeDef),
					ReadMetadataToken(reader, IndexType.TypeDefOrRef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public MemberRefRow ReadMemberRefRow(Token token)
		{
			if (token.Table != TableType.MemberRef)
				throw new ArgumentException("Invalid token type for MemberRefRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new MemberRefRow(
					ReadMetadataToken(reader, IndexType.MemberRefParent),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadHeapToken(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ConstantRow ReadConstantRow(Token token)
		{
			if (token.Table != TableType.Constant)
				throw new ArgumentException("Invalid token type for ConstantRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				var cet = (CilElementType)reader.ReadByte();
				reader.ReadByte();

				return new ConstantRow(
					cet,
					ReadMetadataToken(reader, IndexType.HasConstant),
					ReadHeapToken(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public CustomAttributeRow ReadCustomAttributeRow(Token token)
		{
			if (token.Table != TableType.CustomAttribute)
				throw new ArgumentException("Invalid token type for CustomAttributeRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new CustomAttributeRow(
					ReadMetadataToken(reader, IndexType.HasCustomAttribute),
					ReadMetadataToken(reader, IndexType.CustomAttributeType),
					ReadHeapToken(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public FieldMarshalRow ReadFieldMarshalRow(Token token)
		{
			if (token.Table != TableType.FieldMarshal)
				throw new ArgumentException("Invalid token type for FieldMarshalRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new FieldMarshalRow(
					ReadMetadataToken(reader, IndexType.HasFieldMarshal),
					ReadHeapToken(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public DeclSecurityRow ReadDeclSecurityRow(Token token)
		{
			if (token.Table != TableType.DeclSecurity)
				throw new ArgumentException("Invalid token type for DeclSecurityRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new DeclSecurityRow(
					(System.Security.Permissions.SecurityAction)reader.ReadUInt16(),
					ReadMetadataToken(reader, IndexType.HasDeclSecurity),
					ReadHeapToken(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ClassLayoutRow ReadClassLayoutRow(Token token)
		{
			if (token.Table != TableType.ClassLayout)
				throw new ArgumentException("Invalid token type for ClassLayoutRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new ClassLayoutRow(
					reader.ReadInt16(),
					reader.ReadInt32(),
					ReadIndexValue(reader, TableType.TypeDef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public FieldLayoutRow ReadFieldLayoutRow(Token token)
		{
			if (token.Table != TableType.FieldLayout)
				throw new ArgumentException("Invalid token type for FieldLayoutRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new FieldLayoutRow(
					reader.ReadUInt32(),
					ReadIndexValue(reader, TableType.Field)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public StandAloneSigRow ReadStandAloneSigRow(Token token)
		{
			if (token.Table != TableType.StandAloneSig)
				throw new ArgumentException("Invalid token type for StandAloneSigRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new StandAloneSigRow(
					ReadHeapToken(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public EventMapRow ReadEventMapRow(Token token)
		{
			if (token.Table != TableType.EventMap)
				throw new ArgumentException("Invalid token type for EventMapRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new EventMapRow(
					ReadIndexValue(reader, TableType.TypeDef),
					ReadIndexValue(reader, TableType.Event)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public EventRow ReadEventRow(Token token)
		{
			if (token.Table != TableType.Event)
				throw new ArgumentException("Invalid token type for EventRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new EventRow(
					(EventAttributes)reader.ReadUInt16(),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadMetadataToken(reader, IndexType.TypeDefOrRef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public PropertyMapRow ReadPropertyMapRow(Token token)
		{
			if (token.Table != TableType.PropertyMap)
				throw new ArgumentException("Invalid token type for PropertyMapRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new PropertyMapRow(
					ReadIndexValue(reader, TableType.TypeDef),
					ReadIndexValue(reader, TableType.Property)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public PropertyRow ReadPropertyRow(Token token)
		{
			if (token.Table != TableType.Property)
				throw new ArgumentException("Invalid token type for PropertyRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new PropertyRow(
					(PropertyAttributes)reader.ReadUInt16(),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadHeapToken(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public MethodSemanticsRow ReadMethodSemanticsRow(Token token)
		{
			using (var reader = CreateReaderForToken(token))
			{
				if (token.Table != TableType.MethodSemantics)
					throw new ArgumentException("Invalid token type for MethodSemanticsRow.", @"token");

				return new MethodSemanticsRow(
					(MethodSemanticsAttributes)reader.ReadInt16(),
					ReadIndexValue(reader, TableType.MethodDef),
					ReadMetadataToken(reader, IndexType.HasSemantics)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public MethodImplRow ReadMethodImplRow(Token token)
		{
			if (token.Table != TableType.MethodImpl)
				throw new ArgumentException("Invalid token type for MethodImplRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new MethodImplRow(
					ReadIndexValue(reader, TableType.TypeDef),
					ReadMetadataToken(reader, IndexType.MethodDefOrRef),
					ReadMetadataToken(reader, IndexType.MethodDefOrRef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ModuleRefRow ReadModuleRefRow(Token token)
		{
			if (token.Table != TableType.ModuleRef)
				throw new ArgumentException("Invalid token type for ModuleRefRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new ModuleRefRow(
					ReadHeapToken(reader, IndexType.StringHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public TypeSpecRow ReadTypeSpecRow(Token token)
		{
			if (token.Table != TableType.TypeSpec)
				throw new ArgumentException("Invalid token type for TypeSpecRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new TypeSpecRow(
					ReadHeapToken(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ImplMapRow ReadImplMapRow(Token token)
		{
			if (token.Table != TableType.ImplMap)
				throw new ArgumentException("Invalid token type for ImplMapRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new ImplMapRow(
					(PInvokeAttributes)reader.ReadUInt16(),
					ReadMetadataToken(reader, IndexType.MemberForwarded),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadIndexValue(reader, TableType.ModuleRef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public FieldRVARow ReadFieldRVARow(Token token)
		{
			if (token.Table != TableType.FieldRVA)
				throw new ArgumentException("Invalid token type for FieldRVARow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new FieldRVARow(
					reader.ReadUInt32(),
					ReadIndexValue(reader, TableType.Field)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public AssemblyRow ReadAssemblyRow(Token token)
		{
			if (token.Table != TableType.Assembly)
				throw new ArgumentException("Invalid token type for AssemblyRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new AssemblyRow(
					(AssemblyHashAlgorithm)reader.ReadUInt32(),
					reader.ReadUInt16(),
					reader.ReadUInt16(),
					reader.ReadUInt16(),
					reader.ReadUInt16(),
					(AssemblyAttributes)reader.ReadUInt32(),
					ReadHeapToken(reader, IndexType.BlobHeap),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadHeapToken(reader, IndexType.StringHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public AssemblyProcessorRow ReadAssemblyProcessorRow(Token token)
		{
			if (token.Table != TableType.AssemblyProcessor)
				throw new ArgumentException("Invalid token type for AssemblyProcessorRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new AssemblyProcessorRow(
					reader.ReadUInt32()
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public AssemblyOSRow ReadAssemblyOSRow(Token token)
		{
			if (token.Table != TableType.AssemblyOS)
				throw new ArgumentException("Invalid token type for AssemblyOSRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new AssemblyOSRow(
					reader.ReadInt32(),
					reader.ReadInt32(),
					reader.ReadInt32()
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public AssemblyRefRow ReadAssemblyRefRow(Token token)
		{
			if (token.Table != TableType.AssemblyRef)
				throw new ArgumentException("Invalid token type for AssemblyRefRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new AssemblyRefRow(
					reader.ReadUInt16(),
					reader.ReadUInt16(),
					reader.ReadUInt16(),
					reader.ReadUInt16(),
					(AssemblyAttributes)reader.ReadUInt32(),
					ReadHeapToken(reader, IndexType.BlobHeap),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadHeapToken(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public AssemblyRefProcessorRow ReadAssemblyRefProcessorRow(Token token)
		{
			if (token.Table != TableType.AssemblyRefProcessor)
				throw new ArgumentException("Invalid token type for AssemblyRefProcessorRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new AssemblyRefProcessorRow(
					reader.ReadUInt32(),
					ReadIndexValue(reader, TableType.AssemblyRef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public AssemblyRefOSRow ReadAssemblyRefOSRow(Token token)
		{
			if (token.Table != TableType.AssemblyRefOS)
				throw new ArgumentException("Invalid token type for AssemblyRefOSRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new AssemblyRefOSRow(
					reader.ReadUInt32(),
					reader.ReadUInt32(),
					reader.ReadUInt32(),
					ReadIndexValue(reader, TableType.AssemblyRef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public FileRow ReadFileRow(Token token)
		{
			if (token.Table != TableType.File)
				throw new ArgumentException("Invalid token type for FileRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new FileRow(
					(FileAttributes)reader.ReadUInt32(),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadHeapToken(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ExportedTypeRow ReadExportedTypeRow(Token token)
		{
			if (token.Table != TableType.ExportedType)
				throw new ArgumentException("Invalid token type for ExportedTypeRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new ExportedTypeRow(
					(TypeAttributes)reader.ReadUInt32(),
					(HeapIndexToken)reader.ReadUInt32(),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadMetadataToken(reader, IndexType.Implementation)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ManifestResourceRow ReadManifestResourceRow(Token token)
		{
			if (token.Table != TableType.ManifestResource)
				throw new ArgumentException("Invalid token type for ManifestResourceRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new ManifestResourceRow(
					reader.ReadUInt32(),
					(ManifestResourceAttributes)reader.ReadUInt32(),
					ReadHeapToken(reader, IndexType.StringHeap),
					ReadMetadataToken(reader, IndexType.Implementation)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public NestedClassRow ReadNestedClassRow(Token token)
		{
			if (token.Table != TableType.NestedClass)
				throw new ArgumentException("Invalid token type for NestedClassRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new NestedClassRow(
					ReadIndexValue(reader, TableType.TypeDef),
					ReadIndexValue(reader, TableType.TypeDef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public GenericParamRow ReadGenericParamRow(Token token)
		{
			if (token.Table != TableType.GenericParam)
				throw new ArgumentException("Invalid token type for GenericParamRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new GenericParamRow(
					reader.ReadUInt16(),
					(GenericParameterAttributes)reader.ReadUInt16(),
					ReadMetadataToken(reader, IndexType.TypeOrMethodDef),
					ReadHeapToken(reader, IndexType.StringHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public MethodSpecRow ReadMethodSpecRow(Token token)
		{
			if (token.Table != TableType.MethodSpec)
				throw new ArgumentException("Invalid token type for MethodSpecRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new MethodSpecRow(
					ReadMetadataToken(reader, IndexType.MethodDefOrRef),
					ReadHeapToken(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public GenericParamConstraintRow ReadGenericParamConstraintRow(Token token)
		{
			if (token.Table != TableType.GenericParamConstraint)
				throw new ArgumentException("Invalid token type for GenericParamConstraintRow.", @"token");

			using (var reader = CreateReaderForToken(token))
			{
				return new GenericParamConstraintRow(
					ReadIndexValue(reader, TableType.GenericParam),
					ReadMetadataToken(reader, IndexType.TypeDefOrRef)
				);
			}
		}

		#endregion IMetadataProvider members
	}
}