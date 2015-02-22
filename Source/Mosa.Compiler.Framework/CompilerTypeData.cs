/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	///
	/// </summary>
	public sealed class CompilerTypeData
	{
		#region Properties

		public MosaType MosaType { get; private set; }

		public bool IsTypeAllocated { get; set; }

		#endregion Properties

		#region Methods

		public CompilerTypeData(MosaType mosaType)
		{
			if (mosaType == null)
				throw new ArgumentNullException("mosaType");

			MosaType = mosaType;
		}

		#endregion Methods
	}
}
