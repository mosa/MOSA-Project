/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface for a Disk Controller Device
	/// </summary>
	public interface IDiskControllerDevice
	{
		/// <summary>
		/// Gets the maximun drive count.
		/// </summary>
		/// <value>The drive count.</value>
		uint MaximunDriveCount { get; }

		/// <summary>
		/// Opens the specified drive.
		/// </summary>
		/// <param name="drive">The drive.</param>
		/// <returns></returns>
		bool Open(uint drive);

		/// <summary>
		/// Releases the specified drive.
		/// </summary>
		/// <param name="drive">The drive.</param>
		/// <returns></returns>
		bool Release(uint drive);

		/// <summary>
		/// Reads the block.
		/// </summary>
		/// <param name="drive">The drive.</param>
		/// <param name="block">The block.</param>
		/// <param name="count">The count.</param>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		bool ReadBlock(uint drive, uint block, uint count, byte[] data);

		/// <summary>
		/// Writes the block.
		/// </summary>
		/// <param name="drive">The drive.</param>
		/// <param name="block">The block.</param>
		/// <param name="count">The count.</param>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		bool WriteBlock(uint drive, uint block, uint count, byte[] data);

		/// <summary>
		/// Gets the size of the sector.
		/// </summary>
		/// <param name="drive">The drive.</param>
		/// <returns></returns>
		uint GetSectorSize(uint drive);

		/// <summary>
		/// Gets the total sectors.
		/// </summary>
		/// <param name="drive">The drive.</param>
		/// <returns></returns>
		uint GetTotalSectors(uint drive);

		/// <summary>
		/// Determines whether this instance can write to the specified drive.
		/// </summary>
		/// <param name="drive">The drive.</param>
		/// <returns>
		/// 	<c>true</c> if this instance can write to the specified drive; otherwise, <c>false</c>.
		/// </returns>
		bool CanWrite(uint drive);
	}
}
