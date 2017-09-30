// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.x86.Smbios
{
	/// <summary>
	/// Cpu Structure
	/// </summary>
	public class CpuStructure : SmbiosStructure
	{
		/// <summary>
		/// The vendor
		/// </summary>
		private string vendor = null;

		/// <summary>
		/// The version
		/// </summary>
		private string version = null;

		/// <summary>
		/// The maximum speed
		/// </summary>
		private uint maxSpeed = 0;

		/// <summary>
		/// The socket
		/// </summary>
		private string socket = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="CpuStructure"/> class.
		/// </summary>
		public CpuStructure()
			: base(SmbiosManager.GetStructureOfType(0x04))
		{
			version = GetStringFromIndex(Intrinsic.Load8(address, 0x10u));
			socket = GetStringFromIndex(Intrinsic.Load8(address, 0x04u));
			maxSpeed = Intrinsic.Load16(address, 0x16u);
			vendor = GetStringFromIndex(Intrinsic.Load8(address, 0x07u));
		}

		/// <summary>
		/// Gets the maximum speed.
		/// </summary>
		/// <value>
		/// The maximum speed.
		/// </value>
		public uint MaxSpeed
		{
			get { return maxSpeed; }
		}

		/// <summary>
		/// Gets the socket.
		/// </summary>
		/// <value>
		/// The socket.
		/// </value>
		public string Socket
		{
			get { return socket; }
		}

		/// <summary>
		/// Gets the vendor.
		/// </summary>
		/// <value>
		/// The vendor.
		/// </value>
		public string Vendor
		{
			get { return vendor; }
		}

		/// <summary>
		/// Gets the version.
		/// </summary>
		/// <value>
		/// The version.
		/// </value>
		public string Version
		{
			get { return version; }
		}
	}
}
