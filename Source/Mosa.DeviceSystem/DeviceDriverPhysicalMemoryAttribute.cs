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
	/// 
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.All | System.AttributeTargets.Property, AllowMultiple = true)]
	public class DeviceDriverPhysicalMemoryAttribute : System.Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		public uint MemorySize = 0;
		/// <summary>
		/// 
		/// </summary>
		public uint MemoryAlignment = 1;
		/// <summary>
		/// 
		/// </summary>
		public bool RestrictUnder16M = false;
		/// <summary>
		/// 
		/// </summary>
		public bool RestrictUnder4G = false;
	}

}
