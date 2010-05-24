/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System
{
	/// <summary>
	/// 
	/// </summary>
	public struct Int16
	{
		public const short MaxValue = 32767;
		public const short MinValue = -32768;

		internal short m_value;

		public override string ToString()
		{
			return Int32.CreateString((uint)m_value, true, false);
		}

	}
}
