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
using System.IO;
using System.Text;

namespace Mosa.Runtime.Metadata {

	/// <summary>
	/// Base class for provider heaps.
	/// </summary>
	public abstract class Heap {
		#region Data members

		/// <summary>
		/// Metadata heap buffer.
		/// </summary>
		protected byte[] _metadata;

		/// <summary>
		/// Offset into _metadata, where this heap starts.
		/// </summary>
		protected int _offset;

		/// <summary>
		/// The number of bytes allocated to this heap.
		/// </summary>
		protected int _size;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mosa.Runtime.Metadata.Heap"/>.
		/// </summary>
        /// <param name="metadata">The byte array, which holds the provider.</param>
		/// <param name="offset">The offset into the byte array, where the heap starts.</param>
		/// <param name="size">The size of the heap in bytes.</param>
		public Heap(byte[] metadata, int offset, int size)
		{
			if (null == metadata)
				throw new ArgumentNullException(@"provider");
			
			_metadata = metadata;
			_offset = offset;
			_size = size;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Enables derived classes to access the provider buffer.
		/// </summary>
		protected byte[] Buffer
		{
			get
			{
				return _metadata;
			}
		}

		/// <summary>
		/// Retrieves the size of the heap.
		/// </summary>
		public int Size
		{
			get
			{
				return _size;
			}
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Validates the passed offset in the heap range.
		/// </summary>
		/// <param name="offset">The offset to validate.</param>
		/// <exception cref="System.ArgumentException">Thrown if the offset value is larger than the size of the heap or negative.</exception>
		protected int ValidateOffset(int offset)
		{
			if (0 > offset || offset >= _size)
				throw new ArgumentException(@"Invalid offset value.", @"offset");

			return _offset + offset;
		}

		/// <summary>
		/// Calculates the byte length of prefixed data structures in the blob and user string heap.
		/// </summary>
		/// <param name="offset">The offset, where the length prefix is located.</param>
		/// <returns>The length of the data structure in bytes.</returns>
		/// <exception cref="System.ArgumentException">The specified offset is either negative or out of range.</exception>
		/// <exception cref="System.BadImageFormatException">The provider buffer is malformed. The retrieved length exceeds the heap space.</exception>
		protected int CalculatePrefixLength(ref int offset)
		{
			// FIXME: Check preconditions
			if (0 > offset || offset > _offset + _size)
				throw new ArgumentException(@"Invalid prefix offset specified.");

			// Return value
			int result;

			if (0xC0 == (_metadata[offset] & 0xC0))
			{
				// A 4 byte length...
				result = ((_metadata[offset] & 0x1F) << 24) + (_metadata[offset + 1] << 16) + (_metadata[offset + 2] << 8) + _metadata[offset + 3];
				offset += 4;
			}
			else if (0x80 == (_metadata[offset] & 0x80))
			{
				// A 2 byte length...
				result = ((_metadata[offset] & 0x3F) << 8) + _metadata[offset + 1];
				offset += 2;
			}
			else
			{
				result = _metadata[offset] & 0x7F;
				offset += 1;
			}

			// Make sure there's enough room left in the heap
			if (offset + result > _offset + _size)
				throw new BadImageFormatException();

			return result;
		}

		#endregion // Methods

		#region Static methods

		/// <summary>
		/// Creates an instance of a specific heap type.
		/// </summary>
		/// <param name="type">The type of the heap to create.</param>
		/// <param name="provider">The provider buffer, which contains the heap.</param>
		/// <param name="offset">The offset into the buffer, where the heap starts.</param>
		/// <param name="size">The size of the heap in bytes.</param>
		/// <returns>An instance of the requested heap type.</returns>
		/// <exception cref="System.ArgumentException">An invalid heap type was requested.</exception>
		public static Heap CreateHeap(IMetadataProvider provider, HeapType type, byte[] metadata, int offset, int size)
		{
			switch (type)
			{
				case HeapType.String:
					return new StringHeap(metadata, offset, size);

				case HeapType.Guid:
					return new GuidHeap(metadata, offset, size);

				case HeapType.Blob:
					return new BlobHeap(metadata, offset, size);

				case HeapType.UserString:
					return new UserStringHeap(metadata, offset, size);

				case HeapType.Tables:
					return new TableHeap(provider, metadata, offset, size);
			}

			throw new ArgumentException(@"Invalid heap type.", @"type");
		}

		#endregion // Static methods
	}
}
