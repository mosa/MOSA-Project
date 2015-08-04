// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	/// Specifies the application elements on which it is valid to apply an attribute.
	/// </summary>
	[Serializable]
	[Flags]
	public enum AttributeTargets
	{
		/// <summary>
		/// Attribute can be applied to an assembly.
		/// </summary>
		Assembly = 1,

		/// <summary>
		/// Attribute can be applied to a module.
		/// </summary>
		Module = 2,

		/// <summary>
		/// Attribute can be applied to a class.
		/// </summary>
		Class = 4,

		/// <summary>
		/// Attribute can be applied to a structure; that is, a value type.
		/// </summary>
		Struct = 8,

		/// <summary>
		/// Attribute can be applied to an enumeration.
		/// </summary>
		Enum = 16,

		/// <summary>
		/// Attribute can be applied to a constructor.
		/// </summary>
		Constructor = 32,

		/// <summary>
		/// Attribute can be applied to a method.
		/// </summary>
		Method = 64,

		/// <summary>
		/// Attribute can be applied to a property.
		/// </summary>
		Property = 128,

		/// <summary>
		/// Attribute can be applied to a field.
		/// </summary>
		Field = 256,

		/// <summary>
		/// Attribute can be applied to an event.
		/// </summary>
		Event = 512,

		/// <summary>
		/// Attribute can be applied to an interface.
		/// </summary>
		Interface = 1024,

		/// <summary>
		/// Attribute can be applied to a parameter.
		/// </summary>
		Parameter = 2048,

		/// <summary>
		/// Attribute can be applied to a delegate.
		/// </summary>
		Delegate = 4096,

		/// <summary>
		/// Attribute can be applied to a return value.
		/// </summary>
		ReturnValue = 8192,

		/// <summary>
		/// Attribute can be applied to a generic parameter.
		/// </summary>
		GenericParameter = 16384,

		/// <summary>
		/// Attribute can be applied to any application element.
		/// </summary>
		All = 32767,
	}
}
