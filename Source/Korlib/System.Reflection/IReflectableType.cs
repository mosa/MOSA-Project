// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Reflection
{
	/// <summary>
	/// Represents a type that you can reflect over.
	/// </summary>
	public interface IReflectableType
	{
		/// <summary>
		/// Retrieves an object that represents this type.
		/// </summary>
		/// <returns>An object that represents this type.</returns>
		TypeInfo GetTypeInfo();
	}
}
