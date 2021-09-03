// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace System.Security
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	public sealed class SecurityCriticalAttribute : Attribute
	{
		private SecurityCriticalScope _val;

		public SecurityCriticalAttribute()
		{
		}

		public SecurityCriticalAttribute(SecurityCriticalScope scope) => this._val = scope;

		[Obsolete("SecurityCriticalScope is only used for .NET 2.0 transparency compatibility.")]
		public SecurityCriticalScope Scope => this._val;
	}
}
