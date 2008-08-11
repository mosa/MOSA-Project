/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Runtime.Metadata {

	/// <summary>
	/// Provides convenient access to the provider table heap.
	/// </summary>
	public sealed class TableHeap : Heap {

		#region Types

		/// <summary>
		/// Delegate used to determine the record sizes with the given heap size.
		/// </summary>
		/// <param name="heap">The table heap making the request.</param>
		/// <returns>The size of the record for the given heap sizes.</returns>
        private delegate int SizeofDelegate(IMetadataProvider heap);

		#endregion // Types

		#region Static members

		private static int[][] _indexTables = new int[][] {
			new int[] { (int)TokenTypes.TypeDef, (int)TokenTypes.TypeRef, (int)TokenTypes.TypeSpec },
			new int[] { (int)TokenTypes.Field, (int)TokenTypes.Param, (int)TokenTypes.Property },
			new int[] { (int)TokenTypes.MethodDef, (int)TokenTypes.Field, (int)TokenTypes.TypeRef, (int)TokenTypes.TypeDef, (int)TokenTypes.Param, (int)TokenTypes.InterfaceImpl, (int)TokenTypes.MemberRef, (int)TokenTypes.Module, /*(int)TokenTypes.Permission,*/ (int)TokenTypes.Property, (int)TokenTypes.Event, (int)TokenTypes.StandAloneSig, (int)TokenTypes.ModuleRef, (int)TokenTypes.TypeSpec, (int)TokenTypes.Assembly, (int)TokenTypes.AssemblyRef, (int)TokenTypes.File, (int)TokenTypes.ExportedType, (int)TokenTypes.ManifestResource },
			new int[] { (int)TokenTypes.Field, (int)TokenTypes.Param },
			new int[] { (int)TokenTypes.TypeDef, (int)TokenTypes.MethodDef, (int)TokenTypes.Assembly },
			new int[] { (int)TokenTypes.TypeDef, (int)TokenTypes.TypeRef, (int)TokenTypes.ModuleRef, (int)TokenTypes.MethodDef, (int)TokenTypes.TypeSpec },
			new int[] { (int)TokenTypes.Event, (int)TokenTypes.Property },
			new int[] { (int)TokenTypes.MethodDef, (int)TokenTypes.MemberRef },
			new int[] { (int)TokenTypes.Field, (int)TokenTypes.MethodDef },
			new int[] { (int)TokenTypes.File, (int)TokenTypes.AssemblyRef, (int)TokenTypes.ExportedType },
			new int[] { -1, -1, (int)TokenTypes.MethodDef, (int)TokenTypes.MemberRef, -1 },
			new int[] { (int)TokenTypes.Module, (int)TokenTypes.ModuleRef, (int)TokenTypes.AssemblyRef, (int)TokenTypes.TypeRef },
			new int[] { (int)TokenTypes.TypeDef, (int)TokenTypes.MethodDef }
		};

		private static int[] _indexBits = new int[] {
			2, 2, 5, 1, 2, 3, 1, 1, 1, 2, 3, 2, 1
		};

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
		/// Initializes a new instance of <see cref="Mosa.Runtime.MetadataTableHeap"/>.
		/// </summary>
		/// <param name="provider">The provider buffer to use for the heap.</param>
		/// <param name="offset">The offset into the provider buffer, where the table heap starts.</param>
		/// <param name="size">The size of the table heap in bytes.</param>
		public TableHeap(IMetadataProvider provider, byte [] metadata, int offset, int size)
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

				// Adjust the table offset for the header so far
				nextTableOffset += 4 + 2 + 1 + 1 + 8 + 8;

				// Create the row count array
				_rowCounts = new int[(int)TokenTypes.MaxTable >> 24];
				for (long valid = _valid, i = 0; 0 != valid; valid >>= 1, i++)
				{
					if (1 == (valid & 1))
					{
						_rowCounts[i] = reader.ReadInt32();
						nextTableOffset += 4;
					}
				}

				// Calculate the index sizes
				_indexSize = new int[(int)IndexType.IndexCount];
				for (int i = 0; i < (int)IndexType.CodedIndexCount; i++)
				{
					int maxrows = 0;
					for (int table = 0; table < _indexTables[i].Length; table++)
					{
						if (-1 != _indexTables[i][table] && maxrows < _rowCounts[(_indexTables[i][table] >> 24)])
						{
							maxrows = _rowCounts[(_indexTables[i][table] >> 24)];
						}
					}

					_indexSize[i] = (maxrows < (1 << (16 - _indexBits[i])) ? 2 : 4);
				}

				// Calculate the heap index sizes...
				_indexSize[(int)IndexType.StringHeap] = 2+2*(_heapSize & 1);
				_indexSize[(int)IndexType.GuidHeap] = 2 + (_heapSize & 2);
				_indexSize[(int)IndexType.BlobHeap] = (0 == (_heapSize & 4) ? 2 : 4);

                int tableCount = (int)TokenTypes.MaxTable >> 24;

                // Calculate the record sizes
                _recordSize = CalculateRecordSizes(tableCount);

				// Calculate the table offsets
                _tableOffsets = new int[tableCount];
                for (int i = 0; i < tableCount; i++)
				{
					if (0 != _rowCounts[i])
					{
						_tableOffsets[i] = nextTableOffset;
						nextTableOffset += _recordSize[i] * _rowCounts[i];
					}
				}
			}

            _metadataProvider = provider;
		}

        private int[] CalculateRecordSizes(int tableCount)
        {
            int[] sizes = new int[tableCount];
            int sheapIdx = GetIndexSize(IndexType.StringHeap);
            int gheapIdx = GetIndexSize(IndexType.GuidHeap);
            int bheapIdx = GetIndexSize(IndexType.BlobHeap);

            sizes[(int)TokenTypes.Module >> 24] = (2 + sheapIdx + 3 * gheapIdx);
            sizes[(int)TokenTypes.TypeRef >> 24] = (GetIndexSize(IndexType.ResolutionScope) + 2 * sheapIdx);
            sizes[(int)TokenTypes.TypeDef >> 24] = (4 + 2 * sheapIdx + GetIndexSize(IndexType.TypeDefOrRef) + GetIndexSize(TokenTypes.Field) + GetIndexSize(TokenTypes.MethodDef));
            sizes[(int)TokenTypes.Field >> 24] = (2 + sheapIdx + bheapIdx);
            sizes[(int)TokenTypes.MethodDef >> 24] = (4 + 2 + 2 + sheapIdx + bheapIdx + GetIndexSize(TokenTypes.Param));
            sizes[(int)TokenTypes.Param >> 24] = (2 + 2 + sheapIdx);
            sizes[(int)TokenTypes.InterfaceImpl >> 24] = (GetIndexSize(TokenTypes.TypeDef) + GetIndexSize(IndexType.TypeDefOrRef));
            sizes[(int)TokenTypes.MemberRef >> 24] = (GetIndexSize(IndexType.MemberRefParent) + sheapIdx + bheapIdx);
            sizes[(int)TokenTypes.Constant >> 24] = (2 + GetIndexSize(IndexType.HasConstant) + bheapIdx);
            sizes[(int)TokenTypes.CustomAttribute >> 24] = (GetIndexSize(IndexType.HasCustomAttribute) + GetIndexSize(IndexType.CustomAttributeType) + bheapIdx);
            sizes[(int)TokenTypes.FieldMarshal >> 24] = (GetIndexSize(IndexType.HasFieldMarshal) + bheapIdx);
            sizes[(int)TokenTypes.DeclSecurity >> 24] = (2 + GetIndexSize(IndexType.HasDeclSecurity) + bheapIdx);
            sizes[(int)TokenTypes.ClassLayout >> 24] = (2 + 4 + GetIndexSize(TokenTypes.TypeDef));
            sizes[(int)TokenTypes.FieldLayout >> 24] = (4 + GetIndexSize(TokenTypes.Field));
            sizes[(int)TokenTypes.StandAloneSig >> 24] = (bheapIdx);
            sizes[(int)TokenTypes.EventMap >> 24] = (GetIndexSize(TokenTypes.TypeDef) + GetIndexSize(TokenTypes.Event));
            sizes[(int)TokenTypes.Event >> 24] = (2 + sheapIdx + GetIndexSize(IndexType.TypeDefOrRef));
            sizes[(int)TokenTypes.PropertyMap >> 24] = (GetIndexSize(TokenTypes.TypeDef) + GetIndexSize(TokenTypes.Property));
            sizes[(int)TokenTypes.Property >> 24] = (2 + sheapIdx + bheapIdx);
            sizes[(int)TokenTypes.MethodSemantics >> 24] = (2 + GetIndexSize(TokenTypes.MethodDef) + GetIndexSize(IndexType.HasSemantics));
            sizes[(int)TokenTypes.MethodImpl >> 24] = (GetIndexSize(TokenTypes.TypeDef) + 2 * GetIndexSize(IndexType.MethodDefOrRef));
            sizes[(int)TokenTypes.ModuleRef >> 24] = (sheapIdx);
            sizes[(int)TokenTypes.TypeSpec >> 24] = (bheapIdx);
            sizes[(int)TokenTypes.ImplMap >> 24] = (2 + GetIndexSize(IndexType.MemberForwarded) + sheapIdx + GetIndexSize(TokenTypes.ModuleRef));
            sizes[(int)TokenTypes.FieldRVA >> 24] = (4 + GetIndexSize(TokenTypes.Field));
            sizes[(int)TokenTypes.Assembly >> 24] = (4 + 2 + 2 + 2 + 2 + 4 + bheapIdx + 2 * sheapIdx);
            sizes[(int)TokenTypes.AssemblyProcessor >> 24] = (4);
            sizes[(int)TokenTypes.AssemblyOS >> 24] = (4 + 4 + 4);
            sizes[(int)TokenTypes.AssemblyRef >> 24] = (2 + 2 + 2 + 2 + 4 + 2 * bheapIdx + 2 * sheapIdx);
            sizes[(int)TokenTypes.AssemblyRefProcessor >> 24] = (4 + GetIndexSize(TokenTypes.AssemblyRef));
            sizes[(int)TokenTypes.AssemblyRefOS >> 24] = (4 + 4 + 4 + GetIndexSize(TokenTypes.AssemblyRef));
            sizes[(int)TokenTypes.File >> 24] = (4 + sheapIdx + bheapIdx);
            sizes[(int)TokenTypes.ExportedType >> 24] = (4 + 4 + 2 * sheapIdx + GetIndexSize(IndexType.Implementation));
            sizes[(int)TokenTypes.ManifestResource >> 24] = (4 + 4 + sheapIdx + GetIndexSize(IndexType.Implementation));
            sizes[(int)TokenTypes.NestedClass >> 24] = (2 * GetIndexSize(TokenTypes.TypeDef));
            sizes[(int)TokenTypes.GenericParam >> 24] = (2 + 2 + GetIndexSize(IndexType.TypeOrMethodDef) + sheapIdx);
            sizes[(int)TokenTypes.MethodSpec >> 24] = (GetIndexSize(IndexType.MethodDefOrRef) + bheapIdx);
            sizes[(int)TokenTypes.GenericParamConstraint >> 24] = (GetIndexSize(TokenTypes.GenericParam) + GetIndexSize(IndexType.TypeDefOrRef));

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
		/// <param name="type">The heap type to retrieve the index size for.</param>
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
        private int GetIndexSize(TokenTypes tokenTypes)
		{
			int table = (int)tokenTypes >> 24;
			if (0 > table || table >= (int)TokenTypes.MaxTable)
				throw new ArgumentException(@"Invalid token type.", @"tokenTypes");

			if (_rowCounts[table] > 65535)
				return 4;

			return 2;
		}

		/// <summary>
		/// Read and decode an index value from the reader.
		/// </summary>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="index">The index type to read.</param>
		/// <returns>The index value.</returns>
        private TokenTypes ReadIndexValue(BinaryReader reader, IndexType index)
		{
			int width = GetIndexSize(index);
            TokenTypes value = (TokenTypes)(2 == width ? reader.ReadInt16() : reader.ReadInt32());
			
			// Do we need to decode a coded index?
            if (index < IndexType.CodedIndexCount)
            {
                int bits = _indexBits[(int)index];
                int mask = 1;
                for (int i = 1; i < bits; i++) mask = (mask << 1) | 1;

                // Get the table
                int table = (int)value & mask;

                // Correct the value
                value = (TokenTypes)(((int)value >> bits) | _indexTables[(int)index][table]);
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

		/// <summary>
		/// Read and decode an index value from the reader.
		/// </summary>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="index">The index type to read.</param>
		/// <returns>The index value.</returns>
        private TokenTypes ReadIndexValue(BinaryReader reader, TokenTypes table)
		{
			int width = GetIndexSize(table);
            return (TokenTypes)(2 == width ? reader.ReadInt16() : reader.ReadInt32()) | table;
        }

        private BinaryReader CreateReaderForToken(TokenTypes token)
        {
            // Calculate the table and row index
            TokenTypes table = (token & TokenTypes.TableMask);
            TokenTypes row = (token & TokenTypes.RowIndexMask);
            Debug.Assert(table < TokenTypes.MaxTable);
            if (table >= TokenTypes.MaxTable)
                throw new ArgumentException(@"Invalid table specified in token.", @"token");
            if (token > GetMaxTokenValue(table))
                throw new ArgumentException(@"Row is out of bounds.", @"token");
            if (0 == row)
                throw new ArgumentException(@"Invalid row index.", @"token");
            int tableIdx = (int)(table) >> 24;
            int tableOffset = _tableOffsets[tableIdx] + ((int)row - 1) * _recordSize[tableIdx];

            BinaryReader reader = new BinaryReader(new MemoryStream(_metadata), Encoding.UTF8);
            reader.BaseStream.Position = tableOffset;
            return reader;
        }

        #endregion // Methods

        #region IMetadataProvider members

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

        public void Read(TokenTypes token, out ModuleRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.Module)
                throw new ArgumentException("Invalid token type for ModuleRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new ModuleRow(
                    reader.ReadInt16(),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.GuidHeap),
                    ReadIndexValue(reader, IndexType.GuidHeap),
                    ReadIndexValue(reader, IndexType.GuidHeap)
                );
            }
        }

        public void Read(TokenTypes token, out TypeRefRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.TypeRef)
                throw new ArgumentException("Invalid token type for TypeRefRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new TypeRefRow(
                    ReadIndexValue(reader, IndexType.ResolutionScope),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.StringHeap)
                );
            }
        }

        public void Read(TokenTypes token, out TypeDefRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.TypeDef)
                throw new ArgumentException("Invalid token type for TypeDefRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new TypeDefRow(
                    (TypeAttributes)reader.ReadUInt32(),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.TypeDefOrRef),
                    ReadIndexValue(reader, TokenTypes.Field),
                    ReadIndexValue(reader, TokenTypes.MethodDef)
                );
            }
        }

        public void Read(TokenTypes token, out FieldRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.Field)
                throw new ArgumentException("Invalid token type for FieldRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new FieldRow(
			        (FieldAttributes)reader.ReadUInt16(),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.BlobHeap)
                );
            }
        }

        public void Read(TokenTypes token, out MethodDefRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.MethodDef)
                throw new ArgumentException("Invalid token type for MethodDefRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new MethodDefRow(
			        reader.ReadUInt32(),
			        (MethodImplAttributes)reader.ReadUInt16(),
			        (MethodAttributes)reader.ReadUInt16(),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.BlobHeap),
                    ReadIndexValue(reader, TokenTypes.Param)
                );
            }
        }

        public void Read(TokenTypes token, out ParamRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.Param)
                throw new ArgumentException("Invalid token type for ParamRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new ParamRow(
			        (ParameterAttributes)reader.ReadUInt16(),
			        reader.ReadInt16(),
                    ReadIndexValue(reader, IndexType.StringHeap)
                );
            }
        }

        public void Read(TokenTypes token, out InterfaceImplRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.InterfaceImpl)
                throw new ArgumentException("Invalid token type for InterfaceImplRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new InterfaceImplRow(
                    ReadIndexValue(reader, TokenTypes.TypeDef),
                    ReadIndexValue(reader, IndexType.TypeDefOrRef)
                );
            }
        }

        public void Read(TokenTypes token, out MemberRefRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.MemberRef)
                throw new ArgumentException("Invalid token type for MemberRefRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new MemberRefRow(
                    ReadIndexValue(reader, IndexType.MemberRefParent),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.BlobHeap)
                );
            }
        }

        public void Read(TokenTypes token, out ConstantRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.Constant)
                throw new ArgumentException("Invalid token type for ConstantRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                CilElementType cet = (CilElementType)reader.ReadByte();
    			reader.ReadByte();

                result = new ConstantRow(
                    cet,
                    ReadIndexValue(reader, IndexType.HasConstant),
                    ReadIndexValue(reader, IndexType.BlobHeap)
                );
            }
        }

        public void Read(TokenTypes token, out CustomAttributeRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.CustomAttribute)
                throw new ArgumentException("Invalid token type for CustomAttributeRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new CustomAttributeRow(
                    ReadIndexValue(reader, IndexType.HasCustomAttribute),
                    ReadIndexValue(reader, IndexType.CustomAttributeType),
                    ReadIndexValue(reader, IndexType.BlobHeap)
                );
            }
        }

        public void Read(TokenTypes token, out FieldMarshalRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.FieldMarshal)
                throw new ArgumentException("Invalid token type for FieldMarshalRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new FieldMarshalRow(
                    ReadIndexValue(reader, IndexType.HasFieldMarshal),
                    ReadIndexValue(reader, IndexType.BlobHeap)
                );
            }
        }

        public void Read(TokenTypes token, out DeclSecurityRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.DeclSecurity)
                throw new ArgumentException("Invalid token type for DeclSecurityRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new DeclSecurityRow(
			        (System.Security.Permissions.SecurityAction)reader.ReadUInt16(),
                    ReadIndexValue(reader, IndexType.HasDeclSecurity),
                    ReadIndexValue(reader, IndexType.BlobHeap)
                );
            }
        }

        public void Read(TokenTypes token, out ClassLayoutRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.ClassLayout)
                throw new ArgumentException("Invalid token type for ClassLayoutRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new ClassLayoutRow(
			        reader.ReadUInt16(),
			        reader.ReadUInt32(),
                    ReadIndexValue(reader, TokenTypes.TypeDef)
                );
            }
        }

        public void Read(TokenTypes token, out FieldLayoutRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.FieldLayout)
                throw new ArgumentException("Invalid token type for FieldLayoutRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new FieldLayoutRow(
			        reader.ReadUInt32(),
                    ReadIndexValue(reader, TokenTypes.Field)
                );
            }
        }

        public void Read(TokenTypes token, out StandAloneSigRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.StandAloneSig)
                throw new ArgumentException("Invalid token type for StandAloneSigRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new StandAloneSigRow(
                    ReadIndexValue(reader, IndexType.BlobHeap)
                );
            }
        }

        public void Read(TokenTypes token, out EventMapRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.EventMap)
                throw new ArgumentException("Invalid token type for EventMapRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new EventMapRow(
                    ReadIndexValue(reader, TokenTypes.TypeDef),
                    ReadIndexValue(reader, TokenTypes.Event)
                );
            }
        }

        public void Read(TokenTypes token, out EventRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.Event)
                throw new ArgumentException("Invalid token type for EventRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new EventRow(
			        (EventAttributes)reader.ReadUInt16(),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.TypeDefOrRef)
                );
            }
        }

        public void Read(TokenTypes token, out PropertyMapRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.PropertyMap)
                throw new ArgumentException("Invalid token type for PropertyMapRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new PropertyMapRow(
                    ReadIndexValue(reader, TokenTypes.TypeDef),
                    ReadIndexValue(reader, TokenTypes.Property)
                );
            }
        }

        public void Read(TokenTypes token, out PropertyRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.Property)
                throw new ArgumentException("Invalid token type for PropertyRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new PropertyRow(
			        (PropertyAttributes)reader.ReadUInt16(),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.BlobHeap)
                );
            }
        }

        public void Read(TokenTypes token, out MethodSemanticsRow result)
        {
            using (BinaryReader reader = CreateReaderForToken(token))
            {
                if ((token & TokenTypes.TableMask) != TokenTypes.MethodSemantics)
                    throw new ArgumentException("Invalid token type for MethodSemanticsRow.", @"token");

                result = new MethodSemanticsRow(
			        (MethodSemanticsAttributes)reader.ReadInt16(),
                    ReadIndexValue(reader, TokenTypes.MethodDef),
                    ReadIndexValue(reader, IndexType.HasSemantics)
                );
            }
        }

        public void Read(TokenTypes token, out MethodImplRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.MethodImpl)
                throw new ArgumentException("Invalid token type for MethodImplRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new MethodImplRow(
                    ReadIndexValue(reader, TokenTypes.TypeDef),
                    ReadIndexValue(reader, IndexType.MethodDefOrRef),
                    ReadIndexValue(reader, IndexType.MethodDefOrRef)
                );
            }
        }

        public void Read(TokenTypes token, out ModuleRefRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.ModuleRef)
                throw new ArgumentException("Invalid token type for ModuleRefRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new ModuleRefRow(
                    ReadIndexValue(reader, IndexType.StringHeap)
                );
            }
        }

        public void Read(TokenTypes token, out TypeSpecRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.TypeSpec)
                throw new ArgumentException("Invalid token type for TypeSpecRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new TypeSpecRow(
                    ReadIndexValue(reader, IndexType.BlobHeap)
                );
            }
        }

        public void Read(TokenTypes token, out ImplMapRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.ImplMap)
                throw new ArgumentException("Invalid token type for ImplMapRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new ImplMapRow(
			        (PInvokeAttributes)reader.ReadUInt16(),
                    ReadIndexValue(reader, IndexType.MemberForwarded),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, TokenTypes.ModuleRef)
                );
            }
        }

        public void Read(TokenTypes token, out FieldRVARow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.FieldRVA)
                throw new ArgumentException("Invalid token type for FieldRVARow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new FieldRVARow(
			        reader.ReadUInt32(),
                    ReadIndexValue(reader, TokenTypes.Field)
                );
            }
        }

        public void Read(TokenTypes token, out AssemblyRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.Assembly)
                throw new ArgumentException("Invalid token type for AssemblyRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new AssemblyRow(
			        (AssemblyHashAlgorithm)reader.ReadUInt32(),
			        reader.ReadUInt16(),
			        reader.ReadUInt16(),
			        reader.ReadUInt16(),
			        reader.ReadUInt16(),
			        (AssemblyFlags)reader.ReadUInt32(),
			        ReadIndexValue(reader, IndexType.BlobHeap),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.StringHeap)
                );
            }
        }

        public void Read(TokenTypes token, out AssemblyProcessorRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.AssemblyProcessor)
                throw new ArgumentException("Invalid token type for AssemblyProcessorRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new AssemblyProcessorRow(
                    reader.ReadUInt32()
                );
            }
        }

        public void Read(TokenTypes token, out AssemblyOSRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.AssemblyOS)
                throw new ArgumentException("Invalid token type for AssemblyOSRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new AssemblyOSRow(
			        reader.ReadInt32(),
			        reader.ReadInt32(),
			        reader.ReadInt32()
                );
            }
        }

        public void Read(TokenTypes token, out AssemblyRefRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.AssemblyRef)
                throw new ArgumentException("Invalid token type for AssemblyRefRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new AssemblyRefRow(
			        reader.ReadUInt16(),
			        reader.ReadUInt16(),
			        reader.ReadUInt16(),
			        reader.ReadUInt16(),
			        (AssemblyFlags)reader.ReadUInt32(),
                    ReadIndexValue(reader, IndexType.BlobHeap),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.BlobHeap)
                );
            }
        }

        public void Read(TokenTypes token, out AssemblyRefProcessorRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.AssemblyRefProcessor)
                throw new ArgumentException("Invalid token type for AssemblyRefProcessorRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new AssemblyRefProcessorRow(
			        reader.ReadUInt32(),
                    ReadIndexValue(reader, TokenTypes.AssemblyRef)
                );
            }
        }

        public void Read(TokenTypes token, out AssemblyRefOSRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.AssemblyRefOS)
                throw new ArgumentException("Invalid token type for AssemblyRefOSRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new AssemblyRefOSRow(
			        reader.ReadUInt32(),
			        reader.ReadUInt32(),
			        reader.ReadUInt32(),
                    ReadIndexValue(reader, TokenTypes.AssemblyRef)
                );
            }
        }

        public void Read(TokenTypes token, out FileRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.File)
                throw new ArgumentException("Invalid token type for FileRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new FileRow(
			        (FileAttributes)reader.ReadUInt32(),
			        ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.BlobHeap)
                );
            }
        }

        public void Read(TokenTypes token, out ExportedTypeRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.ExportedType)
                throw new ArgumentException("Invalid token type for ExportedTypeRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new ExportedTypeRow(
			        (TypeAttributes)reader.ReadUInt32(),
			        (TokenTypes)reader.ReadUInt32(),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.Implementation)
                );
            }
        }

        public void Read(TokenTypes token, out ManifestResourceRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.ManifestResource)
                throw new ArgumentException("Invalid token type for ManifestResourceRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new ManifestResourceRow(
			        reader.ReadUInt32(),
			        (ManifestResourceAttributes)reader.ReadUInt32(),
                    ReadIndexValue(reader, IndexType.StringHeap),
                    ReadIndexValue(reader, IndexType.Implementation)
                );
            }
        }

        public void Read(TokenTypes token, out NestedClassRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.NestedClass)
                throw new ArgumentException("Invalid token type for NestedClassRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new NestedClassRow(
                    ReadIndexValue(reader, TokenTypes.TypeDef),
                    ReadIndexValue(reader, TokenTypes.TypeDef)
                );
            }
        }

        public void Read(TokenTypes token, out GenericParamRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.GenericParam)
                throw new ArgumentException("Invalid token type for GenericParamRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new GenericParamRow(
			        reader.ReadUInt16(),
			        (GenericParamAttributes)reader.ReadUInt16(),
                    ReadIndexValue(reader, IndexType.TypeOrMethodDef),
                    ReadIndexValue(reader, IndexType.StringHeap)
                );
            }
        }

        public void Read(TokenTypes token, out MethodSpecRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.MethodSpec)
                throw new ArgumentException("Invalid token type for MethodSpecRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new MethodSpecRow(
                    ReadIndexValue(reader, IndexType.MethodDefOrRef),
                    ReadIndexValue(reader, IndexType.BlobHeap)
                );
            }
        }

        public void Read(TokenTypes token, out GenericParamConstraintRow result)
        {
            if ((token & TokenTypes.TableMask) != TokenTypes.GenericParamConstraint)
                throw new ArgumentException("Invalid token type for GenericParamConstraintRow.", @"token");

            using (BinaryReader reader = CreateReaderForToken(token))
            {
                result = new GenericParamConstraintRow(
                    ReadIndexValue(reader, TokenTypes.GenericParam),
                    ReadIndexValue(reader, IndexType.TypeDefOrRef)
                );
            }
        }

        #endregion // IMetadataProvider members
    }
}
