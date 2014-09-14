/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

#pragma warning disable 169, 414

namespace Mosa.Internal.Plug
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class MethodAttribute : Attribute
	{
		private string target;
		private string signature = null;

		public MethodAttribute(string target, string signature)
		{
			this.target = target;
			this.signature = signature;
		}

		public MethodAttribute(string target)
		{
			this.target = target;
		}
	}
}