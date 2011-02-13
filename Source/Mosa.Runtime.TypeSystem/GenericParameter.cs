/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.TypeSystem
{
	/// <summary>
	/// 
	/// </summary>
	public class GenericParameter
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly GenericParamAttributes flags;

		/// <summary>
		/// 
		/// </summary>
		private readonly string name;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericParameter"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="flags">The flags.</param>
		public GenericParameter(string name, GenericParamAttributes flags)
		{
			this.name = name;
			this.flags = flags;
		}

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public GenericParamAttributes Flags { get { return flags; } }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get { return name; } }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return name;
		}

	}
}
