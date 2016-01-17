// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86.Smbios
{
	/// <summary>
	///
	/// </summary>
	public class CpuStructure : SmbiosStructure
	{
		/// <summary>
		///
		/// </summary>
		private string vendor = null;

		/// <summary>
		///
		/// </summary>
		private string version = null;

		/// <summary>
		///
		/// </summary>
		private uint maxSpeed = 0;

		/// <summary>
		///
		/// </summary>
		private string socket = null;

		/// <summary>
		///
		/// </summary>
		public CpuStructure()
			: base(SmbiosManager.GetStructureOfType(0x04))
		{
			version = GetStringFromIndex(Native.Get8(address + 0x10u));
			socket = GetStringFromIndex(Native.Get8(address + 0x04u));
			maxSpeed = Native.Get16(address + 0x16u);
			vendor = GetStringFromIndex(Native.Get8(address + 0x07u));
		}

		/// <summary>
		///
		/// </summary>
		public uint MaxSpeed
		{
			get { return maxSpeed; }
		}

		/// <summary>
		///
		/// </summary>
		public string Socket
		{
			get { return socket; }
		}

		/// <summary>
		///
		/// </summary>
		public string Vendor
		{
			get { return vendor; }
		}

		/// <summary>
		///
		/// </summary>
		public string Version
		{
			get { return version; }
		}
	}
}
