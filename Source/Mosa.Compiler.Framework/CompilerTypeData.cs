// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Compiler Type Data
	/// </summary>
	public sealed class CompilerTypeData
	{
		#region Properties

		public MosaType MosaType { get; }

		public bool IsTypeAllocated { get; set; }

		#endregion Properties

		#region Methods

		public CompilerTypeData(MosaType mosaType)
		{
			MosaType = mosaType ?? throw new ArgumentNullException(nameof(mosaType));
		}

		#endregion Methods
	}
}
