// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>
	/// Provides custom attributes for reflection objects that support them.
	/// </summary>
	[ComVisibleAttribute(true)]
	public interface ICustomAttributeProvider
	{
		/// <summary>
		/// Returns an array of all of the custom attributes defined on this member, excluding named attributes, or an empty array if there are no custom attributes.
		/// </summary>
		/// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attribute.</param>
		/// <returns>An array of Objects representing custom attributes, or an empty array.</returns>
		object[] GetCustomAttributes(bool inherit);

		/// <summary>
		/// Returns an array of custom attributes defined on this member, identified by type, or an empty array if there are no custom attributes of that type.
		/// </summary>
		/// <param name="attributeType">The type of the custom attributes.</param>
		/// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attribute.</param>
		/// <returns>An array of Objects representing custom attributes, or an empty array.</returns>
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		/// <summary>
		/// Indicates whether one or more instance of attributeType is defined on this member.
		/// </summary>
		/// <param name="attributeType">The type of the custom attributes.</param>
		/// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attribute.</param>
		/// <returns><c>true</c> if the attributeType is defined on this member; <c>false</c> otherwise.</returns>
		bool IsDefined(Type attributeType, bool inherit);
	}
}
