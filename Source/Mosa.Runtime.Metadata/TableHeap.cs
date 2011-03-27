/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Runtime.Metadata
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

		#endregion // Types

		#region Static members

		private static readonly int[][] IndexTables = new[]
		{
			new[] { (int)TokenTypes.TypeDef, (int)TokenTypes.TypeRef, (int)TokenTypes.TypeSpec },
			new[] { (int)TokenTypes.Field, (int)TokenTypes.Param, (int)TokenTypes.Property },
			new[] { (int)TokenTypes.MethodDef, (int)TokenTypes.Field, (int)TokenTypes.TypeRef, (int)TokenTypes.TypeDef, (int)TokenTypes.Param, (int)TokenTypes.InterfaceImpl, (int)TokenTypes.MemberRef, (int)TokenTypes.Module, /*(int)TokenTypes.Permission,*/ (int)TokenTypes.Property, (int)TokenTypes.Event, (int)TokenTypes.StandAloneSig, (int)TokenTypes.ModuleRef, (int)TokenTypes.TypeSpec, (int)TokenTypes.Assembly, (int)TokenTypes.AssemblyRef, (int)TokenTypes.File, (int)TokenTypes.ExportedType, (int)TokenTypes.ManifestResource },
			new[] { (int)TokenTypes.Field, (int)TokenTypes.Param },
			new[] { (int)TokenTypes.TypeDef, (int)TokenTypes.MethodDef, (int)TokenTypes.Assembly },
			new[] { (int)TokenTypes.TypeDef, (int)TokenTypes.TypeRef, (int)TokenTypes.ModuleRef, (int)TokenTypes.MethodDef, (int)TokenTypes.TypeSpec },
			new[] { (int)TokenTypes.Event, (int)TokenTypes.Property },
			new[] { (int)TokenTypes.MethodDef, (int)TokenTypes.MemberRef },
			new[] { (int)TokenTypes.Field, (int)TokenTypes.MethodDef },
			new[] { (int)TokenTypes.File, (int)TokenTypes.AssemblyRef, (int)TokenTypes.ExportedType },
			new[] { -1, -1, (int)TokenTypes.MethodDef, (int)TokenTypes.MemberRef, -1 },
			new[] { (int)TokenTypes.Module, (int)TokenTypes.ModuleRef, (int)TokenTypes.AssemblyRef, (int)TokenTypes.TypeRef },
			new[] { (int)TokenTypes.TypeDef, (int)TokenTypes.MethodDef }
		};

		private static readonly MetadataTable[][] IndexTables2 = new[]
		{
		    new[] { MetadataTable.TypeDef, MetadataTable.TypeRef, MetadataTable.TypeSpec },
		    new[] { MetadataTable.Field, MetadataTable.Param, MetadataTable.Property },
		    new[] { MetadataTable.MethodDef, MetadataTable.Field, MetadataTable.TypeRef, MetadataTable.TypeDef, MetadataTable.Param, MetadataTable.InterfaceImpl, MetadataTable.MemberRef, MetadataTable.Module, /*TableTypes.Permission,*/ MetadataTable.Property, MetadataTable.Event, MetadataTable.StandAloneSig, MetadataTable.ModuleRef, MetadataTable.TypeSpec, MetadataTable.Assembly, MetadataTable.AssemblyRef, MetadataTable.File, MetadataTable.ExportedType, MetadataTable.ManifestResource },
		    new[] { MetadataTable.Field, MetadataTable.Param },
		    new[] { MetadataTable.TypeDef, MetadataTable.MethodDef, MetadataTable.Assembly },
		    new[] { MetadataTable.TypeDef, MetadataTable.TypeRef, MetadataTable.ModuleRef, MetadataTable.MethodDef, MetadataTable.TypeSpec },
		    new[] { MetadataTable.Event, MetadataTable.Property },
		    new[] { MetadataTable.MethodDef, MetadataTable.MemberRef },
		    new[] { MetadataTable.Field, MetadataTable.MethodDef },
		    new[] { MetadataTable.File, MetadataTable.AssemblyRef, MetadataTable.ExportedType },
		    new[] { MetadataTable.Assembly, MetadataTable.Assembly, MetadataTable.MethodDef, MetadataTable.MemberRef, MetadataTable.Assembly },
		    new[] { MetadataTable.Module, MetadataTable.ModuleRef, MetadataTable.AssemblyRef, MetadataTable.TypeRef },
		    new[] { MetadataTable.TypeDef, MetadataTable.MethodDef }
		};

		private static readonly int[] IndexBits = new[] {
			2, 2, 5, 1, 2, 3, 1, 1, 1, 2, 3, 2, 1
		};

		private const int TableCount = ((int)MetadataTable.GenericParamConstraint >> 24) + 1;

		#endregion // Static members

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

		#endregion // Data members

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
			using (BinaryReader reader = new BinaryReader(new MemoryStream(metadata), Encoding.UTF8))
			{
				reader.BaseStream.Position = offset;
				reader.ReadUInt32();
				if (2 != reader.ReadByte() || 0 != reader.ReadByte())
					throw new BadImageFormatException();
				_heapSize = reader.ReadByte();
				reader.ReadByte();
				_valid = reader.ReadInt64();
				_sorted = reader.ReadInt64();

				// Adjust the table offset for the _header so far
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

			sizes[(int)MetadataTable.Module >> 24] = (2 + sheapIdx + 3 * gheapIdx);
			sizes[(int)MetadataTable.TypeRef >> 24] = (GetIndexSize(IndexType.ResolutionScope) + 2 * sheapIdx);
			sizes[(int)MetadataTable.TypeDef >> 24] = (4 + 2 * sheapIdx + GetIndexSize(IndexType.TypeDefOrRef) + GetIndexSize(MetadataTable.Field) + GetIndexSize(MetadataTable.MethodDef));
			sizes[(int)MetadataTable.Field >> 24] = (2 + sheapIdx + bheapIdx);
			sizes[(int)MetadataTable.MethodDef >> 24] = (4 + 2 + 2 + sheapIdx + bheapIdx + GetIndexSize(MetadataTable.Param));
			sizes[(int)MetadataTable.Param >> 24] = (2 + 2 + sheapIdx);
			sizes[(int)MetadataTable.InterfaceImpl >> 24] = (GetIndexSize(MetadataTable.TypeDef) + GetIndexSize(IndexType.TypeDefOrRef));
			sizes[(int)MetadataTable.MemberRef >> 24] = (GetIndexSize(IndexType.MemberRefParent) + sheapIdx + bheapIdx);
			sizes[(int)MetadataTable.Constant >> 24] = (2 + GetIndexSize(IndexType.HasConstant) + bheapIdx);
			sizes[(int)MetadataTable.CustomAttribute >> 24] = (GetIndexSize(IndexType.HasCustomAttribute) + GetIndexSize(IndexType.CustomAttributeType) + bheapIdx);
			sizes[(int)MetadataTable.FieldMarshal >> 24] = (GetIndexSize(IndexType.HasFieldMarshal) + bheapIdx);
			sizes[(int)MetadataTable.DeclSecurity >> 24] = (2 + GetIndexSize(IndexType.HasDeclSecurity) + bheapIdx);
			sizes[(int)MetadataTable.ClassLayout >> 24] = (2 + 4 + GetIndexSize(MetadataTable.TypeDef));
			sizes[(int)MetadataTable.FieldLayout >> 24] = (4 + GetIndexSize(MetadataTable.Field));
			sizes[(int)MetadataTable.StandAloneSig >> 24] = (bheapIdx);
			sizes[(int)MetadataTable.EventMap >> 24] = (GetIndexSize(MetadataTable.TypeDef) + GetIndexSize(MetadataTable.Event));
			sizes[(int)MetadataTable.Event >> 24] = (2 + sheapIdx + GetIndexSize(IndexType.TypeDefOrRef));
			sizes[(int)MetadataTable.PropertyMap >> 24] = (GetIndexSize(MetadataTable.TypeDef) + GetIndexSize(MetadataTable.Property));
			sizes[(int)MetadataTable.Property >> 24] = (2 + sheapIdx + bheapIdx);
			sizes[(int)MetadataTable.MethodSemantics >> 24] = (2 + GetIndexSize(MetadataTable.MethodDef) + GetIndexSize(IndexType.HasSemantics));
			sizes[(int)MetadataTable.MethodImpl >> 24] = (GetIndexSize(MetadataTable.TypeDef) + 2 * GetIndexSize(IndexType.MethodDefOrRef));
			sizes[(int)MetadataTable.ModuleRef >> 24] = (sheapIdx);
			sizes[(int)MetadataTable.TypeSpec >> 24] = (bheapIdx);
			sizes[(int)MetadataTable.ImplMap >> 24] = (2 + GetIndexSize(IndexType.MemberForwarded) + sheapIdx + GetIndexSize(MetadataTable.ModuleRef));
			sizes[(int)MetadataTable.FieldRVA >> 24] = (4 + GetIndexSize(MetadataTable.Field));
			sizes[(int)MetadataTable.Assembly >> 24] = (4 + 2 + 2 + 2 + 2 + 4 + bheapIdx + 2 * sheapIdx);
			sizes[(int)MetadataTable.AssemblyProcessor >> 24] = (4);
			sizes[(int)MetadataTable.AssemblyOS >> 24] = (4 + 4 + 4);
			sizes[(int)MetadataTable.AssemblyRef >> 24] = (2 + 2 + 2 + 2 + 4 + 2 * bheapIdx + 2 * sheapIdx);
			sizes[(int)MetadataTable.AssemblyRefProcessor >> 24] = (4 + GetIndexSize(MetadataTable.AssemblyRef));
			sizes[(int)MetadataTable.AssemblyRefOS >> 24] = (4 + 4 + 4 + GetIndexSize(MetadataTable.AssemblyRef));
			sizes[(int)MetadataTable.File >> 24] = (4 + sheapIdx + bheapIdx);
			sizes[(int)MetadataTable.ExportedType >> 24] = (4 + 4 + 2 * sheapIdx + GetIndexSize(IndexType.Implementation));
			sizes[(int)MetadataTable.ManifestResource >> 24] = (4 + 4 + sheapIdx + GetIndexSize(IndexType.Implementation));
			sizes[(int)MetadataTable.NestedClass >> 24] = (2 * GetIndexSize(MetadataTable.TypeDef));
			sizes[(int)MetadataTable.GenericParam >> 24] = (2 + 2 + GetIndexSize(IndexType.TypeOrMethodDef) + sheapIdx);
			sizes[(int)MetadataTable.MethodSpec >> 24] = (GetIndexSize(IndexType.MethodDefOrRef) + bheapIdx);
			sizes[(int)MetadataTable.GenericParamConstraint >> 24] = (GetIndexSize(MetadataTable.GenericParam) + GetIndexSize(IndexType.TypeDefOrRef));

			return sizes;
		}

		#endregion // Construction

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

		#endregion // Properties

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
		private int GetIndexSize(MetadataTable table)
		{
			int _table = (int)table >> 24;
			if (_table < 0 || _table > TableCount)
				throw new ArgumentException(@"Invalid token type.", @"tokenTypes");

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
		private TokenTypes ReadIndexValue(BinaryReader reader, IndexType index)
		{
			int width = GetIndexSize(index);
			TokenTypes value = (TokenTypes)(2 == width ? (0x0000FFFF & (int)reader.ReadInt16()) : reader.ReadInt32());

			// Do we need to decode a coded index?
			if (index < IndexType.CodedIndexCount)
			{
				int bits = IndexBits[(int)index];
				int mask = 1;
				for (int i = 1; i < bits; i++) mask = (mask << 1) | 1;

				// Get the table
				int table = (int)value & mask;

				// Correct the value
				value = (TokenTypes)(((int)value >> bits) | IndexTables[(int)index][table]);
			}
			else
			{
				switch (index)
				{
					case IndexType.StringHeap:
						value |= TokenTypes.String;
						break;

					case IndexType.GuidHeap:
						value |= TokenTypes.Guid;
						break;

					case IndexType.BlobHeap:
						value |= TokenTypes.Blob;
						break;
				}
			}

			return value;
		}

		private MetadataToken ReadIndexValue2(BinaryReader reader, IndexType index)
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

			return new MetadataToken(IndexTables2[(int)index][table], value);
		}

		/// <summary>
		/// Read and decode an index value from the reader.
		/// </summary>
		/// <param name="reader">The reader to read From.</param>
		/// <param name="table">The index type to read.</param>
		/// <returns>The index value.</returns>
		private MetadataToken ReadIndexValue(BinaryReader reader, MetadataTable table)
		{
			return new MetadataToken(table, GetIndexSize(table) == 2 ? reader.ReadInt16() : reader.ReadInt32());
		}

		private BinaryReader CreateReaderForToken(MetadataToken token)
		{
			if (token.RID > GetMaxTokenValue(token.Table).RID)
				throw new ArgumentException(@"Row is out of bounds.", @"token");
			if (token.RID == 0)
				throw new ArgumentException(@"Invalid row index.", @"token");
			int tableIdx = (int)(token.Table) >> 24;
			int tableOffset = _tableOffsets[tableIdx] + ((int)(token.RID - 1) * _recordSize[tableIdx]);

			BinaryReader reader = new BinaryReader(new MemoryStream(_metadata), Encoding.UTF8);
			reader.BaseStream.Position = tableOffset;
			return reader;
		}

		#endregion // Methods

		#region IMetadataProvider members

		/// <summary>
		/// Gets the rows.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns></returns>
		public int GetRowCount(TokenTypes table)
		{
			return _rowCounts[((int)table >> 24)];
		}

		/// <summary>
		/// Gets the rows.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns></returns>
		public int GetRowCount(MetadataTable table)
		{
			return _rowCounts[((int)table >> 24)];
		}

		/// <summary>
		/// Retrieves the number of rows in a specified table.
		/// </summary>
		/// <param name="table">The table to retrieve the row for.</param>
		/// <returns>The row count in the table.</returns>
		/// <exception cref="System.ArgumentException">Invalid token type specified for table.</exception>
		public TokenTypes GetMaxTokenValue(TokenTypes table)
		{
			if (table != (TokenTypes.TableMask & table) || (int)table > 0x2C000000)
				throw new ArgumentException(@"Invalid table token type.", @"table");

			return table | (TokenTypes)_rowCounts[((int)table >> 24)];
		}

		/// <summary>
		/// Retrieves the number of rows in a specified table.
		/// </summary>
		/// <param name="token">The metadata token.</param>
		/// <returns>The row count in the table.</returns>
		/// <exception cref="System.ArgumentException">Invalid token type specified for table.</exception>
		public MetadataToken GetMaxTokenValue(MetadataTable table)
		{
			return new MetadataToken(table, _rowCounts[((int)table >> 24)]);
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ModuleRow ReadModuleRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.Module)
				throw new ArgumentException("Invalid token type for ModuleRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new ModuleRow(
					 reader.ReadUInt16(),
					 ReadIndexValue(reader, IndexType.StringHeap),
					 ReadIndexValue(reader, IndexType.GuidHeap),
					 ReadIndexValue(reader, IndexType.GuidHeap),
					 ReadIndexValue(reader, IndexType.GuidHeap)
				 );
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public TypeRefRow ReadTypeRefRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.TypeRef)
				throw new ArgumentException("Invalid token type for TypeRefRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new TypeRefRow(
					ReadIndexValue2(reader, IndexType.ResolutionScope),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, IndexType.StringHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public TypeDefRow ReadTypeDefRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.TypeDef)
				throw new ArgumentException("Invalid token type for TypeDefRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new TypeDefRow(
					(TypeAttributes)reader.ReadUInt32(),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue2(reader, IndexType.TypeDefOrRef),
					ReadIndexValue(reader, MetadataTable.Field),
					ReadIndexValue(reader, MetadataTable.MethodDef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public FieldRow ReadFieldRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.Field)
				throw new ArgumentException("Invalid token type for FieldRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new FieldRow(
					(FieldAttributes)reader.ReadUInt16(),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public MethodDefRow ReadMethodDefRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.MethodDef)
				throw new ArgumentException("Invalid token type for MethodDefRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new MethodDefRow(
					reader.ReadUInt32(),
					(MethodImplAttributes)reader.ReadUInt16(),
					(MethodAttributes)reader.ReadUInt16(),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, IndexType.BlobHeap),
					ReadIndexValue(reader, MetadataTable.Param)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ParamRow ReadParamRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.Param)
				throw new ArgumentException("Invalid token type for ParamRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new ParamRow(
					(ParameterAttributes)reader.ReadUInt16(),
					reader.ReadInt16(),
					ReadIndexValue(reader, IndexType.StringHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public InterfaceImplRow ReadInterfaceImplRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.InterfaceImpl)
				throw new ArgumentException("Invalid token type for InterfaceImplRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new InterfaceImplRow(
					ReadIndexValue(reader, MetadataTable.TypeDef),
					ReadIndexValue2(reader, IndexType.TypeDefOrRef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public MemberRefRow ReadMemberRefRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.MemberRef)
				throw new ArgumentException("Invalid token type for MemberRefRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new MemberRefRow(
					ReadIndexValue2(reader, IndexType.MemberRefParent),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ConstantRow ReadConstantRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.Constant)
				throw new ArgumentException("Invalid token type for ConstantRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				CilElementType cet = (CilElementType)reader.ReadByte();
				reader.ReadByte();

				return new ConstantRow(
					cet,
					ReadIndexValue2(reader, IndexType.HasConstant),
					ReadIndexValue(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public CustomAttributeRow ReadCustomAttributeRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.CustomAttribute)
				throw new ArgumentException("Invalid token type for CustomAttributeRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new CustomAttributeRow(
					ReadIndexValue2(reader, IndexType.HasCustomAttribute),
					ReadIndexValue2(reader, IndexType.CustomAttributeType),
					ReadIndexValue(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public FieldMarshalRow ReadFieldMarshalRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.FieldMarshal)
				throw new ArgumentException("Invalid token type for FieldMarshalRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new FieldMarshalRow(
					ReadIndexValue2(reader, IndexType.HasFieldMarshal),
					ReadIndexValue(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public DeclSecurityRow ReadDeclSecurityRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.DeclSecurity)
				throw new ArgumentException("Invalid token type for DeclSecurityRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new DeclSecurityRow(
					(System.Security.Permissions.SecurityAction)reader.ReadUInt16(),
					ReadIndexValue2(reader, IndexType.HasDeclSecurity),
					ReadIndexValue(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ClassLayoutRow ReadClassLayoutRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.ClassLayout)
				throw new ArgumentException("Invalid token type for ClassLayoutRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new ClassLayoutRow(
					reader.ReadInt16(),
					reader.ReadInt32(),
					ReadIndexValue(reader, MetadataTable.TypeDef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public FieldLayoutRow ReadFieldLayoutRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.FieldLayout)
				throw new ArgumentException("Invalid token type for FieldLayoutRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new FieldLayoutRow(
					reader.ReadUInt32(),
					ReadIndexValue(reader, MetadataTable.Field)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public StandAloneSigRow ReadStandAloneSigRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.StandAloneSig)
				throw new ArgumentException("Invalid token type for StandAloneSigRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new StandAloneSigRow(
					ReadIndexValue(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public EventMapRow ReadEventMapRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.EventMap)
				throw new ArgumentException("Invalid token type for EventMapRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new EventMapRow(
					ReadIndexValue(reader, MetadataTable.TypeDef),
					ReadIndexValue(reader, MetadataTable.Event)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public EventRow ReadEventRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.Event)
				throw new ArgumentException("Invalid token type for EventRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new EventRow(
					(EventAttributes)reader.ReadUInt16(),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue2(reader, IndexType.TypeDefOrRef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public PropertyMapRow ReadPropertyMapRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.PropertyMap)
				throw new ArgumentException("Invalid token type for PropertyMapRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new PropertyMapRow(
					ReadIndexValue(reader, MetadataTable.TypeDef),
					ReadIndexValue(reader, MetadataTable.Property)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public PropertyRow ReadPropertyRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.Property)
				throw new ArgumentException("Invalid token type for PropertyRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new PropertyRow(
					(PropertyAttributes)reader.ReadUInt16(),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public MethodSemanticsRow ReadMethodSemanticsRow(MetadataToken token)
		{
			using (BinaryReader reader = CreateReaderForToken(token))
			{
				if (token.Table != MetadataTable.MethodSemantics)
					throw new ArgumentException("Invalid token type for MethodSemanticsRow.", @"token");

				return new MethodSemanticsRow(
					(MethodSemanticsAttributes)reader.ReadInt16(),
					ReadIndexValue(reader, MetadataTable.MethodDef),
					ReadIndexValue2(reader, IndexType.HasSemantics)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public MethodImplRow ReadMethodImplRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.MethodImpl)
				throw new ArgumentException("Invalid token type for MethodImplRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new MethodImplRow(
					ReadIndexValue(reader, MetadataTable.TypeDef),
					ReadIndexValue2(reader, IndexType.MethodDefOrRef),
					ReadIndexValue2(reader, IndexType.MethodDefOrRef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ModuleRefRow ReadModuleRefRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.ModuleRef)
				throw new ArgumentException("Invalid token type for ModuleRefRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new ModuleRefRow(
					ReadIndexValue(reader, IndexType.StringHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public TypeSpecRow ReadTypeSpecRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.TypeSpec)
				throw new ArgumentException("Invalid token type for TypeSpecRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new TypeSpecRow(
					ReadIndexValue(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ImplMapRow ReadImplMapRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.ImplMap)
				throw new ArgumentException("Invalid token type for ImplMapRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new ImplMapRow(
					(PInvokeAttributes)reader.ReadUInt16(),
					ReadIndexValue2(reader, IndexType.MemberForwarded),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, MetadataTable.ModuleRef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public FieldRVARow ReadFieldRVARow(MetadataToken token)
		{
			if (token.Table != MetadataTable.FieldRVA)
				throw new ArgumentException("Invalid token type for FieldRVARow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new FieldRVARow(
					reader.ReadUInt32(),
					ReadIndexValue(reader, MetadataTable.Field)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public AssemblyRow ReadAssemblyRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.Assembly)
				throw new ArgumentException("Invalid token type for AssemblyRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new AssemblyRow(
					(AssemblyHashAlgorithm)reader.ReadUInt32(),
					reader.ReadUInt16(),
					reader.ReadUInt16(),
					reader.ReadUInt16(),
					reader.ReadUInt16(),
					(AssemblyAttributes)reader.ReadUInt32(),
					ReadIndexValue(reader, IndexType.BlobHeap),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, IndexType.StringHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public AssemblyProcessorRow ReadAssemblyProcessorRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.AssemblyProcessor)
				throw new ArgumentException("Invalid token type for AssemblyProcessorRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
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
		public AssemblyOSRow ReadAssemblyOSRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.AssemblyOS)
				throw new ArgumentException("Invalid token type for AssemblyOSRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
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
		public AssemblyRefRow ReadAssemblyRefRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.AssemblyRef)
				throw new ArgumentException("Invalid token type for AssemblyRefRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new AssemblyRefRow(
					reader.ReadUInt16(),
					reader.ReadUInt16(),
					reader.ReadUInt16(),
					reader.ReadUInt16(),
					(AssemblyAttributes)reader.ReadUInt32(),
					ReadIndexValue(reader, IndexType.BlobHeap),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public AssemblyRefProcessorRow ReadAssemblyRefProcessorRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.AssemblyRefProcessor)
				throw new ArgumentException("Invalid token type for AssemblyRefProcessorRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new AssemblyRefProcessorRow(
					reader.ReadUInt32(),
					ReadIndexValue(reader, MetadataTable.AssemblyRef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public AssemblyRefOSRow ReadAssemblyRefOSRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.AssemblyRefOS)
				throw new ArgumentException("Invalid token type for AssemblyRefOSRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new AssemblyRefOSRow(
					reader.ReadUInt32(),
					reader.ReadUInt32(),
					reader.ReadUInt32(),
					ReadIndexValue(reader, MetadataTable.AssemblyRef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public FileRow ReadFileRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.File)
				throw new ArgumentException("Invalid token type for FileRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new FileRow(
					(FileAttributes)reader.ReadUInt32(),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ExportedTypeRow ReadExportedTypeRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.ExportedType)
				throw new ArgumentException("Invalid token type for ExportedTypeRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new ExportedTypeRow(
					(TypeAttributes)reader.ReadUInt32(),
					(TokenTypes)reader.ReadUInt32(),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue2(reader, IndexType.Implementation)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public ManifestResourceRow ReadManifestResourceRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.ManifestResource)
				throw new ArgumentException("Invalid token type for ManifestResourceRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new ManifestResourceRow(
					reader.ReadUInt32(),
					(ManifestResourceAttributes)reader.ReadUInt32(),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue2(reader, IndexType.Implementation)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public NestedClassRow ReadNestedClassRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.NestedClass)
				throw new ArgumentException("Invalid token type for NestedClassRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new NestedClassRow(
					ReadIndexValue(reader, MetadataTable.TypeDef),
					ReadIndexValue(reader, MetadataTable.TypeDef)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public GenericParamRow ReadGenericParamRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.GenericParam)
				throw new ArgumentException("Invalid token type for GenericParamRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new GenericParamRow(
					reader.ReadUInt16(),
					(GenericParameterAttributes)reader.ReadUInt16(),
					ReadIndexValue2(reader, IndexType.TypeOrMethodDef),
					ReadIndexValue(reader, IndexType.StringHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public MethodSpecRow ReadMethodSpecRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.MethodSpec)
				throw new ArgumentException("Invalid token type for MethodSpecRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new MethodSpecRow(
					ReadIndexValue2(reader, IndexType.MethodDefOrRef),
					ReadIndexValue(reader, IndexType.BlobHeap)
				);
			}
		}

		/// <summary>
		/// Reads the specified token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public GenericParamConstraintRow ReadGenericParamConstraintRow(MetadataToken token)
		{
			if (token.Table != MetadataTable.GenericParamConstraint)
				throw new ArgumentException("Invalid token type for GenericParamConstraintRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new GenericParamConstraintRow(
					ReadIndexValue(reader, MetadataTable.GenericParam),
					ReadIndexValue2(reader, IndexType.TypeDefOrRef)
				);
			}
		}

		#endregion // IMetadataProvider members
	}
}
