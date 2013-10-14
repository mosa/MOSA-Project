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
	public interface IHardwareDevice
	{
		/// <summary>
		/// Initializes this instance.
		/// </summary>
		/// <returns></returns>
		bool Initialize();

		/// <summary>
		/// Resets this instance.
		/// </summary>
		void Reset();
	}
}