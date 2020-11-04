// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Runtime.InteropServices
{
	/// <summary>
	/// Implementation of the "System.Runtime.InteropServices.ComVisibleAttribute" class
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class
		| AttributeTargets.Struct | AttributeTargets.Enum |
		AttributeTargets.Method | AttributeTargets.Property |
		AttributeTargets.Field | AttributeTargets.Interface |
		AttributeTargets.Delegate, Inherited = false)]
	[ComVisible(true)]
	public sealed class ComVisibleAttribute : Attribute
	{
		private readonly bool Visible = false;

		/// <summary>
		///
		/// </summary>
		public ComVisibleAttribute(bool visibility)
		{
			Visible = visibility;
		}

		/// <summary>
		/// Value
		/// </summary>
		public bool Value
		{
			get { return Visible; }
		}
	}
}
