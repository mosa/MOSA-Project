// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class VirtualMemoryManager
{
	// TODO - implementation:

	#region Private Members

	// list of page pools (representing physical pages)
	// page pool consist of bitmap + # of entries + # of free entries
	//

	#endregion Private Members

	#region Public API

	public static void Start()
	{
	}

	public static Pointer GetMemoryPages(int count)
	{
		return Pointer.Zero;
	}

	public static void AllocatePage(Pointer virtualPage)
	{
		return;
	}

	public static void Map(Pointer virtualPage, Pointer physicalPage)
	{
		return;
	}

	#endregion Public API
}
