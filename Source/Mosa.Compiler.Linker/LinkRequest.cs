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
		/// The position within the code stream where the virtual address is patched
		/// </summary>
		private readonly int symbolOffset;

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
		private readonly string targetSymbol;

		/// <summary>
		/// Holds an offset to apply to the link target.
		/// </summary>
		private readonly long targetOffset;

		/// <summary>
		/// Holds the patch instructions
		/// </summary>
		private readonly Patch[] patches;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of LinkRequest.
		/// </summary>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="patches">The patches.</param>
		/// <param name="symbolName">The symbol whose code is being patched.</param>
		/// <param name="symbolOffset">The symbol offset.</param>
		/// <param name="methodRelativeBase">The symbol relative base.</param>
		/// <param name="targetSymbol">The linker symbol to link against.</param>
		/// <param name="targetOffset">An offset to apply to the link target.</param>
		public LinkRequest(LinkType linkType, Patch[] patches, string symbolName, int symbolOffset, int methodRelativeBase, string targetSymbol, long targetOffset)
		{
			this.symbolName = symbolName;
			this.symbolOffset = symbolOffset;
			this.linkType = linkType;
			this.methodRelativeBase = methodRelativeBase;
			this.targetSymbol = targetSymbol;
			this.targetOffset = targetOffset;
			this.patches = patches;
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
		public int SymbolOffset
		{
			get { return symbolOffset; }
		}

		/// <summary>
		/// Gets the name of the symbol.
		/// </summary>
		/// <value>The name of the symbol.</value>
		public string TargetSymbol
		{
			get { return targetSymbol; }
		}

		/// <summary>
		/// Gets the offset to apply to the link target.
		/// </summary>
		/// <value>The offset.</value>
		public long TargetOffset
		{
			get { return targetOffset; }
		}

		/// <summary>
		/// Gets the patches.
		/// </summary>
		public Patch[] Patches
		{
			get { return patches; }
		}

		#endregion // Properties
	}
}
