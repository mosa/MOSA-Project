// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.GDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosa.Tool.GDBDebugger.GDB
{
	public class Register
	{
		public RegisterDefinition Definition { get; private set; }

		public string Name { get { return Definition.Name; } }

		public int Size { get { return Definition.Size; } }

		public ulong Value { get; private set; }
		public ulong ValueExtended { get; private set; }

		public Register(RegisterDefinition definition, ulong value)
		{
			definition = Definition;
			Value = value;
		}
	}
}
