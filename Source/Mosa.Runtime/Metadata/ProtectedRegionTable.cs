// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	/// <summary>
	/// Protected Region Table
	/// </summary>
	public struct ProtectedRegionTable
	{
		#region layout

		// uint			_numberOfRegions;
		// Pointer[]	_protectedRegionDefinitions

		#endregion layout

		public Pointer Ptr;

		public ProtectedRegionTable(Pointer ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr.IsNull;

		public uint NumberOfRegions => Intrinsic.Load32(Ptr);

		public ProtectedRegionDefinition GetProtectedRegionDefinition(uint slot)
		{
			return new ProtectedRegionDefinition(Intrinsic.LoadPointer(Ptr, Pointer.Size + (Pointer.Size * (int)slot)));
		}
	}
}
