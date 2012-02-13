/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Internal
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class PlugTypeAttribute : Attribute
	{
		private string target;

		public PlugTypeAttribute(string target)
		{
			this.target = target;
		}
	}

}
