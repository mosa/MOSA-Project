/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Tools.Compiler.Symbols.Pdb
{
	/// <summary>
	/// 
	/// </summary>
	public enum CvEntryType : ushort
	{
		/// <summary>
		/// 
		/// </summary>
		PublicSymbol3 = 0x110E,
		/// <summary>
		/// 
		/// </summary>
		PublicFunction13 = 0x1125,
		/// <summary>
		/// 
		/// </summary>
		Unknown_3 = 0x1129
	}
}
