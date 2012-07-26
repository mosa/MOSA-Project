/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Runtime.InteropServices;

namespace System
{
	/// <summary>
	/// Implementation of the "System.Delegate" class.
	/// </summary>
	public class Delegate
	{
		protected uint methodPointer = 0;
		protected object instance = null;

		internal Delegate()
		{
		}

		internal Delegate(object instance, uint methodPointer)
		{
			this.instance = instance;
			this.methodPointer = methodPointer;
		}

	}
}
