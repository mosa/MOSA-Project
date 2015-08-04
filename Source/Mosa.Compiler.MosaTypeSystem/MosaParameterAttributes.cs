// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.MosaTypeSystem
{
	/// <summary>
	/// Parameter Attributes.
	/// </summary>
	[Flags]
	public enum MosaParameterAttributes : ushort
	{
		/// <summary>
		/// Parameter is [In]
		/// </summary>
		In = 0x0001,

		/// <summary>
		/// Parameter is [out]
		/// </summary>
		Out = 0x0002,

		/// <summary>
		/// Parameter is optional
		/// </summary>
		Optional = 0x0010,

		/// <summary>
		/// Parameter has default value.
		/// </summary>
		HasDefault = 0x1000,

		/// <summary>
		/// Parameter has FieldMarshal.
		/// </summary>
		HasFieldMarshal = 0x2000,
	}
}
