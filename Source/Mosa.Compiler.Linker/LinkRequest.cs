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
	public sealed class LinkRequest
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
		private readonly long offset;

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
		public LinkRequest(LinkType linkType, string symbolName, int methodOffset, int methodRelativeBase, string targetSymbolName, long offset)
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
			get { return symbolName; }
		}

		/// <summary>
		/// Determines the relative base of the link request.
		/// </summary>
		public int MethodRelativeBase
		{
			get { return methodRelativeBase; }
		}

		/// <summary>
		/// The type of link required
		/// </summary>
		public LinkType LinkType
		{
			get { return linkType; }
		}

		/// <summary>
		/// The position within the code stream where the virtualAddress is patched.
		/// </summary>
		public int MethodOffset
		{
			get { return methodOffset; }
		}

		/// <summary>
		/// Gets the name of the symbol.
		/// </summary>
		/// <value>The name of the symbol.</value>
		public string TargetSymbolName
		{
			get { return targetSymbolName; }
		}

		/// <summary>
		/// Gets the offset to apply to the link target.
		/// </summary>
		/// <value>The offset.</value>
		public long Offset
		{
			get { return offset; }
		}

		#endregion // Properties
	}
}
