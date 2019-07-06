// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Diagnostics;

namespace Mosa.Workspace.Kernel.Internal.Debug
{
	public class InvalidMemoryAccess : Exception
	{
		public ulong Address { get; private set; }

		public InvalidMemoryAccess(ulong address)
		{
			Address = address;
		}

		public override string ToString()
		{
			return "Invalid Memory Access at 0x" + Address.ToString("X");
		}
	}
}
