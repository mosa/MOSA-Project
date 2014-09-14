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
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class FieldAttribute : Attribute
	{
		private string target;

		public FieldAttribute(string target)
		{
			this.target = target;
		}
	}
}