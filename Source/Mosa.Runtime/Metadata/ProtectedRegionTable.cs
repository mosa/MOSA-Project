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
		// IntPtr[]	_protectedRegionDefinitions

		#endregion layout

		public IntPtr Ptr;

		public ProtectedRegionTable(IntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == IntPtr.Zero;

		public uint NumberOfRegions => Intrinsic.Load32(Ptr);

		public ProtectedRegionDefinition GetProtectedRegionDefinition(uint slot)
		{
			return new ProtectedRegionDefinition(Intrinsic.LoadPointer(Ptr, IntPtr.Size + (IntPtr.Size * (int)slot)));
		}
	}
}
