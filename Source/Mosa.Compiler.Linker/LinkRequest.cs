/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// Represents a linking request to the assembly linker.
	/// </summary>
	public struct LinkRequest
	{
		#region Data members

		/// <summary>
		/// The method whose code is being patched.
		/// </summary>
		private readonly string symbolName;

		/// <summary>
		/// The position within the code stream where the virtualAddress is patched
		/// </summary>
		private readonly int methodOffset;

		/// <summary>
		/// Holds the relative request flag.
		/// </summary>
		private readonly int methodRelativeBase;

		/// <summary>
		/// The type of the link operation to perform.
		/// </summary>
		private readonly LinkType linkType;

		/// <summary>
		/// Holds the symbol name to link against.
		/// </summary>
		private readonly string targetSymbolName;

		/// <summary>
		/// Holds an offset to apply to the link target.
		/// </summary>
		private readonly IntPtr offset;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of LinkRequest.
		/// </summary>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="symbolName">The method whose code is being patched.</param>
		/// <param name="methodOffset">The method offset.</param>
		/// <param name="methodRelativeBase">The method relative base.</param>
		/// <param name="targetSymbolName">The linker symbol to link against.</param>
		/// <param name="offset">An offset to apply to the link target.</param>
		public LinkRequest(LinkType linkType, string symbolName, int methodOffset, int methodRelativeBase, string targetSymbolName, IntPtr offset)
		{
			this.symbolName = symbolName;
			this.methodOffset = methodOffset;
			this.linkType = linkType;
			this.methodRelativeBase = methodRelativeBase;
			this.targetSymbolName = targetSymbolName;
			this.offset = offset;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// The method whose code is being patched.
		/// </summary>
		public string LinkSymbol
		{
			get { return this.symbolName; }
		}

		/// <summary>
		/// Determines the relative base of the link request.
		/// </summary>
		public int MethodRelativeBase
		{
			get { return this.methodRelativeBase; }
		}

		/// <summary>
		/// The type of link required
		/// </summary>
		public LinkType LinkType
		{
			get { return this.linkType; }
		}

		/// <summary>
		/// The position within the code stream where the virtualAddress is patched.
		/// </summary>
		public int MethodOffset
		{
			get { return this.methodOffset; }
		}

		/// <summary>
		/// Gets the name of the symbol.
		/// </summary>
		/// <value>The name of the symbol.</value>
		public string TargetSymbolName
		{
			get { return this.targetSymbolName; }
		}

		/// <summary>
		/// Gets the offset to apply to the link target.
		/// </summary>
		/// <value>The offset.</value>
		public IntPtr Offset
		{
			get { return this.offset; }
		}

		#endregion // Properties
	}
}
