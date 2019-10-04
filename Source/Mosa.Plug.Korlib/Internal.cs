// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Plug.Korlib.Runtime;
using Mosa.Runtime;
using Mosa.Runtime.Plug;
using System.Collections.Generic;

namespace Mosa.Plug.Korlib
{
	internal static class Internal
	{
		internal static List<RuntimeAssembly> Assemblies;

		[Plug("Mosa.Runtime.StartUp::InitializeRuntimeMetadata")]
		internal static void Setup()
		{
			Assemblies = new List<RuntimeAssembly>();

			// Get AssemblyListTable and Assembly count
			var assemblyListTable = Intrinsic.GetAssemblyListTable();
			uint assemblyCount = assemblyListTable.Load32();

			// Loop through and populate the array
			for (int i = 0; i < assemblyCount; i++)
			{
				// Get the pointer to the Assembly Metadata
				var ptr = assemblyListTable.LoadPointer(Pointer.Size + (Pointer.Size * i));
				Assemblies.Add(new RuntimeAssembly(ptr.ToIntPtr()));
			}
		}
	}
}
