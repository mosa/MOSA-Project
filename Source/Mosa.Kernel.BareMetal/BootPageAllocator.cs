// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public class BootPageAllocator
{
	#region Private Members

	private static Pointer BootReserveStartPage;
	private static uint BootReserveSize;
	private static uint UsedPages;

	#endregion Private Members

	#region Public API

	internal static void Setup()
	{
		Debug.WriteLine("BootPageAllocator:Setup()");

		var region = Platform.GetBootReservedRegion();

		BootReserveStartPage = region.Address;
		BootReserveSize = (uint)region.Size / Page.Size;

		UsedPages = 0;

		Debug.WriteLine("BootPageAllocator:Setup()");
	}

	public static Pointer AllocatePage()
	{
		return AllocatePages(1);
	}

	public static Pointer AllocatePages(uint pages = 1)
	{
		// TODO: Acquire lock

		var result = BootReserveStartPage + UsedPages * Page.Size;

		UsedPages += pages;

		Page.ClearPage(result);

		Debug.WriteLine(" > Boot Page Allocated @ ", new Hex(result));

		// TODO: Release lock

		return result;
	}

	#endregion Public API
}
