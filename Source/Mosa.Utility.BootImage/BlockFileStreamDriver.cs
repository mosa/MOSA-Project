// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Disks;
using Mosa.DeviceSystem.Framework;

namespace Mosa.Utility.BootImage;

/// <summary>
/// Block File Stream Driver
/// </summary>
/// <seealso cref="BaseDeviceDriver" />
/// <seealso cref="IDiskDevice" />
public class BlockFileStreamDriver : BaseDeviceDriver, IDiskDevice, IDisposable
{
	public string Filename { get; }

	/// <summary>
	/// The disk file
	/// </summary>
	protected FileStream diskFile;

	/// <summary>
	/// The block offset
	/// </summary>
	public uint BlockOffset = 0;

	/// <summary>
	/// Initializes a new instance of the <see cref="BlockFileStreamDriver" /> class.
	/// </summary>
	/// <param name="filename">The filename.</param>
	public BlockFileStreamDriver(string filename)
	{
		Filename = filename;
		diskFile = new FileStream(Filename, FileMode.OpenOrCreate);
	}

	public override void Initialize()
	{
		Device.Name = "DiskDevice_" + Path.GetFileName(Filename);
	}

	public override void Probe() => Device.Status = DeviceStatus.Available;

	public override void Start()
	{
		Device.Status = DeviceStatus.Online;
	}

	public override bool OnInterrupt() => true;

	/// <summary>
	/// Releases unmanaged and - optionally - managed resources
	/// </summary>
	public void Dispose()
	{
		diskFile.Close();
	}

	/// <summary>
	/// Gets a value indicating whether this instance can write.
	/// </summary>
	/// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
	public bool CanWrite => true;

	/// <summary>
	/// Gets the total blocks.
	/// </summary>
	/// <value>The total blocks.</value>
	public uint TotalBlocks => (uint)(diskFile.Length / 512);

	/// <summary>
	/// Gets the size of the block.
	/// </summary>
	/// <value>The size of the block.</value>
	public uint BlockSize => 512;

	/// <summary>
	/// Reads the block.
	/// </summary>
	/// <param name="block">The block.</param>
	/// <param name="count">The count.</param>
	/// <returns></returns>
	public byte[] ReadBlock(uint block, uint count)
	{
		var data = new byte[count * 512];
		ReadBlock(block, count, data);
		return data;
	}

	/// <summary>
	/// Reads the block.
	/// </summary>
	/// <param name="block">The block.</param>
	/// <param name="count">The count.</param>
	/// <param name="data">The data.</param>
	/// <returns></returns>
	public bool ReadBlock(uint block, uint count, byte[] data)
	{
		diskFile.Seek((block + BlockOffset) * 512, SeekOrigin.Begin);
		diskFile.Read(data, 0, (int)(count * 512));
		return true;
	}

	/// <summary>
	/// Writes the block.
	/// </summary>
	/// <param name="block">The block.</param>
	/// <param name="count">The count.</param>
	/// <param name="data">The data.</param>
	/// <returns></returns>
	public bool WriteBlock(uint block, uint count, byte[] data)
	{
		diskFile.Seek((block + BlockOffset) * 512, SeekOrigin.Begin);
		diskFile.Write(data, 0, (int)(count * 512));
		return true;
	}
}
