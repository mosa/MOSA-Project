// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// MAC Address
	/// </summary>
	public class MACAddress
	{
		/// <summary>
		///
		/// </summary>
		protected byte[] address;

		/// <summary>
		/// Initializes a new instance of the <see cref="MACAddress"/> class.
		/// </summary>
		/// <param name="address">The address.</param>
		public MACAddress(byte[] address)
		{
			if ((address == null) || (address.Length != 6))
				throw new System.Exception("Invalid MAC address");

			this.address = new byte[6];

			for (int i = 0; i < 6; i++)
				this.address[i] = address[i];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MACAddress"/> class.
		/// </summary>
		/// <param name="mac">The mac.</param>
		public MACAddress(MACAddress mac)
			: this(mac.address)
		{
		}

		/// <summary>
		/// Compares to.
		/// </summary>
		/// <param name="mac">The mac.</param>
		/// <returns></returns>
		public bool CompareTo(MACAddress mac)
		{
			//if ((mac.address == null) || (this.address == null)) return false;

			for (int i = 0; i < 6; i++)
				if (mac.address[i] != address[i])
					return false;

			return true;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return address[0].ToString("X") + ':' + address[1].ToString("X") + ':' + address[2].ToString("X") + ':' +
				address[3].ToString("X") + ':' + address[4].ToString("X") + ':' + address[5].ToString("X");
		}
	}
}
