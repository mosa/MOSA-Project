/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Internal.Plug
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class PlugMethodAttribute : Attribute
	{
		private string target;
		private string signature = null;

		public PlugMethodAttribute(string target, string signature)
		{
			this.target = target;
			this.signature = signature;
		}

		public PlugMethodAttribute(string target)
		{
			this.target = target;
		}
	}
}