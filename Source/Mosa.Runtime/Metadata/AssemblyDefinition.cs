// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct AssemblyDefinition
	{
		#region layout

		// IntPtr _name;
		// IntPtr _customAttributes;
		// uint _attributes;
		// uint _numberOfTypes;

		#endregion layout

		public IntPtr Ptr;

		public AssemblyDefinition(IntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == IntPtr.Zero;

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public CustomAttributeTable CustomAttributeTable => new CustomAttributeTable(Intrinsic.LoadPointer(Ptr, IntPtr.Size));

		public uint Attributes => Intrinsic.Load32(Ptr, IntPtr.Size * 2);

		public uint NumberOfTypes => Intrinsic.Load32(Ptr, IntPtr.Size * 3);

		public TypeDefinition GetTypeDefinition(uint slot)
		{
			return new TypeDefinition(Intrinsic.LoadPointer(Ptr, 4 + (IntPtr.Size * 4) + (IntPtr.Size * (int)slot)));
		}
	}
}
