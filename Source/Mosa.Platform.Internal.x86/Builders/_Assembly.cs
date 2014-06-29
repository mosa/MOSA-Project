/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;
using System.Runtime.InteropServices;
using System.Reflection;

namespace System
{
	public sealed unsafe class _Assembly : Assembly
	{
		private uint* m_pointer;

		internal _Assembly(uint* pointer)
		{
			this.m_pointer = pointer;
		}
	}
}