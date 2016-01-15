// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

#pragma warning disable 169, 414

namespace Mosa.Runtime.Plug
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
