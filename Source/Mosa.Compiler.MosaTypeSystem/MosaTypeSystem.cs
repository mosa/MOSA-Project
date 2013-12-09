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
	public class MosaTypeSystem
	{
		private List<MosaType> Types = new List<MosaType>();

		public void AddType(MosaType type)
		{
			Types.Add(type);
		}
	}
}