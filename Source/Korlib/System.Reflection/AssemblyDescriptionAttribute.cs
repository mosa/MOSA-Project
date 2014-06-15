/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace System.Reflection
{
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyDescriptionAttribute : Attribute
	{
		private readonly string description;

		/// <summary>
		/// The assembly description.
		/// </summary>
		public string Description
		{
			get { return this.description; }
		}

		/// <summary>
		/// Initializes a new instance of the AssemblyDescriptionAttribute class.
		/// </summary>
		/// <param name="description">The assembly description.</param>
		public AssemblyDescriptionAttribute(string description)
		{
			this.description = description;
		}
	}
}