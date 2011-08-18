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
	public interface IInterruptHandler
	{
		/// <summary>
		/// Gets the IRQ.
		/// </summary>
		/// <value>The IRQ.</value>
		byte IRQ { get; }
		/// <summary>
		/// Enables this instance.
		/// </summary>
		void Enable();
		/// <summary>
		/// Disables this instance.
		/// </summary>
		void Disable();
	}
}
