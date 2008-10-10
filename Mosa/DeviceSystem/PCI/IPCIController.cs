/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	/// Interface to a PCI Controller
	/// </summary>
	public interface IPCIController
	{
		/// <summary>
		/// Reads from configuraton space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <returns></returns>
		uint ReadConfig(byte bus, byte slot, byte function, byte register);
		
		/// <summary>
		/// Writes to configuraton space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <param name="value">The value.</param>
		void WriteConfig(byte bus, byte slot, byte function, byte register, uint value);
	}
}
