/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers.Kernel;

namespace Mosa.DeviceDrivers
{
    /// <summary>
    /// 
    /// </summary>
	public class IOPortResources
	{
		// This array represents a bit map of IO ports which should not be
		// used by Plug-and-Play (PnP) systems, such as for ISA and PCI
        /// <summary>
        /// 
        /// </summary>
		public byte[] reservedForLegacyISADevices;

		// All legacy ISA cards occupy the IO region from 0x0100 through 0x3FF
        /// <summary>
        /// 
        /// </summary>
		public const ushort StartLegacyISAPort = 0x0100;
        /// <summary>
        /// 
        /// </summary>
		public const ushort EndLegacyISAPort = 0x3FF;
        /// <summary>
        /// 
        /// </summary>
		public const ushort Size = 768; // EndLegacyISAPort - StartLegacyISAPort


        /// <summary>
        /// Initializes a new instance of the <see cref="IOPortResources"/> class.
        /// </summary>
		public IOPortResources()
		{
			reservedForLegacyISADevices = new byte[Size / 8];
			ReserveDefaultLegacyISADevicePorts();
		}

		/// <summary>
		/// Determines whether the specific port in ISA legacy region.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <returns>
		/// 	<c>true</c> if true if specific port in ISA legacy region; otherwise, <c>false</c>.
		/// </returns>
		public bool IsPortInISALegacyRegion(ushort port)
		{
			return ((port >= StartLegacyISAPort) && (port <= EndLegacyISAPort));
		}

		/// <summary>
		/// Reserves for ISA legacy devices.  Can be used by Plug-and-Play (PnP) 
		/// systems, either ISA or PCI, to avoid IO ports used by legacy ISA 
		/// devices. 
		/// </summary>
		/// <param name="port">The port.</param>
		public void ReserveForISALegacyDevices(ushort port)
		{
			if (!IsPortInISALegacyRegion(port))
				return; // can not be a legacy ISA port, ignore it

			ushort location = (ushort)((port - StartLegacyISAPort) / 8);
			reservedForLegacyISADevices[location] = (byte)(reservedForLegacyISADevices[location] | (1 << (port % 8)));
		}

		/// <summary>
		/// Reserves for ISA legacy devices. Can be initialized by PnP aware BIOS. 
		/// Can be used by Plug-and-Play (PnP) systems, either ISA or PCI, to avoid 
		/// IO ports used by legacy ISA devices
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="size">The size.</param>
		public void ReserveForISALegacyDevices(ushort port, ushort size)
		{
			for (int i = 0; i < size; i++)
				ReserveForISALegacyDevices((ushort)(port + i));
		}

		/// <summary>
		/// Determines whether port is reserved for ISA legacy devices.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <returns>
		/// 	<c>true</c> if the specified port is reserved for ISAlegacy devices; otherwise, <c>false</c>.
		/// </returns>
		public bool IsReservedForISALegacyDevices(ushort port)
		{
			if ((port < StartLegacyISAPort) || (port > EndLegacyISAPort))
				return false;

			byte value = reservedForLegacyISADevices[(port - StartLegacyISAPort) / 8];

			return (value & (byte)(1 << (port % 8))) != 0;
		}

        /// <summary>
        /// Gets the IO port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
		public IReadWriteIOPort GetIOPort(ushort port, ushort offset)
		{
			return HAL.RequestIOPort((ushort)(port + offset));
		}

        /// <summary>
        /// Reserves the default legacy ISA device ports.
        /// </summary>
		protected void ReserveDefaultLegacyISADevicePorts()
		{
			// A list of IO ports to avoid for PnP purposes
			ReserveForISALegacyDevices(0x0CF8, 4); // PCI Controller
			ReserveForISALegacyDevices(0x01F0, 8); // 1st Fixed Harddisk
			ReserveForISALegacyDevices(0x0170, 8); // 2nd Fixed Harddisk
			ReserveForISALegacyDevices(0x0274, 8); // ISA PnP I/O port
			ReserveForISALegacyDevices(0x03F0, 8); // Primary Floppy
			ReserveForISALegacyDevices(0x0370, 6); // Secondary Floppy
			ReserveForISALegacyDevices(0x03F8, 8); // COM1
			ReserveForISALegacyDevices(0x02F8, 8); // COM2
			ReserveForISALegacyDevices(0x02EA, 16); // 8514 Controller
			ReserveForISALegacyDevices(0x03B0, 16); // Mono/EGA/VGA
			ReserveForISALegacyDevices(0x03C0, 16); // Mono/EGA/VGA
		}
	}
}
