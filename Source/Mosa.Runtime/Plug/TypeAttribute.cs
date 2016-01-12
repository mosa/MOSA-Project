// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

#pragma warning disable 169, 414

namespace Mosa.Runtime.Plug
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class TypeAttribute : Attribute
	{
		private string target;

		public TypeAttribute(string target)
		{
			this.target = target;
		}
	}
}
