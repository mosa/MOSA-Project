// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.Workspace.FileSystem.Debug.Synthetic
{
	/// <summary>
	/// Emulates a ram disk device
	/// </summary>
	/// <seealso cref="Mosa.DeviceSystem.BaseDeviceDriver" />
	/// <seealso cref="Mosa.DeviceSystem.IDiskDevice" />
	public class RamDiskDevice : BaseDeviceDriver, IDiskDevice
	{
		/// <summary>
		/// The total blocks
		/// </summary>
		public uint TotalBlocks { get; }

		/// <summary>
		/// The memory
		/// </summary>
		protected byte[] mem;

		/// <summary>
		/// Initializes a new instance of the <see cref="RamDiskDevice" /> class.
		/// </summary>
		/// <param name="blocks">The blocks.</param>
		public RamDiskDevice(uint blocks)
		{
			TotalBlocks = blocks;
			mem = new byte[blocks * 512];
		}

		public override void Initialize()
		{
			Device.Name = "RamDiskDevice_" + ((TotalBlocks * 512) / (1024 * 1024)).ToString() + "Mb";
		}

		public override void Probe() => Device.Status = DeviceStatus.Available;

		public override void Start()
		{
			Device.Status = DeviceStatus.Online;
		}

		public override bool OnInterrupt() => true;

		/// <summary>
		/// Gets a value indicating whether this instance can write.
		/// </summary>
		/// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
		public bool CanWrite { get { return true; } }

		/// <summary>
		/// Gets the size of the block.
		/// </summary>
		/// <value>The size of the block.</value>
		public uint BlockSize { get { return 512; } }

		/// <summary>
		/// Reads the block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public byte[] ReadBlock(uint block, uint count)
		{
			var data = new byte[512];
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
			for (int i = 0; i < 512; i++)
				data[i] = mem[(block * 512) + i];

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
			for (int i = 0; i < 512; i++)
				mem[(block * 512) + i] = data[i];
			return true;
		}
	}
}
