// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Reflection
{
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyTitleAttribute : Attribute
	{
		private readonly string title;

		/// <summary>
		/// The assembly title.
		/// </summary>
		public string Title
		{
			get { return title; }
		}

		/// <summary>
		/// Initializes a new instance of the AssemblyTitleAttribute class.
		/// </summary>
		/// <param name="title">The assembly title.</param>
		public AssemblyTitleAttribute(string title)
		{
			this.title = title;
		}
	}
}
