// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public class GenericPartition
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericPartition"/> class.
		/// </summary>
		/// <param name="index">The index.</param>
		public GenericPartition(uint index)
		{
			this.Index = index;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="GenericPartition"/> is bootable.
		/// </summary>
		/// <value><c>true</c> if bootable; otherwise, <c>false</c>.</value>
		public bool Bootable { get; set; }

		/// <summary>
		/// Gets the partition index.
		/// </summary>
		/// <value>The partition index.</value>

/* Unmerged change from project 'Mosa.Utility.FileSystem'
Before:
		public uint Index { get { return index; } }
After:
		public uint Index { get; } }
*/
		public uint Index { get; private set; }

		/// <summary>
		/// Gets or sets the start LBA.
		/// </summary>
		/// <value>The start LBA.</value>
		public uint StartLBA { get; set; }

		/// <summary>
		/// Gets the end LBA.
		/// </summary>
		/// <value>The end LBA.</value>
		public uint EndLBA { get { return StartLBA + TotalBlocks; } }

		/// <summary>
		/// Gets or sets the total blocks.
		/// </summary>
		/// <value>The total blocks.</value>
		public uint TotalBlocks { get; set; }

		/// <summary>
		/// Gets or sets the type of the partition.
		/// </summary>
		/// <value>The type of the partition.</value>
		public byte PartitionType { get; set; }

		/// <summary>
		/// Gets or sets the GUID.
		/// </summary>
		/// <value>The GUID.</value>
		public uint[] GUID { get; set; }
	}
}
