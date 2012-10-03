/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;
using Mosa.Compiler.Linker;

namespace Mosa.Tool.TypeExplorer
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class ExplorerLinkerSection : ExtendedLinkerSection
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TestLinkerSection"/> class.
		/// </summary>
		/// <param name="kind">The kind of the section.</param>
		/// <param name="name">The name.</param>
		/// <param name="address">The address.</param>
		public ExplorerLinkerSection(SectionKind kind, string name, long address) :
			base(kind, name, address)
		{
			stream = new MemoryStream();
		}

		#endregion // Construction

	}
}
