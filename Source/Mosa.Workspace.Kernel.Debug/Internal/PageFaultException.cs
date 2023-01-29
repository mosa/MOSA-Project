// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

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
