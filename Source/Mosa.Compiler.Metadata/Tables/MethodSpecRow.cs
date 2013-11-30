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
	public class MethodSpecRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MethodSpecRow" />.
		/// </summary>
		/// <param name="method">The method table index of the MethodSpecRow.</param>
		/// <param name="instantiationBlob">The instantiation BLOB.</param>
		public MethodSpecRow(Token method, HeapIndexToken instantiationBlob)
		{
			Method = method;
			InstantiationBlob = instantiationBlob;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the method.
		/// </summary>
		/// <value>
		/// The method.
		/// </value>
		public Token Method { get; private set; }

		/// <summary>
		/// Gets the instantiation BLOB.
		/// </summary>
		/// <value>
		/// The instantiation BLOB.
		/// </value>
		public HeapIndexToken InstantiationBlob { get; private set; }

		#endregion Properties
	}
}