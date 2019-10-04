// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

		public uint NumberOfRegions => Ptr.Load32();

		public ProtectedRegionDefinition GetProtectedRegionDefinition(uint slot)
		{
			return new ProtectedRegionDefinition(Ptr.LoadPointer(Pointer.Size + (Pointer.Size * (int)slot)));
		}
	}
}
