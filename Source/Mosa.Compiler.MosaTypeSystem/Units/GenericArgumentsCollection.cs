// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.ObjectModel;

namespace Mosa.Compiler.MosaTypeSystem
{
	/// <summary>
	/// Removes duplicates and preserves order
	/// </summary>
	public class GenericArgumentsCollection : KeyedCollection<uint, MosaType>
	{
		public GenericArgumentsCollection() { }
		public GenericArgumentsCollection(GenericArgumentsCollection other) : base()
		{
			foreach (var item in other)
				this.Add(item);
		}

		protected override uint GetKeyForItem(MosaType item) => item.ID;
	}
}
