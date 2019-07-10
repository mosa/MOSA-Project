// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Diagnostics;

namespace Mosa.Workspace.Kernel.Internal
{
	public class PageFaultException : Exception
	{
		public ulong Address { get; private set; }

		public PageFaultException(ulong address)
		{
			Address = address;
		}

		public override string ToString()
		{
			return "Page Fault at 0x" + Address.ToString("X");
		}
	}
}
