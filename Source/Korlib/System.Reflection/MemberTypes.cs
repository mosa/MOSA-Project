// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Reflection
{
	[Flags]
	public enum MemberTypes
	{
		/// <summary>
		/// </summary>
		Constructor = 0x01,

		/// <summary>
		/// </summary>
		Event = 0x02,

		/// <summary>
		/// </summary>
		Field = 0x04,

		/// <summary>
		/// </summary>
		Method = 0x08,

		/// <summary>
		/// </summary>
		Property = 0x10,

		/// <summary>
		/// </summary>
		TypeInfo = 0x20,

		/// <summary>
		/// </summary>
		Custom = 0x40,

		/// <summary>
		/// </summary>
		NestedType = 0x80,

		/// <summary>
		/// </summary>
		All = 0xBF
	}
}
