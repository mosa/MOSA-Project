/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.ClassLib
{
	public class Pair<F, S>
	{
		public F First;
		public S Second;

		public Pair(F first, S second)
		{
			this.First = first;
			this.Second = second;
		}
	}
}
