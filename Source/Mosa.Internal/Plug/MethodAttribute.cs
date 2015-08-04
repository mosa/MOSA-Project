// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
