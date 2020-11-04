// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	/// Specifies the usage of another attribute class. This class cannot be inherited.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class AttributeUsageAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AttributeUsageAttribute"/> class with
		/// the specified list of <see cref="AttributeTargets"/>, the <see cref="AllowMultiple"/>
		/// value, and the <see cref="Inherited"/> value.
		/// </summary>
		/// <param name="validOn">The set of values combined using a bitwise OR operation to indicate which
		/// program elements are valid.</param>
		public AttributeUsageAttribute(AttributeTargets validOn)
		{
			this.validOn = validOn;
		}

		private bool allowMultiple = true;

		/// <summary>
		/// Gets or sets a <see cref="Boolean"/> value indicating whether more than one instance of
		/// the indicated attribute can be specified for a single program element.
		/// </summary>
		/// <value><c>true</c> if [allow multiple]; otherwise, <c>false</c>.</value>
		public bool AllowMultiple
		{
			get { return allowMultiple; }
			set { allowMultiple = value; }
		}

		private bool inherited = false;

		/// <summary>
		/// Gets or sets a <see cref="Boolean"/> value indicating whether the indicated attribute can
		/// be inherited by derived classes and overriding members.
		/// </summary>
		/// <value><c>true</c> if inherited; otherwise, <c>false</c>.</value>
		public bool Inherited
		{
			get { return inherited; }
			set { inherited = value; }
		}

		private readonly AttributeTargets validOn;

		/// <summary>
		/// Gets a set of values identifying which program elements that the indicated
		/// attribute can be applied to.
		/// </summary>
		/// <value>The valid on.</value>
		public AttributeTargets ValidOn
		{
			get { return validOn; }
		}
	}
}
