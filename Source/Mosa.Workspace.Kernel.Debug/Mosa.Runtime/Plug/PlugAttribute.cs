// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

#pragma warning disable 169, 414

namespace Mosa.Runtime.Plug
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class PlugAttribute : Attribute
	{
		private readonly string target;

		public PlugAttribute(string target)
		{
			this.target = target;
		}
	}
}
