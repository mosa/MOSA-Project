/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;

namespace Mosa.Compiler.MosaTypeSystem
{
	/// <summary>
	/// Property Attributes.
	/// </summary>
	[Flags]
	public enum MosaPropertyAttributes : ushort
	{
		/// <summary>
		/// Property is special. Name describes how.
		/// </summary>
		SpecialName = 0x0200,
		/// <summary>
		/// Runtime(metadata internal APIs) should check name encoding.
		/// </summary>
		RTSpecialName = 0x0400,
		/// <summary>
		/// Property has default
		/// </summary>
		HasDefault = 0x1000,
	}
}