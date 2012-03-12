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
	/// Interface to a PCI Controller
	/// </summary>
	public interface IPCIController
	{
		/// <summary>
		/// Reads from configuration space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <returns></returns>
		uint ReadConfig32(byte bus, byte slot, byte function, byte register);
		/// <summary>
		/// Reads from configuration space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <returns></returns>
		ushort ReadConfig16(byte bus, byte slot, byte function, byte register);
		/// <summary>
		/// Reads from configuration space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <returns></returns>
		byte ReadConfig8(byte bus, byte slot, byte function, byte register);
		/// <summary>
		/// Writes to configuration space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <param name="value">The value.</param>
		void WriteConfig32(byte bus, byte slot, byte function, byte register, uint value);
		/// <summary>
		/// Writes to configuration space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <param name="value">The value.</param>
		void WriteConfig16(byte bus, byte slot, byte function, byte register, ushort value);
		/// <summary>
		/// Writes to configuration space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <param name="value">The value.</param>
		void WriteConfig8(byte bus, byte slot, byte function, byte register, byte value);
	}
}
