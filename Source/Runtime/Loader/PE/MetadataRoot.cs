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

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Runtime.Loader.PE
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
		/// Holds the assembly of this provider root.
		/// </summary>
		private readonly IMetadataModule _assemblyImage;

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
		byte[] metadata;

		#endregion // Data members

		/// <summary>
		/// Initializes a new instance of the <see cref="MetadataRoot"/> class.
		/// </summary>
		/// <param name="assemblyImage">The assembly image.</param>
		public MetadataRoot(IMetadataModule assemblyImage)
		{
			_assemblyImage = assemblyImage;
		}

		#region Methods

		/// <summary>
		/// Tries to populate the provider root structure from the given stream.
		/// </summary>
		/// <param name="metadata">Byte array, which holds the (uint)</param>
		/// <returns>True, if the stream contains a valid and supported provider format.</returns>
		public void Initialize(byte[] metadata)
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

		TokenTypes IMetadataProvider.GetMaxTokenValue(TokenTypes tokenType)
		{
			TokenTypes result = 0;
			switch ((tokenType & TokenTypes.TableMask))
			{
				case TokenTypes.String: // String heap size
					result = TokenTypes.String | (TokenTypes)((Heap)_streams[(int)HeapType.String]).Size;
					break;

				case TokenTypes.UserString: // User string heap size
					result = TokenTypes.UserString | (TokenTypes)((Heap)_streams[(int)HeapType.UserString]).Size;
					break;

				case TokenTypes.Blob: // Blob heap size
					result = TokenTypes.Blob | (TokenTypes)((Heap)_streams[(int)HeapType.Blob]).Size;
					break;

				case TokenTypes.Guid: // Guid heap size
					result = TokenTypes.Guid | (TokenTypes)(((Heap)_streams[(int)HeapType.Guid]).Size / 16);
					break;

				default:
					result = ((TableHeap)_streams[(int)HeapType.Tables]).GetMaxTokenValue(tokenType);
					break;
			}
			return result;
		}

		string IMetadataProvider.ReadString(TokenTypes token)
		{
			switch ((TokenTypes.TableMask & token))
			{
				case TokenTypes.String:
					{
						StringHeap sheap = (StringHeap)_streams[(int)HeapType.String];
						return sheap.ReadString(ref token);
					}
				case TokenTypes.UserString:
					{
						UserStringHeap usheap = (UserStringHeap)_streams[(int)HeapType.UserString];
						return usheap.ReadString(ref token);
					}
				default:
					throw new ArgumentException(@"Invalid token for a string.", @"token");
			}
		}

		Guid IMetadataProvider.ReadGuid(TokenTypes token)
		{
			if ((TokenTypes.TableMask & token) == TokenTypes.Guid)
			{
				GuidHeap gheap = (GuidHeap)_streams[(int)HeapType.Guid];
				return gheap.ReadGuid(ref token);
			}
			else
			{
				throw new ArgumentException(@"Invalid token for a guid.", @"token");
			}
		}

		byte[] IMetadataProvider.ReadBlob(TokenTypes token)
		{
			if (TokenTypes.Blob == (TokenTypes.TableMask & token))
			{
				BlobHeap bheap = (BlobHeap)_streams[(int)HeapType.Blob];
				return bheap.ReadBlob(ref token);
			}
			else
			{
				throw new ArgumentException(@"Invalid token for a blob.", @"token");
			}
		}

		void IMetadataProvider.Read(TokenTypes token, out ModuleRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out TypeRefRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out TypeDefRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out FieldRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out MethodDefRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out ParamRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out InterfaceImplRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out MemberRefRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out ConstantRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out CustomAttributeRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out FieldMarshalRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out DeclSecurityRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out ClassLayoutRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out FieldLayoutRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out StandAloneSigRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out EventMapRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out EventRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out PropertyMapRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out PropertyRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out MethodSemanticsRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out MethodImplRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out ModuleRefRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out TypeSpecRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out ImplMapRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out FieldRVARow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out AssemblyRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out AssemblyProcessorRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out AssemblyOSRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out AssemblyRefRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out AssemblyRefProcessorRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out AssemblyRefOSRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out FileRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out ExportedTypeRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out ManifestResourceRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out NestedClassRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out GenericParamRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out MethodSpecRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out GenericParamConstraintRow result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		void IMetadataProvider.Read(TokenTypes token, out FieldRow[] result)
		{
			TableHeap theap = (TableHeap)_streams[(int)HeapType.Tables];
			theap.Read(token, out result);
		}

		#endregion // IMetadataProvider members
	}
}
