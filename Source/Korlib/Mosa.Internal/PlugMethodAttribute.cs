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
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class PlugMethodAttribute : Attribute
	{
		public string Target;
		public string Signature = null;
	}

}
