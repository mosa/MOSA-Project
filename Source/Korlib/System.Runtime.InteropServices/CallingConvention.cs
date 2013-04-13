/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System.Runtime.InteropServices
{
	[Serializable]
	[ComVisible(true)]
	public enum CallingConvention
	{
		Winapi = 1,
		Cdecl = 2,
		StdCall = 3,
		ThisCall = 4,
		FastCall = 5,
	}
}