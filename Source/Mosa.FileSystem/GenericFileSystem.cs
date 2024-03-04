﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Disks;
using Mosa.FileSystem.VFS;

namespace Mosa.FileSystem;

/// <summary>
/// Generic File System
/// </summary>
public abstract class GenericFileSystem
{
	/// <summary>
	/// The partition
	/// </summary>
	protected IPartitionDevice partition;

	/// <summary>
	/// Gets or sets the size of the block.
	/// </summary>
	/// <value>
	/// The size of the block.
	/// </value>
	public uint BlockSize { get; protected set; }

	/// <summary>
	/// Returns true if ... is valid.
	/// </summary>
	/// <value>
	///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
	/// </value>
	public bool IsValid { get; protected set; }

	/// <summary>
	/// Gets the volume label.
	/// </summary>
	/// <value>The volume label.</value>
	public string VolumeLabel { get; protected set; }

	/// <summary>
	/// Gets the serial number.
	/// </summary>
	/// <value>The serial number.</value>
	public byte[] SerialNumber { get; protected set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="GenericFileSystem"/> class.
	/// </summary>
	/// <param name="partition">The partition.</param>
	public GenericFileSystem(IPartitionDevice partition)
	{
		this.partition = partition;
		BlockSize = partition.BlockSize;
		IsValid = false;
		VolumeLabel = string.Empty;
		SerialNumber = new byte[0];
	}

	/// <summary>
	/// Creates the VFS mount.
	/// </summary>
	/// <returns></returns>
	public abstract IFileSystem CreateVFSMount();
}
