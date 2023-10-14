// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.MosaTypeSystem;

/// <summary>
/// Removes duplicates and preserves order
/// </summary>
public class GenericArgumentsCollection : List<MosaType>
{
	public GenericArgumentsCollection()
	{
	}

	public GenericArgumentsCollection(GenericArgumentsCollection other) : base()
	{
		foreach (var item in other)
			this.Add(item);
	}
}
