/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System
{
	/// <summary>
	/// Indicates that an enumeration can be treated as a bit field; that is, a set of flags.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Enum, Inherited = false)]
	public class FlagsAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the System.FlagsAttribute class.
		/// </summary>
		public FlagsAttribute()
		{
		}
	}
}