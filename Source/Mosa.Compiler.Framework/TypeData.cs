// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Compiler Type Data
	/// </summary>
	public sealed class TypeData
	{
		#region Properties

		public MosaType MosaType { get; }

		public bool IsTypeAllocated { get; set; }

		#endregion Properties

		#region Methods

		public TypeData(MosaType mosaType)
		{
			if (mosaType == null)
				throw new ArgumentNullException(nameof(mosaType));

			MosaType = mosaType;
		}

		#endregion Methods
	}
}
