/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System.Security
{
	[AttributeUsage(AttributeTargets.Module, AllowMultiple = true, Inherited = false)]
	public sealed class UnverifiableCodeAttribute : System.Attribute
	{
	}
}