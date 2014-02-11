/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaAttribute
	{
		public MosaMethod CtorMethod { get; internal set; }

		//public byte[] Blob { get; internal set; }

		public IList<object> Values { get; internal set; }

		public MosaAttribute()
		{
			Values = new List<object>();
		}

		public override string ToString()
		{
			return "Ctor: " + CtorMethod.ToString();
		}
	}
}