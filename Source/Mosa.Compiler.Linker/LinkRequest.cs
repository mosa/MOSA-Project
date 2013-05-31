/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// Represents a linking request to the assembly linker.
	/// </summary>
	public sealed class LinkRequest
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of LinkRequest.
		/// </summary>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="patches">The patches.</param>
		/// <param name="symbolName">The symbol that is being patched.</param>
		/// <param name="symbolOffset">The symbol offset.</param>
		/// <param name="methodRelativeBase">The base virtualAddress, if a relative link is required.</param>
		/// <param name="targetSymbol">The linker symbol to link against.</param>
		/// <param name="targetOffset">An offset to apply to the link target.</param>
		public LinkRequest(LinkType linkType, Patch[] patches, string symbolName, int symbolOffset, int methodRelativeBase, string targetSymbol, long targetOffset)
		{
			this.SymbolName = symbolName;
			this.SymbolOffset = symbolOffset;
			this.LinkType = linkType;
			this.MethodRelativeBase = methodRelativeBase;
			this.TargetSymbol = targetSymbol;
			this.TargetOffset = targetOffset;
			this.Patches = patches;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// The method whose code is being patched.
		/// </summary>
		public string SymbolName { get; private set; }

		/// <summary>
		/// Determines the relative base of the link request.
		/// </summary>
		public int MethodRelativeBase { get; private set; }

		/// <summary>
		/// The type of link required
		/// </summary>
		public LinkType LinkType { get; private set; }

		/// <summary>
		/// The position within the code stream where the virtualAddress is patched.
		/// </summary>
		public int SymbolOffset { get; private set; }

		/// <summary>
		/// Gets the name of the symbol.
		/// </summary>
		/// <value>The name of the symbol.</value>
		public string TargetSymbol { get; private set; }

		/// <summary>
		/// Gets the offset to apply to the link target.
		/// </summary>
		/// <value>The offset.</value>
		public long TargetOffset { get; private set; }

		/// <summary>
		/// Gets the patches.
		/// </summary>
		public Patch[] Patches { get; private set; }

		#endregion Properties
	}
}