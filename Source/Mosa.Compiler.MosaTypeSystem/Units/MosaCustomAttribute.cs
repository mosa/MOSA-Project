/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using System.Collections.Generic;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaCustomAttribute
	{
		public struct Argument
		{
			public Argument(MosaType type, object value)
			{
				this.Type = type;
				this.Value = value;
			}

			public readonly MosaType Type;
			public readonly object Value;
		}

		public class NamedArgument
		{
			public NamedArgument(string name, bool isField, Argument arg)
			{
				this.Name = name;
				this.IsField = isField;
				this.Argument = arg;
			}

			public string Name { get; private set; }
			public bool IsField { get; private set; }
			public Argument Argument { get; set; }
		}

		public MosaMethod Constructor { get; private set; }

		public Argument[] Arguments { get; private set; }

		public NamedArgument[] NamedArguments { get; private set; }

		public MosaCustomAttribute(MosaMethod ctor, Argument[] args, NamedArgument[] namedArgs)
		{
			Constructor = ctor;
			Arguments = args;
			NamedArguments = namedArgs;
		}
	}
}