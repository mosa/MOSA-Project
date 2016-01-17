// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			get { return description; }
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
