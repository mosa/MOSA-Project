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
	static public class PortRange
	{
		/// <summary>
		/// Gets the port list.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		static public ushort[] GetPortList(ushort start, ushort count)
		{
			ushort[] list = new ushort[count];

			for (int i = 0; i < count; i++)
				list[i] = (ushort)(start + i);

			return list;
		}
	}
}
