﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// Version
	/// </summary>
	public enum Version : uint
	{
		/// <summary>
		/// Invalid version
		/// </summary>
		None = 0x00,

		/// <summary>
		/// Currrent version
		/// </summary>
		Current = 0x01,
	}
}
