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

namespace Mosa.Compiler.Metadata
{
	/// <summary>
	/// Base class for provider heaps.
	/// </summary>
	public abstract class Heap
	{
	
		#region Properties

		/// <summary>
		/// Metadata heap buffer.
		/// </summary>
		public byte[] Metadata { get; protected set; }

		/// <summary>
		/// Offset into metadata, where this heap starts.
		/// </summary>
		public int Offset { get; protected set; }

		/// <summary>
		/// The number of bytes allocated to this heap.
		/// </summary>
		public int Size { get; protected set; }
		
		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mosa.Compiler.Metadata.Heap"/>.
		/// </summary>
		/// <param name="metadata">The byte array, which holds the provider.</param>
		/// <param name="offset">The offset into the byte array, where the heap starts.</param>
		/// <param name="size">The size of the heap in bytes.</param>
		public Heap(byte[] metadata, int offset, int size)
		{
			this.Metadata = metadata;
			this.Offset = offset;
			this.Size = size;
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Validates the passed offset in the heap range.
		/// </summary>
		/// <param name="offset">The offset to validate.</param>
		/// <exception cref="System.ArgumentException">Thrown if the offset value is larger than the size of the heap or negative.</exception>
		protected int ValidateOffset(int offset)
		{
			if (offset < 0 || offset >= Size)
				throw new ArgumentException(@"Invalid Offset value.", @"Offset");

			return this.Offset + offset;
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
			if (0 > offset || offset > offset + Size)
				throw new ArgumentException(@"Invalid prefix Offset specified.");

			// Return value
			int result;

			if (0xC0 == (Metadata[offset] & 0xC0))
			{
				// A 4 byte length...
				result = ((Metadata[offset] & 0x1F) << 24) + (Metadata[offset + 1] << 16) + (Metadata[offset + 2] << 8) + Metadata[offset + 3];
				offset += 4;
			}
			else if (0x80 == (Metadata[offset] & 0x80))
			{
				// A 2 byte length...
				result = ((Metadata[offset] & 0x3F) << 8) + Metadata[offset + 1];
				offset += 2;
			}
			else
			{
				result = Metadata[offset] & 0x7F;
				offset += 1;
			}

			// Make sure there's enough room left in the heap
			if (offset + result > offset + Size)
				throw new BadImageFormatException();

			return result;
		}

		#endregion Methods

		#region Static methods

		/// <summary>
		/// Creates an instance of a specific heap type.
		/// </summary>
		/// <param name="provider">The provider buffer, which contains the heap.</param>
		/// <param name="type">The type of the heap to create.</param>
		/// <param name="metadata">The metadata.</param>
		/// <param name="offset">The offset into the buffer, where the heap starts.</param>
		/// <param name="size">The size of the heap in bytes.</param>
		/// <returns>An instance of the requested heap type.</returns>
		/// <exception cref="System.ArgumentException">An invalid heap type was requested.</exception>
		public static Heap CreateHeap(IMetadataProvider provider, HeapType type, byte[] metadata, int offset, int size)
		{
			switch (type)
			{
				case HeapType.String: return new StringHeap(metadata, offset, size);
				case HeapType.Guid: return new GuidHeap(metadata, offset, size);
				case HeapType.Blob: return new BlobHeap(metadata, offset, size);
				case HeapType.UserString: return new UserStringHeap(metadata, offset, size);
				case HeapType.Tables: return new TableHeap(provider, metadata, offset, size);
			}

			throw new ArgumentException(@"Invalid heap type.", @"type");
		}

		#endregion Static methods
	}
}