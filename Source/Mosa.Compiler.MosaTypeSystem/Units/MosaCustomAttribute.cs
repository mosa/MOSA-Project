/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaCustomAttribute
	{
		public MosaMethod Constructor { get; private set; }

		public object[] Arguments { get; private set; }

		public MosaCustomAttribute(MosaMethod ctor, object[] args)
		{
			Constructor = ctor;
			Arguments = args;
		}
	}
}