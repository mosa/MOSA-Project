/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Metadata.Tables
{
	/// <summary>
	///
	/// </summary>
	public class InterfaceImplRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="InterfaceImplRow"/> struct.
		/// </summary>
		/// <param name="class">The @class.</param>
		/// <param name="interface">The @interface.</param>
		public InterfaceImplRow(Token @class, Token @interface)
		{
			Class = @class;
			Interface = @interface;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the class.
		/// </summary>
		/// <value>The class.</value>
		public Token Class { get; private set; }

		/// <summary>
		/// Gets the interface.
		/// </summary>
		/// <value>The interface.</value>
		public Token Interface { get; private set; }

		#endregion Properties
	}
}