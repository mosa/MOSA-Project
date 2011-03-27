/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Runtime.Metadata.Loader.PE
{

	/// <summary>
	/// Metadata root structure according to ISO/IEC 23271:2006 (E), §24.2.1
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class MetadataRoot : IMetadataProvider
	{
		#region Constants

		/// <summary>
		/// Signature constant of the provider root.
		/// </summary>
		private const uint MetadataRootSignature = 0x424A5342;

		#endregion // Constants

		#region Data members

		/// <summary>
		/// Major version, 1 (ignore on read).
		/// </summary>
		private ushort MajorVersion;

		/// <summary>
		/// Minor version, 1 (ignore on read).
		/// </summary>
		private ushort MinorVersion;

		/// <summary>
		/// Reserved, always 0 (according to ISO/IEC 23271:2006 (E), §24.1)
		/// </summary>
		private uint Reserved;

		/// <summary>
		/// UTF8-encoded version string of the provider format.
		/// </summary>
		private string Version;

		/// <summary>
		/// Reserved, always 0 (according to ISO/IEC 23271:2006 (E), §24.1)
		/// </summary>
		private ushort Flags;

		/// <summary>
		/// Array of provider streams found in the source file.
		/// </summary>
		private Heap[] _streams = new Heap[(int)HeapType.MaxType];

		/// <summary>
		/// Metadata binary byte array.
		/// </summary>	
		private byte[] metadata;

		#endregion // Data members

		/// <summary>
		/// Initializes a new instance of the <see cref="MetadataRoot"/> class.
		/// </summary>
		/// <param name="metadata">Byte array, which holds the (uint)</param>
		public MetadataRoot(byte[] metadata)
		{
			Initialize(metadata);
		}

		#region Methods

		/// <summary>
		/// Tries to populate the provider root structure from the given stream.
		/// </summary>
		/// <param name="metadata">Byte array, which holds the (uint)</param>
		/// <returns>True, if the stream contains a valid and supported provider format.</returns>
		protected void Initialize(byte[] metadata)
		{
			this.metadata = metadata;
			MemoryStream ms = new MemoryStream(metadata);
			BinaryReader reader = new BinaryReader(ms);
			string name;

			uint signature = reader.ReadUInt32();
			if (MetadataRootSignature != signature)
				throw new ArgumentException(@"Invalid provider format.", @"provider");

			MajorVersion = reader.ReadUInt16();
			MinorVersion = reader.ReadUInt16();
			if (1 != MajorVersion || 1 != MinorVersion)
				throw new BadImageFormatException("Unsupported provider format.");

			reader.ReadUInt32();
			int length = reader.ReadInt32();
			byte[] version = reader.ReadBytes(length);
			Version = Encoding.UTF8.GetString(version, 0, Array.IndexOf<byte>(version, 0));
			reader.ReadUInt16();
			ushort streams = reader.ReadUInt16();

			// Read stream headers
			for (ushort i = 0; i < streams; i++)
			{
				int offset = reader.ReadInt32();
				int size = reader.ReadInt32();
				int position = (int)reader.BaseStream.Position;
				length = Array.IndexOf<byte>(metadata, 0, position, 32);
				name = Encoding.ASCII.GetString(metadata, position, length - position);
				HeapType kind;
				if (name.Equals("#Strings"))
				{
					kind = HeapType.String;
				}
				else if (name.Equals("#US"))
				{
					kind = HeapType.UserString;
				}
				else if (name.Equals("#Blob"))
				{
					kind = HeapType.Blob;
				}
				else if (name.Equals("#GUID"))
				{
					kind = HeapType.Guid;
				}
				else if (name.Equals("#~"))
				{
					kind = HeapType.Tables;
				}
				else
				{
					throw new NotSupportedException();
				}

				_streams[(int)kind] = Heap.CreateHeap(this, kind, metadata, offset, size);

				// Move to the next stream
				reader.BaseStream.Position = length + (4 - length % 4);
			}
		}

		/// <summary>
		/// Retrieves the requested provider heap.
		/// </summary>
		/// <param name="type">The requested provider heap.</param>
		/// <returns>The provider heap requested.</returns>
		/// <exception cref="System.ArgumentException"><paramref name="type"/> is invalid.</exception>
		public Heap GetHeap(HeapType type)
		{
			if (0 > type || type >= HeapType.MaxType)
				throw new ArgumentException(@"Invalid heap type.", @"type");

			return _streams[(int)type];
		}

		#endregion // Methods

		#region IMetadataProvider members

		/// <summary>
		/// Gets the rows.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns></returns>
		int IMetadataProvider.GetRowCount(TokenTypes table)
		{
			return ((TableHeap)_streams[(int)HeapType.Tables]).GetRowCount(table);
		}

		/// <summary>
		/// Gets the row count.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns></returns>
		int IMetadataProvider.GetRowCount(MetadataTable table)
		{
			return ((TableHeap)_streams[(int)HeapType.Tables]).GetRowCount(table);
		}

		MetadataToken IMetadataProvider.GetMaxTokenValue(MetadataTable table)
		{
			return ((TableHeap)_streams[(int)HeapType.Tables]).GetMaxTokenValue(table);
		}

		/// <summary>
		/// Reads a string heap or user string heap entry.
		/// </summary>
		/// <param name="token">The token of the string to read.</param>
		/// <returns></returns>
		string IMetadataProvider.ReadString(TokenTypes token)
		{
			switch ((TokenTypes.TableMask & token))
			{
				case TokenTypes.String:
					{
						StringHeap sheap = (StringHeap)_streams[(int)HeapType.String];
						return sheap.ReadString(token);
					}
				case TokenTypes.UserString:
					{
						UserStringHeap usheap = (UserStringHeap)_streams[(int)HeapType.UserString];
						return usheap.ReadString(token);
					}
				default:
					throw new ArgumentException(@"Invalid token for a string.", @"token");
			}
		}

		/// <summary>
		/// Reads a guid heap entry.
		/// </summary>
		/// <param name="token">The token of the guid heap entry to read.</param>
		/// <returns></returns>
		Guid IMetadataProvider.ReadGuid(TokenTypes token)
		{
			if ((TokenTypes.TableMask & token) == TokenTypes.Guid)
			{
				GuidHeap gheap = (GuidHeap)_streams[(int)HeapType.Guid];
				return gheap.ReadGuid(token);
			}
			else
			{
				throw new ArgumentException(@"Invalid token for a guid.", @"token");
			}
		}

		/// <summary>
		/// Reads a blob heap entry.
		/// </summary>
		/// <param name="token">The token of the blob heap entry to read.</param>
		/// <returns></returns>
		byte[] IMetadataProvider.ReadBlob(TokenTypes token)
		{
			if (TokenTypes.Blob == (TokenTypes.TableMask & token))
			{
				BlobHeap bheap = (BlobHeap)_streams[(int)HeapType.Blob];
				return bheap.ReadBlob(token);
			}
			else
			{
				throw new ArgumentException(@"Invalid token for a blob.", @"token");
			}
		}

		/// <summary>
		/// Reads a module row from provider.
		/// </summary>
		/// <param name="token">The module row token.</param>
		/// <returns></returns>
		ModuleRow IMetadataProvider.ReadModuleRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadModuleRow(token);
		}

		/// <summary>
		/// Reads a type reference row from provider.
		/// </summary>
		/// <param name="token">The type reference row token.</param>
		/// <returns></returns>
		TypeRefRow IMetadataProvider.ReadTypeRefRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadTypeRefRow(token);
		}

		/// <summary>
		/// Reads a type definition row from provider.
		/// </summary>
		/// <param name="token">The type definition row token.</param>
		/// <returns></returns>
		TypeDefRow IMetadataProvider.ReadTypeDefRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadTypeDefRow(token);
		}

		/// <summary>
		/// Reads a _stackFrameIndex definition row from provider.
		/// </summary>
		/// <param name="token">The field definition row token.</param>
		/// <returns></returns>
		FieldRow IMetadataProvider.ReadFieldRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadFieldRow(token);
		}

		/// <summary>
		/// Reads a method definition row from provider.
		/// </summary>
		/// <param name="token">The method definition row token.</param>
		/// <returns></returns>
		MethodDefRow IMetadataProvider.ReadMethodDefRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadMethodDefRow(token);
		}

		/// <summary>
		/// Reads a parameter row from provider.
		/// </summary>
		/// <param name="token">The parameter row token.</param>
		/// <returns></returns>
		ParamRow IMetadataProvider.ReadParamRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadParamRow(token);
		}

		/// <summary>
		/// Reads an interface implementation row from provider.
		/// </summary>
		/// <param name="token">The interface implementation row token.</param>
		/// <returns></returns>
		InterfaceImplRow IMetadataProvider.ReadInterfaceImplRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadInterfaceImplRow(token);
		}

		/// <summary>
		/// Reads an member reference row from provider.
		/// </summary>
		/// <param name="token">The member reference row token.</param>
		/// <returns></returns>
		MemberRefRow IMetadataProvider.ReadMemberRefRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadMemberRefRow(token);
		}

		/// <summary>
		/// Reads a constant row from provider.
		/// </summary>
		/// <param name="token">The constant row token.</param>
		/// <returns></returns>
		ConstantRow IMetadataProvider.ReadConstantRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadConstantRow(token);
		}

		/// <summary>
		/// Reads a constant row from provider.
		/// </summary>
		/// <param name="token">The constant row token.</param>
		/// <returns></returns>
		CustomAttributeRow IMetadataProvider.ReadCustomAttributeRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadCustomAttributeRow(token);
		}

		/// <summary>
		/// Reads a _stackFrameIndex marshal row from provider.
		/// </summary>
		/// <param name="token">The _stackFrameIndex marshal row token.</param>
		/// <returns></returns>
		FieldMarshalRow IMetadataProvider.ReadFieldMarshalRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadFieldMarshalRow(token);
		}

		/// <summary>
		/// Reads a declarative security row from provider.
		/// </summary>
		/// <param name="token">The declarative security row token.</param>
		/// <returns></returns>
		DeclSecurityRow IMetadataProvider.ReadDeclSecurityRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadDeclSecurityRow(token);
		}

		/// <summary>
		/// Reads a class layout row from provider.
		/// </summary>
		/// <param name="token">The class layout row token.</param>
		/// <returns></returns>
		ClassLayoutRow IMetadataProvider.ReadClassLayoutRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadClassLayoutRow(token);
		}

		/// <summary>
		/// Reads a _stackFrameIndex layout row from provider.
		/// </summary>
		/// <param name="token">The _stackFrameIndex layout row token.</param>
		/// <returns></returns>
		FieldLayoutRow IMetadataProvider.ReadFieldLayoutRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadFieldLayoutRow(token);
		}

		/// <summary>
		/// Reads a standalone signature row from provider.
		/// </summary>
		/// <param name="token">The standalone signature row token.</param>
		/// <returns></returns>
		StandAloneSigRow IMetadataProvider.ReadStandAloneSigRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadStandAloneSigRow(token);
		}

		/// <summary>
		/// Reads a event map row from provider.
		/// </summary>
		/// <param name="token">The event map row token.</param>
		/// <returns></returns>
		EventMapRow IMetadataProvider.ReadEventMapRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadEventMapRow(token);
		}

		/// <summary>
		/// Reads a event row from provider.
		/// </summary>
		/// <param name="token">The event row token.</param>
		/// <returns></returns>
		EventRow IMetadataProvider.ReadEventRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadEventRow(token);
		}

		/// <summary>
		/// Reads a property map row from provider.
		/// </summary>
		/// <param name="token">The property map row token.</param>
		/// <returns></returns>
		PropertyMapRow IMetadataProvider.ReadPropertyMapRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadPropertyMapRow(token);
		}

		/// <summary>
		/// Reads a property row from provider.
		/// </summary>
		/// <param name="token">The property row token.</param>
		/// <returns></returns>
		PropertyRow IMetadataProvider.ReadPropertyRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadPropertyRow(token);
		}

		/// <summary>
		/// Reads a method semantics row from provider.
		/// </summary>
		/// <param name="token">The method semantics row token.</param>
		/// <returns></returns>
		MethodSemanticsRow IMetadataProvider.ReadMethodSemanticsRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadMethodSemanticsRow(token);
		}

		/// <summary>
		/// Reads a method impl row from provider.
		/// </summary>
		/// <param name="token">The method impl row token.</param>
		/// <returns></returns>
		MethodImplRow IMetadataProvider.ReadMethodImplRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadMethodImplRow(token);
		}

		/// <summary>
		/// Reads a module ref row from provider.
		/// </summary>
		/// <param name="token">The module ref row token.</param>
		/// <returns></returns>
		ModuleRefRow IMetadataProvider.ReadModuleRefRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadModuleRefRow(token);
		}

		/// <summary>
		/// Reads a typespec row from provider.
		/// </summary>
		/// <param name="token">The typespec row token.</param>
		/// <returns></returns>
		TypeSpecRow IMetadataProvider.ReadTypeSpecRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadTypeSpecRow(token);
		}

		/// <summary>
		/// Reads a implementation map row from provider.
		/// </summary>
		/// <param name="token">The implementation map row token.</param>
		/// <returns></returns>
		ImplMapRow IMetadataProvider.ReadImplMapRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadImplMapRow(token);
		}

		/// <summary>
		/// Reads a _stackFrameIndex rva row from provider.
		/// </summary>
		/// <param name="token">The _stackFrameIndex rva row token.</param>
		/// <returns></returns>
		FieldRVARow IMetadataProvider.ReadFieldRVARow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadFieldRVARow(token);
		}

		/// <summary>
		/// Reads a assembly row from provider.
		/// </summary>
		/// <param name="token">The assembly row token.</param>
		/// <returns></returns>
		AssemblyRow IMetadataProvider.ReadAssemblyRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadAssemblyRow(token);
		}

		/// <summary>
		/// Reads a assembly processor row from provider.
		/// </summary>
		/// <param name="token">The assembly processor row token.</param>
		/// <returns></returns>
		AssemblyProcessorRow IMetadataProvider.ReadAssemblyProcessorRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadAssemblyProcessorRow(token);
		}

		/// <summary>
		/// Reads a assembly os row from provider.
		/// </summary>
		/// <param name="token">The assembly os row token.</param>
		/// <returns></returns>
		AssemblyOSRow IMetadataProvider.ReadAssemblyOSRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadAssemblyOSRow(token);
		}

		/// <summary>
		/// Reads a assembly reference row from provider.
		/// </summary>
		/// <param name="token">The assembly reference row token.</param>
		/// <returns></returns>
		AssemblyRefRow IMetadataProvider.ReadAssemblyRefRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadAssemblyRefRow(token);
		}

		/// <summary>
		/// Reads a assembly reference processor row from provider.
		/// </summary>
		/// <param name="token">The assembly reference processor row token.</param>
		/// <returns></returns>
		AssemblyRefProcessorRow IMetadataProvider.ReadAssemblyRefProcessorRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadAssemblyRefProcessorRow(token);
		}

		/// <summary>
		/// Reads a assembly reference os row from provider.
		/// </summary>
		/// <param name="token">The assembly reference os row token.</param>
		/// <returns></returns>
		AssemblyRefOSRow IMetadataProvider.ReadAssemblyRefOSRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadAssemblyRefOSRow(token);
		}

		/// <summary>
		/// Reads a file row from provider.
		/// </summary>
		/// <param name="token">The file row token.</param>
		/// <returns></returns>
		FileRow IMetadataProvider.ReadFileRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadFileRow(token);
		}

		/// <summary>
		/// Reads an exported type row from provider.
		/// </summary>
		/// <param name="token">The exported type row token.</param>
		/// <returns></returns>
		ExportedTypeRow IMetadataProvider.ReadExportedTypeRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadExportedTypeRow(token);
		}

		/// <summary>
		/// Reads a manifest resource row from provider.
		/// </summary>
		/// <param name="token">The manifest resource row token.</param>
		/// <returns></returns>
		ManifestResourceRow IMetadataProvider.ReadManifestResourceRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadManifestResourceRow(token);
		}

		/// <summary>
		/// Reads a manifest resource row from provider.
		/// </summary>
		/// <param name="token">The manifest resource row token.</param>
		/// <returns></returns>
		NestedClassRow IMetadataProvider.ReadNestedClassRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadNestedClassRow(token);
		}

		/// <summary>
		/// Reads a generic parameter row from provider.
		/// </summary>
		/// <param name="token">The generic parameter row token.</param>
		/// <returns></returns>
		GenericParamRow IMetadataProvider.ReadGenericParamRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadGenericParamRow(token);
		}

		/// <summary>
		/// Reads a method specification row from provider.
		/// </summary>
		/// <param name="token">The method specification row token.</param>
		/// <returns></returns>
		MethodSpecRow IMetadataProvider.ReadMethodSpecRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadMethodSpecRow(token);
		}

		/// <summary>
		/// Reads a generic parameter constraint row from provider.
		/// </summary>
		/// <param name="token">The generic parameter constraint row token.</param>
		/// <returns></returns>
		GenericParamConstraintRow IMetadataProvider.ReadGenericParamConstraintRow(MetadataToken token)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			return theap.ReadGenericParamConstraintRow(token);
		}

		/// <summary>
		/// Gets the heaps of a specified type
		/// </summary>
		/// <param name="heapType">Type of the heap.</param>
		/// <returns></returns>
		IList<Heap> IMetadataProvider.GetHeaps(HeapType heapType)
		{
			List<Heap> list = new List<Heap>();
			list.Add(_streams[(int)heapType]);
			return list;
		}

		#endregion // IMetadataProvider members
	}
}
