// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Runtime.Metadata
{
	public struct CustomAttributeArgument
	{
		#region layout

		// Pointer _name;
		// bool _isField;
		// Pointer _argumentType;
		// int _argumentSize;

		#endregion layout

		public Pointer Ptr;

		public CustomAttributeArgument(Pointer ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr.IsNull;

		public string Name => (string)Intrinsic.GetObjectFromAddress(Ptr.LoadPointer());

		public bool IsField => Ptr.Load8(Pointer.Size) == 0;

		public TypeDefinition ArgumentType => new TypeDefinition(Ptr.LoadPointer(Pointer.Size * 2));

		public uint ArgumentSize => Ptr.Load32(Pointer.Size * 3);

		public Pointer GetArgumentValue()
		{
			return Ptr.LoadPointer(Pointer.Size * 4);
		}
	}
}
