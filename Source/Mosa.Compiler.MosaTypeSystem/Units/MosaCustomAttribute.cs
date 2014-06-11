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
		public MosaMethod Constructor { get; private set; }

		public Tuple<object, MosaTypeCode>[] Arguments { get; private set; }

		public Tuple<string, bool, object, MosaTypeCode>[] NamedArguments { get; private set; }

		public MosaCustomAttribute(MosaMethod ctor, Tuple<object, MosaTypeCode>[] args, Tuple<string, bool, object, MosaTypeCode>[] namedArgs)
		{
			Constructor = ctor;
			Arguments = args;
			NamedArguments = namedArgs;
		}
	}
}