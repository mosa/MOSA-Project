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

		private static readonly TableTypes[][] IndexTables2 = new[]
		{
		    new[] { TableTypes.TypeDef, TableTypes.TypeRef, TableTypes.TypeSpec },
		    new[] { TableTypes.Field, TableTypes.Param, TableTypes.Property },
		    new[] { TableTypes.MethodDef, TableTypes.Field, TableTypes.TypeRef, TableTypes.TypeDef, TableTypes.Param, TableTypes.InterfaceImpl, TableTypes.MemberRef, TableTypes.Module, /*TableTypes.Permission,*/ TableTypes.Property, TableTypes.Event, TableTypes.StandAloneSig, TableTypes.ModuleRef, TableTypes.TypeSpec, TableTypes.Assembly, TableTypes.AssemblyRef, TableTypes.File, TableTypes.ExportedType, TableTypes.ManifestResource },
		    new[] { TableTypes.Field, TableTypes.Param },
		    new[] { TableTypes.TypeDef, TableTypes.MethodDef, TableTypes.Assembly },
		    new[] { TableTypes.TypeDef, TableTypes.TypeRef, TableTypes.ModuleRef, TableTypes.MethodDef, TableTypes.TypeSpec },
		    new[] { TableTypes.Event, TableTypes.Property },
		    new[] { TableTypes.MethodDef, TableTypes.MemberRef },
		    new[] { TableTypes.Field, TableTypes.MethodDef },
		    new[] { TableTypes.File, TableTypes.AssemblyRef, TableTypes.ExportedType },
		    new[] { TableTypes.Assembly, TableTypes.Assembly, TableTypes.MethodDef, TableTypes.MemberRef, TableTypes.Assembly },
		    new[] { TableTypes.Module, TableTypes.ModuleRef, TableTypes.AssemblyRef, TableTypes.TypeRef },
		    new[] { TableTypes.TypeDef, TableTypes.MethodDef }
		};

		private static readonly int[] IndexBits = new[] {
			2, 2, 5, 1, 2, 3, 1, 1, 1, 2, 3, 2, 1
		};

		private const int TableCount = ((int)TableTypes.GenericParamConstraint >> 24) + 1;

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

			sizes[(int)TableTypes.Module >> 24] = (2 + sheapIdx + 3 * gheapIdx);
			sizes[(int)TableTypes.TypeRef >> 24] = (GetIndexSize(IndexType.ResolutionScope) + 2 * sheapIdx);
			sizes[(int)TableTypes.TypeDef >> 24] = (4 + 2 * sheapIdx + GetIndexSize(IndexType.TypeDefOrRef) + GetIndexSize(TableTypes.Field) + GetIndexSize(TableTypes.MethodDef));
			sizes[(int)TableTypes.Field >> 24] = (2 + sheapIdx + bheapIdx);
			sizes[(int)TableTypes.MethodDef >> 24] = (4 + 2 + 2 + sheapIdx + bheapIdx + GetIndexSize(TableTypes.Param));
			sizes[(int)TableTypes.Param >> 24] = (2 + 2 + sheapIdx);
			sizes[(int)TableTypes.InterfaceImpl >> 24] = (GetIndexSize(TableTypes.TypeDef) + GetIndexSize(IndexType.TypeDefOrRef));
			sizes[(int)TableTypes.MemberRef >> 24] = (GetIndexSize(IndexType.MemberRefParent) + sheapIdx + bheapIdx);
			sizes[(int)TableTypes.Constant >> 24] = (2 + GetIndexSize(IndexType.HasConstant) + bheapIdx);
			sizes[(int)TableTypes.CustomAttribute >> 24] = (GetIndexSize(IndexType.HasCustomAttribute) + GetIndexSize(IndexType.CustomAttributeType) + bheapIdx);
			sizes[(int)TableTypes.FieldMarshal >> 24] = (GetIndexSize(IndexType.HasFieldMarshal) + bheapIdx);
			sizes[(int)TableTypes.DeclSecurity >> 24] = (2 + GetIndexSize(IndexType.HasDeclSecurity) + bheapIdx);
			sizes[(int)TableTypes.ClassLayout >> 24] = (2 + 4 + GetIndexSize(TableTypes.TypeDef));
			sizes[(int)TableTypes.FieldLayout >> 24] = (4 + GetIndexSize(TableTypes.Field));
			sizes[(int)TableTypes.StandAloneSig >> 24] = (bheapIdx);
			sizes[(int)TableTypes.EventMap >> 24] = (GetIndexSize(TableTypes.TypeDef) + GetIndexSize(TableTypes.Event));
			sizes[(int)TableTypes.Event >> 24] = (2 + sheapIdx + GetIndexSize(IndexType.TypeDefOrRef));
			sizes[(int)TableTypes.PropertyMap >> 24] = (GetIndexSize(TableTypes.TypeDef) + GetIndexSize(TableTypes.Property));
			sizes[(int)TableTypes.Property >> 24] = (2 + sheapIdx + bheapIdx);
			sizes[(int)TableTypes.MethodSemantics >> 24] = (2 + GetIndexSize(TableTypes.MethodDef) + GetIndexSize(IndexType.HasSemantics));
			sizes[(int)TableTypes.MethodImpl >> 24] = (GetIndexSize(TableTypes.TypeDef) + 2 * GetIndexSize(IndexType.MethodDefOrRef));
			sizes[(int)TableTypes.ModuleRef >> 24] = (sheapIdx);
			sizes[(int)TableTypes.TypeSpec >> 24] = (bheapIdx);
			sizes[(int)TableTypes.ImplMap >> 24] = (2 + GetIndexSize(IndexType.MemberForwarded) + sheapIdx + GetIndexSize(TableTypes.ModuleRef));
			sizes[(int)TableTypes.FieldRVA >> 24] = (4 + GetIndexSize(TableTypes.Field));
			sizes[(int)TableTypes.Assembly >> 24] = (4 + 2 + 2 + 2 + 2 + 4 + bheapIdx + 2 * sheapIdx);
			sizes[(int)TableTypes.AssemblyProcessor >> 24] = (4);
			sizes[(int)TableTypes.AssemblyOS >> 24] = (4 + 4 + 4);
			sizes[(int)TableTypes.AssemblyRef >> 24] = (2 + 2 + 2 + 2 + 4 + 2 * bheapIdx + 2 * sheapIdx);
			sizes[(int)TableTypes.AssemblyRefProcessor >> 24] = (4 + GetIndexSize(TableTypes.AssemblyRef));
			sizes[(int)TableTypes.AssemblyRefOS >> 24] = (4 + 4 + 4 + GetIndexSize(TableTypes.AssemblyRef));
			sizes[(int)TableTypes.File >> 24] = (4 + sheapIdx + bheapIdx);
			sizes[(int)TableTypes.ExportedType >> 24] = (4 + 4 + 2 * sheapIdx + GetIndexSize(IndexType.Implementation));
			sizes[(int)TableTypes.ManifestResource >> 24] = (4 + 4 + sheapIdx + GetIndexSize(IndexType.Implementation));
			sizes[(int)TableTypes.NestedClass >> 24] = (2 * GetIndexSize(TableTypes.TypeDef));
			sizes[(int)TableTypes.GenericParam >> 24] = (2 + 2 + GetIndexSize(IndexType.TypeOrMethodDef) + sheapIdx);
			sizes[(int)TableTypes.MethodSpec >> 24] = (GetIndexSize(IndexType.MethodDefOrRef) + bheapIdx);
			sizes[(int)TableTypes.GenericParamConstraint >> 24] = (GetIndexSize(TableTypes.GenericParam) + GetIndexSize(IndexType.TypeDefOrRef));

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
		private int GetIndexSize(TableTypes table)
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
		private TokenTypes ReadIndexValue(BinaryReader reader, IndexType index)
		{
			TokenTypes value = (TokenTypes)(GetIndexSize(index) == 2 ? (0x0000FFFF & (int)reader.ReadInt16()) : reader.ReadInt32());

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

				default:
					throw new ArgumentException(@"Invalid IndexType.");
			}

			return value;
		}

		private Token ReadIndexValue2(BinaryReader reader, IndexType index)
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

			return new Token(IndexTables2[(int)index][table], value);
		}

		/// <summary>
		/// Read and decode an index value from the reader.
		/// </summary>
		/// <param name="reader">The reader to read From.</param>
		/// <param name="table">The index type to read.</param>
		/// <returns>The index value.</returns>
		private Token ReadIndexValue(BinaryReader reader, TableTypes table)
		{
			return new Token(table, GetIndexSize(table) == 2 ? reader.ReadInt16() : reader.ReadInt32());
		}

		private BinaryReader CreateReaderForToken(Token token)
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
		public int GetRowCount(TableTypes table)
		{
			return _rowCounts[((int)table >> 24)];
		}

		/// <summary>
		/// Retrieves the number of rows in a specified table.
		/// </summary>
		/// <param name="token">The metadata token.</param>
		/// <returns>The row count in the table.</returns>
		/// <exception cref="System.ArgumentException">Invalid token type specified for table.</exception>
		public Token GetMaxTokenValue(TableTypes table)
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
			if (token.Table != TableTypes.Module)
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
		public TypeRefRow ReadTypeRefRow(Token token)
		{
			if (token.Table != TableTypes.TypeRef)
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
		public TypeDefRow ReadTypeDefRow(Token token)
		{
			if (token.Table != TableTypes.TypeDef)
				throw new ArgumentException("Invalid token type for TypeDefRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new TypeDefRow(
					(TypeAttributes)reader.ReadUInt32(),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue2(reader, IndexType.TypeDefOrRef),
					ReadIndexValue(reader, TableTypes.Field),
					ReadIndexValue(reader, TableTypes.MethodDef)
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
			if (token.Table != TableTypes.Field)
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
		public MethodDefRow ReadMethodDefRow(Token token)
		{
			if (token.Table != TableTypes.MethodDef)
				throw new ArgumentException("Invalid token type for MethodDefRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new MethodDefRow(
					reader.ReadUInt32(),
					(MethodImplAttributes)reader.ReadUInt16(),
					(MethodAttributes)reader.ReadUInt16(),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, IndexType.BlobHeap),
					ReadIndexValue(reader, TableTypes.Param)
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
			if (token.Table != TableTypes.Param)
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
		public InterfaceImplRow ReadInterfaceImplRow(Token token)
		{
			if (token.Table != TableTypes.InterfaceImpl)
				throw new ArgumentException("Invalid token type for InterfaceImplRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new InterfaceImplRow(
					ReadIndexValue(reader, TableTypes.TypeDef),
					ReadIndexValue2(reader, IndexType.TypeDefOrRef)
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
			if (token.Table != TableTypes.MemberRef)
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
		public ConstantRow ReadConstantRow(Token token)
		{
			if (token.Table != TableTypes.Constant)
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
		public CustomAttributeRow ReadCustomAttributeRow(Token token)
		{
			if (token.Table != TableTypes.CustomAttribute)
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
		public FieldMarshalRow ReadFieldMarshalRow(Token token)
		{
			if (token.Table != TableTypes.FieldMarshal)
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
		public DeclSecurityRow ReadDeclSecurityRow(Token token)
		{
			if (token.Table != TableTypes.DeclSecurity)
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
		public ClassLayoutRow ReadClassLayoutRow(Token token)
		{
			if (token.Table != TableTypes.ClassLayout)
				throw new ArgumentException("Invalid token type for ClassLayoutRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new ClassLayoutRow(
					reader.ReadInt16(),
					reader.ReadInt32(),
					ReadIndexValue(reader, TableTypes.TypeDef)
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
			if (token.Table != TableTypes.FieldLayout)
				throw new ArgumentException("Invalid token type for FieldLayoutRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new FieldLayoutRow(
					reader.ReadUInt32(),
					ReadIndexValue(reader, TableTypes.Field)
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
			if (token.Table != TableTypes.StandAloneSig)
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
		public EventMapRow ReadEventMapRow(Token token)
		{
			if (token.Table != TableTypes.EventMap)
				throw new ArgumentException("Invalid token type for EventMapRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new EventMapRow(
					ReadIndexValue(reader, TableTypes.TypeDef),
					ReadIndexValue(reader, TableTypes.Event)
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
			if (token.Table != TableTypes.Event)
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
		public PropertyMapRow ReadPropertyMapRow(Token token)
		{
			if (token.Table != TableTypes.PropertyMap)
				throw new ArgumentException("Invalid token type for PropertyMapRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new PropertyMapRow(
					ReadIndexValue(reader, TableTypes.TypeDef),
					ReadIndexValue(reader, TableTypes.Property)
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
			if (token.Table != TableTypes.Property)
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
		public MethodSemanticsRow ReadMethodSemanticsRow(Token token)
		{
			using (BinaryReader reader = CreateReaderForToken(token))
			{
				if (token.Table != TableTypes.MethodSemantics)
					throw new ArgumentException("Invalid token type for MethodSemanticsRow.", @"token");

				return new MethodSemanticsRow(
					(MethodSemanticsAttributes)reader.ReadInt16(),
					ReadIndexValue(reader, TableTypes.MethodDef),
					ReadIndexValue2(reader, IndexType.HasSemantics)
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
			if (token.Table != TableTypes.MethodImpl)
				throw new ArgumentException("Invalid token type for MethodImplRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new MethodImplRow(
					ReadIndexValue(reader, TableTypes.TypeDef),
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
		public ModuleRefRow ReadModuleRefRow(Token token)
		{
			if (token.Table != TableTypes.ModuleRef)
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
		public TypeSpecRow ReadTypeSpecRow(Token token)
		{
			if (token.Table != TableTypes.TypeSpec)
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
		public ImplMapRow ReadImplMapRow(Token token)
		{
			if (token.Table != TableTypes.ImplMap)
				throw new ArgumentException("Invalid token type for ImplMapRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new ImplMapRow(
					(PInvokeAttributes)reader.ReadUInt16(),
					ReadIndexValue2(reader, IndexType.MemberForwarded),
					ReadIndexValue(reader, IndexType.StringHeap),
					ReadIndexValue(reader, TableTypes.ModuleRef)
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
			if (token.Table != TableTypes.FieldRVA)
				throw new ArgumentException("Invalid token type for FieldRVARow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new FieldRVARow(
					reader.ReadUInt32(),
					ReadIndexValue(reader, TableTypes.Field)
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
			if (token.Table != TableTypes.Assembly)
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
		public AssemblyProcessorRow ReadAssemblyProcessorRow(Token token)
		{
			if (token.Table != TableTypes.AssemblyProcessor)
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
		public AssemblyOSRow ReadAssemblyOSRow(Token token)
		{
			if (token.Table != TableTypes.AssemblyOS)
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
		public AssemblyRefRow ReadAssemblyRefRow(Token token)
		{
			if (token.Table != TableTypes.AssemblyRef)
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
		public AssemblyRefProcessorRow ReadAssemblyRefProcessorRow(Token token)
		{
			if (token.Table != TableTypes.AssemblyRefProcessor)
				throw new ArgumentException("Invalid token type for AssemblyRefProcessorRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new AssemblyRefProcessorRow(
					reader.ReadUInt32(),
					ReadIndexValue(reader, TableTypes.AssemblyRef)
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
			if (token.Table != TableTypes.AssemblyRefOS)
				throw new ArgumentException("Invalid token type for AssemblyRefOSRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new AssemblyRefOSRow(
					reader.ReadUInt32(),
					reader.ReadUInt32(),
					reader.ReadUInt32(),
					ReadIndexValue(reader, TableTypes.AssemblyRef)
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
			if (token.Table != TableTypes.File)
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
		public ExportedTypeRow ReadExportedTypeRow(Token token)
		{
			if (token.Table != TableTypes.ExportedType)
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
		public ManifestResourceRow ReadManifestResourceRow(Token token)
		{
			if (token.Table != TableTypes.ManifestResource)
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
		public NestedClassRow ReadNestedClassRow(Token token)
		{
			if (token.Table != TableTypes.NestedClass)
				throw new ArgumentException("Invalid token type for NestedClassRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new NestedClassRow(
					ReadIndexValue(reader, TableTypes.TypeDef),
					ReadIndexValue(reader, TableTypes.TypeDef)
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
			if (token.Table != TableTypes.GenericParam)
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
		public MethodSpecRow ReadMethodSpecRow(Token token)
		{
			if (token.Table != TableTypes.MethodSpec)
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
		public GenericParamConstraintRow ReadGenericParamConstraintRow(Token token)
		{
			if (token.Table != TableTypes.GenericParamConstraint)
				throw new ArgumentException("Invalid token type for GenericParamConstraintRow.", @"token");

			using (BinaryReader reader = CreateReaderForToken(token))
			{
				return new GenericParamConstraintRow(
					ReadIndexValue(reader, TableTypes.GenericParam),
					ReadIndexValue2(reader, IndexType.TypeDefOrRef)
				);
			}
		}

		#endregion // IMetadataProvider members
	}
}
