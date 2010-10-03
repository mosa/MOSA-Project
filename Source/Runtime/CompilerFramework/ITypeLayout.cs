/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{

	/// <summary>
	/// </summary>
	public interface ITypeLayout
	{
		int GetMethodTableOffset(RuntimeMethod method);

		int GetInterfaceSlotOffset(RuntimeType type);
	}
}
