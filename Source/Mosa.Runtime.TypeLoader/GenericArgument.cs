/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Runtime.TypeLoader
{
	/// <summary>
	/// 
	/// </summary>
	public struct GenericArgument
	{
		/// <summary>
		/// 
		/// </summary>
		private int typeHandle;

		/// <summary>
		/// Gets or sets the type handle.
		/// </summary>
		/// <value>The type handle.</value>
		public int TypeHandle
		{
			get { return typeHandle; }
			set { typeHandle = value; }
		}
	}
}
