/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.EmulatedKernel
{
	/// <summary>
	/// 
	/// </summary>
	public class PCIDevice
	{
		/// <summary>
		/// 
		/// </summary>
		protected byte[] config;

		/// <summary>
		/// 
		/// </summary>
		protected uint[] barMasks;

		/// <summary>
		/// Initializes a new instance of the <see cref="PCIDevice"/> class.
		/// </summary>
		public PCIDevice()
		{
			config = new byte[256];
			barMasks = new uint[6];

			for (int i = 0; i < 6; i++)
				barMasks[i] = 0xFFFFFFFF;
		}

		/// <summary>
		/// Reads the configuration
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public byte ReadConfig8(byte index)
		{
			return config[index];
		}

		/// <summary>
		/// Reads the configuration
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public ushort ReadConfig16(byte index)
		{
			return (ushort)((config[index] | (config[index + 1] << 8)));
		}

		/// <summary>
		/// Reads the configuration
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public uint ReadConfig32(byte index)
		{
			return (uint)((config[index] | (config[index + 1] << 8) | (config[index + 2] << 16) | (config[index + 3] << 24)));
		}

		/// <summary>
		/// Writes the configuration
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="data">The data.</param>
		public void WriteConfig8(byte index, byte data)
		{
			config[index] = data;
		}

		/// <summary>
		/// Writes the configuration
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="data">The data.</param>
		public void WriteConfig16(byte index, ushort data)
		{
			config[index] = (byte)(data & 0xFF);
			config[index + 1] = (byte)((data >> 8) & 0xFF);
		}

		/// <summary>
		/// Writes the configuration
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="data">The data.</param>
		public void WriteConfig32(byte index, uint data)
		{
			config[index] = (byte)(data & 0xFF);
			config[index + 1] = (byte)((data >> 8) & 0xFF);
			config[index + 2] = (byte)((data >> 16) & 0xFF);
			config[index + 3] = (byte)((data >> 24) & 0xFF);
		}

		/// <summary>
		/// Gets or sets the vendor ID.
		/// </summary>
		/// <value>The vendor ID.</value>
		public ushort VendorID
		{
			get { return ReadConfig16(0x00); }
			set { WriteConfig16(0x00, value); }
		}

		/// <summary>
		/// Gets or sets the device ID.
		/// </summary>
		/// <value>The device ID.</value>
		public ushort DeviceID
		{
			get { return ReadConfig16(0x02); }
			set { WriteConfig16(0x02, value); }
		}

		/// <summary>
		/// Gets or sets the revision.
		/// </summary>
		/// <value>The revision.</value>
		public byte Revision
		{
			get { return ReadConfig8(0x08); }
			set { WriteConfig8(0x08, value); }
		}

		/// <summary>
		/// Gets or sets the class code.
		/// </summary>
		/// <value>The class code.</value>
		public byte ClassCode
		{
			get { return ReadConfig8(0x0A); }
			set { WriteConfig8(0x0A, value); }
		}

		/// <summary>
		/// Gets or sets the sub class code.
		/// </summary>
		/// <value>The revision.</value>
		public byte SubClassCode
		{
			get { return ReadConfig8(0x09); }
			set { WriteConfig8(0x09, value); }
		}

		/// <summary>
		/// Gets or sets the sub vendor ID.
		/// </summary>
		/// <value>The sub vendor ID.</value>
		public ushort SubVendorID
		{
			get { return ReadConfig16(0x2C); }
			set { WriteConfig16(0x2C, value); }
		}

		/// <summary>
		/// Gets the sub device ID.
		/// </summary>
		/// <value>The sub device ID.</value>
		public ushort SubDeviceID
		{
			get { return ReadConfig16(0x2E); }
			set { WriteConfig16(0x0C, value); }
		}

		/// <summary>
		/// Gets or sets the IRQ.
		/// </summary>
		/// <value>The IRQ.</value>
		public byte IRQ
		{
			get { return ReadConfig8(0x3F); }
			set { WriteConfig8(0x3F, value); }
		}

		/// <summary>
		/// Gets or sets the type of the header.
		/// </summary>
		/// <value>The type of the header.</value>
		public byte HeaderType
		{
			get { return ReadConfig8(0x0E); }
			set { WriteConfig8(0x0E, value); }
		}

		/// <summary>
		/// Gets the base address region.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public uint GetBaseAddressRegion(byte index)
		{
			return ReadConfig32((byte)(0x10 + index * 4));
		}

		/// <summary>
		/// Sets the base adddress region.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="data">The data.</param>
		public void SetBaseAdddressRegion(byte index, uint data)
		{
			WriteConfig32((byte)(0x10 + index * 4), data);
		}

		/// <summary>
		/// Sets the bar mask.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="mask">The mask.</param>
		public void SetBarMask(byte index, uint mask)
		{
			barMasks[index] = mask;
		}

		/// <summary>
		/// Gets the bar mask.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public uint GetBarMask(byte index)
		{
			return barMasks[index];
		}

		/// <summary>
		/// Determines whether [is read only] [the specified index].
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>
		/// 	<c>true</c> if [is read only] [the specified index]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsReadOnly(byte index)
		{
			if (index < 0x04) return true;
			if ((index >= 0x6) && (index <= 0x07)) return true;
			if ((index >= 0x8) && (index <= 0x0B)) return true;
			if (index == 0x0F) return true;
			if ((index >= 0x2C) && (index <= 0x2F)) return true;
			if (index >= 0x3D) return true;

			return false;
		}

		/// <summary>
		/// Gets the write mask.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public uint GetWriteMask32(byte index)
		{
			// Must be 32-bit aligned, otherwise no read-only attributes are applied
			if (index % 4 != 0)
				return 0xFFFFFFFF;

			uint mask = 0x00;

			if (IsReadOnly((byte)(index + 0))) mask = mask | 0x000000FF;
			if (IsReadOnly((byte)(index + 1))) mask = mask | 0x0000FF00;
			if (IsReadOnly((byte)(index + 2))) mask = mask | 0x00FF0000;
			if (IsReadOnly((byte)(index + 3))) mask = mask | 0xFF000000;

			return mask;
		}
	}
}
